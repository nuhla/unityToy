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
        public static float duration = 4f;

        public static float waitSecounds = 6f;
        [SerializeField] GameObject pageOne;

        [SerializeField] Transform LandObject;

        [SerializeField]
        List<Transform> BridgeBlocks;

        [SerializeField]
        Transform MainBridge;

        private Vector3 BridgeStartScale;
        private Vector3 landScale;

        private float AngleIncrease = 0;





        bool rotating = false;


        // Start is called before the first frame update
        void Start()
        {
            if (pageOne == null)
            {
                pageOne = GameObject.Find("FirstPage");
            }

            if (BridgeBlocks != null && BridgeBlocks.Count != 0)
            {
                AngleIncrease = 180 / (float)BridgeBlocks.Count + 1;

                Debug.Log(AngleIncrease + "------" + BridgeBlocks.Count);
            }

            if (MainBridge == null)
            {
                MainBridge = GameObject.Find("Bridge").transform;
            }

            if (LandObject == null)
            {
                LandObject = GameObject.Find("land").transform;
            }

            BridgeStartScale = MainBridge.localScale;
            landScale = LandObject.transform.localScale;



            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            StartCoroutine(rotateObject(pageOne, rotation, duration, waitSecounds));
        }

        // Update is called once per frame


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
                for (int i = 0; i < BridgeBlocks.Count; i++)
                {
                    //------------------------ Rotate Right---------------------------------------------//
                    BridgeBlocks[i].localRotation = Quaternion.Lerp(Quaternion.Euler(new Vector3(0, 0, 180))
                    , Quaternion.Euler(new Vector3(0, 0, 160f + (AngleIncrease * (i + 1)))), (counter / duration));

                    //------------------------ Castle Scale ---------------------------------------------//
                    MainBridge.localScale = Vector3.Lerp(BridgeStartScale, new Vector3(1, 1, BridgeStartScale.z), (counter / duration));

                    LandObject.localScale = Vector3.Lerp(landScale, Vector3.one, (counter / duration));
                }
                yield return null;
            }
            rotating = false;
        }

    }
}