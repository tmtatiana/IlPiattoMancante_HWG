using UnityEngine;

public class SpaghettiAndMeatballs : MonoBehaviour
{
    //stores the prefab for the meatball
    public GameObject meatballPrefab;
    //stores the prefab for the dish that has one less meatball, will be swapped to
    public GameObject secondDishPrefab;
    //references the player inventory so the meatball can be added to it
    public InventoryManager inventory;
    //keeps track of whether or not the player has taken a meatball from the dish.
    //will be used to prevent player from taking multiple meatballs from the same dish
    private bool meatballTaken = false;

    public void OnInteract()
    {
        //checks to see if a meatball has been taken from the dish yet
        if (meatballTaken)
        {
            //if the code reaches this point, it means the meatball was taken already. Return early
            return;
        }

        //creates the meatball, but sets it as inactive so it doesn't appear in the scene
        GameObject meatball = Instantiate(meatballPrefab);
        meatball.SetActive(false);

        //attempts to add the meatball to the inventory, and uses a bool variable to track whether or not that attempt was successfull
        bool added = inventory.AddItemToFirstAvailableSlot(meatball);

        //checks to see if the meatball was not added to the inventory
        if (!added)
        {
            //if the code reaches this point, that means the meatball wasn't added. Destroy the meatball and return early
            Destroy(meatball);
            return;
        }

        //disables the RigidBody on the meatball
        Rigidbody rb = meatball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        //sets meatballTaken to true so the player cannot interact with the spaghetti and meatballs again
        meatballTaken = true;

        //spawns the updated dish at the same position as the previous one
        Instantiate(secondDishPrefab, transform.position, transform.rotation);
        //destroys the old dish prefab in the scene
        Destroy(gameObject);
    }

}
