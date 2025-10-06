using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float moveSpeed;

    [Header("Waypoints")]
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

        Vector3 fromVector = Vector3.right;
        int oldAngle = (int) Vector3.Angle(fromVector, moveDirection);

        moveDirection.Normalize();
        // Mathf.RoundToInt();

        
        // float oldAngle = Vector3.Angle(fromVector, moveDirection);

        transform.position += Time.deltaTime * moveSpeed * moveDirection;

        // once the angle of the vector from enemy to waypoint changes, that means the enemy has PASSED
        // their waypoint, meaning it's time to switch to the next waypoint!
        float newAngle = Vector3.Angle(fromVector, targetWaypoint.position - transform.position);

        bool isAtWaypoint = Vector3.Distance(transform.position, targetWaypoint.position) < Mathf.Epsilon;
        // bool hasPassedWaypoint = ;
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
