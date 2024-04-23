using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Person person;
    [SerializeField] private Niro niro;
    [SerializeField] private SpeechManager speechManager;
    [SerializeField] private Puck puck;
    [SerializeField] private TextMeshProUGUI scoreText;

    private int niroScore = 0;
    private int personScore = 0;
    private void Awake()
    {
        puck.OnGoal += Puck_OnGoal;
    }

    private void Start()
    {
        niro.Init(puck, person);
        speechManager.Init(niro);
        UpdateScoreText();
        niro.transform.position = niro.SpawnPoint.position;
        person.transform.position = person.SpawnPoint.position;
        RestartWithDelay(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart(true);
        }
    }

    private void Puck_OnGoal(object sender, Gate gate)
    {
        puck.enabled = false;
        bool personMadeGoal = gate == niro.Gate;
        if (personMadeGoal)
        {
            personScore++;
        }
        else
        {
            niroScore++;
        }
        UpdateScoreText();
        StartCoroutine(RestartWithDelay(personMadeGoal));
    }
    private IEnumerator RestartWithDelay(bool isPlayerMadeLastGoal)
    {
        yield return new WaitForSeconds(2);
        Restart(isPlayerMadeLastGoal);
    }

    private void Restart(bool isPersonMadeLastGoal)
    {
        puck.enabled = true;
        puck.Respawn(isPersonMadeLastGoal);
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"{personScore}:{niroScore}";
    }
}
