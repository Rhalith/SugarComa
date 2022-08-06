using UnityEngine;

public enum RouteSelectorDirection
{
    None,
    Left,
    Right,
}

[System.Serializable]
public record RouteSelector
{
    public GameObject left;
    public GameObject right;

    public void SetMaterial(RouteSelectorDirection direction, Material material, Mesh mesh)
    {
        switch (direction)
        {
            case RouteSelectorDirection.Left: 
                left.GetComponent<Renderer>().material = material;
                left.GetComponent<MeshFilter>().mesh = mesh;
                break;
            case RouteSelectorDirection.Right: 
                right.GetComponent<Renderer>().material = material;
                right.GetComponent<MeshFilter>().mesh = mesh;
                break;
        }
    }

    public bool HasSelector => left != null && right != null;
}
