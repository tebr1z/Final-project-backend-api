using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Core.Entities
{
    public class ForumPost
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int ThreadId { get; set; }
        public ForumThread Thread { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime PostedAt { get; set; }
        public ForumThread ForumThread { get; set; }
    }

}
