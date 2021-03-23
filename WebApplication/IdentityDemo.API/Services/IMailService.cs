using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.API.Services
{
    public interface IMailService
    {
        Task SendMail(string toEmail, string subject, string content);
    }
}
