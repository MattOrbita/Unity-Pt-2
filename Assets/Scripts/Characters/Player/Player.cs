using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Singleton;

    [SerializeField] float moveSpeed;
    [SerializeField] float shootDistance;
    [SerializeField] bool isShootHeldDown;
    [SerializeField] GameObject shootLinePrefab;

    InputAction moveAction;
    InputAction shootAction;

    void Awake()
    {
        Singleton = this; // not really a proper use of a singleton lol
    }

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        shootAction = InputSystem.actions.FindAction("Attack");
    }

    void Update()
    {
        Move();
        Shoot();
    }

    void Move()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(moveInput.x, moveInput.y, 0);

        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }

    void Shoot()
    {
        bool isShooting = shootAction.IsPressed();

        if (isShooting)
        {
            // if shoot is being held down, then we don't shoot over and over again
            if (isShootHeldDown)
            {
                return;
            }

            isShootHeldDown = true;

            // otherwise, let's shoot!
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, shootDistance);

            // create a shoot line!
            Vector3 endpoint;

            // if we hit something, then the endpoint of this line is whatever we hit; otherwise, the line
            // just extends as far as our shoot distance!
            if (hit.distance > Mathf.Epsilon)
            {
                endpoint = new Vector3(hit.point.x, hit.point.y, 0);
            }
            else
            {
                endpoint = transform.position + transform.up * shootDistance;
            }

            GameObject shootLine = Instantiate(shootLinePrefab);
            shootLine.GetComponent<ShootLine>().SetStartAndEnd(transform.position, endpoint);
        }

        // if shoot is not being pressed, then we unset isShootHeldDown, allowing player to shoot again!
        else
        {
            isShootHeldDown = false;
        }
    }
}
