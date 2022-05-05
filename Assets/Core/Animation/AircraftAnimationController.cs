using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Core.Animation
{
    public class AircraftAnimationController
    {
        #region Fields

        private Animator _aircraftAnimator;

        #endregion

        #region Properties
        #endregion

        #region Setup

        public AircraftAnimationController(Animator animator)
        {
            _aircraftAnimator = animator;
        }
        
        #endregion

        #region Methods

        public void Update()
        {

        }

        #endregion
    }
}
