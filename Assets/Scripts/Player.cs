using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Gate gate;
    [SerializeField] private Transform spawnPoint;
    private PlayerMovement movement;

    public Transform SpawnPoint => spawnPoint;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    public Gate Gate => gate;
}
