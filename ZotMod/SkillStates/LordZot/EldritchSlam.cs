using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace LordZot.SkillStates
{
    public class EldritchSlam : BaseSkillState
    {

        public static float BaseDuration = 0.65f;
       
 

       

        public override void OnEnter()
        {
           
            
          
          

           

            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        
    }
}