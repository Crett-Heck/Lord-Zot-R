using RoR2;
using System;
using UnityEngine;

namespace LordZot.Modules.Achievements
{
    internal class MasteryAchievement : BaseMasteryUnlockable
    {
        public override string AchievementTokenPrefix => LordZot.DEVELOPER_PREFIX + "_ZOT_BODY_MASTERY";
        //the name of the sprite in your bundle
        public override string AchievementSpriteName => "texMasteryAchievement";
        //the token of your character's unlock achievement if you have one
        public override string PrerequisiteUnlockableIdentifier => LordZot.DEVELOPER_PREFIX + "_ZOT_BODY_UNLOCKABLE_REWARD_ID";

        public override string RequiredCharacterBody => "ZotBody";
        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}