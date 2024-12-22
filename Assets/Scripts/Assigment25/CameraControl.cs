using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
namespace Assigment25
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField]
        private Transform book;
        private bool StartRotate = true;


        private float speed = 18;
        private Quaternion finalRotation;
        private float time = 60;
        float counter = 0;


        // Start is called before the first frame update
        void Start()
        {
            finalRotation = Quaternion.Euler(new Vector3(transform.position.x, 44, transform.position.z));



        }

        // Update is called once per frame
        void Update()
        {
            RoateCamera();
        }


        private void RoateCamera()
        {



            if (counter < time && StartRotate)
            {
                counter += Time.time / time;


                transform.RotateAround(book.transform.position, Vector3.down, speed * Time.deltaTime);
                transform.LookAt(book.transform, Vector3.up);
            }
            else
            {
                StartRotate = false;


            }

        }




    }
}
