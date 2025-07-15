using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Tests
{
    public class FloaterBehavior : MonoBehaviour
    {
        [SerializeField] private LayerMask _waterLayer;
        [SerializeField] private float _speedUpLimit = 1.15f;
        [SerializeField] private float _speedUp = 1f;
        [SerializeField] private Rigidbody _rigidbody;

        private void OnValidate()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == _waterLayer)
            {
                float difference = (other.transform.position.y - transform.position.y) * _speedUp;

                Vector3 force = new Vector3(0f, Mathf.Clamp((Mathf.Abs(Physics.gravity.y) * difference), 0, Mathf.Abs(Physics.gravity.y) * _speedUpLimit), 0f);

                _rigidbody.AddForce(force, ForceMode.Acceleration);
                _rigidbody.linearDamping = 0.99f;
                _rigidbody.angularDamping = 0.8f;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == _waterLayer)
            {
                _rigidbody.linearDamping = 0f;
                _rigidbody.angularDamping = 0f;
            }
        }
    }
}
