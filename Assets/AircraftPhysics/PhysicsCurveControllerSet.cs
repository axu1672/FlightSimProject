using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.AircraftPhysics
{
    public class PhysicsCurveControllerSet
    {
        #region Drag

        private AnimationCurve _dragU;
        private AnimationCurve _dragD;

        private AnimationCurve _dragL;
        private AnimationCurve _dragR;

        private AnimationCurve _dragF;
        private AnimationCurve _dragB;

        #endregion

        #region Lift

        private AnimationCurve _coefficientOfLift;

        #endregion

        #region Induced Drag

        private AnimationCurve _coefficientOfInducedDrag;

        #endregion

        #region External Curves

        private Dictionary<string, AnimationCurve> _externalCurves;

        #endregion

        #region Steering Curves

        private AnimationCurve _steeringCurve;

        #endregion

        #region Steering Curve



        #endregion

        #region Setup

        public PhysicsCurveControllerSet(
            AnimationCurve dU, AnimationCurve dD, AnimationCurve dL, AnimationCurve dR,
            AnimationCurve dF, AnimationCurve dB, AnimationCurve coL, AnimationCurve coID,
            AnimationCurve sC)
        {
            _dragU = dU;
            _dragD = dD;
            _dragL = dL;
            _dragR = dR;
            _dragF = dF;
            _dragB = dB;
            _coefficientOfLift = coL;
            _coefficientOfInducedDrag = coID;
            _steeringCurve = sC;
        }

        #endregion

        #region Public Methods

        public float EvaluateCoefficientOfDrag(Vector3 localizedVelocity)
        {
            float lvx = Mathf.Abs(localizedVelocity.x);
            float lvy = Mathf.Abs(localizedVelocity.y);
            float lvz = Mathf.Abs(localizedVelocity.z);

            float xEval;
            float yEval;
            float zEval;

            if (lvx < 0)
            {
                xEval = _dragB.Evaluate(lvx);
            }
            else
            {
                xEval = _dragF.Evaluate(lvx);
            }

            if (lvy < 0)
            {
                yEval = _dragL.Evaluate(lvy);
            }
            else
            {
                yEval = _dragR.Evaluate(lvy);
            }

            if (lvz < 0)
            {
                zEval = _dragU.Evaluate(lvz);
            }
            else
            {
                zEval = _dragD.Evaluate(lvz);
            }

            // Scale Coefficient using direction
            return ScaleDragBy3Directions(xEval, yEval, zEval, localizedVelocity.normalized).magnitude;
        }

        public float EvaluateCoefficientOfLift(float rads)
        {
            float deg = Mathf.Rad2Deg * rads;

            return _coefficientOfLift.Evaluate(rads);
        }

        public float EvaluateCoefficientOfInducedDrag(Vector3 localizedVelocity)
        {
            float liftVelocity = Mathf.Max(0 , localizedVelocity.z);
            return _coefficientOfInducedDrag.Evaluate(liftVelocity);
        }

        #endregion

        #region Private Methods

        private Vector3 ScaleDragBy3Directions(float x, float y, float z, Vector3 value)
        {
            Vector3 res = value;

            res.x *= x;
            res.y *= y;
            res.z *= z;

            return res;
        }

        #endregion

        #region External Curve Setup
        #endregion
    }
}
