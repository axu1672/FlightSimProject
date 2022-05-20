using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.AircraftPhysics
{
    public class F22PhysicsObject : AircraftPhysicsObject
    {
        #region Properties

        public override float LiftPower => 100;

        public override float InducedDragValue => 20;

        public override float Mass => 100;

        public override float MinimumThrustForce => 100;

        public override float MaximumThrustForce => 10000;

        #endregion

        public F22PhysicsObject(Rigidbody rb,
            AnimationCurve dU, AnimationCurve dD, AnimationCurve dL, AnimationCurve dR,
            AnimationCurve dF, AnimationCurve dB, AnimationCurve coL, AnimationCurve coID, AnimationCurve sC) : base(rb,
             dU,  dD,  dL,  dR,
             dF,  dB,  coL,  coID, sC)
        {

        }

        public void EngineOn()
        {
            base.ThrottleValue = 0;
        }

        public void EngineOff()
        {

        }
    }
}
