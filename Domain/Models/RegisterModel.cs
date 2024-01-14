using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Backend.Models
{
    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
