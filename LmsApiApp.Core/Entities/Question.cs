using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Core.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }  // Soru metni
        public string Type { get; set; }  // Soru türü (multiple-choice, açık uçlu vb.)
        public virtual ICollection<Answer> Answers { get; set; }  // Soruya verilen cevaplar
        public int Points { get; set; }  // Sorunun puanı
        public bool IsClosed { get; set; }  // Öğretmen tarafından kapandı mı?
        public int TestId { get; set; }
        public Test Test { get; set; }
    
    }
}
