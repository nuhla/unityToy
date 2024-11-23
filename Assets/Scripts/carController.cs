using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carController : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 3f;
    public float rorationspeed =20f;

    public bool isMoving = false;

    public GameObject care;


    void Start()
    {
        if (care == null)
        {
            care = GameObject.Find("Car");
            if (care != null)
            {
                Debug.Log("found");
                //transform.transform.localRotation= Quaternion.identity;

            }
        }

    }

    // Update is called once per frame
    private void FixedUpdate()
    {



        float horizantal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float virtical = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        Vector3 movment = new Vector3(horizantal,0, virtical).normalized;
        transform.Translate(movment * speed * Time.deltaTime, Space.World);

        if (movment == Vector3.zero)
        {
            isMoving= false;
            return;
        }else{
            isMoving= true;
           //transform.forward = movment;
           Quaternion toRotation = Quaternion.LookRotation(movment, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, .09f);
        }
       
    }


}
