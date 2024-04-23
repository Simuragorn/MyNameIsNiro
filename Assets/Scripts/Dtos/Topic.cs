using System;
using System.Collections.Generic;

namespace Assets.Scripts.Dtos
{
    public class Topic
    {
        public string Name;
        public IntegerRange RequiredRespectRange;
        public List<Replica> Replicas;
    }
}
