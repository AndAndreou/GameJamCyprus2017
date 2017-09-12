using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [Header("Movement")]
    public bool adaptiveCamera = true;
    public float speed = 1;
    public float movementRadius = 5;

    [Header("Gizmos")]
    public bool showGizmos = true;
    public bool showTargetLookPosGizmo = true;
    public Color targetLookPosGizmoColor = Color.red;
    public float targetLookPosGizmoSize = 0.5f;
    public bool showMovementRadiusGizmo = true;
    public Color movementRadiusGizmoColor = Color.blue;

    Vector3 targetLookPos;
    bool onTargetSitPos = true;
    Vector3 targetSitPosition;
    Vector3 lastSitPosition;
    float targetSitPosTimer = 0;
    Vector3 midPointPos;
    float zInitRotation;
    bool lookImidiatetly = false;


    List<RobotController> players;
    Vector3 lastAlivePlayerPos;

    void Start()
    {
        // Get midpoint point
        midPointPos = GetMidpoint();
    }

    void LateUpdate()
    {
        if (!onTargetSitPos)
        {
            if (targetSitPosTimer < 3f)
            {
                transform.position = Vector3.Slerp(lastSitPosition, targetSitPosition, targetSitPosTimer / 3f);
                targetSitPosTimer += Time.deltaTime;
            }
            else if (targetSitPosTimer >= 3f)
            {
                transform.position = targetSitPosition;
                onTargetSitPos = true;
                targetSitPosTimer = 0;
            }
        }

        targetLookPos = getTargetLookPos();
        Quaternion targetRotation = Quaternion.LookRotation(targetLookPos - transform.position);

        // rotate towards the target point.
        transform.rotation = lookImidiatetly ? targetRotation : Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        lookImidiatetly = false;
    }

    public void SetTargetSitPosition(Vector3 targetSitPosition, bool repositionimidiatetly = false)
    {
        if (repositionimidiatetly)
        {
            transform.position = targetSitPosition;
        }
        else
        {
            this.targetSitPosition = targetSitPosition;
            lastSitPosition = transform.position;
            onTargetSitPos = false;
        }
    }

    public void SetMidpoint(Vector3 midPointPos, bool lookimidiatetly = false)
    {
        this.midPointPos = midPointPos;
        this.lookImidiatetly = lookimidiatetly;
    }

    Vector3 GetMidpoint()
    {
        Vector3 midPointPos = Vector3.zero;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(-5F, 0F, 0));
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit))
        {
            midPointPos = rayHit.point;
            midPointPos = Vector3.RotateTowards(midPointPos, transform.position, 0, 0);
        }

        return midPointPos;
    }

    Vector3 getTargetLookPos()
    {
        // Get players
        players = GameManager.GetPlayers();

        // Check if the camera position should adapt to players positions
        if (adaptiveCamera && players.Count > 0)
        {
            targetLookPos = Vector3.zero;
            int totalAlivePlayers = 0;

            // Only take into account the players which are currently alive
            foreach (RobotController player in players)
            {
                if (player.isAlive)
                {
                    targetLookPos += player.transform.position;
                    totalAlivePlayers++;
                    lastAlivePlayerPos = player.transform.position;
                }
            }

            // If nobody is alive, look at where the last alive player last was
            if (totalAlivePlayers == 0)
            {
                targetLookPos = lastAlivePlayerPos;
            }
            else
            {
                targetLookPos /= totalAlivePlayers;
            }

            // Ensure that target position is within camera movement radius
            Vector3 diff = targetLookPos - midPointPos;
            float distanceFromMidpoint = diff.magnitude;
            if (distanceFromMidpoint > movementRadius)
            {
                targetLookPos = midPointPos + (diff / distanceFromMidpoint) * movementRadius;
            }

            return targetLookPos;
        }
        else
        {
            return midPointPos;
        }
    }

    void OnDrawGizmos()
    {
        if (showGizmos)
        {
            if (showTargetLookPosGizmo)
            {
                Gizmos.color = targetLookPosGizmoColor;
                Gizmos.DrawSphere(targetLookPos, targetLookPosGizmoSize);
                Gizmos.DrawLine(transform.position, targetLookPos);
                Gizmos.DrawLine(transform.position, midPointPos);
            }

            if (showMovementRadiusGizmo)
            {
                Gizmos.color = movementRadiusGizmoColor;
                Gizmos.DrawWireSphere(midPointPos, movementRadius);
            }
        }
    }
}
