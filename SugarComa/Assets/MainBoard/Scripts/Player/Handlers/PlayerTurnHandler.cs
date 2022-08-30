using System;
using Steamworks;
using UnityEngine;
using System.Linq;
using Assets.MainBoard.Scripts.Networking;
using Assets.MainBoard.Scripts.GameManaging;

namespace Assets.MainBoard.Scripts.Player.Handlers
{
    public static class PlayerTurnHandler
    {
        private static int _index = -1;
        private static SteamId[] _steamIds;
        private static GameObject[] _players;
        private static PlayerHandler _playerHandler;

        /// <summary>
        /// Index of the current player.
        /// </summary>
        public static int Index => _index;

        /// <summary>
        /// Count of the players.
        /// </summary>
        public static int PlayerCount => _players.Length;

        /// <summary>
        /// Players' Steam IDs
        /// </summary>
        public static SteamId[] SteamIds => _steamIds;

        /// <summary>
        /// Player list.
        /// </summary>
        public static GameObject[] Players => _players;

        /// <summary>
        /// Current player.
        /// </summary>
        public static GameObject Current => _players[_index];

        /// <summary>
        /// Previous player.
        /// </summary>
        public static GameObject Prev
        {
            get
            {
                int index = _index - 1;
                return index > 0 ? _players[index] : _players[^1];
            }
        }
        
        /// <summary>
        /// Next player.
        /// </summary>
        public static GameObject Next
        {
            get
            {
                int index = _index + 1;
                return index < _players.Length ? _players[index] : _players[0];
            }
        }

        /// <summary>
        /// Change current player to the next player.
        /// </summary>
        /// <returns><see cref="GameObject"/> Next player.</returns>
        public static GameObject NextPlayer()
        {
            int index = _index + 1;
            _index = index < _players.Length ? index : 0;
            return Current;
        }

        /// <summary>
        /// Change current player to the previous player.
        /// </summary>
        /// <returns><see cref="GameObject"/> Previous player.</returns>
        public static GameObject PrevPlayer()
        {
            int index = _index - 1;
            _index = index > 0 ? index : _players.Length - 1;
            return Current;
        }

        /// <summary>
        /// Manually assign current player.
        /// </summary>
        /// <param name="index"></param>
        /// <returns><see cref="GameObject"/> Current player.</returns>
        public static GameObject ChangeTurn(int index)
        {
            if (index < 0) _index = 0;
            else if (index > _players.Length) _index = _players.Length - 1;
            else _index = index;

            return Current;
        }

        /// <summary>
        /// Update players by steam ID.
        /// </summary>
        /// <param name="steamIds">Players' Steam IDs</param>
        public static void UpdatePlayers(SteamId[] steamIds)
        {
            // reset turn
            _index = 0;

            // if the length of given players is different from the player list length
            if (steamIds.Length != _steamIds.Length)
            {
                SortPlayers(steamIds);
                return;
            }

            int halfLength = steamIds.Length / 2 + 1;
            for (int i = 0; i < halfLength; ++i)
            {
                int index = -1;
                SteamId steamId = steamIds[i];
                GameObject gameObject = _players[index];

                for (int j = 0; j < steamIds.Length; j++)
                {
                    if (steamId == _steamIds[j])
                    {
                        index = j;
                        break;
                    }
                }
                
                if (index != -1)
                {
                    (_steamIds[index], _steamIds[i]) = (_steamIds[i], steamId);
                    (_players[index], _players[i]) = (_players[i], gameObject);
                }
            }
        }

        /// <summary>
        /// Create and spawn new player.
        /// </summary>
        /// <param name="steamIds">Players' Steam IDs</param>
        public static void SpawnPlayers(SteamId[] steamIds)
        {
            // set fields
            _index = 0;
            _steamIds = steamIds;
            _players = new GameObject[steamIds.Length];
            _playerHandler = NetworkManager.Instance.playerHandler;

            // create gameObjects
            for (int i = 0; i < steamIds.Length; ++i)
            {
                _players[i] = _playerHandler.CreatePlayer(steamIds[i]);
            }
        }

        /// <summary>
        /// If there is a player with the steam id in use, return the player otherwise return null.
        /// </summary>
        /// <param name="steamId">Player steam id.</param>
        /// <returns><see cref="GameObject"/> The Player.</returns>
        public static GameObject GetPlayer(SteamId steamId)
        {
            for (int i = 0; i < _steamIds.Length; i++)
                if (steamId == _steamIds[i])
                    return _players[i];
            return null;
        }

        /// <summary>
        /// Remove player from player list by give steam id.
        /// </summary>
        /// <param name="steamId">Player steam id.</param>
        public static void RemovePlayer(SteamId steamId)
        {
            int index = -1;
            for (int i = 0; i < _steamIds.Length; i++)
            {
                if (steamId == _steamIds[i])
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                _players = _players.Where((_, i) => i != index).ToArray();
                _steamIds = _steamIds.Where((_, i) => i != index).ToArray();
            }
        }

        private static void SortPlayers(SteamId[] steamIds)
        {
            GameObject[] players = new GameObject[steamIds.Length];
   
            for (int i = 0; i < steamIds.Length; ++i)
            {
                var steamId = steamIds[i];
                int index = Array.FindIndex(_steamIds, (id) => id == steamId);
                if (index != -1)
                {
                    players[i] = _players[index];
                }
            }

            _players = players;
            _steamIds = steamIds;
        }
    }
}