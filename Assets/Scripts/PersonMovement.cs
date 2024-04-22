using UnityEngine;

public class PersonMovement : MonoBehaviour
{
    [SerializeField] private float force;

    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 directionVector = new Vector2(horizontal, vertical).normalized;
        rigidbody.AddForce(force * Time.deltaTime * directionVector, ForceMode2D.Impulse);
    }
}
