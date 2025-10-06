using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [SerializeField] bool isOpen;

    public void OpenExit(bool setOpen)
    {
        isOpen = setOpen;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpen || collision.tag != "Player")
        {
            return;
        }
        
        Debug.Log("2d");
    }
}
