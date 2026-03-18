using UnityEngine;
using UnityEngine.InputSystem;

public class AnimPlayer : MonoBehaviour
{
    PlayerController moveScript;
    Animator playerAnimator;
    bool moves = false;
    //Vector2 initialPos;
    //Vector2 newPos;
    //InputAction movement;

    void Start()
    {
        // Reference scripts
        playerAnimator = GetComponent<Animator>();
        moveScript = GetComponent<PlayerController>();
        // Do not have the player start in a walk
        playerAnimator.SetBool("moving", moves);
    }
    void Update()
    {
        PrimativeInputReader();
    }

    void PrimativeInputReader()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) 
            || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            Debug.Log("Player hitting movement keys");
            moves = true;
        } else
        {
            moves = false;
        }
        
        playerAnimator.SetBool("moving", moves);
        return;
    }

    /* Also did not work
    //Checks if there is translation input and disables/enables Animator accordingly
    public void ChecksForMovement(Vector3 movePlayer)
    {
        float moveMagnitude = movePlayer.magnitude;
        
        // If data is being stored in the movement
        if (moveMagnitude != 0f)
        {
            // There is a degree of movement recorded
            Debug.Log("Movement recorded");
            moves = true;
        }
        else
        {
            Debug.Log("No movement recorded");
            moves = false;
        }

        playerAnimator.SetBool("moving", moves);
        return;
    }
    */

    /* Dysfunctional
    void Update()
    {
        // Update InputAction reference
        //movement = moveScript.moveAction;
        //Debug.Log("InputAction should be stored");

        // Call method to measure input
        moves = ChecksForMovement(movement);
        playerAnimator.SetBool("moving", moves);
    }

    //Checks if there is translation input and disables/enables Animator accordingly
    bool CheckingMovement(InputAction indicator)
    {
        // If data is being stored in the movement
        if (indicator != null)
        {
            // if the input action indicator is not empty the player is moving the character
            Debug.Log("Indicator is not null");
            return moves = true;
        }
        else
        {
            Debug.Log("Indicator is null");
            return moves = false;
        }
        
        
    }
    */

    /* Old version
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Reference component
        moveScript = GetComponent<PlayerController>();
        playerAnimator = GetComponent<Animator>();
        playerAnimator.SetBool("moving", moves);
        initialPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        playerAnimator.SetBool("moving", moves);
        newPos = transform.position;
        moves = CheckIfMoved(initialPos, newPos);
        //idk if i have to reassign here or if the initial assignment works but jic
        playerAnimator.SetBool("moving", moves);
    }
    // Checking if player position has changed
    bool CheckIfMoved(Vector3 initial, Vector3 newSpot)
    {
        // Comparing the player's new pos to their old pos
        float distanceMoved = Mathf.Abs(Vector3.Distance(newSpot, initial));

        // If the player has not moved significantly the walking animation should not be triggered
        if (newSpot == null || initial == null)
        {
            Debug.Log("Failed position storage");
            return false;
        }
        // significant movement
        else if (distanceMoved > 0.01)
        {
            return moves = true;
        }
        // insignificant movement
        else
        {
            return moves = false;
        }
    }
    */

}
