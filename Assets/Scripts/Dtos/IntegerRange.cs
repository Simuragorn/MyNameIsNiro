using System;

namespace Assets.Scripts.Dtos
{
    [Serializable]
    public class IntegerRange
    {
        public int Min;
        public int Max;

        public bool Contains(int number)
        {
            return Min <= number && Max >= number;
        }
    }
}
