using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public GameObject car;
    private bool isMovingBackwords=false;
    private float newRotaion;
    private float speed =.1f;
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isMovingBackwords = car.GetComponent<carController>().isMoving;
        Debug.Log(isMovingBackwords+ "    ---   isMovingBackwords");

        if (isMovingBackwords)
        {
            newRotaion += 270 * speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(new Vector3(0, newRotaion, 0));


        }

    }
}
