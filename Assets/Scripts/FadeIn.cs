using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    // Referencing child's canvas component that houses an alpha value
    public CanvasGroup screenCol;

    // Variables to store keyframe alpha values
    public float startAlpha = 1f;
    public float endAlpha = 0f;
    
    // Variables tracking time spent in function
    public float fadeDuration = 1.20f;
    public float elapsedTime = 0f;


    // When enabled the image transparency should gradually decrease to zero
    private void OnEnable()
    {
        StartCoroutine(FadeIntoScene());
    }


    // Coroutine to gradually decrease the alpha
    IEnumerator FadeIntoScene()
    {
        
        // Continuously decrease the alpha during the duration of the fading in seq
        while (elapsedTime  < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            screenCol.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime/fadeDuration);
            yield return null;
        }
    }

}
