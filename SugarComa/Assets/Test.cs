using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private bool isIn;
    float i = 0;
    private void Update()
    {
        if (!isIn)
        {
            i++;
            print(i);
        }

    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Plane"))
        {
            isIn = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
