using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Core.Entities
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }  // Cevap metni
        public bool IsCorrect { get; set; }  // Doğru cevap mı?a
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
