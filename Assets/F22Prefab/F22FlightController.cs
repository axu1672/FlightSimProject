using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Core;
using Assets.F22Prefab;

public class F22FlightController : MonoBehaviour
{
    public F22Aircraft state;
    public Rigidbody body;
    public Animator _animator;

    public float test;

    [Header("Physics Curves: Drag")]
    [SerializeField]
    AnimationCurve dragU;
    [SerializeField]
    AnimationCurve dragF;
    [SerializeField]
    AnimationCurve dragB;
    [SerializeField]
    AnimationCurve dragL;
    [SerializeField]
    AnimationCurve dragR;
    [SerializeField]
    AnimationCurve dragD;

    [Header("Physics Curves: Lift")]
    [SerializeField]
    AnimationCurve coefficientOfLift;
    [SerializeField]
    AnimationCurve coefficientOfInducedDrag;


    public float DragCoefficient
    {
        get { return 0; }
        set { test = value; }
    }

    private float l_aileron_rollvalue = 0;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        state = new F22Aircraft(body);
        _animator.Play("AileronMove");
    }

    // Update is called once per frame
    void Update()
    {
        state.Update();
        
        if (Input.GetKey(KeyCode.Keypad4))
        {
            if (l_aileron_rollvalue < 1) l_aileron_rollvalue += .01f;
        }

        if (Input.GetKey(KeyCode.Keypad6))
        {
            if (l_aileron_rollvalue > -1) l_aileron_rollvalue -= .01f;           
        }

        _animator.SetFloat("Blend", l_aileron_rollvalue);
    }
}
