using UnityEngine;

public class BoxGloves : MonoBehaviour, IDamageItems
{
    #region SerializeFields
    [SerializeField] ItemObject _itemObject;
    [SerializeField] GameObject _player;
    [SerializeField] int damage;
    [SerializeField] PlayerMovement _playerMovement;
    #endregion

    private PlayerCollector otherPlayersCollector;

    public bool isHitPlayer;

    public void DamageHealth(PlayerCollector playerCollector)
    {
        if(otherPlayersCollector != null) playerCollector.health -= damage;
    }

    public void UseItem()
    {
        if (isHitPlayer)
        {
            DamageHealth(otherPlayersCollector);
        }
        _itemObject.RemoveItem();
        _playerMovement.GameController.ChangeInventory();
        _playerMovement.isUserInterfaceActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject != _player)
        {
            isHitPlayer = true;
            otherPlayersCollector = other.GetComponent<PlayerCollector>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other != _player)
        {
            isHitPlayer = false; otherPlayersCollector = null;
        }
    }
}
