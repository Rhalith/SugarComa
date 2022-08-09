using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MiniGames.FallingStars.Scripts.Player.PlayerManaging
{
    public class ClassicMeteor : PlayerManager
    {
        public Coroutine _coroutine;
        private readonly PlayerManager _playerManager;

        public ClassicMeteor(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        public IEnumerator DamageToPlayer(float damage = 0)
        {
            while (_playerManager.PlayerSpec.Health > 0)
            {
                _playerManager.DamagePlayer(damage);
                yield return new WaitForSeconds(1f);
            }
        }
        public void StopIteration()
        {
            _playerManager.StopCoroutine(_coroutine);
        }
    }
}