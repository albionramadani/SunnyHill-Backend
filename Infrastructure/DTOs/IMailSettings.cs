using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public interface IMailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string EmailSub { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
        public string EmailSubject { get; set; }
        public string EmailFrom { get; set; }
        public string EmailBody { get; set; }
    }
}
