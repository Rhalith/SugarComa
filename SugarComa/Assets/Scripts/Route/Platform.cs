using UnityEngine;

public class Platform : MonoBehaviour
{
    [HideInInspector] public Vector3 position; // game object position.

    [Tooltip("Özelliði yoksa boþ býrak!")]
    public PlatformSpecification specification;

    [HideInInspector] public PlatformSpecification myspec;

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
        var renderer = GetComponent<Renderer>();

        switch (specification)
        {
            case PlatformSpecification.Selection:
                //TODO
                break;
            case PlatformSpecification.Gold: renderer.material = GameManager.PlatformMaterial.goldMaterial; break;
            case PlatformSpecification.Heal: renderer.material = GameManager.PlatformMaterial.healMaterial; break;
            default:
                break;
        }
    }

    public void ResetSpec()
    {
        specification = myspec;
    }
}
