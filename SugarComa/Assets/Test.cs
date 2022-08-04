using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private bool isIn;
    private void OnEnable()
    {
        StartCoroutine(enumerator());
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Plane"))
        {
            isIn = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    IEnumerator enumerator()
    {
        float i = 0;
        while (!isIn)
        {
            yield return new WaitForSeconds(0.01f);
            i++;
            print(i);
        }
    }
}
