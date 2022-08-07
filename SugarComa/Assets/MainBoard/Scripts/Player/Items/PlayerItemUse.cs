using Assets.MainBoard.Scripts.Player.Movement;
using Assets.MainBoard.Scripts.Player.States;
using Assets.MainBoard.Scripts.Utils.InventorySystem;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Player.Items
{
    public class PlayerItemUse : MonoBehaviour
    {
        [SerializeField] Transform playerTransform;
        [SerializeField] ItemPool _itemPool;
        [SerializeField] private PlayerStateContext _playerStateContext;
        [SerializeField] float speed;
        private Vector3 _rotationY;
        private bool isPosSet;
        private bool isRotSet;

        /// <summary>
        /// Keep first position before using item.
        /// </summary>
        private void KeepPosition()
        {
            if (!isPosSet)
            {
                _rotationY = new Vector3(playerTransform.eulerAngles.x, playerTransform.eulerAngles.y, playerTransform.eulerAngles.z);
                isPosSet = true;
            }
        }

        private void FollowMouse()
        {
            Plane plane = new(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (plane.Raycast(ray, out float hitdist))
            {
                Vector3 targetpoint = ray.GetPoint(hitdist);
                Quaternion targetrotation = Quaternion.LookRotation(targetpoint - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, speed * Time.deltaTime);
            }
        }
        private void FixedUpdate()
        {
            if (ItemPool._isItemUsing && _playerStateContext.isMyTurn)
            {
                if (ItemUsing.BoxGlovesUsing)
                {
                    OnBoxGlovesUsing();
                }
                //TODO
                else
                {

                }
            }
            //else if(!ItemPool._isItemUsing && !ItemUsing.BoxGlovesUsing && isRotSet)
            //{
            //    if (isRotSet)
            //    {
            //        StartCoroutine(rotation());
            //        isRotSet = false;
            //    }
            //    //playerTransform.eulerAngles = _rotationY;
            //    isPosSet =false;
            //}
        }

        //TODO fix it
        //private IEnumerator rotation()
        //{
        //    while (true)
        //    {
        //        transform.Rotate(_rotationY * Time.deltaTime * 15f);
        //        if (playerTransform.eulerAngles.y - _rotationY.y < 3f && playerTransform.eulerAngles.y - _rotationY.y > -3f)
        //        {
        //            StopAllCoroutines();
        //        }

        //        yield return new WaitForSeconds(0.01f);
        //    }

        //}
        private void OnBoxGlovesUsing()
        {
            KeepPosition();
            FollowMouse();
            if (_playerStateContext.UseMouseItem && !_itemPool._playerInventory.activeInHierarchy)
            {
                ItemUsing.BoxGlovesUsing = true;
                _itemPool.UseCurrentItem();
                _playerStateContext.UseMouseItem = false;
                isRotSet = true;
            }
        }
    }
}