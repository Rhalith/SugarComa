using UnityEngine;

public class Platform : MonoBehaviour
{
    [HideInInspector] public Vector3 position; // game object position.

    [Tooltip("Özelliði yoksa boþ býrak!")]
    public PlatformSpecification specification;

    private PlatformSpecification myspec;

    private PlatformSpecSet _platformSpecSet;

    public bool isSelector;

    public RouteSelector selector;

    public bool HasSelector => selector != null && selector.HasSelector;

    private void Start()
    {
        myspec = specification;
        position = transform.position;
        SetSpec();
    }

    private void SetSpec()
    {
        _platformSpecSet = new PlatformSpecSet();
        _platformSpecSet.SetSpec(GetComponent<MeshFilter>(), GetComponent<Renderer>(), specification);

    }

    public void ResetSpec()
    {
        specification = myspec;
    }

    public PlatformSpecification GetPlatformSpec()
    {
        return myspec;
    }
}
