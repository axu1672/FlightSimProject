using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Core;
using Assets.Core.Animation;
using Assets.F22Prefab;
using Assets.AircraftPhysics;

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

    [Header("Control Curves: Steering")]
    [SerializeField]
    AnimationCurve steeringCurve;

    private AircraftPhysicsObject _aircraftPhysicsObject;

    private AircraftAnimationController _aircraftAnimationController;

    public float DragCoefficient
    {
        get { return 0; }
        set { test = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        state = new F22Aircraft(body);
        _animator.Play("AileronMove");

        _aircraftPhysicsObject = new F22PhysicsObject(body, dragU, dragD, 
            dragL, dragR, dragF, dragB, 
            coefficientOfLift, coefficientOfInducedDrag, steeringCurve);

        _aircraftAnimationController = new AircraftAnimationController(_animator);
    }

    // Update is called once per frame
    void Update()
    {
        _aircraftAnimationController.Update();
        _aircraftPhysicsObject.Update();
    }
}
