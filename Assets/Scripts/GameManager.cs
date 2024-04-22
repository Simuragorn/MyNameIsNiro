using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Niro niro;
    [SerializeField] private Puck puck;
    [SerializeField] private TextMeshProUGUI scoreText;

    private int niroScore = 0;
    private int playerScore = 0;
    private void Awake()
    {
        puck.OnGoal += Puck_OnGoal;
    }

    private void Start()
    {
        niro.Init(puck);
        UpdateScoreText();
        niro.transform.position = niro.SpawnPoint.position;
        player.transform.position = player.SpawnPoint.position;
        RestartWithDelay(true);
    }

    private void Puck_OnGoal(object sender, Gate gate)
    {
        puck.enabled = false;
        bool playerMadeGoal = gate == niro.Gate;
        if (playerMadeGoal)
        {
            playerScore++;
        }
        else
        {
            niroScore++;
        }
        UpdateScoreText();
        StartCoroutine(RestartWithDelay(playerMadeGoal));
    }
    private IEnumerator RestartWithDelay(bool isPlayerMadeLastGoal)
    {
        yield return new WaitForSeconds(2);
        Restart(isPlayerMadeLastGoal);
    }

    private void Restart(bool isPlayerMadeLastGoal)
    {
        puck.enabled = true;
        puck.Respawn(isPlayerMadeLastGoal);
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"{playerScore}:{niroScore}";
    }
}
