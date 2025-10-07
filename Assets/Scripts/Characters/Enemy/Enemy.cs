using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float moveSpeed;
    [SerializeField] Animator animator;

    [Header("Waypoints")]
    [SerializeField] float targetProximity;
    [SerializeField] float pauseDuration;
    [SerializeField] int targetWaypointIndex = 0;
    [SerializeField] List<Transform> waypoints;

    Coroutine pauseCoroutine;

    [Header("States")]
    [SerializeField] bool isActive;
    [SerializeField] bool isPatrolling;

    public void ActivateEnemy(bool setActive)
    {
        isActive = setActive;
    }

    void Start()
    {
        // start enemy off in idle animation
        animator.SetBool("isWalking", false);
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

        // go towards current target waypoint, unless we're pausing
        if (pauseCoroutine == null)
        {
            Transform targetWaypoint = waypoints[targetWaypointIndex];
            MoveToTarget(targetWaypoint.position);

            // once the enemy reaches the current waypoint, we pause, then move on to the next waypoint
            if (Vector3.Distance(transform.position, targetWaypoint.position) < targetProximity)
            {
                targetWaypointIndex++;

                if (targetWaypointIndex >= waypoints.Count)
                {
                    targetWaypointIndex = 0;
                }

                pauseCoroutine = StartCoroutine(DoPatrolPause());
            }
        }
    }

    IEnumerator DoPatrolPause()
    {
        animator.SetBool("isWalking", false);

        yield return new WaitForSeconds(pauseDuration);

        pauseCoroutine = null;
    }

    void MoveToTarget(Vector3 target)
    {
        // activate walking animation
        animator.SetBool("isWalking", true);

        // face the target
        Vector3 moveDirection = target - transform.position;
        moveDirection.Normalize();

        transform.up = -moveDirection;

        // finally, MOVE towards target!
        transform.position += Time.deltaTime * moveSpeed * moveDirection;
    }

    void FollowPlayer()
    {
        Vector3 playerPosition = Player.Singleton.transform.position;
        MoveToTarget(playerPosition);
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
