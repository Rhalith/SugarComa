using UnityEngine;

public class Platform : MonoBehaviour
{
    [HideInInspector] public Vector3 position; // game object position.

    [Tooltip("Özelliði yoksa boþ býrak!")]
    public PlatformSpec spec;

    private PlatformSpec myspec;

    private PlatformSpecSet _platformSpecSet;

    public RouteSelector selector;

    public bool HasSelector => selector != null && selector.HasSelector;

    private void Start()
    {
        myspec = spec;
        position = transform.position;
        SetSpec();
    }

    private void SetSpec()
    {
        _platformSpecSet = new PlatformSpecSet();
        _platformSpecSet.SetSpec(GetComponent<MeshFilter>(), GetComponent<Renderer>(), spec);
    }

    public void ResetSpec()
    {
        spec = myspec;
    }

    public PlatformSpec GetPlatformSpec()
    {
        return myspec;
    }

    public void SetSelectorMaterials(RouteSelectorDirection direction)
    {
        switch (direction)
        {
            case RouteSelectorDirection.Left:
                selector.SetMaterial(RouteSelectorDirection.Left, GameManager.SelectionMaterial.greenMaterial);
                selector.SetMaterial(RouteSelectorDirection.Right, GameManager.SelectionMaterial.redMaterial);
                break;
            case RouteSelectorDirection.Right:
                selector.SetMaterial(RouteSelectorDirection.Right, GameManager.SelectionMaterial.greenMaterial);
                selector.SetMaterial(RouteSelectorDirection.Left, GameManager.SelectionMaterial.redMaterial);
                break;
            default:
                selector.SetMaterial(RouteSelectorDirection.Left, GameManager.SelectionMaterial.redMaterial);
                selector.SetMaterial(RouteSelectorDirection.Right, GameManager.SelectionMaterial.redMaterial);
                break;
        }
    }
}
