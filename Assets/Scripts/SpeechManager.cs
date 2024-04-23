using Assets.Scripts.Constants;
using Assets.Scripts.Dtos;
using Assets.Scripts.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class SpeechManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI replicaTextComponent;
    [SerializeField] private float delayPerReplica;
    [SerializeField] private int maxDelaysCountPerQuestion;

    public event System.EventHandler<Topic> OnTopicEnded;

    private SpeechManagerSaveData saveData;
    private Topic currentTopic;
    private Replica currentReplica;
    private Niro niro;
    private List<Topic> topics;
    private bool? userAnswer;

    public void Init(GameManager gameManager, Niro currentNiro)
    {
        gameManager.OnSavingData += GameManager_OnSavingData;
        OnTopicEnded += SpeechManager_OnTopicEnded;
        niro = currentNiro;

        LoadData();
        LoadTopics();
        StartNewTopic();
    }

    private void GameManager_OnSavingData(object sender, EventArgs e)
    {
        SaveData();
    }

    private void LoadData()
    {
        saveData = SaveHelper.Load<SpeechManagerSaveData>(SaveDataConstants.SpeechManagerSaveDataKey);
    }

    private void SaveData()
    {
        SaveHelper.Save(SaveDataConstants.SpeechManagerSaveDataKey, saveData);
    }

    private void SpeechManager_OnTopicEnded(object sender, Topic topic)
    {
        saveData.DiscussedTopicNames.Add(topic.Name);
        StartNewTopic();
    }

    private void StartNewTopic()
    {
        List<Topic> suitableTopics = topics.Where(t => t.RequiredRespectRange.Contains(niro.Respect) &&
        !saveData.DiscussedTopicNames.Contains(t.Name)).ToList();
        if (suitableTopics.Count == 0)
        {
            saveData.DiscussedTopicNames.Clear();
            suitableTopics = topics.Where(t => t.RequiredRespectRange.Contains(niro.Respect)).ToList();
        }
        int topicIndex = UnityEngine.Random.Range(0, suitableTopics.Count);
        currentTopic = suitableTopics[topicIndex];
        StartCoroutine(PerformReplicas());
    }

    private IEnumerator PerformReplicas()
    {
        for (int i = 0; i < currentTopic.Replicas.Count; ++i)
        {
            Replica newReplica = currentTopic.Replicas[i];
            yield return SetNewReplica(newReplica);
        }
        var endedTopic = currentTopic;
        currentReplica = null;
        currentTopic = null;
        OnTopicEnded?.Invoke(this, endedTopic);
    }

    private IEnumerator SetNewReplica(Replica newReplica)
    {
        currentReplica = newReplica;
        replicaTextComponent.text = currentReplica.Text;
        yield return new WaitForSeconds(delayPerReplica);

        if (currentReplica.IsQuestion)
        {
            for (int i = 0; i < maxDelaysCountPerQuestion; ++i)
            {
                if (userAnswer.HasValue)
                {
                    break;
                }
                yield return new WaitForSeconds(delayPerReplica);
            }
        }
    }

    private void LoadTopics()
    {
        topics = new List<Topic>();

        List<string> topicFilePaths = Directory.GetFiles(FileConstants.TopicsFolderPath).ToList();
        foreach (var topicPath in topicFilePaths)
        {
            var topic = JsonHelper.ReadFromJson<Topic>(topicPath);
            topics.Add(topic);
        }
    }
}
