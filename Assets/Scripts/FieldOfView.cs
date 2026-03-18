using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldOfView : MonoBehaviour
{
    public float Radius;
    [Range(0,360)]
    public float Angle;
    public float CrouchSubtraction;
    public float ChaseAddition;
    private bool hasSubtracted = false;
    private bool hasAdded = false;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    private PlayerController PlayerController;

    float distanceToPlayer = 50;
    public string LoseScreen;

    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("PlayerHitBox");
        PlayerController = playerRef.GetComponentInParent<PlayerController>();
        StartCoroutine(FOVRoutine());
        

    }

    void Update()
    {
        if (PlayerController.isCrouching && !hasSubtracted)
        {
            Angle = Angle - CrouchSubtraction;
            hasSubtracted = true;
        }
        else if (!PlayerController.isCrouching && hasSubtracted)
        {
            hasSubtracted = false;
            Angle = Angle + CrouchSubtraction;
        }
        if (canSeePlayer && !hasAdded)
        {
            Angle = Angle + ChaseAddition;
            hasAdded = true;
        }
        else if (!canSeePlayer && hasAdded)
        {
            hasAdded = false;
            Angle = Angle - ChaseAddition;
        }

        // Dont ask why this is in the FOV script
        distanceToPlayer = Vector3.Distance(playerRef.transform.position, transform.position);
        if (distanceToPlayer < 12)
        {
            SceneManager.LoadScene(LoseScreen);
        }
    }



    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, Radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < Angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;

                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
}