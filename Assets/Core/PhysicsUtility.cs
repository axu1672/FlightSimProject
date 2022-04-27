using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Core
{
    public class PhysicsUtility
    {
        private AnimationCurve _dragU;
        private AnimationCurve _dragD;
        private AnimationCurve _dragL;
        private AnimationCurve _dragR;
        private AnimationCurve _dragF;
        private AnimationCurve _dragB;

        private AnimationCurve _cOfLift;

        public PhysicsUtility(AnimationCurve dragU, AnimationCurve dragD,
            AnimationCurve dragL, AnimationCurve dragR, AnimationCurve dragF,
            AnimationCurve dragB, AnimationCurve cOfLift)
        {
            _dragU = dragU;
            _dragD = dragD;
                
            _dragL = dragL;
            _dragR = dragR;

            _dragF = dragF;
            _dragB = dragB;

            _cOfLift = cOfLift;
        }

        public float GetCoefficientOfDrag(Vector3 localizedVelocity, Vector3 value)
        {
            float weightU = _dragU.Evaluate(localizedVelocity.y);
            float weightD = _dragD.Evaluate(localizedVelocity.y);

            float weightF = _dragF.Evaluate(localizedVelocity.x);
            float weightB = _dragB.Evaluate(localizedVelocity.x);

            float weightL = _dragL.Evaluate(localizedVelocity.z);
            float weightR = _dragR.Evaluate(localizedVelocity.z);
            return 0;
        }

        private float ScaleTo3D(float xS, float yS, float zS, Vector3 value)
        {
            return 0;
        }

        public float GetCoefficientOfLift(float angleOfAttack)
        {
            return _cOfLift.Evaluate(angleOfAttack);
        }
    }
}
