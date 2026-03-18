using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour
{
    // Referencing child's canvas component that houses an alpha value
    public CanvasGroup screenCol;

    // Variables to store keyframe alpha values
    public float startAlpha = 0f;
    public float endAlpha = 1f;

    // Variables tracking time spent in function
    public float fadeDuration = 3.8f;
    public float elapsedTime = 0f;


    // When enabled the image transparency should gradually decrease to zero
    private void OnEnable()
    {
        StartCoroutine(FadeOutOfScene());
    }


    // Coroutine to gradually decrease the alpha
    IEnumerator FadeOutOfScene()
    {

        // Continuously decrease the alpha during the duration of the fading in seq
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            Debug.Log("elapsedTime = "+ elapsedTime);
            screenCol.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            yield return null;
        }
    }
}
