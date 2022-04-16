using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Core
{
    public static class AircraftPhysics
    {
        
        public static void Update(Aircraft state)
        {
            ApplyForceVectors(state);
        }

        private static void ApplyForceVectors(Aircraft state)
        {
            float thrustMagnitude = state.ThrustForceValue;

            Vector3 combinedVector = CreateThrustVector(thrustMagnitude)
                + CreateDragVector(state.LocalVelocity)
                + CreateLiftVector(state.LocalVelocity, state.Pitch);

            state.ApplyForceVector(combinedVector);

            //state.ApplyForceVector(CreateWeightVector(19700));
        }

        private static Vector3 CreateThrustVector(float magnitude)
        {
            return magnitude * Vector3.forward;
        }

        private static Vector3 CreateDragVector(Vector3 localVelocity)
        {
            Vector3 dir = -localVelocity.normalized;
            float v2 = localVelocity.sqrMagnitude;
            float Cd = GetCoefficientOfDrag(v2);

            return Cd * v2 * dir;
        }

        private static Vector3 CreateLiftVector(Vector3 localVelocity, float pitch)
        {
            Vector3 liftVelocity = Vector3.ProjectOnPlane(localVelocity, Vector3.right);
            var v2 = liftVelocity.sqrMagnitude;

            float Cl = GetCoefficientOfLift(pitch);
            Vector3 liftDir = Vector3.Cross(liftVelocity.normalized, Vector3.right);
            float liftMagnitude = v2 * Cl;

            Vector3 lift = liftDir * liftMagnitude;

            float dragForce = Cl * Cl;
            Vector3 dragDir = -liftVelocity.normalized;
            Vector3 inducedDrag = dragDir * dragForce * v2 * GetInducedDragCoefficient();

            return lift + inducedDrag;
        }

        private static Vector3 CreateWeightVector(float mass)
        {
            float gmass = mass * 1000;
            Vector3 dir = Vector3.down;
            float magnitude = gmass * 9.81f;

            if ( magnitude > TerminalVelocity(mass)) return Vector3.zero;
            return dir * magnitude;
        }

        #region Physics Coefficients

        public static float GetCoefficientOfLift(float angleOfAttack)
        {
            float Cl = 1;
            return Cl;
        }

        public static float GetCoefficientOfDrag(float v2)
        {
            return 1;
        }

        private static float GetInducedDragCoefficient()
        {
            return 1;
        }

        public static Vector3 GetLiftVelocity(Vector3 localVelocity)
        {
            return Vector3.Project(localVelocity, Vector3.right);
        }

        public static float TerminalVelocity(float m)
        {
            return (float)Math.Sqrt((2*m*9.81f)/(1.225*100*0.1f));
        }

        #endregion
    }
}
