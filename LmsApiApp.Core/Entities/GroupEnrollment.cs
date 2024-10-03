using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Core.Entities
{
    public class GroupEnrollment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; } // Student or teacher linked to the group
        public int GroupId { get; set; }
        public Group Group { get; set; } // Group linked to the user
        public DateTime EnrolledDate { get; set; }
    }
}
