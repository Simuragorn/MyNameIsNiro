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
    [SerializeField] private TextMeshProUGUI replicTextComponent;
    [SerializeField] private float delayPerReplica;
    [SerializeField] private int maxDelaysCountPerQuestion;

    public event System.EventHandler OnTopicEnded;

    private Topic currentTopic;
    private Replica currentReplica;
    private Niro niro;
    private List<Topic> topics;
    private bool? userAnswer;

    public void Init(Niro currentNiro)
    {
        niro = currentNiro;
        LoadTopics();
        StartNewTopic();
        OnTopicEnded += SpeechManager_OnTopicEnded;
    }

    private void SpeechManager_OnTopicEnded(object sender, EventArgs e)
    {
        StartNewTopic();
    }

    private void StartNewTopic()
    {
        List<Topic> suitableTopics = topics.Where(t => t.RequiredRespectRange.Contains(niro.Respect)).ToList();

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
        currentReplica = null;
        currentTopic = null;
        OnTopicEnded?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator SetNewReplica(Replica newReplica)
    {
        currentReplica = newReplica;
        replicTextComponent.text = currentReplica.Text;
        yield return new WaitForSeconds(delayPerReplica);

        if (!currentReplica.IsQuestion)
        {
            yield return null;
        }
        for (int i = 0; i < maxDelaysCountPerQuestion; ++i)
        {
            if (userAnswer.HasValue)
            {
                break;
            }
            yield return new WaitForSeconds(delayPerReplica);
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
