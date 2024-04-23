using Assets.Scripts.Constants;
using System.Collections.Generic;

namespace Assets.Scripts.Dtos
{
    public class SpeechManagerSaveData : SaveData
    {
        public List<string> DiscussedTopicNames;

        public SpeechManagerSaveData() : base(SaveDataConstants.SpeechManagerSaveDataKey)
        {
            DiscussedTopicNames = new List<string>();
        }
    }
}
