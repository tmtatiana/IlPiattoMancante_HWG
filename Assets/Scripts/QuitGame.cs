using UnityEngine;
using UnityEngine.InputSystem;

public class QuitGame : MonoBehaviour
{
    private InputAction quit;

    void Awake()
    {
        quit = new InputAction(binding: "<Keyboard>/escape", interactions: "press");
        quit.performed += OnQuit;
        quit.Enable();

    }
    void OnDestroy()
    {
        quit.performed -= OnQuit;
        quit.Disable();
        quit.Dispose();
    }
    private void OnQuit(InputAction.CallbackContext context)
    {
        Application.Quit();
    }
}

