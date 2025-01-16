
using UnityEngine;
namespace Assigment37
{
    public class CharachterController : MonoBehaviour
    {

        private CapsuleCollider capsuleCollider;
        private Rigidbody rigidbody;
        private float currentRotation;
        private Quaternion targetRotation;
        private Vector3 Vol;

        [SerializeField]
        private float rotval;

        [SerializeField]
        private float somthspeed = 0.5f;
        [SerializeField]
        private float speed = 12;
        [SerializeField]
        PhysicMaterial charm;
        private bool Jump;
        void Start()
        {
            try
            {
                capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
                capsuleCollider.material = charm;
                rigidbody = gameObject.GetComponent<Rigidbody>();
            }
            catch
            {
                capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
                capsuleCollider.material = charm;
                rigidbody = gameObject.AddComponent<Rigidbody>();

            }
            //----------------------------- RidgitBody Settings -------------//

            rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        }

        // Update is called once per frame
        void Update()
        {


            Vol = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            if (Input.GetKeyDown(KeyCode.Space)) Jump = true;



        }

        private void FixedUpdate()
        {
            if (Jump)
            {

                rigidbody.AddForce(new Vector3(rigidbody.velocity.x, 10, rigidbody.velocity.z), ForceMode.Impulse);
                Jump = false;
            }

            // -------------------------Normlize Vector -----------------------//
            Vol = Vol.normalized * speed;
            Vol.y = rigidbody.velocity.y;


            // ------------------- Move The Charchter ---------------------------//
            rigidbody.velocity = Vol;
            Vector3 directionRotation = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);


            // ------------------- Rotate The Charchter ---------------------------//
            if (directionRotation != Vector3.zero)
            {
                Quaternion forwardsRotation = Quaternion.LookRotation(directionRotation);
                rigidbody.MoveRotation(forwardsRotation);
            }





        }
    }
}