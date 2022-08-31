using UnityEngine.EventSystems;
using UnityEngine;

namespace Assets.MainBoard.Scripts.UI
{
    public class EnlargeUIButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        Vector3 cachedScale;

        void Start()
        {

            cachedScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {

            transform.localScale *= 1.25f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {

            transform.localScale = cachedScale;
        }
    }
}