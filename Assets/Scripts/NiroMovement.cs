using Assets.Scripts.Constants;
using System.Linq;
using UnityEngine;

public class NiroMovement : MonoBehaviour
{
    [SerializeField] private float force;
    private Niro niro;
    private Puck puck;
    private Vector2 targetPoint;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetPoint, 0.5f);
    }

    private Vector2 GetTargetDirection()
    {
        if (!puck.enabled)
        {
            return (niro.SpawnPoint.transform.position - transform.position).normalized;
        }
        return GetDirectionToPuck();
    }

    private Vector2 GetDirectionToPuck()
    {
        Vector2 targetPosition = puck.transform.position;
        if (puck.ActiveField == null)
        {
            var predictedPoint = TryPredictFieldCollisionPoint(puck.transform.position, puck.Velocity);
            if (predictedPoint.HasValue)
            {
                targetPosition = predictedPoint.Value;

                Debug.DrawRay(transform.position, (targetPosition - (Vector2)transform.position).normalized * 10);
                Debug.Log("Predicting");
            }
        }
        targetPoint = targetPosition;
        return (targetPosition - (Vector2)transform.position).normalized;
    }

    private Vector2? TryPredictFieldCollisionPoint(Vector2 fromPoint, Vector2 movementDirection)
    {
        const int maxCycles = 10;
        for (int cycle = 0; cycle < maxCycles; cycle++)
        {
            int fieldLayer = LayerMask.NameToLayer(LayerConstants.FieldLayer);
            int borderLayer = LayerMask.NameToLayer(LayerConstants.BorderLayer);

            int neededLayers = LayerMask.GetMask(LayerConstants.BorderLayer, LayerConstants.FieldLayer);
            var hits = Physics2D.RaycastAll(fromPoint, movementDirection, 50, neededLayers);
            RaycastHit2D? possibleHit = hits.FirstOrDefault(h => h.point != fromPoint);
            if (possibleHit == null || possibleHit.Value.collider == null)
            {
                return null;
            }
            var hit = possibleHit.Value;
            if (hit.collider.gameObject.layer == fieldLayer)
            {
                var field = hit.collider.GetComponent<PlayerField>();
                if (!field.IsPerson)
                {
                    return hit.point;
                }
            }
            if (hit.collider.gameObject.layer == borderLayer)
            {
                fromPoint = hit.point;
                movementDirection = Vector2.Reflect(movementDirection, hit.normal);
                Debug.DrawRay(fromPoint, movementDirection * 10);
                continue;
            }
        }
        return null;
    }
}
