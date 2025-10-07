using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float moveSpeed;

    [Header("Waypoints")]
    [SerializeField] float targetProximity;
    [SerializeField] int targetWaypointIndex = 0;
    [SerializeField] List<Transform> waypoints;

    [Header("States")]
    [SerializeField] bool isActive;
    [SerializeField] bool isPatrolling;

    public void ActivateEnemy(bool setActive)
    {
        isActive = setActive;
    }

    void Update()
    {
        if (isActive)
        {
            if (isPatrolling)
            {
                DoPatrolRoutine();
            }
            else
            {
                FollowPlayer();
            }
        }
    }

    void DoPatrolRoutine()
    {
        // can't do a patrol routine if there are no waypoints to begin with
        if (waypoints.Count == 0)
        {
            return;
        }

        // go towards current target waypoint
        Transform targetWaypoint = waypoints[targetWaypointIndex];

        Vector3 moveDirection = targetWaypoint.position - transform.position;
        moveDirection.Normalize();

        transform.position += Time.deltaTime * moveSpeed * moveDirection;

        // once the enemy reaches the current waypoint, we move on to the NEXT waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < targetProximity)
        {
            targetWaypointIndex++;

            if (targetWaypointIndex >= waypoints.Count)
            {
                targetWaypointIndex = 0;
            }
        }
    }

    void FollowPlayer()
    {
        Vector3 playerPosition = Player.Singleton.transform.position;
        Vector3 moveDirection = playerPosition - transform.position;
        moveDirection.Normalize();

        transform.position += Time.deltaTime * moveSpeed * moveDirection;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        RestartLevel();
    }

    void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
