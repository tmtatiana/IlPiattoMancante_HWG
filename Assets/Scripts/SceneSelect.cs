using UnityEngine;

public class SceneSelect : MonoBehaviour
{
    public GameObject pauseMenu;

    public MonoBehaviour cameraLookScript;

    public bool isPaused;

    public void Unpause() //Unpauses Game while in pause screen
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPaused = false;

        cameraLookScript.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void SampleScene()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene"); //Changes scene to SampleScene
    }

    public void TitleScene()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen"); //Changes scene to TitleScene
    }

    public void CreditsScene()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("CreditsScreen"); //Changes scene to SampleScene
    }

    public void Endgame()
    {
        Time.timeScale = 1f;
        Application.Quit(); //used to quit game with a button press
    }

    // Load into cut scene
    public void CutScene()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("CutScene"); //Starts player at the cutscene after pressing start
    }

}
