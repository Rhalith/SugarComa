using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healPlatformAnimation : MonoBehaviour
{
    [SerializeField] List<GameObject> particles;

    public void ChangeTheParticles(string isOff = "off")
    {
        foreach (var item in particles)
        {
            if (isOff.Equals("off")) item.SetActive(false);
            else item.SetActive(true);
        }
    }
}
