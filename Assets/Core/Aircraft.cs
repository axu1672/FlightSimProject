using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Core
{
    public class Aircraft
    {
        #region Force

        // Thrust Values
        public const float _maximumThrustValue = 100;

        public const float _minimumEngineThrottle = .05f;

        private float _throttle;

        private float _thrustRateOfChange = .05f;


        // Drag Values
        private const float _dragCoefficient = 10;

        // Lift Values
        private float _liftCoefficient = 10;

        // Induced Drag Values
        private const float _pi = (float)Math.PI;

        private const float _efficiencyFactor = 2;

        private float _aspectRatio = 2;

        // Aircraft State
        private Vector3 _velocity; // rigidbody velocity

        private Vector3 _localVelocity;

        private Vector3 _angularVelocity;

        private Vector3 _localAngularVelocity;

        private float _pitch = 0 ; // Pitch Rotation

        private float _yaw = 0;

        private float _roll = 0;

        private Rigidbody _rb;

        #endregion

        #region Properties

        public Quaternion InverseRotation
        {
            get
            {
                if (_rb == null) return Quaternion.identity;
                return Quaternion.Inverse(_rb.rotation);
            }
        }

        public Quaternion AngleConverted
        {
            get
            {
                return Quaternion.Euler(_roll, _yaw, _pitch);
            }
        }

        public float ThrustForceValue 
        { 
            get
            {
                return Math.Max(_minimumEngineThrottle, _throttle) * _maximumThrustValue;
            } 
        }

        public float Pitch { get { return _pitch; } }

        public Vector3 LocalVelocity { get { return _localVelocity; } }

        public float LiftValue 
        {
            get
            {
                Vector3 liftVel = AircraftPhysics.GetLiftVelocity(_localVelocity);
                _liftCoefficient = AircraftPhysics.GetCoefficientOfLift(_pitch * Mathf.Rad2Deg);
                return _liftCoefficient * (float)Math.Pow(liftVel.magnitude, 2);
            }
        }

        public float InducedDrag
        {
            get
            {
                Vector3 liftVel = AircraftPhysics.GetLiftVelocity(_localVelocity);
                _liftCoefficient = AircraftPhysics.GetCoefficientOfLift(_pitch * Mathf.Rad2Deg);
                return _liftCoefficient / (_pi * _aspectRatio * _efficiencyFactor);
            }
        }

        #endregion

        #region Initialization

        public Aircraft(Rigidbody physicsIn)
        {
            _rb = physicsIn;
        }

        #endregion

        #region State

        public void ApplyForceVector(Vector3 force)
        {
            _rb.AddRelativeForce(force);
        }

        public void Update()
        {
            UpdateRigidBody();
            CalculateStateVariables();
            ConvertRigidbodyData();
            CalculateAngleOfAttack();
        }

        private void UpdateRigidBody()
        {
            AircraftPhysics.Update(this);
        }

        private void CalculateStateVariables()
        {
            _velocity = _rb.velocity;

            Quaternion inverseRotation = Quaternion.Inverse(_rb.rotation);

            _angularVelocity = _rb.angularVelocity;           
        }

        private void ConvertRigidbodyData()
        {
            _localVelocity = InverseRotation * _velocity;
            _localAngularVelocity = InverseRotation * _angularVelocity;
        }

        private void CalculateAngleOfAttack()
        {
            _pitch = Mathf.Atan2(-_localVelocity.y, _localVelocity.z) * Mathf.Rad2Deg;
            _yaw = Mathf.Atan2(-_localAngularVelocity.x, _localAngularVelocity.z) * Mathf.Rad2Deg;
        }


        #endregion

        #region User Control

        public void AdjustThrottle(ParameterDirection dir)
        {
            float roc = (int)dir * _thrustRateOfChange;

            _throttle += roc;
        }

        public void AdjustRoll()
        {

        }

        public void AdjustPitch()
        {

        }

        #endregion
    }

    public enum ParameterDirection 
    { 
        Increase = 1,
        Decrease = -1,
    }

}
