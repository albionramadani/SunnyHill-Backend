using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AuthenticatedModel
    {
        public Guid Id { get; set; }
        public UserModel UserData { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}
