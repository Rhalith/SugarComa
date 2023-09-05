using Assets.MainBoard.Scripts.Utils.InventorySystem;
using Assets.MainBoard.Scripts.Utils.PlatformUtils;
using Assets.MainBoard.Scripts.Networking.Utils;
using Assets.MainBoard.Scripts.GameManaging;
using Assets.MainBoard.Scripts.Player.Items;
using Assets.MainBoard.Scripts.Player.Utils;
using Assets.MainBoard.Scripts.Networking;
using Assets.MainBoard.Scripts.Route;
using Assets.MainBoard.Scripts.UI;
using UnityEngine;
using Assets.MainBoard.Scripts.Networking.MainBoardNetworking;

namespace Assets.MainBoard.Scripts.Player.Movement
{
    public class PlayerCollector : MonoBehaviour
    {
        #region Properties
        public int health;
        public int gold;
        public int goblet;
        public bool isDead;
        #endregion

        #region SerializeFields
        [SerializeField] private GameController _gameController;
        [SerializeField] private GobletSelection _gobletSelection;
        [SerializeField] private PlayerInventory _playerInventory;
        [SerializeField] private ScriptKeeper _scriptKeeper;
        [SerializeField] Item item;
        #endregion

        #region Properties
        public GameController GameController { set => _gameController = value; }
        public ScriptKeeper ScriptKeeper { get => _scriptKeeper; }
        #endregion

        private void Start()
        {
            SendPlayerSpecUpdate();
        }

        public void CheckCurrentNode(Platform platform)
        {
            if(_scriptKeeper.playerStateContext.Running.CurrentStep > 0)
            {
                switch (platform.spec)
                {
                    case PlatformSpec.Goal: GobletSelection(); break;
                }
            }
            else
            {
                switch (platform.spec)
                {
                    case PlatformSpec.Gold: AddGold(Random.Range(5, 8)); break;
                    case PlatformSpec.Heal: AddHealth(); break;
                    case PlatformSpec.Gift: AddItem(); break;
                    case PlatformSpec.Jackpot: RandomJackpot(5); break;
                    case PlatformSpec.Goal: GobletSelection(); break;
                }
            }
            
        }

        /// <summary>
        /// Adding value to player's gold
        /// </summary>
        /// <param name="value"></param>
        void AddGold(int value)
        {
            gold += value;

            // Update player specs on other players
            SendPlayerSpecUpdate();

            _gameController.ChangeText();
        }

        /// <summary>
        /// Adding health, if value is null it will full the health.
        /// </summary>
        /// <param name="value"></param>
        void AddHealth(int value = 25)
        {
            health += value;
            if (health > 25)
            {
                health = 25;
            }
            // Update player specs on other players
            SendPlayerSpecUpdate();

            _gameController.ChangeText();
        }

        public void DamagePlayer(int value)
        {
            health -= value;
            if (health <= 0)
            {
                health = 0;
                KillPlayer();
            }

            // Update player specs on other players
            SendPlayerSpecUpdate();

            _gameController.ChangeText(null, _scriptKeeper);
        }

        private void SendPlayerSpecUpdate()
        {
            byte[] data = NetworkHelper.Serialize(new PlayerSpecNetworkData((byte)gold, (byte)health, (byte)goblet));
            SteamServerManager.Instance.SendingMessageToAll(data);
        }

        public void AddItem()
        {
            //int i = Random.Range(1, 11);
            int i = 8;
            switch (i)
            {
                case 1:
                    item.shield.OnAddItem();
                    break;
                case 2:
                    item.car.OnAddItem();
                    break;
                case 3:
                    // TODO: İsmi düzelt
                    item.sapan.OnAddItem();
                    break;
                case 4:
                    item.teleport.OnAddItem();
                    break;
                case 5:
                    item.bee.OnAddItem();
                    break;
                case 6:
                    item.magnet.OnAddItem();
                    break;
                case 7:
                    item.randombox.OnAddItem();
                    break;
                case 8:
                    item.boxgloves.OnAddItem();
                    break;
                case 9:
                    item.healbox.OnAddItem();
                    break;
                case 10:
                    item.hook.OnAddItem();
                    break;
                default:
                    break;
            }
            _gameController.ChangeInventory();
        }

        void RandomJackpot(int value)
        {
            int i = Random.Range(1, 4);
            switch (i)
            {
                case 1: AddItem(); break;
                case 2: AddGold(Random.Range(value, 8)); break;
                case 3: AddHealth(value); break;
            }
        }

        void KillPlayer()
        {
            _scriptKeeper.playerCamera.Priority = 3;

            // TODO: Fix it.
            //_playerAnimation.StartDeath();
            //ScriptKeeper._playerStateContext

            isDead = true;
        }

        void GobletSelection()
        {
            _gobletSelection.OpenGobletSelection();
        }

        [System.Serializable]
        public class Item
        {
            public ItemObject shield;
            public ItemObject car;
            public ItemObject sapan;
            public ItemObject teleport;
            public ItemObject bee;
            public ItemObject magnet;
            public ItemObject randombox;
            public ItemObject boxgloves;
            public ItemObject healbox;
            public ItemObject hook;
        }
    }
}