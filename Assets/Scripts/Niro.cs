using UnityEngine;

public class Niro : MonoBehaviour
{
    [SerializeField] private Gate gate;
    [SerializeField] private Transform spawnPoint;
    private NiroMovement movement;

    public Gate Gate => gate;
    public Transform SpawnPoint => spawnPoint;

    private void Awake()
    {
        movement = GetComponent<NiroMovement>();
    }

    public void Init(Puck puck)
    {
        movement.Init(this, puck);
    }
}
