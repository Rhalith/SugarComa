using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDForBoxGloves : MonoBehaviour
{
    [SerializeField] GameObject current;
    RagdollOnOff ragdollOnOff;

    private void Start()
    {
        ragdollOnOff = current.GetComponent<RagdollOnOff>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        var rdof = collision.gameObject.GetComponent<RagdollOnOff>();
        if (rdof != null && rdof != ragdollOnOff)
        {

            //ÇALIÞMIYOR BÝR TÜRLÜ :(
            //Vector3 direction = new Vector3(0, current.transform.eulerAngles.y, 0);
            //print(direction);
            //collision.gameObject.GetComponent<Rigidbody>().AddForce(direction * 800f);
            //print(collision.gameObject.GetComponent<Rigidbody>());


            //rdof.RagDollOn();
        }
    }
}
