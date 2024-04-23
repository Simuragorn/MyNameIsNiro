using UnityEngine;

public class Niro : MonoBehaviour
{
    [SerializeField] private Gate gate;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int initialRespect;
    private NiroMovement movement;
    private int respect;

    public Gate Gate => gate;
    public Transform SpawnPoint => spawnPoint;
    public int Respect => respect;

    private void Awake()
    {
        respect = initialRespect;
        movement = GetComponent<NiroMovement>();
    }

    public void Init(Puck puck, Person person)
    {
        movement.Init(this, puck, person);
    }
}
