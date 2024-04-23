using Assets.Scripts.Dtos;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class SaveHelper
    {
        public static void Save<T>(string key, T data) where T : SaveData
        {
            string jsonString = JsonUtility.ToJson(data,true);
            PlayerPrefs.SetString(key, jsonString);
        }

        public static T Load<T>(string key) where T : SaveData, new()
        {
            if (!PlayerPrefs.HasKey(key))
            {
                return new();
            }
            string jsonString = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<T>(jsonString);
        }
    }
}
