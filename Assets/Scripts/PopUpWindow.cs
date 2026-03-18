using System.Collections;
using UnityEngine;

public class PopupWindow : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float showTime = 3f;
    public float fadeDuration = 1.5f;

    void Start()
    {
        StartCoroutine(ShowAndFade());
    }

    IEnumerator ShowAndFade()
    {
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(showTime);

        float timer = 0;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }
}
