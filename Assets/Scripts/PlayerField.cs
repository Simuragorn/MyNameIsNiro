using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerField : MonoBehaviour
{
    [SerializeField] private bool isPerson;

    public bool IsPerson => isPerson;
}
