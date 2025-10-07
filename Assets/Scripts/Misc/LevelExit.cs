using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [SerializeField] bool isOpen;
    [SerializeField] string nextLevelName;

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

        GameManager.Singleton.LoadLevel(nextLevelName);
    }
}
