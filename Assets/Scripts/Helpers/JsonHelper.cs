using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class JsonHelper
    {
        public static T ReadFromJson<T>(string filePath) where T : class
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("File does not exist");
            }
            string jsonString = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(jsonString);
        }
    }
}
