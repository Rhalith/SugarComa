using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public Selection selection;
    public Platform platform;

    private void Awake()
    {
        if (_instance != null && _instance != this)  Destroy(gameObject);
        else  _instance = this;
    }

    [System.Serializable]
    public class Selection
    {
        public Material greenMaterial;
        public Material redMaterial;
    }

    [System.Serializable]
    public class Platform
    {
        public Material goldMaterial;
        public Material healMaterial;
    }
}
