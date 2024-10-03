using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Core.Entities
{
    public  class ChatMessage
    {
        public int Id { get; set; }
        public string MessageContent { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public DateTime SentAt { get; set; }
    }

}
