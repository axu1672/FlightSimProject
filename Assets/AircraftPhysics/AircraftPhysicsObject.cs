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

        #endregion

        #region Properties

        public float LiftPower => 100;

        public float InducedDragValue => 20;

        public float Mass => 100;

        public float MinimumThrustForce => 100;

        public float MaximumThrustForce => 10000;

        #endregion

        #region Setup Methods

        public AircraftPhysicsObject(Rigidbody rb, 
            AnimationCurve dU, AnimationCurve dD, AnimationCurve dL, AnimationCurve dR,
            AnimationCurve dF, AnimationCurve dB, AnimationCurve coL, AnimationCurve coID)
        {
            _body = rb;
            _curveSet = new PhysicsCurveControllerSet(dU, dD, dL, dR, dF, dB, coL, coID);
        }

        #endregion

        #region Physics Evaluation Methods

        public void GetState()
        {
            _inverseRotation = Quaternion.Inverse(_body.rotation);

            _localVelocity = _inverseRotation * _body.velocity;

            _localAngularVelocity = _inverseRotation * _body.angularVelocity;

            AngleOfAttack = Mathf.Atan2(_localVelocity.y, _localVelocity.z);

            Yaw = Mathf.Atan2(_localAngularVelocity.x, _localAngularVelocity.z);
        }

        public Vector3 EvaluateThrust(float throttle)
        {
            return throttle * MaximumThrustForce * Vector3.forward;
        }

        public Vector3 EvaluateDrag()
        {
            float lv2 = _localVelocity.sqrMagnitude;

            float coefficientOfDrag = _curveSet.EvaluateCoefficientOfDrag(_localVelocity);

            // Coefficient * local velocity squared * negative direction of local velocity
            Vector3 drag = coefficientOfDrag * lv2 * -_localVelocity.normalized;

            return drag;
        }

        public Vector3 EvaluateWeight()
        {
            return new Vector3();
        }

        public Vector3 EvaluateLift()
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

        #endregion

        #region Private Methods
        #endregion

        #region Public Methods
        #endregion
    }
}
