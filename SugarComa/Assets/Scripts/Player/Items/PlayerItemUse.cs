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
        KeepPosition();
        if (ItemPool._isItemUsing && _playerInput.isMyTurn)
        {
            FollowMouse();
            if (_playerInput.useMouseItem && !_itemPool._playerInventory.activeInHierarchy)
            {
                _itemPool.UseCurrentItem();
                _itemPool.CloseItem();
            }
        }
        else
        {
            playerTransform.eulerAngles = _rotationY;
            isPosSet=false;
        }
    }

}
