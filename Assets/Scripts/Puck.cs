using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Puck : MonoBehaviour
{
    [SerializeField] private Transform spawnPointForPlayer;
    [SerializeField] private Transform spawnPointForNiro;

    public event EventHandler<Gate> OnGoal;
    private Rigidbody2D rigidbody;
    private Collider2D collider;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var gate = collision.GetComponent<Gate>();
        if (gate != null)
        {
            OnGoal?.Invoke(this, gate);
        }
    }

    public void Respawn(bool forPlayer)
    {
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
