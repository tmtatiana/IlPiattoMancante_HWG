using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System.Linq; //I honestly have ZERO idea how Linq works google saved me

public class ChefBehaviour : MonoBehaviour
{
    private FieldOfView FieldOfView;

    public List<Transform> wayPoint = new List<Transform>();

    NavMeshAgent navMeshAgent;

    public int currentWaypointIndex = 0;

    public bool isStunned;

    public AudioClip stunSound;
    public AudioSource chefAudio;

    private bool dontKillMyEars = false;
    private bool canPlaySound = true;

    // Start is called before the first frame update
    void Start()
    {
        isStunned = false;
        chefAudio = GetComponent<AudioSource>();
        chefAudio.playOnAwake = false;
        FieldOfView = GetComponent<FieldOfView>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        wayPoint = GameObject.FindGameObjectsWithTag("Waypoints")
            .OrderBy(go => go.name) // This sorts by name so like WayPoint1, then WayPoint2, if we want to change pathing, we change name of waypoint
            .Select(go => go.transform)
            .ToList();

    }

    // Update is called once per frame
    void Update()
    {
        if (!FieldOfView.canSeePlayer && !isStunned)
        {
            Pathing();
            navMeshAgent.speed = 5;
        }
        else if (FieldOfView.canSeePlayer && !isStunned)
        {
            navMeshAgent.SetDestination(FieldOfView.playerRef.transform.position);
            navMeshAgent.speed = 6;
        }
        else if (isStunned)
        {
            StartCoroutine(Stunned());
            
        }
    }

    private IEnumerator Stunned()
    {
        navMeshAgent.speed = 0;

        //call function to play stun sound
        if (!dontKillMyEars && canPlaySound)
        {
            PlayStunSound();
            dontKillMyEars = true;

        }



        print("im so stunned");
        yield return new WaitForSeconds(3f);
        navMeshAgent.speed = 6;
        navMeshAgent.SetDestination(FieldOfView.playerRef.transform.position);
        print("now im not");
        isStunned = false;
        dontKillMyEars = false;
        StartCoroutine(StunSoundCooldown());
    }

    private IEnumerator StunSoundCooldown()
    {
        canPlaySound = false;
        yield return new WaitForSeconds(4f);
        canPlaySound = true;
    }

    void Pathing()
    {

        if (wayPoint == null || wayPoint.Count == 0) //Every help forum had something along these lines, its like a please dont crash safety net
        {
            return;

        }

        float distanceToWayPoint = Vector3.Distance(wayPoint[currentWaypointIndex].position, transform.position);

        if (distanceToWayPoint <= 3)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % wayPoint.Count;

        }

        navMeshAgent.SetDestination(wayPoint[currentWaypointIndex].position);



    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "meatballprojectile" || collision.gameObject.name == "meatballToThrow_HWG(Clone)")
        {
            isStunned = true;
            print("Meatball detected");
        }
    }

    void PlayStunSound()
    {
        if (chefAudio != null && stunSound != null)
        {
            chefAudio.PlayOneShot(stunSound);
        }
    }
    
}
