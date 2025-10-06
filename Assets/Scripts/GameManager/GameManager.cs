using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Vector2 spawnPoint;

    Coroutine fadeCoroutine;

    public void LoadLevel(string newLevel)
    {
        StartCoroutine(LoadLevelHelper(newLevel));
    }
    IEnumerator LoadLevelHelper(string newLevel)
    {
        fadeCoroutine = StartCoroutine(PerformFade(false));

        // we just wait until the fade out finishes
        while (fadeCoroutine != null)
        {
            yield return null;
        }

        // THEN we load the new level
        SceneManager.LoadScene(newLevel);
    }

    IEnumerator PerformFade(bool fadeIn)
    {
        yield return null;

        fadeCoroutine = null;
    }

    void Start()
    {
        StartCoroutine(PerformFade(true));

        Vector3 spawnPoint3D = new Vector3(spawnPoint.x, spawnPoint.y, 0);
        Player.Singleton.transform.position = spawnPoint3D;
    }
}
