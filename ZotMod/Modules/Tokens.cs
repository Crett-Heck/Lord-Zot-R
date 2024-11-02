using R2API;
using System;

namespace LordZot.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region Zot
            string prefix = LordZot.DEVELOPER_PREFIX + "_ZOT_BODY_";

            string desc = "A stranger from another reality, Zot has come to this planet seeking ever more power.<color=#CCD3E0>" + System.Environment.NewLine + System.Environment.NewLine;
            desc = desc + "<!> An exceedingly heavy survivor who gains power from natural resources and gems." + System.Environment.NewLine;
            desc = desc + "<!> Zot cannot pickup items. <color=#FFE400>" + System.Environment.NewLine;
            desc = desc + "<!> - Completely unable to sprint, relying on ability usage for mobility. Additionally, enemies that impact surfaces take velocity-based damage." + System.Environment.NewLine;
            desc = desc + "<!> - Cannot be knocked back, and causes collateral damage to foes when landing, and even when taking mere footsteps.  Stepping directly on an enemy deals extra damage." + System.Environment.NewLine;
            desc = desc + "<!> - Abilities (And Jump!) can be held down to infuse them with Gem Power, increasing their magnitude with no upper limit." + System.Environment.NewLine;
            desc = desc + "<!> - The rules of this world do not apply to Lord Zot. His attacks do not utilize Base Damage, nor do they interact with On-Hit Effects. He is immune to many effects, including Void Reavers." + System.Environment.NewLine;
            desc = desc + "<!> - Zot's Gem Power regeneration increases proportionally to his maximum Gem Power, as does his Mass." + System.Environment.NewLine + System.Environment.NewLine;
            desc = desc + " (Debug cheats: LCTRL+U = Gem Power Boost, LCTRL+L = Charge Jump based on a percentage of Gem Power instead of a flat value)";

            string outro = "... And so he demanifested. Not yet fully attuned to this reality, he vowed to return, to gain greater might from this land.";
            string outroFailure = "... In a bright light, Lord Zot disappeared amidst the destruction, to remanifest later in a more convenient time and place.";

            LanguageAPI.Add(prefix + "NAME", "Lord Zot");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Mystic Titan");
            LanguageAPI.Add(prefix + "LORE", "Test Zot Lore Token");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Mystic Titan");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "<style=cIsDamage>Zot is unable to sprint.</style> <style=cIsUtility>Each footstep deals mild damage and knockback in an area." +
                " Zot cannot use or hold items.<color=#FFE400>" + System.Environment.NewLine +
                " Hold jump to begin storing power for a mighty leap. There is no maximum charge." + System.Environment.NewLine +
                " Zot deals Mass-scaling damage in a radius when landing, with area and damage increasing based on velocity.</style>" + System.Environment.NewLine +
                " When dashing, Zot gains Momentum, boosting the power and knockback of his next melee attack.</style>" + System.Environment.NewLine +
                " Momentum quickly fades. The amount of Momentum gained is proportional to Zot's Mass.</style>" + System.Environment.NewLine +
                " Press V mid-air to toggle float. This costs Gem Power to maintain.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_NAME", "Eldritch Fury");
            LanguageAPI.Add(prefix + "PRIMARY_DESCRIPTION", "Zot winds up and throws a mighty punch with one of his shielded gauntlets, alternating arms with each swing." +
                "Deals <style=cIsDamage>the greater of 200 flat damage and 1% of current Gem Power.</style>. Always costs at least 1 Gem Power to use." + System.Environment.NewLine +
                "<style=cIsUtility>Effect falls off based on distance.</style>");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_NAME", "Gem Bulwark");
            LanguageAPI.Add(prefix + "SECONDARY_DESCRIPTION", " <style=cIsUtility>Tap to deflect nearby enemies and attacks</style> with a backhand." + System.Environment.NewLine +
                "Hold to begin charging a gem-powered beam that, once released, deals damage at the location of its end-point." + System.Environment.NewLine +
                "<style=cIsUtility>Can be charged indefinitely, increasing blast radius and damage. Backhand knockback increases with maximum Gem Power.</style>");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_NAME", "Titan's Stride");
            LanguageAPI.Add(prefix + "UTILITY_DESCRIPTION", "Tap quickly to <style=cIsUtility>dash incredibly fast</style> in a direction." + System.Environment.NewLine +
                "Grants <style=cIsUtility>Momentum</style> based on Zot's Mass. Momentum falls off quickly," +
                "but adds 100% of its value to Zot's next melee attack's damage, and 1000% of its value as knockback." + System.Environment.NewLine +
                "When grounded, this ability behaves differently; it will <style=cIsUtility>navigate terrain and avoid entering the air.</style>");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_NAME", "Eldritch Slam");
            LanguageAPI.Add(prefix + "SPECIAL_DESCRIPTION", "Zot holds a fist in the air, and then hurls an immense blow with high knockback." +
                "Deals <style=cIsDamage>massive damage and knockback based on charge.</style> (If aimed towards the ground, half damage is dealt in an AoE, and enemies are knocked upwards.)" + System.Environment.NewLine +
                "<style=cIsUtility>Effect falls off based on distance.</style>");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Zot: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Zot, beat the game on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Zot: Mastery");
            #endregion
            #endregion
        }
    }
}