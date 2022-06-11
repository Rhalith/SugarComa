using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private bool _isGameOver;
    private float _totalGameTime;
    [SerializeField] private SelectionMaterial _selectionMaterial;
    [SerializeField] private PlatformMaterial _platformMaterial;

    public static PlatformMaterial PlatformMaterial
    {
        get
        {
            if (_instance == null) return null;
            return _instance._platformMaterial;
        }
    }

    public static SelectionMaterial SelectionMaterial
    {
        get
        {
            if (_instance == null) return null;
            return _instance._selectionMaterial;
        }
    }

    public static bool IsGameOver
    {
        get
        {
            if (_instance == null) return false;
            return _instance._isGameOver;
        }
    }

    public static float TotalGameTime
    {
        get
        {
            if (_instance == null) return 0f;
            return _instance._totalGameTime;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (_isGameOver) return;

        _totalGameTime += Time.deltaTime;
    }
}