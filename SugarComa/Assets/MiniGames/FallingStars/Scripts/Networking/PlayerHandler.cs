using Steamworks;
using UnityEngine;
using System.Linq;
using Assets.MainBoard.Scripts.Networking;
using Assets.MiniGames.FallingStars.Scripts.Player;

namespace Assets.MiniGames.FallingStars.Scripts.Networking
{
    public class PlayerHandler : MonoBehaviour
    {
        private static PlayerHandler _instance;
        public static PlayerHandler Instance => _instance;

        #region SerializeFields
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _remotePlayerPrefab;
        [SerializeField] private GameObject _playerParent;
        [SerializeField] private Camera _mapCam;
        #endregion

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                DestroyImmediate(this);
                return;
            }

            _instance = this;
        }

        void Start()
        {
            var steamIds = SteamLobbyManager.Instance.inLobby.Keys.ToArray();
            
            foreach (var id in steamIds)
            {
                CreatePlayer(id);
            }
        }

        /// <summary>
        /// Creates player.
        /// </summary>
        public GameObject CreatePlayer(SteamId id)
        {
            GameObject createdObject;
            if (SteamManager.Instance.PlayerSteamId == id)
            {
                createdObject = Instantiate(_playerPrefab, _playerParent.transform);

                UpdateMapCam(createdObject);
            }
            else
            {
                createdObject = Instantiate(_remotePlayerPrefab, _playerParent.transform);
            }
            return createdObject;
        }

        void UpdateMapCam(GameObject playerObj)
        {
            playerObj.GetComponent<PlayerMovement>().MainCam = _mapCam;
        }
    }
}