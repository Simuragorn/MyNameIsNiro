using System;

namespace Assets.Scripts.Dtos
{
    [Serializable]
    public class Replica
    {
        public string Text;
        public bool IsQuestion;

        public int PositiveAnswerScore;
        public int NegativeAnswerScore;
    }
}
