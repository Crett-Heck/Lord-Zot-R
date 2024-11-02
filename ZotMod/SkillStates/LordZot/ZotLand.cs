using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using UnityEngine;
namespace LordZot.SkillStates
{
    class ZotLand : BaseSkillState
    {
        public float stopwatch;

        public override void OnEnter()
        {
            base.OnEnter();

            this.stopwatch = 0;
            base.PlayCrossfade("Body", "AscendDescend 2", 0.2f);

        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.stopwatch += Time.fixedDeltaTime;

            if (this.stopwatch >= 1)
            {
                this.outer.SetNextStateToMain();


            }





        }
        public override void OnExit()
        {

            base.OnExit();

        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}
