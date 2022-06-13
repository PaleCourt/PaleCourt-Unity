using UnityEngine;

namespace FiveKnights
{
    public class Mace2 : MonoBehaviour
    {
        public float LaunchSpeed = 50;
        public float SpinSpeed = 60;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _rb.velocity = Vector3.up * LaunchSpeed;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        private void FixedUpdate()
        {
            Vector3 rot = transform.rotation.eulerAngles;
            rot.z += SpinSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }
    }
}