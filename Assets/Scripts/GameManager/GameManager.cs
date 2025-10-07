using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;

    [Header("General")]
    [SerializeField] Vector2 spawnPoint;

    [Header("Timers")]
    [SerializeField] float enemyActivationDelay;
    [SerializeField] float fadeDuration;

    [Header("References")]
    [SerializeField] LevelExit levelExit;
    [SerializeField] List<Enemy> enemies;

    public void LoadLevel(string newLevel)
    {
        StartCoroutine(LoadLevelHelper(newLevel));
    }
    IEnumerator LoadLevelHelper(string newLevel)
    {
        InGameUI inGameUI = InGameUI.Singleton;

        // let's fade out completely
        inGameUI.SetBlackScreenVisible(true);

        // let's wait for the fade to actually start
        while (!inGameUI.IsFadeInProgress())
        {
            yield return null;
        }

        // once it starts, let's wait for it to finish
        while (inGameUI.IsFadeInProgress())
        {
            yield return null;
        }

        // THEN we load the new level
        SceneManager.LoadScene(newLevel);
    }

    void Awake()
    {
        Singleton = this;
    }

    void Start()
    {
        InGameUI.Singleton.SetBlackScreenVisible(false);

        GetNecessaryReferences();
        TeleportPlayerToSpawn();

        Invoke("ActivateEnemies", enemyActivationDelay);
    }

    void GetNecessaryReferences()
    {
        // get references to all enemies
        Enemy[] enemies = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        this.enemies.AddRange(enemies);
        
        // get reference to level exit
        levelExit = GameObject.FindAnyObjectByType<LevelExit>();
    }

    void TeleportPlayerToSpawn()
    {
        Vector3 spawnPoint3D = new Vector3(spawnPoint.x, spawnPoint.y, 0);
        Player.Singleton.transform.position = spawnPoint3D;
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
