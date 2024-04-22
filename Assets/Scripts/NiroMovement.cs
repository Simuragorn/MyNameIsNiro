using UnityEngine;

public class NiroMovement : MonoBehaviour
{
    [SerializeField] private float force;
    private Niro niro;
    private Puck puck;

    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Init(Niro currentNiro, Puck currentPuck)
    {
        niro = currentNiro;
        puck = currentPuck;
    }

    void Update()
    {
        Vector2 directionVector = GetTargetDirection();
        rigidbody.AddForce(force * Time.deltaTime * directionVector, ForceMode2D.Impulse);
    }

    private Vector2 GetTargetDirection()
    {
        if (!puck.enabled)
        {
            return (niro.SpawnPoint.transform.position - transform.position).normalized;
        }
        return (puck.transform.position - transform.position).normalized;
    }
}
