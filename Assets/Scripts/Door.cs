using UnityEngine;

public class Door : MonoBehaviour
{
    public static Door Instance { get; private set; }

    [Header("Game Info")]
    [SerializeField] private int enemyCount;
    [SerializeField] private bool isDoorOpen;

    [Header("Debug Variables")]
    [SerializeField] private bool isGameActive = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    private void Start()
    {
        isDoorOpen = false;
        enemyCount = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length;
    }

    public void ActiveGame() { isGameActive = true; }
    public void PauseGame() { isGameActive = false; }
    public bool IsGameActive() { return isGameActive; }

    public int GetEnemyCount() { return enemyCount; }
    public void KillEnemy()
    {
        enemyCount--;
        if (enemyCount <= 0)
        {
            isDoorOpen = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isDoorOpen)
        {
            PauseGame();
            UIManager.Instance.ShowUI(UIManager.GameUI.Win);
            Debug.Log("You Win!");
        }
    }
}
