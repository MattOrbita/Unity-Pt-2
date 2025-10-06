using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Singleton;

    [SerializeField] float moveSpeed;

    InputAction moveAction;

    void Awake()
    {
        Singleton = this; // not really a proper use of a singleton lol
    }

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(moveInput.x, moveInput.y, 0);
        
        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }
}
