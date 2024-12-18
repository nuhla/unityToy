using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
namespace Assigment25
{

    public class BookController : MonoBehaviour
    {

        [SerializeField] Transform pageOne;

        [SerializeField] Transform PageTow;


        // Start is called before the first frame update
        void Start()
        {


        }

        // Update is called once per frame
        void Update()
        {

        }

        // IEnumerator RotatePage()
        // {
        //     float speed = .1f;
        //     while (pageOne.rotation.y < 180)
        //     {

        //         pageOne.rotation = Quaternion.Slerp(pageOne.rotation, Quaternion.Euler(0,0,180), speed * Time.deltaTime);
        //     }
        // }

        // void Start()
        // {
        //     Quaternion rotation2 = Quaternion.Euler(new Vector3(0, 0, 90));
        //     StartCoroutine(rotateObject(objectToRotate, rotation2, 3f));
        // }


        // Let's say we want to make the rotation (0,0,90) from whatever the current rotation is.
        //  The code below will change the rotation to 0,0,90 in 3 seconds.

        // bool rotating = false;
        // public GameObject objectToRotate;
        // IEnumerator rotateObject(GameObject gameObjectToMove, Quaternion newRot, float duration)
        // {
        //     if (rotating)
        //     {
        //         yield break;
        //     }
        //     rotating = true;

        //     Quaternion currentRot = gameObjectToMove.transform.rotation;

        //     float counter = 0;
        //     while (counter < duration)
        //     {
        //         counter += Time.deltaTime;
        //         gameObjectToMove.transform.rotation = Quaternion.Lerp(currentRot, newRot, counter / duration);
        //         yield return null;
        //     }
        //     rotating = false;
        // }

    }
}