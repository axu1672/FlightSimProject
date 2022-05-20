using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.AircraftPhysics
{
    public class AircraftPhysicsObject
    {
        #region Fields

        private Rigidbody _body;

        private PhysicsCurveControllerSet _curveSet;

        private Quaternion _inverseRotation;

        private Vector3 _localVelocity;

        private Vector3 _localAngularVelocity;

        private float AngleOfAttack;

        private float Yaw;

        private float _baseThrottle = 1;

        #endregion

        #region Properties

        public virtual float LiftPower => 100;

        public virtual float InducedDragValue => 20;

        public virtual float Mass => 100;

        public virtual float MinimumThrustForce => 100;

        public virtual float MaximumThrustForce => 10000;

        public virtual float ThrottleValue
        {
            get { return _baseThrottle; }
        }

        #endregion

        #region Setup Methods

        public AircraftPhysicsObject(Rigidbody rb, 
            AnimationCurve dU, AnimationCurve dD, AnimationCurve dL, AnimationCurve dR,
            AnimationCurve dF, AnimationCurve dB, AnimationCurve coL, AnimationCurve coID,
            AnimationCurve sC)
        {
            _body = rb;
            _curveSet = new PhysicsCurveControllerSet(dU, dD, dL, dR, dF, dB, coL, coID, sC);
        }

        #endregion

        #region Physics Evaluation Methods

        private void GetState()
        {
            _inverseRotation = Quaternion.Inverse(_body.rotation);

            _localVelocity = _inverseRotation * _body.velocity;

            _localAngularVelocity = _inverseRotation * _body.angularVelocity;

            AngleOfAttack = Mathf.Atan2(_localVelocity.y, _localVelocity.z);

            Yaw = Mathf.Atan2(_localAngularVelocity.x, _localAngularVelocity.z);
        }

        private Vector3 EvaluateThrust(float throttle)
        {
            return throttle * MaximumThrustForce * Vector3.forward;
        }

        private Vector3 EvaluateDrag()
        {
            float lv2 = _localVelocity.sqrMagnitude;

            float coefficientOfDrag = _curveSet.EvaluateCoefficientOfDrag(_localVelocity);

            // Coefficient * local velocity squared * negative direction of local velocity
            Vector3 drag = coefficientOfDrag * lv2 * -_localVelocity.normalized;

            return drag;
        }

        private Vector3 EvaluateWeight()
        {
            return new Vector3();
        }

        private Vector3 EvaluateLift()
        {
            Vector3 liftVelocity = Vector3.ProjectOnPlane(_localVelocity, Vector3.right);

            float liftv2 = liftVelocity.sqrMagnitude;

            float liftCoefficient = _curveSet.EvaluateCoefficientOfLift(AngleOfAttack * Mathf.Rad2Deg);

            float liftForce = liftv2 * liftCoefficient * LiftPower;

            Vector3 liftDir = Vector3.Cross(liftVelocity.normalized, Vector3.right);

            Vector3 lift = liftDir * liftForce;

            Vector3 inducedDrag = (-_localVelocity.normalized) * liftv2 * InducedDragValue * _curveSet.EvaluateCoefficientOfInducedDrag(_localVelocity);

            return lift + inducedDrag;
        }

        private void ApplyPhysicsUpdate()
        {
            GetState();
            Vector3 T = EvaluateThrust(ThrottleValue);
            Vector3 D = EvaluateDrag();
            Vector3 W = EvaluateWeight();
            Vector3 L = EvaluateLift();

            _body.AddRelativeForce(T);
            _body.AddRelativeForce(D);
            _body.AddRelativeForce(W);
            _body.AddRelativeForce(L);
        }

        #endregion

        #region Steering Method

        private Vector3 CalculateSteeringVector()
        {
            Vector3 ret = new Vector3();

            return ret;
        }

        private void UpdateSteering()
        {
            Vector3 steeringAdjustment = CalculateSteeringVector();

            _body.AddRelativeTorque(steeringAdjustment * Mathf.Deg2Rad, ForceMode.VelocityChange);
        }

        #endregion

        #region Private Methods
        #endregion

        #region Public Methods

        public void Update()
        {
            ApplyPhysicsUpdate();
            UpdateSteering();
        }

        #endregion
    }
}
