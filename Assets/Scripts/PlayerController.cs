using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //allows movement speed, mouse sensitivity, and look limit to be altered in inspector
    public float movementSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float xboxControllerSensitivity = 150f;
    public float lookLimit = 80f;

    private CharacterController controller;
    private Transform Camera;
    private float verticalRotation = 0f;

    //input action references to be assigned in inspector - this will allow us to set keybinds for movement within Unity
    public InputAction moveAction;
    public InputAction lookAction;

    //initializes how far down the player will crouch, can be adjusted in inspector
    public float crouchDistance = 0.5f;

    //initializes variables for the original height, the camera's original y value, and whether or not the player is crouching
    private float originalHeight;
    private float originalCameraY;
    public bool isCrouching = false;

    //initializes input action for crouching
    private InputAction crouchAction;

    //initializes variables, camera, and LayerMask needed for crouching mechanic
    public float standUpCheck = 2f;
    private bool tryToStandUp = false;
    public LayerMask crouchMask = Physics.DefaultRaycastLayers;
    public float standUpCheckCastOriginOffset = 0.2f;
    public Camera playerCamera;

    //setup for player gravity
    public float gravity = -9.81f;
    public float checkGroundRadius = 0.3f;

    private float verticalVelocity = 0f;

    //this will setup the player to be able to crouch while the Shift key/left shoulder is held down

    //this is where the audio variable is stored
    public AudioClip CrouchSound;
    private AudioSource CrouchAudio;
    void Awake()
    {
        crouchAction = new InputAction(binding: "<Keyboard>/leftShift");
        crouchAction.AddBinding("<Gamepad>/leftShoulder");
        crouchAction.Enable();
    }

    //this will enable Input Actions so they can be read from
    void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        crouchAction.Enable();
    }
    //this will disable Input Actions when the object is disabled. This helps prevent memory leaks and prevents listeners from being active unnecessarily
    void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        crouchAction.Disable();
    }

    void Start()
    {
        //resets crouch state in the event that the player restarts the game
        isCrouching = false;
        tryToStandUp = false;

        //finds the CharacterController component attached to the GameObject
        controller = GetComponent<CharacterController>();
        //finds the correct child object and stores it in the Camera variable
        Camera = transform.GetChild(0);

        //locks the cursor to the center of the screen to prevent it from moving outside the game window
        Cursor.lockState = CursorLockMode.Locked;
        //hides the cursor
        Cursor.visible = false;

        //stores the original values for the player's height and the camera's y position so we can easily restore them later when the player is not crouching anymore
        originalHeight = controller.height;
        originalCameraY = Camera.localPosition.y;

        //storing audio references
        CrouchAudio = GetComponent<AudioSource>();
        CrouchAudio.playOnAwake = false;
        CrouchAudio.loop = false;
    }

    void Update()
        //calls two movement-related functions every frame
    {
        Movement();
        Look();

        //determines whether to call StartCrouch, set tryToStandUp to true, and/or call EndCrouch
        bool shiftHeld = crouchAction.ReadValue<float>() > 0.5f;

        if (shiftHeld && !isCrouching)
        {
            StartCrouch();
        }
        else if (!shiftHeld && isCrouching)
        {
            tryToStandUp = true;
        }
        if (isCrouching && tryToStandUp)
        {
            EndCrouch();
        }
    }
    void Movement()
    {
        //reads and stores input from current frame regarding movement in either direction
        Vector2 input = moveAction.ReadValue<Vector2>();

        //if the player is moving forwards, double the movement speed. Otherwise, move at the normal speed
        float currentSpeed = input.y > 0 ? movementSpeed * 2f : movementSpeed;
        //maps the movement correctly relative to the direction the player is currently facing.
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        //actually moves the player depending on the direction and speed of movement determined above
        controller.Move(move * currentSpeed * Time.deltaTime);

        //checks if the player is standing on something by using a sphere cast
        Vector3 feetPosition = transform.position - Vector3.up * (originalHeight / 2 - controller.radius);
        bool isStanding = Physics.CheckSphere(feetPosition, checkGroundRadius);

        if (isStanding && verticalVelocity < 0)
        {
            //resets vertical velocity when the player is standing on something to prevent it from accumulating unnecessarily
            verticalVelocity = -2f;
        }
        else
        {
            //applies gravity over time whenever the player is in the air
            verticalVelocity += gravity * Time.deltaTime;
        }
        //this code actually moves the player based upon the vertical velocity
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }
    void Look()
    {
        //reads the mouse movement since last frame and stores it
        Vector2 lookDelta = lookAction.ReadValue<Vector2>();

        //this code will check if the last device that provided input was a Gamepad, for the purpose of handling sensitivity differently if it is a Gamepad
        bool usingXboxController = Gamepad.current != null
            && lookAction.activeControl?.device is Gamepad;

        float scalar = usingXboxController ? Time.deltaTime : 1f;

        float sensitivity = usingXboxController ? xboxControllerSensitivity : mouseSensitivity;

        //scales those values read by the mouse sensitivity to control how fast or slow the mouse moves in response to the input
        float mouseX = lookDelta.x * sensitivity * scalar;
        float mouseY = lookDelta.y * sensitivity * scalar;

        //rotates the entire player left/right accordingly
        transform.Rotate(Vector3.up * mouseX);

        //ensures that the player looks up when they move the mouse up, and vice versa
        verticalRotation -= mouseY;
        //this clamps the vertical angle to prevent the player from rotating beyond straight up or straight down. Otherwise, they would be able to cause the camera to flip
        verticalRotation = Mathf.Clamp(verticalRotation, -lookLimit, lookLimit);
        //this makes sure that the clamped vertical rotation only applies to the camera and not the player's body rotation
        Camera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
    void StartCrouch()
    {
        //makes sure the player isn't already crouching before running the code
        if (isCrouching) return;
        //if the code reaches this point, that means the player isn't crouching yet. We now can set the isCrouching variable to true
        isCrouching = true;

        tryToStandUp = false;

        //lowers the CharacterController's height so the collider also lowers and the player will be able to fit under lower objects like a table
        controller.height -= crouchDistance;

        //shifts the collider's center up by half of the crouch distance
        //this is to keep it correctly centered on the player after changing the height.
        controller.center = new Vector3(controller.center.x, controller.center.y - (crouchDistance / 2), controller.center.z);

        //lowers the camera accordingly so the player's POV matches the height when crouching
        Camera.localPosition = new Vector3(Camera.localPosition.x, originalCameraY - crouchDistance, Camera.localPosition.z);

        if (CrouchAudio != null && CrouchSound != null)
        {
            CrouchAudio.clip = CrouchSound;
            CrouchAudio.loop = true;
            CrouchAudio.Play();
        }
    }

    void EndCrouch()
    {

        //makes sure the player is crouching before running the code
        if (!isCrouching) return;
        //if the code reaches this point, it means the player is crouching.

        //before allowing the player to stand up, we will cast a sphere upwards to check if there is anything above the player (such as a table)
        //that would prevent them from being able to stand up.

        Vector3 castOrigin = playerCamera.transform.position;

        if (Physics.SphereCast(castOrigin, controller.radius, Vector3.up, out RaycastHit hit, standUpCheck + standUpCheckCastOriginOffset, crouchMask))
        {
            //if the code reaches this point, it means that there is something above the player, so they shouldn't be able to stand yet.
            //returns from the function without letting the player stand back up
            return;
        }
        //if the code reaches this point, it means there is nothing above the player preventing them from standing.
        //We can now set the isCrouching variable to false
        isCrouching = false;

        //at this point, the player is no longer trying to stand up, so the variable can be set to false
        tryToStandUp = false;

        //restores the CharacterController's original height and center values
        controller.height = originalHeight;
        controller.center = new Vector3(controller.center.x, controller.center.y + (crouchDistance / 2), controller.center.z);

        //restores the camera to its original position
        Camera.localPosition = new Vector3(Camera.localPosition.x, originalCameraY, Camera.localPosition.z);

        if (CrouchAudio != null)
        {
            CrouchAudio.loop = false;
            CrouchAudio.Stop();
        }
    }
}