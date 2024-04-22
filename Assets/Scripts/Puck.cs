using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Puck : MonoBehaviour
{
    [SerializeField] private Transform spawnPointForPlayer;
    [SerializeField] private Transform spawnPointForNiro;

    public event EventHandler<Gate> OnGoal;
    public PlayerField ActiveField { get; private set; }
    public Vector2 Velocity => rigidbody.velocity;
    private Rigidbody2D rigidbody;
    private Collider2D collider;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerField>(out var field))
        {
            ActiveField = field;
        }
        if (collision.TryGetComponent<Gate>(out var gate))
        {
            OnGoal?.Invoke(this, gate);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerField>(out var field) && ActiveField == field)
        {
            ActiveField = null;
        }
    }

    public void Respawn(bool forPlayer)
    {
        ActiveField = null;
        rigidbody.velocity = Vector2.zero;
        if (forPlayer)
        {
            transform.position = spawnPointForPlayer.position;
        }
        else
        {
            transform.position = spawnPointForNiro.position;
        }
    }
}
