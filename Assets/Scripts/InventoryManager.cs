using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InventoryManager : MonoBehaviour
{
    //sets the number of inventory slots and locks it in
    public const int InventoryCapacity = 4;
    //an array for items currently held in inventory
    private GameObject[] slots = new GameObject[InventoryCapacity];

    //variable to track the selected/currently active inventory slot

    private int activeSlot = 0;

    private InputAction slot1Action;
    private InputAction slot2Action;
    private InputAction slot3Action;
    private InputAction slot4Action;

    public event Action OnInventoryChanged;
    public event Action<int> OnSlotChanged;

    void Awake()
    {
        //creates an input action for each arrow key, to allow the player to be able to toggle between inventory slots
        slot1Action = new InputAction(binding: "<Keyboard>/leftArrow", interactions: "press");
        slot2Action = new InputAction(binding: "<Keyboard>/rightArrow", interactions: "press");
        slot3Action = new InputAction(binding: "<Keyboard>/upArrow", interactions: "press");
        slot4Action = new InputAction(binding: "<Keyboard>/downArrow", interactions: "press");

        //adds all of the Xbox controller D-pad buttons as keybinds for the inventory
        slot1Action.AddBinding("<Gamepad>/dpad/left").WithInteraction("press");
        slot2Action.AddBinding("<Gamepad>/dpad/right").WithInteraction("press");
        slot3Action.AddBinding("<Gamepad>/dpad/up").WithInteraction("press");
        slot4Action.AddBinding("<Gamepad>/dpad/down").WithInteraction("press");

        //prepares each inventory slot to be set as the active slot whenever the corresponding key is pressed
        slot1Action.performed += _ => SetActiveSlot(0);
        slot2Action.performed += _ => SetActiveSlot(1);
        slot3Action.performed += _ => SetActiveSlot(2);
        slot4Action.performed += _ => SetActiveSlot(3);

        slot1Action.Enable();
        slot2Action.Enable();
        slot3Action.Enable();
        slot4Action.Enable();
    }

    void OnDestroy()
    {
        //disables and disposes of each action to prevent memory leaks
        slot1Action.Disable(); slot1Action.Dispose();
        slot2Action.Disable(); slot2Action.Dispose();
        slot3Action.Disable(); slot3Action.Dispose();
        slot4Action.Disable(); slot4Action.Dispose();
    }

    //this function sets the active slot to be whatever index the function is given
    public void SetActiveSlot(int index)
    {
        activeSlot = index;
        OnSlotChanged?.Invoke(activeSlot);
    }

    //this function returns the index of whichever slot is active

    public int GetActiveSlot()
    {
        return activeSlot;
    }

    //this function will try to place an item in the first available inventory slot, and return false if the inventory is full
    public bool AddItemToFirstAvailableSlot(GameObject item)
    {
        //loops through every index in the inventory
        for (int i = 0; i < InventoryCapacity; i++)
        {
            //if the current inventory slot is empty
            if (slots[i] == null)
            {
                //assign the item in question to that inventory slot and return true
                slots[i] = item;
                OnInventoryChanged?.Invoke();
                return true;
            }
        }
        //if this part of the function is reached, that means every inventory slot was full. Returns false
        return false;
    }

    //this function will remove and return the item in the slot that is currently active
    public GameObject RemoveItemFromActiveSlot()
    {
        GameObject item = slots[activeSlot];

        //if there is no item in the active slot, return null
        if (item == null)
        {
            return null;
        }

        //clears the slot and returns the item so it can be used afterwards
        slots[activeSlot] = null;
        OnInventoryChanged?.Invoke();
        return item;
    }

    //this function will return the item in the currently active slot
    public GameObject GetActiveItem()
    {
        return slots[activeSlot];
    }

    //this function will check if every inventory slot is full, and will return true if every slot is full
    public bool IsFull()
    {
        //loops through every index in the inventory array
        for (int i = 0; i < InventoryCapacity; i++)
        {
            //returns false if an item is not found in a slot
            if (slots[i] == null) return false;
        }
        //if the code reaches this point, every inventory slot must be full. Returns true
        return true;
    }

    public GameObject GetItemInSlot(int index)
    {
        return slots[index];
    }
}
