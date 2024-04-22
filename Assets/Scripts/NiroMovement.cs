using Assets.Scripts.Constants;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NiroMovement : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private List<Transform> beforeHitPoints;
    private Niro niro;
    private Puck puck;
    private Person person;
    private Vector2 targetPoint;

    private int beforeHitPointIndex = 0;

    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Init(Niro currentNiro, Puck currentPuck, Person currentPerson)
    {
        niro = currentNiro;
        puck = currentPuck;
        person = currentPerson;

        puck.OnGoal += Puck_OnGoal;
    }

    private void Puck_OnGoal(object sender, Gate e)
    {
        beforeHitPointIndex = beforeHitPointIndex = Random.Range(0, beforeHitPoints.Count);
    }

    void Update()
    {
        const float minOffset = 0.1f;
        targetPoint = GetTargetPoint();
        if (Vector2.Distance((Vector2)transform.position, targetPoint) > minOffset)
        {
            Vector2 directionVector = (targetPoint - (Vector2)transform.position).normalized;
            rigidbody.AddForce(force * Time.deltaTime * directionVector, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetPoint, 0.5f);
    }

    private Vector2 GetTargetPoint()
    {
        if (!puck.enabled)
        {
            return beforeHitPoints[beforeHitPointIndex].position;
        }
        return GetPuckTargetPoint();
    }

    private Vector2 GetPuckTargetPoint()
    {
        Vector2 targetPosition = puck.transform.position;
        Vector2? predictedPoint = null;
        if (puck.ActiveField == null)
        {
            targetPosition = niro.SpawnPoint.position;
            predictedPoint = TryPredictFieldCollisionPoint(puck.transform.position, puck.Velocity);
        }
        else if (puck.ActiveField.IsPerson)
        {
            Vector2 possiblePuckMovement = puck.transform.position - person.transform.position;
            predictedPoint = TryPredictFieldCollisionPoint(person.transform.position, possiblePuckMovement);
        }
        if (predictedPoint.HasValue)
        {
            targetPosition = predictedPoint.Value;

            Debug.DrawRay(transform.position, (targetPosition - (Vector2)transform.position).normalized * 10);
            Debug.Log("Predicting");
        }
        return targetPosition;
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
