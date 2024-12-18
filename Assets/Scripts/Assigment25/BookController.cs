using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
namespace Assigment25
{

    public class BookController : MonoBehaviour
    {

        [SerializeField] GameObject pageOne;

        [SerializeField] Transform PageTow;



        bool rotating = false;


        // Start is called before the first frame update
        void Start()
        {
            if (pageOne == null)
            {
                pageOne = GameObject.Find("FirstPage");
            }


            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            StartCoroutine(rotateObject(pageOne, rotation, 5f, 8.5f));
        }

        // Update is called once per frame
        void Update()
        {


        }


        // Let's say we want to make the rotation (0,0,90) from whatever the current rotation is.
        //  The code below will change the rotation to 0,0,90 in 3 seconds.

        IEnumerator rotateObject(GameObject gameObjectToMove, Quaternion newRot, float duration, float waitSecounds)
        {

            yield return new WaitForSeconds(waitSecounds);
            if (rotating)
            {
                yield break;
            }
            rotating = true;

            Quaternion currentRot = gameObjectToMove.transform.rotation;

            float counter = 0;
            while (counter < duration)
            {
                counter += Time.deltaTime;
                gameObjectToMove.transform.localRotation = Quaternion.Lerp(currentRot, newRot, counter / duration);
                yield return null;
            }
            rotating = false;
        }

    }
}