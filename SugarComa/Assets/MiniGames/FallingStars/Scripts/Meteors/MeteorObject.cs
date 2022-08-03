using System.Collections;
using System.Collections.Generic;
using Assets.MiniGames.FallingStars.Scripts.Meteors;
using Unity.VisualScripting;
using UnityEngine;

public class MeteorObject : MonoBehaviour
{
    private Meteor _meteor;
    void OnEnable()
    {
        _meteor = transform.parent.gameObject.GetComponent<Meteor>();
    }

    void OnCollisionEnter(Collision collision)
    {
        _meteor.CheckHit(collision, null);
    }

    void OnTriggerEnter(Collider collider)
    {
        _meteor.CheckHit(null, collider);
    }
}
