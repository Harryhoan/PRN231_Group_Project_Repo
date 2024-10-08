using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserDTO
{
    public class cUserDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Address { get; set; }
        public string? TelephoneNumber { get; set; }
        public byte Status { get; set; }
        public string? RoleName { get; set; }
    }
}
