using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assigment25
{


    public class spinController : MonoBehaviour
    {
        float yRotaion = 0;
        public bool Invers = false;
        public float speed = 10f;
        void Start()
        {

        }

        // Update is called once per frame
        private void FixedUpdate()
        {

            if (Invers)
                yRotaion -= speed * Time.deltaTime;
            else yRotaion += speed * Time.deltaTime;
            transform.Rotate(new Vector3(0, yRotaion, 0), Space.Self);

        }
    }
}