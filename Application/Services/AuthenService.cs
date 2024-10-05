using Application.Commons;
using Application.IService;
using Application.ServiceResponse;
using Application.Utils;
using Application.ViewModels.UserDTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthenService : IAuthenService
    {
        private readonly AppConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AuthenService(AppConfiguration config, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TokenResponse<string>> cLoginAsync(cLoginUserDTO userObject)
        {
            var response = new TokenResponse<string>();
            try
            {
                var passHash = HashPassWithSHA256.HashWithSHA256(userObject.Password);
                var userLogin = await _unitOfWork.UserRepository.cGetUserByEmailAddressAndPasswordHash(userObject.Username, passHash);
                if (userLogin == null)
                {
                    response.Success = false;
                    response.Message = "Invalid username or password";
                    return response;
                }
                var token = await _unitOfWork.TokenRepo.cGetTokenByUserIdAsync(userLogin.Id);
                if(token != null && token.TokenValue != "success")
                {                   
                    response.Success = false;
                    response.Message = "Please confirm via link in your mail";
                    return response;
                }           
                var auth = userLogin.Role;
                var userId = userLogin.Id;
                var tokenJWT = userLogin.GenerateJsonWebToken(_config, _config.JWTSection.SecretKey, DateTime.Now);
                response.Success = true;
                response.Message = "Login successfully";
                response.DataToken = tokenJWT;
                response.Role = auth;
                response.HintId = userId;
            }
            catch (DbException ex)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ServiceResponse<cRegisterDTO>> cRegisterAsync(cRegisterDTO userObject)
        {
            var response = new ServiceResponse<cRegisterDTO>();
            try
            {
                var existEmail = await _unitOfWork.UserRepository.cCheckEmailAddressExisted(userObject.Email);
                if (existEmail)
                {
                    response.Success = false;
                    response.Message = "Email is already existed";
                    return response;
                }

                var userAccountRegister = _mapper.Map<User>(userObject);
                userAccountRegister.Password = HashPassWithSHA256.HashWithSHA256(userObject.Password);

                //userAccountRegister.ConfirmationToken = Guid.NewGuid().ToString();
                
                userAccountRegister.AccountLocked = false;
                userAccountRegister.Role = "Customer";
                await _unitOfWork.UserRepository.AddAsync(userAccountRegister);
                //Create Token
                var confirmationToken = new Token
                {
                    TokenValue = Guid.NewGuid().ToString(),
                    Type = "confirmation",
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                    UserId = userAccountRegister.Id
                };
                await _unitOfWork.TokenRepo.AddAsync(confirmationToken);
                var confirmationLink =
                    $"https://koifarmmanagement-axevbhdzh9edauf8.eastus-01.azurewebsites.net/confirm?token={confirmationToken.TokenValue}";

                //SendMail
                var emailSend = await SendMail.SendConfirmationEmail(userObject.Email, confirmationLink);
                if (!emailSend)
                {
                    response.Success = false;
                    response.Message = "Error when send mail";
                    return response;
                }
                Order order = new Order();
                order.UserId = userAccountRegister.Id;
                order.ShippingFee = 0;
                order.TotalPrice = 0;
				await _unitOfWork.OrderRepository.AddAsync(order); 

                var accountRegistedDTO = _mapper.Map<cRegisterDTO>(userAccountRegister);
                response.Success = true;
                response.Data = accountRegistedDTO;
                response.Message = "Register successfully.";
            }
            catch (DbException e)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { e.Message };
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { e.Message };
            }

            return response;
        }

        public async Task<ServiceResponse<string>> cResendConfirmationTokenAsync(string email)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var user = await _unitOfWork.UserRepository.cGetByEmailAsync(email);
                if (user == null)
                {
                    response.Success = false;
                    response.Error = "Không tìm thấy người dùng với email này.";
                    return response;
                }
                var token = await _unitOfWork.TokenRepo.cFindByConditionAsync(user.Id, "confirmation");
                if (token != null && token.TokenValue == "success")
                {
                    response.Success = false;
                    response.Message = "Email của bạn đã được xác nhận.";
                    return response;
                }               
                if (DateTime.UtcNow > token.ExpiresAt)
                {
                    await _unitOfWork.TokenRepo.cDeleteTokenAsync(token);
                    var newToken = new Token
                    {
                        TokenValue = Guid.NewGuid().ToString(),
                        Type = "confirmation",
                        CreatedAt = DateTime.UtcNow,
                        ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                        UserId = user.Id
                    };

                    await _unitOfWork.TokenRepo.AddAsync(newToken);
                    
                    var confirmationLink = $"https://koifarmmanagement-axevbhdzh9edauf8.eastus-01.azurewebsites.net/confirm?token={newToken.TokenValue}";
                    var emailSend = await SendMail.SendConfirmationEmail(user.Email, confirmationLink);

                    if (!emailSend)
                    {
                        response.Success = false;
                        response.Message = "Gửi email thất bại.";
                        return response;
                    }

                    response.Success = true;
                    response.Message = "Email xác nhận mới đã được gửi.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Token xác nhận của bạn vẫn còn hiệu lực. Vui lòng kiểm tra email.";
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = "Đã xảy ra lỗi.";
                response.ErrorMessages = new List<string> { e.Message };
            }
            return response;
        }
    }
}
