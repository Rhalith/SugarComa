using UnityEngine;

public class BoxGloves : MonoBehaviour, IDamageItems
{
    #region SerializeFields
    [SerializeField] ItemPool _itemPool;
    [SerializeField] ItemObject _itemObject;
    [SerializeField] GameObject _player;
    [SerializeField] int damage;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] BoxGlovesAnimation _boxGlovesAnimation;
    [SerializeField] Animator _playerAnimator;
    #endregion

    private PlayerCollector otherPlayersCollector;

    public GameObject _hitBox;

    public bool isHitPlayer;

    public void DamageHealth(PlayerCollector playerCollector)
    {
        if(otherPlayersCollector != null) playerCollector.DamagePlayer(damage);
    }

    /// <summary>
    /// Use box gloves.
    /// </summary>
    public void UseItem()
    {
        _hitBox.SetActive(false);
        _boxGlovesAnimation.HitAnimation();
    }
    /// <summary>
    /// Triggers to player taking box gloves animation. Using on boks eldiveni button in User Interface gameObject.
    /// </summary>
    public void TakeGlovesToPlayer()
    {
        ItemUsing.BoxGlovesUsing = true;
        _playerAnimator.SetBool("boks", true);
    }

    /// <summary>
    /// If player is in hitbox damage it and removes item from inventory. 
    /// </summary>
    public void TakeGlovesFromPlayer()
    {
        if (isHitPlayer)
        {
            DamageHealth(otherPlayersCollector);
        }
        _itemObject.RemoveItem();
        _playerMovement.GameController.ChangeInventory();
        _playerMovement.isUserInterfaceActive = false;
        _itemPool.CloseItem();
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
