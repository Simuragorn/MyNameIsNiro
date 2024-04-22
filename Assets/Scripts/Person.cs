using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    [SerializeField] private Gate gate;
    [SerializeField] private Transform spawnPoint;
    private PersonMovement movement;

    public Transform SpawnPoint => spawnPoint;

    private void Awake()
    {
        movement = GetComponent<PersonMovement>();
    }

    public Gate Gate => gate;
}
