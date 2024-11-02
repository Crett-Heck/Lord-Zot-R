using LordZot.SkillStates;
using LordZot.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace LordZot.Modules
{
    public static class States
    {
        internal static void RegisterStates()
        {
            Modules.Content.AddEntityState(typeof(BaseMeleeAttack));
            Modules.Content.AddEntityState(typeof(EldritchFury));
            Modules.Content.AddEntityState(typeof(ZotLand));
            Modules.Content.AddEntityState(typeof(ZotJump));
            Modules.Content.AddEntityState(typeof(BaseLeap));
            Modules.Content.AddEntityState(typeof(GemBulwark));
            Modules.Content.AddEntityState(typeof(ZotSlam));
            Modules.Content.AddEntityState(typeof(TitanStride));
            Modules.Content.AddEntityState(typeof(EldritchSlam));
            Modules.Content.AddEntityState(typeof(ZotLaser));
        }
    }
}