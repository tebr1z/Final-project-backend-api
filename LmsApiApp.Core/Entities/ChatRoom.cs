using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Core.Entities
{
    public class ChatRoom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Participants { get; set; }
        public ICollection<ChatMessage> Messages { get; set; }
    }

}
