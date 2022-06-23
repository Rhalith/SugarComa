using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZortTest : MonoBehaviour
{
    [SerializeField] Texture _material;
    Renderer renderer;
    Mesh mesh;
    void Start()
    {
        renderer = GetComponent<Renderer>();
        for (int i = 0; i < renderer.sharedMaterials.Length; i++)
        {
            renderer.sharedMaterials[i].mainTexture = _material;
        }
    }

}
