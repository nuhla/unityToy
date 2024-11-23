using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace CareToyDriving
{
    public class WeelControler : MonoBehaviour
    {
        // Start is called before the first frame update

        public GameObject weel;
        private GameObject care;
        private float speed = 2.5f;
        private bool ismoving = false;
        public bool isMovingBackwords = false;
      

        private float yRotation= 180;


        float newRotaion;
        // float rightRotationpostion;

        Quaternion initialRotation;
        private void MoveForward()
        {
            if (ismoving)
            {
                newRotaion -= 90*speed * Time.deltaTime;
                weel.transform.rotation = Quaternion.Euler(new Vector3(newRotaion, yRotation, 270));
                

            }
        }

        private void Movebackwords()
        {
            if (isMovingBackwords)
            {
                newRotaion += 90* speed * Time.deltaTime;
                weel.transform.rotation = Quaternion.Euler(new Vector3(newRotaion, yRotation, 270));


            }
        }


   

        private void OnEnable()
        {
            care = GameObject.Find("Car");
        }
        void Start()
        {

           
           // rightRotationpostion = transform.eulerAngles.y;

        }

        // Update is called once per frame
        void Update()
        {
           

            if(Input.GetKey(KeyCode.RightArrow)){
                yRotation=270;
            }
            if(Input.GetKey(KeyCode.UpArrow)){
                yRotation = 180;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                yRotation = 180;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                yRotation = 270;
            }

            ismoving = care.GetComponent<carController>().isMoving;

           

            /// ------------------ Update Functionality controls the moving ---------------//
           
            MoveForward();
            Movebackwords();



        }
    }
}
