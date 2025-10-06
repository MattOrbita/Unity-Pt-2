using System.Collections;
using UnityEngine;

public class ShootLine : MonoBehaviour
{
    [Header("Timers")]
    [SerializeField] float startFadeOutAfter;
    [SerializeField] float fadeDuration;

    [Header("Positions")]
    [SerializeField] Vector3 startPosition;
    [SerializeField] Vector3 endPosition;

    [Header("References")]
    [SerializeField] LineRenderer lineRenderer;

    public void SetStartAndEnd(Vector3 start, Vector3 end)
    {
        startPosition = start;
        endPosition = end;
    }

    void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        // let's first wait until we're ACTUALLY supposed to fade out
        yield return new WaitForSeconds(startFadeOutAfter);

        // perform actual fade out
        float timer = 0;

        Color startColor = lineRenderer.startColor;
        Color endColor = Color.clear;

        while (timer < fadeDuration)
        {
            Color newColor = Color.Lerp(startColor, endColor, timer / fadeDuration);

            lineRenderer.startColor = newColor;
            lineRenderer.endColor = newColor;

            timer += Time.deltaTime;
            yield return null;
        }

        // finally, delete self
        Destroy(gameObject);
    }

    void Update()
    {
        UpdateStartAndEnd();
    }

    void UpdateStartAndEnd()
    {
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
    }
}
