using UnityEngine;

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
}
