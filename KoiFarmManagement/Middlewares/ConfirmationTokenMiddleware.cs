using Application;

namespace KoiFarmManagement.Middlewares
{
    public class ConfirmationTokenMiddleware
    {
        private readonly RequestDelegate _next;
        public ConfirmationTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // create area service temporary
            using (var scope = context.RequestServices.CreateScope())
            {
                // Get the IUnitOfWork from the temporary service scope
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var tokenValue = context.Request.Query["token"];

                if (!string.IsNullOrEmpty(tokenValue))
                {
                    var token = await unitOfWork.TokenRepo.cGetTokenWithUser(tokenValue, "confirmation");

                    if (token != null && token.User != null && !token.User.AccountLocked)
                    {
                        if(DateTime.UtcNow > token.ExpiresAt)
                        {
                            //context.Response.Redirect("https://zodiacgems.vercel.app/login");
                            await context.Response.WriteAsync("Token xác nhận đã hết hạn. Vui lòng yêu cầu gửi lại email xác nhận.");
                            return;
                        }
                        token.User.AccountLocked = true;
                        token.TokenValue = "success";

                        await unitOfWork.SaveChangeAsync();                       
                        context.Response.Redirect("http://localhost:3000/login");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
