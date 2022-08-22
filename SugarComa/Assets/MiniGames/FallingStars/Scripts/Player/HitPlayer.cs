using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Player
{
    public class HitPlayer : MonoBehaviour
    {
        [SerializeField] PlayerChecker _playerChecker;
        public void HitToPlayer()
        {
            foreach (PlayerManager player in _playerChecker.PlayerManagers)
            {
                player.GetHit(_playerChecker.CurrentPlayer.transform, player.transform);
            }
        }
    }
}