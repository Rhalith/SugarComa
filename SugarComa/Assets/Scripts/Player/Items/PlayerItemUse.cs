using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUse : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] ItemPool _itemPool;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] float speed;
    private Vector3 _rotationY;
    private bool isPosSet;
    private bool isRotSet;

    private void KeepPosition()
    {
        if (!isPosSet)
        {
            _rotationY = new Vector3(playerTransform.eulerAngles.x,playerTransform.eulerAngles.y, playerTransform.eulerAngles.z);
            isPosSet = true;
        }
    }

    private void FollowMouse()
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float hitdist;

        if(plane.Raycast(ray, out hitdist))
        {
            Vector3 targetpoint = ray.GetPoint(hitdist);
            Quaternion targetrotation = Quaternion.LookRotation(targetpoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, speed * Time.deltaTime);
        }
    }
    private void FixedUpdate()
    {
        if (ItemPool._isItemUsing && _playerInput.isMyTurn)
        {
            KeepPosition();
            FollowMouse();
            if (_playerInput.useMouseItem && !_itemPool._playerInventory.activeInHierarchy)
            {
                ItemUsing.BoxGlovesUsing = true;
                _itemPool.UseCurrentItem();
                _playerInput.useMouseItem = false;
                isRotSet = true;
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

}
