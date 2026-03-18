using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpItems : MonoBehaviour
{
    //determines how far the player's reach is
    public float pickupReach = 3f;

    //creates a reference to the main camera so the script shoots the raycast from the right place
    public Camera playerCamera;

    //determines how far from the player the throwable spawns, to prevent collision with player
    public float throwableSpawnDistance = 1f;

    private InventoryManager inventory;
    private InputAction interactAction;
    private InputAction throwAction;

    public AudioClip pickupSound;
    private AudioSource pickupAudio;

    public AudioClip throwSound;

    void Awake()
    {
        inventory = GetComponent<InventoryManager>();

        //sets controls for item pickup and door interaction
        interactAction = new InputAction(binding: "<Mouse>/rightButton", interactions: "press");
        interactAction.AddBinding("<Gamepad>/leftTrigger").WithInteraction("press");
        interactAction.performed += _ => TryPickup();
        interactAction.Enable();

        //sets controls for throwing items
        throwAction = new InputAction(binding: "<Mouse>/leftButton", interactions: "press");
        throwAction.AddBinding("<Gamepad>/rightTrigger").WithInteraction("press");
        throwAction.performed += _ => ThrowItem();
        throwAction.Enable();

        pickupAudio = GetComponent<AudioSource>();
        pickupAudio.playOnAwake = false;
    }

    //disables and disposes the action upon destruction to prevent memory leaks
    void OnDestroy()
    {
        interactAction.Disable();
        interactAction.Dispose();
        throwAction.Disable();
        throwAction.Dispose();
    }

    //function that determines whether to throw an item or try to pickup an item
    void HandleInteraction()
    {
        //checks to see if there is an item in the active inventory slot
        if (inventory.GetActiveItem() != null)
        {
            //this means there is an item in the active inventory slot. Function called to throw item
            ThrowItem();
        }
        else
        {
            //this means there is not an item in the active inventory slot. Function to attempt pickup is called
            TryPickup();
        }
    }
            //this function will attempt to pick up an item in the direction the crosshair is facing
    void TryPickup()
        {
            //fires a ray outward into the scene from the direct center point of the screen
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;

            //checks to see if the ray hit anything within pickup reach
            if (!Physics.Raycast(ray, out hit, pickupReach))
            {
                //if this reads as true, it means that no object were within reach. exits the function
                return;
            }

            ToggleDoor toggleDoor = hit.collider.GetComponentInParent<ToggleDoor>();
            if (toggleDoor != null)
            {
                toggleDoor.OnInteract();
                return;
            }

            KitchenDoor door = hit.collider.GetComponentInParent<KitchenDoor>();

            if (door != null)
            {
                door.TryUnlock(inventory);
                return;
            }


            SpaghettiAndMeatballs spaghettiAndMeatballs = hit.collider.GetComponent<SpaghettiAndMeatballs>();
            if (spaghettiAndMeatballs != null)
            {
                spaghettiAndMeatballs.OnInteract();
                PlayPickupSound();
                return;
            }

            //at this point, the ray must have hit an object within reach
            //now, it will check to see if the object has a CanBePickedUp component
            CanBePickedUp item = hit.collider.GetComponent<CanBePickedUp>();
            if (item == null)
            {
                //if this reads as true, that means the item does not have a CanBePickedUp component. Exits the function
                return;
            }

            //at this point, the item can be pickup up. Next, we will attempt to add the item to the first available inventory slot
            //we will exit early if the inventory is full
            bool added = inventory.AddItemToFirstAvailableSlot(item.gameObject);
            if (!added)
            {
                //if this part of the code reads as true, it means the inventory is full. Exits the function
                return;
            }

            //once the item is in an inventory slot, we will disable the Rigidbody physics of the item.
            //this is so it isn't simulated while the object exists only inside the inventory and not in the outside world.
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            //this next piece of code will hide the item from the scene once it is picked up
            item.gameObject.SetActive(false);

            PlayPickupSound();
                
            //this will check to see if the item is the MacGuffin, and if so, end the game
            EndGame macGuffin = item.GetComponent<EndGame>();
            if (macGuffin != null)
            {
                macGuffin.OnPickedUp();
            }

        }




    //this function will cause the item held to be thrown
    void ThrowItem()
    {
        //removes the item from the inventory slot and makes the slot available for a new item
        GameObject itemObject = inventory.RemoveItemFromActiveSlot();
        if (itemObject == null)
        {
            return;
        }
        //finds the item's CanBePickedUp component in order to read the throwForce value
        CanBePickedUp itemData = itemObject.GetComponent<CanBePickedUp>();

        //places the item in front of the camera before re-enabling it, to prevent collision conflict
        itemObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward * throwableSpawnDistance;

        //resets the item's rotation to a neutral rotation angle so it doesn't appear at a strange orientation
        itemObject.transform.rotation = Quaternion.identity;

        //reactivates the object
        itemObject.SetActive(true);

        //re-enables the object's physics now that it is back in the game world
        Rigidbody rb = itemObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            //applies force in the direction the player is facing, to make the object move
            rb.AddForce(playerCamera.transform.forward * itemData.throwForce, ForceMode.Impulse);
        }

        if (pickupAudio != null && throwSound != null)
        {
            pickupAudio.PlayOneShot(throwSound);
        }
    }

    void PlayPickupSound()
    {
        if (pickupAudio != null && pickupSound != null)
        {
            pickupAudio.PlayOneShot(pickupSound);
        }
    }
}
