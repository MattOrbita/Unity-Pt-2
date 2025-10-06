using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float fadeDuration;
    [SerializeField] Vector2 spawnPoint;
    [SerializeField] LevelExit levelExit;

    [Header("Enemy Settings")]
    [SerializeField] float enemyActivationDelay;
    [SerializeField] List<Enemy> enemies;

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

        Enemy[] enemies = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        this.enemies.AddRange(enemies);
        
        levelExit = GameObject.FindAnyObjectByType<LevelExit>();

        Vector3 spawnPoint3D = new Vector3(spawnPoint.x, spawnPoint.y, 0);
        Player.Singleton.transform.position = spawnPoint3D;

        Invoke("ActivateEnemies", enemyActivationDelay);
    }

    void ActivateEnemies()
    {
        // activate them all
        foreach (Enemy enemy in enemies)
        {
            enemy.ActivateEnemy(true);
        }
    }

    void Update()
    {
        OpenExitUponClearingEnemies();
    }

    void OpenExitUponClearingEnemies()
    {
        // once enemies have been cleared, we don't need to be checking this anymore
        if (enemies.Count == 0)
        {
            return;
        }

        // let's get rid of any null (AKA newly-killed) enemies
        foreach (Enemy enemy in enemies)
        {
            if (enemy == null)
            {
                enemies.Remove(enemy);
                break;
            }
        }

        // if all enemies have JUST been cleared, then let's open the exit
        if (enemies.Count == 0)
        {
            levelExit.OpenExit(true);
        }
    }
}
