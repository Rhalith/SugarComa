using Assets.MainBoard.Scripts.Player.Utils;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Player.Items.BoxGloves
{
    public class RDForBoxGloves : MonoBehaviour
    {
        [SerializeField] RagdollOnOff current;
        [SerializeField] float impactForce = 40f;
        private void OnCollisionEnter(Collision collision)
        {
            print(collision.gameObject.name);
            RagdollOnOff rdof = collision.gameObject.GetComponent<RagdollOnOff>();
            if (rdof != null && rdof != current)
            {
                rdof.RagDollOn();
                Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

                foreach (Collider nearby in colliders)
                {
                    Rigidbody rig = nearby.GetComponent<Rigidbody>();
                    if (rig != null)
                    {
                        rig.AddForce(current.transform.forward * impactForce, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}