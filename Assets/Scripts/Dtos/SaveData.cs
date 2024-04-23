using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Dtos
{
    public class SaveData
    {
        [NonSerialized]
        public readonly string Key;

        public SaveData(string key)
        {
            Key = key;
        }
    }
}
