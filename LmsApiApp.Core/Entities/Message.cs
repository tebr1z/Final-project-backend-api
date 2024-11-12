using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Core.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }  // Mesajı gönderen kullanıcı
        public string ReceiverId { get; set; }  // Mesajı alan kullanıcı
        public string Content { get; set; }  // Mesajın içeriği
        public DateTime SentAt { get; set; } = DateTime.Now;  // Mesajın gönderilme zamanı
        public bool IsRead { get; set; } = false;  // Mesajın okunma durumu
        public virtual User Sender { get; set; }  // İlişki: Mesajı gönderen kullanıcı
        public virtual User Receiver { get; set; }  // İlişki: Mesajı alan kullanıcı
    }

}
