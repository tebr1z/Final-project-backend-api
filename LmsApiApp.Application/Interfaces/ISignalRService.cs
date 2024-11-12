using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Interfaces
{
    public interface ISignalRService
    {
        Task SendMessageToUserAsync(string userId, string senderName, string message);
    }
}
