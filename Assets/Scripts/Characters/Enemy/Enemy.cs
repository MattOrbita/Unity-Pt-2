using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] bool isActive;

    public void ActivateEnemy(bool setActive)
    {
        isActive = setActive;
    }

    void Update()
    {
        if (isActive)
        {
            FollowPlayer();
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
