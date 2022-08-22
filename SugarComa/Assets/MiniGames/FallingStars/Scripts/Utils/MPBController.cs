using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPBController : MonoBehaviour
{
    [SerializeField] private Renderer flameRenderer;
    private MaterialPropertyBlock _propertyBlock = null;

    private void Awake()
    {
        _propertyBlock = new();
    }

    public void ChangeMaterial(float value)
    {
        _propertyBlock.SetFloat("_SurfaceInputs", value);
    }
}
