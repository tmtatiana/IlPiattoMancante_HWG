using UnityEngine;

public class KitchenDoor : MonoBehaviour
{
    //stores the door's collider
    public Collider doorCollider;

    //stores the animator component
    private Animator animator;

    //bool variable for tracking whether or not the door has been unlocked
    private bool isUnlocked = false;

    //this is where the audio variable is stored
    public AudioClip DoorOpenSound;
    private AudioSource DoorAudio;
    void Start()
    {
        //obtains the animator component and stores it in the correct variable
        animator = GetComponentInParent<Animator>();

        //storing audio references
        DoorAudio = GetComponent<AudioSource>();
        DoorAudio.playOnAwake = false;
    }

    //this function is written to be called by PickUpItems for door opening functionality
    public void TryUnlock(InventoryManager inventory)
    {
        //this checks if the door is open already, and returns if this is the case
        if (isUnlocked)
        {
            return;
        }

        //this will obtain the active item in the inventory
        GameObject activeItem = inventory.GetActiveItem();
        //if there is no active item, return
        if (activeItem == null)
        {
            return;
        }
        //this checks to see if the item in the active slot is the key.
        //it can tell by looking for a DoorKey component that only the key will have.
        DoorKey key = activeItem.GetComponent<DoorKey>();
        //if the active item is not the key, return
        if (key == null)
        {
            return;
        }
        //if the code reaches this point, the active item must be the key.
        //deletes the key and opens the door
        inventory.RemoveItemFromActiveSlot();
        Destroy(activeItem);
        OpenDoor();
    }

    void OpenDoor()
    {
        //keeps track of the door being unlocked
        isUnlocked = true;

        //starts the animation for opening the door by setting IsOpen to true
        animator.SetBool("IsOpen", true);

        //disables the door's collider to allow the player to walk through once the door begins swinging open
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }
        
        //plays the door opening
        if (DoorAudio != null && DoorOpenSound != null)
        {
            DoorAudio.clip = DoorOpenSound;
            DoorAudio.Play();
        }
    }
}