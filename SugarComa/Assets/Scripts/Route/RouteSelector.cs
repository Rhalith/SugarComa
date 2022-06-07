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

    public void SetMaterial(RouteSelectorDirection direction, Material material)
    {
        switch (direction)
        {
            case RouteSelectorDirection.Left: left.GetComponent<Renderer>().material = material; break;
            case RouteSelectorDirection.Right: right.GetComponent<Renderer>().material = material; break;
        }
    }

    public bool HasSelector => left != null && right != null;
}
