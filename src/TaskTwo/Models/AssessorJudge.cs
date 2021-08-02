using System;

namespace TaskTwo.Models
{
    class AssessorJudge
    {
        public string Login { get; set; }
        public ushort UId { get; set; }
        public ushort DocId { get; set; }
        public bool Judgement { get; set; }
        public bool CorrectJudgement { get; set; }

        public bool IsCorrect
        {
            get
            {
                return Judgement == CorrectJudgement;
            }
        }
    }
}
