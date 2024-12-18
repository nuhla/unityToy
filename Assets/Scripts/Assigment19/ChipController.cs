using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipController : MonoBehaviour
{
    [SerializeField]
    private float _amplitude = 0.008f;

    [SerializeField]
    private float _speed = 1.44f;

    private float loaclPos = 0;
    private float StartPos = 0;
    private float localRotation = 0;
  
    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.localPosition.y;
        localRotation = transform.localRotation.x;
        Debug.Log(localRotation);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private async void FixedUpdate()
    {


        loaclPos = StartPos + Mathf.Sin(Time.time * _speed) * _amplitude;
        transform.localPosition = new Vector3(transform.localPosition.x, loaclPos, transform.localPosition.z);

        float localRota = localRotation + Mathf.Cos(Time.time) * 200;

        transform.localRotation = Quaternion.Euler(new Vector3(Mathf.Deg2Rad * localRota, transform.rotation.y, transform.localPosition.z));




    }
}

