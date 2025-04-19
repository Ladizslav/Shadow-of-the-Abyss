using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance;

    public int collectiblesNeeded = 10;
    public int collectedCollectibles { get; private set; } 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectCollectible()
    {
        collectedCollectibles++;

        if (collectedCollectibles >= collectiblesNeeded)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("CollectorWin");
        }
    }
}
