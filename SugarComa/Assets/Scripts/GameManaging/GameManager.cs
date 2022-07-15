using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private bool _isGameOver;
    private float _totalGameTime;
    [SerializeField] private SelectionMaterial _selectionMaterial;
    [SerializeField] private PlatformTexture _platformTexture;
    [SerializeField] private GoldMeshes _goldMeshes;
    [SerializeField] private HealMeshes _healMeshes;
    [SerializeField] private RandomBoxMeshes _randomBoxMeshes;
    [SerializeField] private TrapMeshes _trapMeshes;
    [SerializeField] private JackpotMeshes _jackpotMeshes;
    [SerializeField] private GoalObject _goalObject;

    public static PlatformTexture PlatformTexture
    {
        get
        {
            if (_instance == null) return null;
            return _instance._platformTexture;
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

    public static GoldMeshes GoldMeshes
    {
        get
        {
            if (_instance == null) return null;
            return _instance._goldMeshes;
        }
    }
    public static HealMeshes HealMeshes
    {
        get
        {
            if (_instance == null) return null;
            return _instance._healMeshes;
        }
    }
    public static RandomBoxMeshes RandomBoxMeshes
    {
        get
        {
            if (_instance == null) return null;
            return _instance._randomBoxMeshes;
        }
    }

    public static TrapMeshes TrapMeshes
    {
        get
        {
            if (_instance == null) return null;
            return _instance._trapMeshes;
        }
    }

    public static JackpotMeshes JackpotMeshes
    {
        get
        {
            if (_instance == null) return null;
            return _instance._jackpotMeshes;
        }
    }

    public static GoalObject GoalObject
    {
        get
        {
            if (_instance == null) return null;
            return _instance._goalObject;
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