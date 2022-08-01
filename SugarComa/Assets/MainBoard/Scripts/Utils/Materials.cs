using UnityEngine;

[System.Serializable]
public class SelectionMaterial
{
    public Material greenMaterial;
    public Material redMaterial;
    public Mesh selectedMesh;
    public Mesh nonselectedMesh;
}

[System.Serializable]
public class PlatformTexture
{
    public Texture texture;
}