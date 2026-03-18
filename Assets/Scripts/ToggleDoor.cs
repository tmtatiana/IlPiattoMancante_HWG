using UnityEngine;
using System.Collections;

public class ToggleDoor : MonoBehaviour

{
    //stores the door's collider
    public Collider doorCollider;

    //stores animator
    private Animator animator;
    public string ChefTag;


    public GameObject chef;
    private bool StartTriggers = false;
    public bool Openable = true;


    //keeps track of whether or not the door is open
    public bool isOpen;

    //this is where the audio variable is stored
    public AudioClip DoorOpenSound;
    private AudioSource DoorAudio;
    void Start()
    {
        //finds the animator component and stores it in variable
        animator = GetComponentInParent<Animator>();

        //storing audio references
        DoorAudio = GetComponent<AudioSource>();
        DoorAudio.playOnAwake = false;

        StartCoroutine(GetChef());
    }



    private IEnumerator GetChef()
    {
        yield return new WaitForSeconds(3f);
        chef = GameObject.FindGameObjectWithTag("Chef");
        StartTriggers = true;
    }



    void OnTriggerEnter(Collider other)
    {
        if (StartTriggers)
        {
            if (other.tag == ChefTag && Openable)
            {
                OpenDoor();
                Openable = false;
                StartCoroutine(DoorCooldown());
            }
        }
        
    }


    private IEnumerator DoorCooldown()
    {
        yield return new WaitForSeconds(10f);
        Openable = true;
        CloseDoor();
    }






    //PickUpItems will call this function when the player interacts with the door
    public void OnInteract()
    {
        //if the door is open, call the function to close it
        if (isOpen)
        {
            CloseDoor();
        }
        else
        //if the code reaches this point, the door must be closed. Call the function to open it
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        //keeps track of the door being open
        isOpen = true;

        //start the animation that opens the door
        animator.SetBool("IsOpen", true);

        //plays the door opening
        if (DoorAudio != null && DoorOpenSound != null)
        {
            DoorAudio.clip = DoorOpenSound;
            DoorAudio.Play();
        }
    }
    void CloseDoor()
    {
        //keeps track of the door being closed
        isOpen = false;

        //starts the animation that closes the door
        animator.SetBool("IsOpen", false);

        if (DoorAudio != null && DoorOpenSound != null)
        {
            DoorAudio.clip = DoorOpenSound;
            DoorAudio.Play();
        }
    }
}
