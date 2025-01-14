using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR 
public class EnableIfDisabledAttribute : PropertyAttribute
{
    public string ConditionName;

    public EnableIfDisabledAttribute(string conditionName)
    {
        ConditionName = conditionName;
    }
}
[CustomPropertyDrawer(typeof(EnableIfDisabledAttribute))]
public class EnableIfDisabledDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EnableIfDisabledAttribute enableIfDisabled = (EnableIfDisabledAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(enableIfDisabled.ConditionName);

        if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean)
        {
            // Enable field only if the condition is false
            bool isConditionFalse = !conditionProperty.boolValue;
            GUI.enabled = isConditionFalse;

            EditorGUI.PropertyField(position, property, label, true);

            GUI.enabled = true; // Reset GUI state
        }
        else
        {
            EditorGUI.HelpBox(position, $"Condition '{enableIfDisabled.ConditionName}' not found or is not a boolean.", MessageType.Error);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
#endif
public enum RabbitBehavior
{
    Stable,
    Walking,
    Jumping,

    Circling,
    Turning,


}

public class RabbitController : MonoBehaviour
{
    /// <summary>
    /// List Of Behaviors Movment Section
    /// </summary>
    [Header("________________________________________________________________________________________________________________________________________")]
    [Header(" Rabbit Behavior List Settings")]
    [Header("________________________________________________________________________________________________________________________________________")]
    [SerializeField]
    [Tooltip("The Lists Of Behaviors will loop Infinitely until the end of the game")]
    private bool LoopList = false;
    public List<RabbitBehavior> rabbitBehaviors;

    /// <summary>
    /// Circular Movment Section
    /// </summary>
    [Header("________________________________________________________________________________________________________________________________________")]
    [Header("Circular Settings")]
    [Header("________________________________________________________________________________________________________________________________________")]
    [Tooltip("The Center of the Circle which  Rabbit Move Around")]
    [SerializeField]
    private Vector3 CenterOfCircularMovment;

    [Tooltip("Radius of The Circle")]
    [SerializeField]
    [Range(0.1f, 500)]
    private float CircularRadius = .5f;

    [Tooltip("If the Movement is not infinite set the amount of time for routing ")]
    [SerializeField]
    [Range(0.000001f, 20)]
    private float Circuling_period = 10;

    [SerializeField]
    [Tooltip("Determining the Direction Of Movement")]
    private bool clockWise = false;

    [SerializeField]
    [Tooltip("The Speed of Routing ")]
    [Range(0.000001f, 100)]
    private float CircularSpeed = 10;

    [SerializeField]
    [Tooltip("Keep Routing all the time")]
#if UNITY_EDITOR
    [EnableIfDisabled("LoopList")]
#endif
    private bool _infinitCycle = false;
    [Space(5)]

    /// <summary>
    /// Walking Section
    /// </summary>

    [Header("________________________________________________________________________________________________________________________________________")]
    [Header("Walking Settings")]
    [Header("________________________________________________________________________________________________________________________________________")]
    [Space(5)]


    [SerializeField]
    [Tooltip("Time in Seconds of Walking")]
    [Range(.000001f, 20)]
    private float walking_period = 7;
    [SerializeField]

    [Tooltip("Speed of Walking")]
    [Range(0.000001f, 100)]
    private float _walkingSpeed = 6;

    /// <summary>
    /// stable Section
    /// </summary>

    [Space(5)]
    [Header("________________________________________________________________________________________________________________________________________")]
    [Header("Stable Settings")]
    [Header("________________________________________________________________________________________________________________________________________")]
    [Space(5)]
    [Tooltip("Time in second to stay stable")]
    [Range(.000001f, 20)]
    public float stablePeriod = 4;

    /// <summary>
    /// Junmping Section
    /// </summary>
    [Header("________________________________________________________________________________________________________________________________________")]
    [Space(5)]
    [Header("Jumbing Settings")]
    [Header("________________________________________________________________________________________________________________________________________")]

    [Space(5)]
    [Tooltip("Time in Seconds for jumping / if not Infinite")]
    [Range(.000001f, 20)]
    [SerializeField]
    private float Jumbing_period = 1;

    [SerializeField]
    [Tooltip("Jump infinitely (all Time)")]
#if UNITY_EDITOR
    [EnableIfDisabled("LoopList")]
#endif
    private bool Jumb_Infinitly = false;
    [SerializeField]
    [Tooltip("Normale Walk or Jump Walk")]
    private bool JumpWhileWalk = false;
    [SerializeField]
    private float _JumpingSpeed = .3f;

    [SerializeField]
    [Range(.000001f, 20)]
    float jumping_Range = .3f;

    /// <summary>
    /// Body Settings
    /// </summary>

    [Header("________________________________________________________________________________________________________________________________________")]
    [Header("Body Parts Settings")]
    [Header("________________________________________________________________________________________________________________________________________")]
    [Space(5)]
    [SerializeField]
    private Transform tail;


    [SerializeField]
    private Transform back;
    [SerializeField]
    private Transform RLeg;
    [SerializeField]
    private Transform LLeg;

    [SerializeField]
    private Transform LFoot;

    [SerializeField]
    private Transform RFoot;

    [SerializeField]
    private Transform LHand;

    [SerializeField]
    private Transform RHand;

    [SerializeField]
    private Transform RAlbo;
    [SerializeField]
    private Transform LAlbo;

    [SerializeField]
    private Transform LEar;
    [SerializeField]
    private Transform REar;

    [SerializeField]
    private Transform Head;

    [SerializeField]
    private Transform Body;

    [SerializeField]
    private Transform LEyeLed;
    [SerializeField]
    private Transform REyeLed;

    private float _amplitude = 800;

    private float localRotation;
    private float _speed = 15f;
    private bool _Walking = false;

    private Vector3 EyeScale;
    float Eyespeed = .1f;
    float OriginalHieghtOfBody;
    // Start is called before the first frame update
    void Start()
    {
        //-----------------------------------------------------------------//
        //--------- Other Inifinit Loops Will Die -------------------------//
        //-----------------------------------------------------------------//
        if (LoopList)
        {
            _infinitCycle = false;
            Jumb_Infinitly = false;
        }

        ///---------------------------------------------------------------///
        /// ------------------- Get all Needed Body Parts ----------------///
        ///---------------------------------------------------------------///

        if (tail == null)
        {
            tail = GameObject.Find("Tail").transform;
        }

        if (back == null)
        {
            back = GameObject.Find("back").transform;
        }

        if (REyeLed == null || LEyeLed == null)
        {
            REyeLed = GameObject.Find("REyeLed").transform;
            LEyeLed = GameObject.Find("LEyeLed").transform;

        }
        EyeScale = LEyeLed.localScale;

        if (RLeg == null)
        {
            RLeg = GameObject.Find("RLeg").transform;
        }

        if (LLeg == null)
        {
            LLeg = GameObject.Find("LLeg").transform;
        }

        if (RFoot == null)
        {
            RFoot = GameObject.Find("RFoot").transform;
        }

        if (LFoot == null)
        {
            LFoot = GameObject.Find("LFoot").transform;
        }
        if (RHand == null)
        {
            RHand = GameObject.Find("RHand").transform;

        }

        if (LHand == null)
        {
            LHand = GameObject.Find("LHand").transform;

        }

        if (LEar == null)
        {
            LEar = GameObject.Find("LEar").transform;

        }

        if (REar == null)
        {
            REar = GameObject.Find("REar").transform;

        }
        if (Head == null)
        {
            Head = GameObject.Find("Head").transform;

        }

        if (Body == null)
        {
            Body = GameObject.Find("Body").transform;

        }
        OriginalHieghtOfBody = transform.position.y;

        ///--------------------------------------------------------------///
        /// ------------------- Moving Tail continuaslly ----------------///
        /// -------------------------------------------------------------///
        /// 
        if (LLeg && RLeg && LFoot && RFoot && tail && LHand && RHand && REar && LEar && Head && Body && RAlbo && LAlbo)
        {
            localRotation = tail.localRotation.y;
            ///----------------------------------------------------------///
            ///-------------------- Moves the Ear and Tail----------------//
            ///----------------------------------------------------------///
            StartCoroutine(MoveTailEchTowSceounds());


            StartCoroutine(StartMovmet());
        }
        else
        {
            //Debug.Log("Make Sure Your Model Contains all needed Parts");
        }


    }
    //---------------------------------------------------------------------------//
    //------------------- Start Each Move Cycle After another -------------------//
    //---------------------------------------------------------------------------//
    IEnumerator StartMovmet()
    {
        foreach (RabbitBehavior behav in rabbitBehaviors)
        {


            yield return null;
            if (RabbitBehavior.Stable == behav)
            {
                _Walking = true;

                yield return StartCoroutine(StableCycle(stablePeriod));
                _Walking = false;


            }

            if (RabbitBehavior.Walking == behav)
            {
                //Debug.Log("First Walk in Running");
                yield return StartCoroutine(WalkingCycle(walking_period));
                _Walking = false;

            }

            if (RabbitBehavior.Jumping == behav)
            {
                yield return StartCoroutine(JumpingCycle(Jumbing_period));
                _Walking = false;

            }

            if (RabbitBehavior.Circling == behav)
            {
                yield return StartCoroutine(MoveRabbitCircles(Circuling_period));
                _Walking = false;
            }
            if (RabbitBehavior.Turning == behav)
            {
                //                Debug.Log("In Turn function");
                yield return StartCoroutine(ReversDirection());
                _Walking = false;
                //ReversDirection();
            }
        }
        //-------------------------------------------------------------------------------------//
        // --------------------- In case there's an infinite loop of behaviors ----------------//
        //-------------------------------------------------------------------------------------//
        if (LoopList) yield return StartCoroutine(StartMovmet()); else yield break;
    }

    /// <summary>
    /// Reverse Y Direction 
    /// </summary>
    /// <returns></returns>
    IEnumerator ReversDirection()
    {
        float timeElapsed = 0;
        float beriod = .6f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);

        while (timeElapsed < beriod)
        {
            timeElapsed += Time.deltaTime;
            // Debug.Log("In Revers");
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, (timeElapsed / beriod));
            yield return null;

        }
        transform.rotation = endRotation;
        yield return null;

    }
    IEnumerator JumpingCycle(float period)
    {

        float waitetime = .7f;
        float spendTime = .5f;
        // float jumping_Range = transform.localScale.x * .30f;


        //---------------------------------------------------------------------------------------------//
        //---------------------------------- Original Data of Each Body Part---------------------------//
        //---------------------------------------------------------------------------------------------//

        Quaternion LLegOriginalRotation = LLeg.localRotation;
        Quaternion RLegOriginalRotation = RLeg.localRotation;
        Quaternion LFootOriginalRotation = LFoot.localRotation;
        Quaternion RFootOriginalRotation = RFoot.localRotation;
        Quaternion LHandOriginalRotation = LHand.localRotation;
        Quaternion RHandOriginalRotation = RHand.localRotation;
        Quaternion RAlboOriginalRotation = RAlbo.localRotation;
        Quaternion LAlboOriginalRotation = LAlbo.localRotation;
        Vector3 OriginalPositionBody = Body.localPosition;



        float passTime = 0;

        bool condition = !Jumb_Infinitly ? passTime <= period : true;

        while (condition)
        {

            condition = !Jumb_Infinitly ? passTime <= period : true;

            float counter = 0;
            while (counter < waitetime)
            {
                passTime += Time.deltaTime;

                counter += Time.deltaTime;
                spendTime += Time.deltaTime;

                //---------------------------------------------------------------------------------------------//
                //-------------------------------Rotate Leg for Preperation to Jump----------------------------//
                //---------------------------------------------------------------------------------------------//
                LLeg.localRotation = Quaternion.Slerp(LLeg.localRotation, Quaternion.Euler(0, 0, 15), (counter / waitetime));
                RLeg.localRotation = Quaternion.Slerp(RLeg.localRotation, Quaternion.Euler(0, 0, 15), (counter / waitetime));
                LFoot.localRotation = Quaternion.Slerp(LFoot.localRotation, Quaternion.Euler(0, 0, -21), (counter / waitetime));
                RFoot.localRotation = Quaternion.Slerp(RFoot.localRotation, Quaternion.Euler(0, 0, -21), (counter / waitetime));

                //---------------------------------------------------------------------------------------------//
                //-------------------------- moveBodey and Head in Preperation to Jump-------------------------//
                //---------------------------------------------------------------------------------------------//
                Body.localPosition = Vector3.Slerp(Body.localPosition, new Vector3(0, -.35f, 0), (counter / waitetime));
                Head.localPosition = Vector3.Slerp(Head.localPosition, new Vector3(0, -.15f, 0), (counter / waitetime));

                //---------------------------------------------------------------------------------------------//
                //------------------------------- move Hands in Preperation to Jump----------------------------//
                //---------------------------------------------------------------------------------------------//
                LHand.localRotation = Quaternion.Slerp(LHand.localRotation, Quaternion.Euler(0, 0, 55), (counter / waitetime));
                RHand.localRotation = Quaternion.Slerp(RHand.localRotation, Quaternion.Euler(0, 0, 55), (counter / waitetime));
                RAlbo.localRotation = Quaternion.Slerp(RAlbo.localRotation, Quaternion.Euler(40, -90, 0), (counter / waitetime));
                LAlbo.localRotation = Quaternion.Slerp(LAlbo.localRotation, Quaternion.Euler(50, -90, 0), (counter / waitetime));
                yield return null;

            }


            //---------------------------------------------------------------------------------------//
            //----------------------------------------- Part 2 Of Jumping ---------------------------//
            //---------------------------------------------------------------------------------------//

            counter = 0;

            while (counter < waitetime)
            {
                passTime += Time.deltaTime;
                _Walking = true;
                StartCoroutine(MoveEar(REar, 1500, true));
                StartCoroutine(MoveEar(LEar, 1500, true));
                //StartCoroutine(MoveEar(Head, 400, false));
                _Walking = false;
                counter += Time.deltaTime;

                //--------------------------------------------------------------------------------------------//
                //-------------------------------Rotate Leg in Preperation to Jump----------------------------//
                //--------------------------------------------------------------------------------------------//
                LLeg.localRotation = Quaternion.Slerp(LLeg.localRotation, LLegOriginalRotation, (counter / waitetime));
                RLeg.localRotation = Quaternion.Slerp(RLeg.localRotation, RLegOriginalRotation, (counter / waitetime));
                LFoot.localRotation = Quaternion.Slerp(LFoot.localRotation, Quaternion.Euler(0, 0, 60), (counter / waitetime));
                RFoot.localRotation = Quaternion.Slerp(RFoot.localRotation, Quaternion.Euler(0, 0, 60), (counter / waitetime));

                //---------------------------------------------------------------------------------------------//
                //-------------------------- moveBodey and Head Preperation to Jump----------------------------//
                //---------------------------------------------------------------------------------------------//
                Body.localPosition = Vector3.Slerp(Body.localPosition, new Vector3(0, -0.271f, 0), (counter / waitetime));
                Head.localPosition = Vector3.Slerp(Head.localPosition, new Vector3(0, 0, 0), (counter / waitetime));

                //---------------------------------------------------------------------------------------------//
                //---------------------------------- move Hands Preperation to Jump----------------------------//
                //---------------------------------------------------------------------------------------------//
                LHand.localRotation = Quaternion.Slerp(LHand.localRotation, Quaternion.Euler(0, 0, -60), (counter / waitetime));
                RHand.localRotation = Quaternion.Slerp(RHand.localRotation, Quaternion.Euler(0, 0, -60), (counter / waitetime));
                RAlbo.localRotation = Quaternion.Slerp(RAlbo.localRotation, RAlboOriginalRotation, (counter / waitetime));
                LAlbo.localRotation = Quaternion.Slerp(LAlbo.localRotation, LAlboOriginalRotation, (counter / waitetime));


                if (JumpWhileWalk)
                {

                    transform.Translate((Time.deltaTime * _JumpingSpeed * (4.0f / 6.0f) * Vector3.forward), Space.Self);

                    transform.Translate((jumping_Range * Vector3.up), Space.Self);

                }
                else
                {

                    transform.Translate((Time.deltaTime * jumping_Range * Vector3.up), Space.Self);
                }



                yield return null;

            }
            yield return new WaitForSeconds(.3f);
            counter = 0;

            while (counter < waitetime)
            {
                passTime += Time.deltaTime;

                counter += Time.deltaTime;

                //---------------------------------------------------------------------------------------------//
                //-------------------------------Rotate Leg for Preperation to Jump----------------------------//
                //---------------------------------------------------------------------------------------------//
                LLeg.localRotation = Quaternion.Slerp(LLeg.localRotation, LLegOriginalRotation, (counter / waitetime));
                RLeg.localRotation = Quaternion.Slerp(RLeg.localRotation, RLegOriginalRotation, (counter / waitetime));
                LFoot.localRotation = Quaternion.Slerp(LFoot.localRotation, LFootOriginalRotation, (counter / waitetime));
                RFoot.localRotation = Quaternion.Slerp(RFoot.localRotation, RFootOriginalRotation, (counter / waitetime));

                //---------------------------------------------------------------------------------------------//
                //-------------------------- moveBodey and Head Preperation to Jump----------------------------//
                //---------------------------------------------------------------------------------------------//
                Body.localPosition = Vector3.Slerp(Body.localPosition, new Vector3(0, OriginalPositionBody.y, 0), (counter / waitetime));
                Head.localPosition = Vector3.Slerp(Head.localPosition, new Vector3(0, 0, 0), (counter / waitetime));

                //---------------------------------------------------------------------------------------------//
                //---------------------------------- move Hands Preperation to Jump----------------------------//
                //---------------------------------------------------------------------------------------------//
                LHand.localRotation = Quaternion.Slerp(LHand.localRotation, LHandOriginalRotation, (counter / waitetime));
                RHand.localRotation = Quaternion.Slerp(RHand.localRotation, RHandOriginalRotation, (counter / waitetime));
                RAlbo.localRotation = Quaternion.Slerp(RAlbo.localRotation, RAlboOriginalRotation, (counter / waitetime));
                LAlbo.localRotation = Quaternion.Slerp(LAlbo.localRotation, LAlboOriginalRotation, (counter / waitetime));




                if (JumpWhileWalk)
                {

                    transform.Translate((Time.deltaTime * _JumpingSpeed * (2.0f / 6.0f) * Vector3.forward), Space.Self);
                    transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, OriginalHieghtOfBody, transform.position.z), (counter / waitetime));


                }
                else
                {
                    transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, OriginalHieghtOfBody, transform.position.z), (counter / waitetime));
                }
                transform.position = new Vector3(transform.position.x, OriginalHieghtOfBody, transform.position.z);

                yield return null;


            }


            //---------------------------------------------------------------------------------------//
            //----------------------------------------- Part 2 Of Jumping ---------------------------//
            //---------------------------------------------------------------------------------------//
            _Walking = false;
            yield return new WaitForSeconds(.09f);

        }


    }
    //------------------------------------------------------------------------//
    // ------------------------ Recursive Function -------------------------- //
    //------------------------------------------------------------------------//
    IEnumerator MoveTailEchTowSceounds()
    {
        yield return new WaitForSeconds(4);

        StartCoroutine(MoveTail());

        StartCoroutine(EyeBlink());

        yield return MoveTailEchTowSceounds();

    }


    //------------------------------------------------------------------------//
    // --------------------------- Stable Cycle ----------------------------- //
    //------------------------------------------------------------------------//

    IEnumerator StableCycle(float period)
    {

        ///------------------ Moving Hands ------------------------//
        StartCoroutine(RotateBodyParts(RHand, 4, true, 200));
        StartCoroutine(RotateBodyParts(LHand, 4, false, 200));

        StartCoroutine(MoveEar(REar, 800, true));
        StartCoroutine(MoveEar(LEar, 800, false));
        StartCoroutine(MoveEar(Head, 400, false));

        ///---------------- Waljing Period Waiting-----------------//
        yield return new WaitForSeconds(period);
    }


    //-------------------------------------------------------------------------//
    //--------------------------------- The Walk Cycle ------------------------//
    //-------------------------------------------------------------------------//
    IEnumerator WalkingCycle(float period)
    {

        //Debug.Log("InWalke");

        if (!_Walking)
        {
            yield return null;
        }
        _Walking = true;
        StartCoroutine(MoveRabbitForward(period));
        ///------------------ Moving Left Leg----------------------//
        StartCoroutine(RotateBodyParts(LLeg, _walkingSpeed, true, 2700));
        StartCoroutine(RotateBodyParts(LFoot, _walkingSpeed, false, 1000));

        ///------------------ Moving Right Leg---------------------//
        StartCoroutine(RotateBodyParts(RLeg, _walkingSpeed, false, 2700));
        StartCoroutine(RotateBodyParts(RFoot, _walkingSpeed, true, 1000));

        ///------------------ Moving Hands ------------------------//
        StartCoroutine(RotateBodyParts(RHand, _walkingSpeed, true, 1000));
        StartCoroutine(RotateBodyParts(LHand, _walkingSpeed, false, 1000));


        StartCoroutine(MoveEar(REar, 800, true));
        StartCoroutine(MoveEar(LEar, 800, false));
        StartCoroutine(MoveEar(Head, 400, false));
        StartCoroutine(MoveEar(Body, 150, true));
        ///---------------- Waljing Period Waiting-----------------//
        yield return new WaitForSeconds(period);


        // ------------------- Ending Walking Cycle-----------------//
        _Walking = false;


    }
    //-----------------------------------------------------------------------//
    // ---------------------------- transform Movment -----------------------//
    //-----------------------------------------------------------------------//
    IEnumerator MoveRabbitForward(float period)
    {

        float move = transform.localPosition.z;


        while (_Walking)
        {

            transform.Translate(Time.deltaTime * _walkingSpeed * Vector3.forward, Space.Self);

            yield return null;
        }



    }

    IEnumerator MoveRabbitCircles(float period)
    {

        transform.rotation = Quaternion.identity;
        // Radius of the circle
        float speed = (2 * Mathf.PI * CircularSpeed) / (period * 10);           // Speed of the movement
                                                                                // Time for one full revolution (in seconds)
        float passtime = 0f;         // Time elapsed
        float angle = transform.position.z;

        _Walking = true;
        bool looping = true;
        //------------------ Moving Left Leg----------------------//
        StartCoroutine(RotateBodyParts(LLeg, _walkingSpeed, true, 2700));
        StartCoroutine(RotateBodyParts(LFoot, _walkingSpeed, false, 1000));

        ///------------------ Moving Right Leg---------------------//
        StartCoroutine(RotateBodyParts(RLeg, _walkingSpeed, false, 2700));
        StartCoroutine(RotateBodyParts(RFoot, _walkingSpeed, true, 1000));

        //------------------ Moving Hands ------------------------//
        StartCoroutine(RotateBodyParts(RHand, _walkingSpeed, true, 1000));
        StartCoroutine(RotateBodyParts(LHand, _walkingSpeed, false, 1000));


        StartCoroutine(MoveEar(REar, 800, true));
        StartCoroutine(MoveEar(LEar, 800, false));
        StartCoroutine(MoveEar(Head, 400, false));
        StartCoroutine(MoveEar(Body, 150, true));

        looping = !_infinitCycle ? passtime < period : true;
        CenterOfCircularMovment = CenterOfCircularMovment == null ? transform.position : CenterOfCircularMovment;

        while (looping)
        {
            looping = !_infinitCycle ? passtime < period : true;
            passtime += Time.deltaTime;

            if (!clockWise) angle -= CircularSpeed * Time.deltaTime;
            else angle += CircularSpeed * Time.deltaTime;

            float x = CenterOfCircularMovment.x + Mathf.Cos(angle) * (CircularRadius);
            float z = CenterOfCircularMovment.z + Mathf.Sin(angle) * (CircularRadius);

            // Update position
            transform.position = new Vector3(x, CenterOfCircularMovment.y, z);

            // Calculate the direction (tangent vector) to face
            Vector3 direction;
            if (!clockWise) direction = new Vector3(-Mathf.Sin(-angle), 0f, -Mathf.Cos(-angle));
            else direction = new Vector3(-Mathf.Sin(angle), 0f, Mathf.Cos(angle));

            // Make the object face the direction of movement (tangent)
            transform.rotation = Quaternion.LookRotation(direction);

            yield return null;
        }
        _Walking = false;


    }


    // ---------------------------- Tail Movment -----------------------//
    IEnumerator MoveTail()
    {

        float waitetime = 3;
        float spendTime = 0;

        while (spendTime <= waitetime)
        {

            spendTime += Time.deltaTime;
            float localRota = localRotation + Mathf.Cos(Time.time * _speed) * _amplitude;

            float localRota2 = localRotation + Mathf.Cos(Time.time * 5f) * _amplitude;

            tail.localRotation = Quaternion.Euler(new Vector3(tail.localRotation.x, Mathf.Deg2Rad * localRota, tail.localRotation.z));

            back.localRotation = Quaternion.Euler(new Vector3(back.localRotation.x, back.localRotation.y, Mathf.Deg2Rad * localRota2));
            yield return null;

        }

    }


    // ---------------------------- Tail Movment -----------------------//
    IEnumerator EyeBlink()
    {

        float waitetime = 1.5f;
        float spendTime = 0;


        while (spendTime <= waitetime)
        {

            spendTime += Time.deltaTime;
            Eyespeed += .06f;


            float LocalScale = EyeScale.y - Mathf.Sin(Eyespeed) * .055f;

            REyeLed.localScale = new Vector3(REyeLed.localScale.x, LocalScale, REyeLed.localScale.z);
            LEyeLed.localScale = new Vector3(LEyeLed.localScale.x, LocalScale, LEyeLed.localScale.z);
            yield return null;

        }

    }

    // ---------------------------- Tail Movment -----------------------//
    IEnumerator MoveEar(Transform Ear, float amplitude, bool reverse = false)
    {


        while (_Walking)
        {
            float localRota;
            if (reverse)
            {
                localRota = localRotation + Mathf.Cos(Time.time * 2) * amplitude;
            }
            else
            {
                localRota = localRotation + Mathf.Sin(Time.time * 2) * amplitude;
            }

            Ear.localRotation = Quaternion.Euler(new Vector3(Mathf.Deg2Rad * localRota, Ear.localRotation.y, Ear.localPosition.z));
            yield return null;

        }
        /// ------------------------ Return the Original Rotation ------------------------------------//
        while (Ear.localRotation.x != 0)
        {

            Ear.localRotation = Quaternion.Slerp(Ear.localRotation, Quaternion.Euler(0, Ear.localRotation.y, Ear.localRotation.z), 2 * Time.deltaTime);
            yield return null;
        }

    }

    // ------------------------ Other Parts Movment -----------------------//

    IEnumerator RotateBodyParts(Transform bodyPart, float speed, bool reverse = false, float walking_amplitude = 2500)
    {

        while (_Walking)
        {


            float localRota;
            if (reverse)
            {
                localRota = localRotation + Mathf.Cos(Time.time * speed) * walking_amplitude;
            }
            else
            {
                localRota = localRotation + Mathf.Sin(Time.time * speed) * walking_amplitude;
            }

            bodyPart.localRotation = Quaternion.Euler(new Vector3(bodyPart.localRotation.x, bodyPart.localRotation.y, Mathf.Deg2Rad * localRota));

            yield return null;

        }

        /// ------------------------ Return the Original Rotation ------------------------------------//
        while (bodyPart.localRotation.y != 0)
        {

            //if(bodyPart.name== "RLeg" && bodyPart.name == "RLeg")
            bodyPart.localRotation = Quaternion.Slerp(bodyPart.localRotation, Quaternion.Euler(bodyPart.localRotation.x, bodyPart.localRotation.y, 0), speed * Time.deltaTime);
            //Debug.Log("in here");
            yield return null;

        }



    }

    void OnDisable()
    {
        _Walking = false;
        _infinitCycle = false;
        Jumb_Infinitly = false;
        LoopList = false;
        StopAllCoroutines();

    }


}