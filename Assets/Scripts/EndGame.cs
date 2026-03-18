using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    //the name of WinScreen room
    public string WinScreen;

    public void OnPickedUp()
    {
        SceneManager.LoadScene(WinScreen);
    }
}
