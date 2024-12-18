using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private Transform book;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        Quaternion z = Quaternion.Euler(new Vector3(0, 9, 0));
        if (transform.rotation.y > z.y)
        {

            transform.RotateAround(book.transform.position, Vector3.up, 20 * Time.deltaTime);
            transform.LookAt(book.transform, Vector3.up);
        }

        Debug.Log($"{transform.rotation.y} + -- + {z.y}");
    }
}
