using UnityEngine;

public class Platform : MonoBehaviour
{
    [HideInInspector] public Vector3 position; // game object position.

    [Tooltip("�zelli�i yoksa bo� b�rak!")]
    public PlatformSpec spec;

    [Tooltip("Sand�k nereye do�ru gelecekse oray� se�!")]
    public PlatformGoalSpec goalSpec;

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

    public void ActivateMeshRenderer(bool isActive)
    {
        if (isActive) gameObject.GetComponent<MeshRenderer>().enabled = true;
        else gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void SetSelectorMaterials(RouteSelectorDirection direction)
    {
        switch (direction)
        {
            case RouteSelectorDirection.Left:
                selector.SetMaterial(RouteSelectorDirection.Left, GameManager.SelectionMaterial.greenMaterial, GameManager.SelectionMaterial.selectedMesh);
                selector.SetMaterial(RouteSelectorDirection.Right, GameManager.SelectionMaterial.redMaterial, GameManager.SelectionMaterial.nonselectedMesh);
                break;
            case RouteSelectorDirection.Right:
                selector.SetMaterial(RouteSelectorDirection.Right, GameManager.SelectionMaterial.greenMaterial, GameManager.SelectionMaterial.selectedMesh);
                selector.SetMaterial(RouteSelectorDirection.Left, GameManager.SelectionMaterial.redMaterial, GameManager.SelectionMaterial.nonselectedMesh);
                break;
            default:
                selector.SetMaterial(RouteSelectorDirection.Left, GameManager.SelectionMaterial.redMaterial, GameManager.SelectionMaterial.nonselectedMesh);
                selector.SetMaterial(RouteSelectorDirection.Right, GameManager.SelectionMaterial.redMaterial, GameManager.SelectionMaterial.nonselectedMesh);
                break;
        }
    }
}
