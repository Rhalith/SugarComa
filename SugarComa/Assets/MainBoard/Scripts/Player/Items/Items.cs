using Assets.MainBoard.Scripts.Player.Movement;

namespace Assets.MainBoard.Scripts.Player.Items
{
    public interface IItems
    {
        public void UseItem();
    }

    public interface IDamageItems : IItems
    {
        /// <summary>
        /// Decreases player health.
        /// </summary>
        /// <param name="playerCollector"></param>
        public void DamageHealth(PlayerCollector playerCollector);
    }

    public interface IHealerItems : IItems
    {
        /// <summary>
        /// Increases player health.
        /// </summary>
        /// <param name="playerCollector"></param>
        public void HealerHealth(PlayerCollector playerCollector);
    }
}