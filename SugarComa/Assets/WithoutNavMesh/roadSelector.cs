using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roadSelector : MonoBehaviour
{
    [SerializeField] Movement movement;
    [SerializeField] Router routerLeft, routerRight;
    [SerializeField] MeshRenderer leftSelecter, rightSelecter;
    [SerializeField] Material greenMaterial, redMaterial;
    bool playerIn;
    private void Update()
    {
        if (playerIn)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                leftSelecter.material = greenMaterial;
                rightSelecter.material = redMaterial;
                movement.currentRouter = routerLeft;
                movement.routePosition = 0;
                movement.isSelecting = false;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                rightSelecter.material = greenMaterial;
                leftSelecter.material = redMaterial;
                movement.currentRouter = routerRight;
                movement.routePosition = 0;
                movement.isSelecting = false;
            }
            if (movement.routePosition == 0 && Input.GetKeyDown(KeyCode.Y))
            {
                movement.startMove();
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!movement.overrideSelection)
            {
                Debug.Log("girdi"); playerIn = true; movement.isSelecting = true; movement.steps--;

            }
        }
    }
}
