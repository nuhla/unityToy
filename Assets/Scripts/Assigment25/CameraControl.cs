using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Unity.Mathematics;
using Unity.VisualScripting;
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
        private bool lightOpen = false;
        private float time = 60;
        float counter = 0;


        // Start is called before the first frame update
        void Start()
        {
            //finalRotation = Quaternion.Euler(new Vector3(transform.position.x, 44, transform.position.z));
            StartRotate = true;
            StartCoroutine(StopForAseconds());


        }

        // Update is called once per frame
        void Update()
        {
            RoateCamera();



        }


        private IEnumerator StopForAseconds()
        {
            yield return new WaitForSeconds(14);
            float timeounter = 3.2f;
            float countercounter = 0;

            if (StartRotate)
            {
                yield break;
            }
            while (countercounter < timeounter)
            {
                countercounter += Time.deltaTime;
                Debug.Log($"Counter : {countercounter}  Time : {timeounter}");


                transform.RotateAround(book.transform.position, Vector3.down, speed * Time.deltaTime);
                transform.LookAt(book.transform, Vector3.up);
                yield return null;
            }


            StartRotate = false;
        }

        private async void RoateCamera()
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
                lightOpen = true;


            }


        }




    }
}
