using UnityEngine;

public class ExitCutScene : MonoBehaviour
{
    /* Adding SceneSelect script as a component to our GameplayLoader object
     * so we can call its scene management methods.
     * 
     * SCRAP * calling the SampleScene method stopped working so loading the
     * screen is written again within this script.
     */

    /* No longer used
    // Reference variable for SceneSelect
    //SceneSelect sceneModeScript;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //sceneModeScript = gameObject.GetComponent<SceneSelect>();
    }

    // Run sceneloader
    /* Nestled scene call into another function because for some reason
     * calling SamplScene in OnEnable started throwing errors
    
    void SceneLoader()
    {
        sceneModeScript.SampleScene();
        return;
    }

    */

    // Cutscene has ended load our gameplay scene
    void OnEnable()
    {
        Debug.Log("Entered OnEnable() to change to gameplay");
        //SceneLoader();
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    
}
