using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header("Fade In & Out")]
    [SerializeField] bool isBlackScreenVisible;
    [SerializeField] float fadeDuration;
    [SerializeField] Image blackScreen;

    // for fading in & out
    bool wasBlackScreenVisible;
    Coroutine fadeCoroutine;

    void Update()
    {
        PerformFade();
    }

    void PerformFade()
    {
        if (isBlackScreenVisible != wasBlackScreenVisible)
        {
            // if we're already mid-fade, then let's cut that off first
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(PerformFadeHelper(isBlackScreenVisible));
        }

        wasBlackScreenVisible = isBlackScreenVisible;
    }

    IEnumerator PerformFadeHelper(bool fadeIn)
    {
        float timer = 0;

        // our starting color is whatever the color is AT THIS VERY MOMENT
        Color startColor = blackScreen.color;
        Color endColor = fadeIn ? Color.black : Color.clear;

        // we change from startColor to endColor over time
        while (timer < fadeDuration)
        {
            Color newColor = Color.Lerp(startColor, endColor, timer / fadeDuration);
            blackScreen.color = newColor;

            timer += Time.deltaTime;
            yield return null;
        }

        // finalize the color of the black screen
        blackScreen.color = endColor;
        fadeCoroutine = null;
    }
}
