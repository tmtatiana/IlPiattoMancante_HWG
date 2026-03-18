using UnityEngine;
using UnityEngine.UI;

public class InventoryHUD : MonoBehaviour
{
    //references for each slot's UI. Use Inspector to assign each slot's root image
    public Image slot0;
    public Image slot1;
    public Image slot2;
    public Image slot3;

    //use Inspector to assign eachc slot's item image child
    public Image slot0ItemImage;
    public Image slot1ItemImage;
    public Image slot2ItemImage;
    public Image slot3ItemImage;

    //use these in the Inspector to assign each slot's highlight image child
    public Image slot0Highlight;
    public Image slot1Highlight;
    public Image slot2Highlight;
    public Image slot3Highlight;

    //these sprites will show in HUD. Assign the images in the inspector and the code will decide when to display which
    public Sprite meatballSprite;
    public Sprite keySprite;

    private InventoryManager inventory;
    private Image[] slots;
    private Image[] itemImages;
    private Image[] highlights;

    void Start()
    {
        inventory = Object.FindFirstObjectByType<InventoryManager>();

        //all the references will be stored in arrays that we can loop through in the code
        slots = new Image[] { slot0, slot1, slot2, slot3 };
        itemImages = new Image[] { slot0ItemImage, slot1ItemImage, slot2ItemImage, slot3ItemImage };
        highlights = new Image[] { slot0Highlight, slot1Highlight, slot2Highlight, slot3Highlight };

        //makes sure that the HUD Refreshes every time there is an inventory change
        inventory.OnInventoryChanged += RefreshHUD;

        //makes sure the highlight for the active item is refreshed whenever the player switches the active slot without picking up or throwing anything
        inventory.OnSlotChanged += RefreshHighlight;

        //runs both refreshes initially to make sure the HUD is accurate when the scene first loads
        RefreshHUD();
        RefreshHighlight(inventory.GetActiveSlot());
    }

    void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged -= RefreshHUD;
            inventory.OnSlotChanged -= RefreshHighlight;
        }
    }

    void RefreshHUD()
    {
        for (int i = 0; i < InventoryManager.InventoryCapacity; i++)
        {
            GameObject item = inventory.GetItemInSlot(i);

            if (item != null)
            {
                //this code will get the corresponding sprite by checking which component the item has and then assign it to the slot's icon
                Sprite sprite = GetSpriteForItem(item);
                itemImages[i].sprite = sprite;
                itemImages[i].enabled = sprite != null;
            }
            else
            {
                //if the slot is empty the item icon should be hidden
                itemImages[i].sprite = null;
                itemImages[i].enabled = false;
            }
        }
        //refreshes the highlight after updating the item icons just in case the active slot's contents changed
        RefreshHighlight(inventory.GetActiveSlot());
    }

    void RefreshHighlight(int activeSlot)
    {
        for (int i = 0; i < InventoryManager.InventoryCapacity; i++)
        {
            //make sure the highlight is showing for the currently active slot
            highlights[i].enabled = (i == activeSlot);
        }
    }

    Sprite GetSpriteForItem(GameObject item)
    {
        //checks which component the item has to determine which sprite needs to be displayed in the HUD slot
        if (item.GetComponent<DoorKey>() != null) return keySprite;
        if (item.GetComponent<Meatball>() != null) return meatballSprite;
        //returns null if no matching sprite is found
        return null;
    }
}
