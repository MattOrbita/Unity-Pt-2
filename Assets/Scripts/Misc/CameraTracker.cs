using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    [SerializeField] Vector3 cameraOffset;

    void Update()
    {
        MoveCameraToPlayer();
    }

    void MoveCameraToPlayer()
    {
        Camera.main.transform.position = transform.position + cameraOffset;
    }
}
