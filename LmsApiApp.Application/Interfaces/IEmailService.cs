using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(List<string> emails, string subject, string body);
    }
}
