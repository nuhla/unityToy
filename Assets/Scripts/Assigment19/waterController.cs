using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class waterController : MonoBehaviour
{
    [SerializeField]
    private Transform[] deops;
    float loaclPos = 0;
    private float[] StartPos;


    [SerializeField]
    private int delay = 10;
    [SerializeField]
    private float _amplitude = 0.006f;

    [SerializeField]
    private float _speed = .015f;

    // Start is called before the first frame update
    void Start()
    {

        StartPos = new float[deops.Length];
        Debug.Log(deops.Length);

        for (int i = 0; i < deops.Length; i++)
        {
            StartPos[i] = deops[i].localPosition.y;
            Debug.Log(StartPos[i]);
            Debug.Log(deops[i].localPosition.y);
        }

    }

    private async void FixedUpdate()
    {
        int count = 0;
        foreach (Transform yAxis in deops)
        {
            loaclPos = StartPos[count] + Mathf.Cos(Time.time * _speed) * _amplitude;
            yAxis.localPosition = new Vector3(yAxis.localPosition.x, loaclPos, yAxis.localPosition.z);
          
            ++count;
           



        }
    }

    // Update is called once per frame
    void Update()
    {




        //deops[1].localPosition = new Vector3(0, StartPos[0] + loaclPos, 0);

        foreach (Transform trans in deops)
        {
            loaclPos = Mathf.Cos(Time.time) * _amplitude;
            //            trans.localPosition = new Vector3(0, StartPos[0] + loaclPos, 0);
            // Debug.Log(trans.localPosition.y);
        }

    }
}
