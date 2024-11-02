using UnityEngine;
using System;
using RoR2;
using EntityStates;
using EntityStates.GolemMonster;
using EntityStates.Loader;

public class EldritchFury : BaseSkillState
{ 
public override void OnEnter()
{

    base.OnEnter();
    Debug.Log("EldritchFury entered");
    soundplayed = false;

    FuryTapPower = 0f;
        LordZot.LordZot.GemPowerVal = 1000;
    LordZot.LordZot.Busy = true;
    if (LordZot.LordZot.GemPowerVal <= 1f)
    {
        LordZot.LordZot.Gemdrain = 0.1f;
        this.outer.SetNextStateToMain();
    }
    else
    {

        characterDirection.turnSpeed = 300;
        bool lastswing = LordZot.LordZot.LastSwingArm is true;
        if (lastswing)
        {
            this.comboState = this.comboState + 1;
        }

        if (LordZot.LordZot.Gemdrain <= 0)
        { didntstartgemdrained = true; }
        else
        { didntstartgemdrained = false; }
        timeRemaining = 6.0f;
        LordZot.LordZot.Busy = true;
        //  if (LordZot.LordZot.Busy)
        //  { Debug.Log("Made Busy"); }
        this.stopwatch = 0f;
        if (LordZot.LordZot.GemPowerVal * 0.02f > 1)
        {
            FuryTapPower += LordZot.LordZot.GemPowerVal * 0.02f;
            LordZot.LordZot.GemPowerVal -= LordZot.LordZot.GemPowerVal * 0.02f;

        }
        else
        {
            LordZot.LordZot.GemPowerVal -= 1f;
            FuryTapPower = 1f;
        }

        // LordZot.LordZot.ticker2 = 0f;
        this.earlyExitDuration = EldritchFury.baseEarlyExitDuration / this.attackSpeedStat;
        this.animator = LordZot.LordZot.animator;
        this.hasSwung = false;
        this.hasHopped = false;
        this.modelAnimator = LordZot.LordZot.animator;

        ChildLocator component = LordZot.LordZot.component;



        {

        }

        bool @bool = this.animator.GetBool("isMoving");
        bool bool2 = this.animator.GetBool("isGrounded");

        switch (this.comboState)
        {
            case EldritchFury.ComboState.Punch1:
                {
                    LordZot.LordZot.RightTrail.emitting = true;
                    this.attackDuration = EldritchFury.baseComboAttackDuration * 1f / this.attackSpeedStat;
                    if (this.attackSpeedStat < attackspeedaltsoundthreshold)
                        RoR2.Util.PlayAttackSpeedSound(ClapState.attackSoundString, base.gameObject, this.attackSpeedStat * 1f);
                    else
                        RoR2.Util.PlayAttackSpeedSound(ClapState.attackSoundString, base.gameObject, 3f);
                    bool flag3 = @bool || !bool2;
                    if (flag3)
                    {
                        //animator.SetTrigger("Fury1");
                        base.PlayCrossfade("Gesture, Additive", "FireArrow", "FireArrow.playbackRate", this.attackDuration * 2.2f, 0.2f / this.attackSpeedStat);

                    }
                    else
                    {
                        //animator.SetTrigger("Fury1");
                        base.PlayCrossfade("Gesture, Override", "FireArrow", "FireArrow.playbackRate", this.attackDuration * 2.2f, 0.2f / this.attackSpeedStat);
                    }
                    break;
                }
            case EldritchFury.ComboState.Punch2:
                {
                    LordZot.LordZot.LeftTrail.emitting = true;
                    this.attackDuration = EldritchFury.baseComboAttackDuration / this.attackSpeedStat * 1f;
                    if (this.attackSpeedStat < attackspeedaltsoundthreshold)
                        RoR2.Util.PlayAttackSpeedSound(ClapState.attackSoundString, base.gameObject, this.attackSpeedStat * 1f);
                    else
                        RoR2.Util.PlayAttackSpeedSound(ClapState.attackSoundString, base.gameObject, 3f);
                    bool flag4 = @bool || !bool2;
                    if (flag4)
                    {
                        //animator.SetTrigger("Fury2");
                        base.PlayCrossfade("Gesture, Additive", "FireArrow2", "FireArrow.playbackRate", this.attackDuration * 1.4f, 0.2f / this.attackSpeedStat);
                    }
                    else
                    {
                        //animator.SetTrigger("Fury2");
                        base.PlayCrossfade("Gesture, Override", "FireArrow2", "FireArrow.playbackRate", this.attackDuration * 1f, 0.2f / this.attackSpeedStat);
                    }
                    this.hitEffectPrefab = LoaderMeleeAttack.overchargeImpactEffectPrefab;
                    break;
                }
        }
        //    base.characterBody.SetAimTimer(this.attackDuration + 1f);
        this.attackDuration = EldritchFury.baseComboAttackDuration / this.attackSpeedStat;
    }


}

// Token: 0x0600006E RID: 110 RVA: 0x00006314 File Offset: 0x00004514
public override void OnExit()
{
    LordZot.LordZot.Busy = false;
    characterDirection.turnSpeed = 150;
    LordZot.LordZot.LeftTrail.emitting = false;
    LordZot.LordZot.RightTrail.emitting = false;
    AkSoundEngine.DynamicSequenceStop(mferofgod);
    AkSoundEngine.StopPlayingID(mferofgod);
    base.OnExit();
}

// Token: 0x0600006F RID: 111 RVA: 0x00006358 File Offset: 0x00004558
/// <summary>
/// 
/// </summary>
public override void FixedUpdate()
{
    base.FixedUpdate();



    this.stopwatch += Time.fixedDeltaTime;


    if (didntstartgemdrained)
    {

        if (this.stopwatch >= this.attackDuration * 0.26f && soundplayed == false)
        {

            mferofgod = AkSoundEngine.PostEvent(1525938463, base.gameObject);
            AkSoundEngine.DynamicSequencePlay(mferofgod);


            soundplayed = true;
        };

        if (this.stopwatch >= this.attackDuration * 0.64f)
        {

            AkSoundEngine.DynamicSequenceStop(mferofgod);
            AkSoundEngine.StopPlayingID(mferofgod);

        };

        base.characterMotor.velocity.y -= base.characterMotor.velocity.y * 0.02f;



        bool isAuthority = base.isAuthority;
        if (isAuthority)
        {
            {
                switch (this.comboState)
                {
                    case EldritchFury.ComboState.Punch1:
                        {
                            LordZot.LordZot.LastSwingArm = true;
                            bool flageffect1 = this.stopwatch >= this.attackDuration * 0.55f && !this.hasSwung;
                            if (flageffect1)
                            {

                                ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();


                                if (component)
                                {


                                    GameObject original2 = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                                    GameObject original3 = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
                                    GameObject original35 = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                                    GameObject original4 = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");
                                    Transform transform2 = component.FindChild("RightHand");




                                    /*  RoR2.EffectData sonicboomeffectdata22 = new RoR2.EffectData();
                                      GameObject sonicboom22 = Resources.Load<GameObject>("prefabs/effects/impacteffects/impactmercswing");
                                      var direc22 = transform.rotation * Quaternion.Euler(90f, 0f, 0);
                                      sonicboomeffectdata22.scale = 3f;
                                      sonicboomeffectdata22.rotation = direc22;
                                      sonicboomeffectdata22.origin = transform.position;
                                      RoR2.EffectManager.SpawnEffect(sonicboom22, sonicboomeffectdata22, true);*/

                                    if (LordZot.LordZot.momentum > 100f)
                                    {
                                        var direc2 = transform.rotation * Quaternion.Euler(90f, 0f, 0);

                                        GameObject sonicboom2 = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/parentblinkeffect");
                                        RoR2.EffectData sonicboomeffectdata2 = new RoR2.EffectData();

                                        sonicboomeffectdata2.scale = 3f;
                                        sonicboomeffectdata2.rotation = direc2;
                                        sonicboomeffectdata2.origin = transform.position;
                                        RoR2.EffectManager.SpawnEffect(sonicboom2, sonicboomeffectdata2, true);
                                    }
                                    if (transform2)
                                    {
                                        /*  Color32 color = new Color32();
                                          color.a = 255;
                                          color.r = 255;
                                          color.g = 0;
                                          color.b = 0;*/

                                        var position2 = transform2.position;
                                        RoR2.EffectData effectData = new RoR2.EffectData();
                                        effectData.origin = position2;
                                        effectData.scale = 18f + (FuryTapPower * 0.3f);
                                        //   effectData.color = color;

                                        RoR2.EffectData effectData4 = new RoR2.EffectData();
                                        effectData4.origin = position2;
                                        effectData4.scale = 4f + (FuryTapPower * 0.6f);
                                        //    effectData4.color = color;
                                        RoR2.EffectData effectData2 = new RoR2.EffectData();
                                        effectData2.origin = position2;
                                        effectData2.scale = 7f + (FuryTapPower * 0.3f);
                                        //     effectData2.color = color;
                                        RoR2.EffectManager.SpawnEffect(original2, effectData, true);
                                        RoR2.EffectManager.SpawnEffect(original3, effectData4, true);

                                        RoR2.EffectManager.SpawnEffect(original35, effectData2, true);
                                        var position3 = base.characterBody.footPosition;
                                        RoR2.EffectData effectData3 = new RoR2.EffectData();
                                        effectData3.origin = position3;
                                        effectData3.scale = 6f + (FuryTapPower * 0.3f);
                                        RoR2.EffectManager.SpawnEffect(original4, effectData3, true);


                                    }
                                };
                            };

                            break;
                        }
                    case EldritchFury.ComboState.Punch2:
                        {
                            LordZot.LordZot.LastSwingArm = false;
                            bool flageffect1 = this.stopwatch >= this.attackDuration * 0.55f && !this.hasSwung;
                            if (flageffect1)
                            {

                                ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();


                                if (component)
                                {

                                    GameObject original = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                                    GameObject original2 = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                                    GameObject original4 = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");

                                    Transform transform = component.FindChild("LeftHand");

                                    /* RoR2.EffectData sonicboomeffectdata22 = new RoR2.EffectData();
                                     GameObject sonicboom22 = Resources.Load<GameObject>("prefabs/effects/impacteffects/impactmercswing");
                                     var direc22 = transform.rotation * Quaternion.Euler(90f, 0f, 0);
                                     sonicboomeffectdata22.scale = 3f;
                                     sonicboomeffectdata22.rotation = direc22;
                                     sonicboomeffectdata22.origin = transform.position;
                                     RoR2.EffectManager.SpawnEffect(sonicboom22, sonicboomeffectdata22, true);*/
                                    if (LordZot.LordZot.momentum > 100)
                                    {
                                        var direc2 = transform.rotation * Quaternion.Euler(90f, 0f, 0);
                                        GameObject sonicboom2 = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/parentblinkeffect");
                                        RoR2.EffectData sonicboomeffectdata2 = new RoR2.EffectData();

                                        sonicboomeffectdata2.scale = 3f;
                                        sonicboomeffectdata2.rotation = direc2;
                                        sonicboomeffectdata2.origin = transform.position;
                                        RoR2.EffectManager.SpawnEffect(sonicboom2, sonicboomeffectdata2, true);
                                    }
                                    if (transform)
                                    {
                                        var position = transform.position;
                                        RoR2.EffectData effectData = new RoR2.EffectData();
                                        effectData.origin = position;
                                        effectData.scale = 6f + (FuryTapPower * 0.3f);
                                        RoR2.EffectManager.SpawnEffect(original, effectData, true);

                                        RoR2.EffectData effectData2 = new RoR2.EffectData();
                                        effectData2.origin = position;
                                        effectData2.scale = 18f + (FuryTapPower * 0.3f);

                                        RoR2.EffectManager.SpawnEffect(original2, effectData2, true);
                                        var position2 = base.characterBody.footPosition;
                                        RoR2.EffectData effectData3 = new RoR2.EffectData();
                                        effectData3.origin = position2;
                                        effectData3.scale = 6f + (FuryTapPower * 0.3f);
                                        RoR2.EffectManager.SpawnEffect(original4, effectData3, true);


                                    }
                                };
                            };
                            break;
                        }
                }


            }







            bool flag23 = base.isAuthority && this.stopwatch >= this.attackDuration * 0.55f;
            if (flag23)
            {
                bool flag24 = !this.hasSwung;
                if (flag24)
                {



                    //RoR2.EffectData effectData = new RoR2.EffectData();
                    var position = base.characterBody.corePosition + (base.characterDirection.forward * 6f); //Two units in front of player
                                                                                                             //  effectData.origin = position;
                                                                                                             //  effectData.scale = 11f;
                                                                                                             //   RoR2.EffectManager.SpawnEffect(this.explodePrefab, effectData, false);
                    RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();

                    LandingBlast.losType = RoR2.BlastAttack.LoSType.NearestHit;
                    LandingBlast.attacker = characterBody.gameObject;
                    LandingBlast.inflictor = characterBody.gameObject;
                    LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
                    LandingBlast.baseDamage = (40 * 8f * FuryTapPower) + (LordZot.LordZot.momentum * 1f);
                    LandingBlast.baseForce = (3500f + FuryTapPower * 15);
                    LandingBlast.bonusForce = GetAimRay().direction + characterBody.characterDirection.forward * (LordZot.LordZot.momentum * 10);
                    LandingBlast.position = position;
                    LandingBlast.radius = (21f + FuryTapPower * 0.3f);
                    LandingBlast.crit = characterBody.RollCrit();
                    LandingBlast.attackerFiltering = RoR2.AttackerFiltering.NeverHitSelf;
                    LandingBlast.Fire();
                    this.shakeEmitter = transform.gameObject.AddComponent<RoR2.ShakeEmitter>();
                    this.shakeEmitter.wave = new Wave
                    {
                        amplitude = 0.4f,
                        frequency = 180f,
                        cycleOffset = 0f
                    };
                    this.shakeEmitter.duration = 0.4f;
                    this.shakeEmitter.radius = 50f;
                    this.shakeEmitter.amplitudeTimeDecay = true;
                    GameObject sonicboom = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/SonicBoomEffect");
                    RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();
                        LordZot.LordZot.DisplayMomentum = true;
                    Quaternion randomSpin = Quaternion.AngleAxis(UnityEngine.Random.Range(-100f, 100f), Vector3.forward);
                    var direc = modelTransform.rotation;
                    sonicboomeffectdata.scale = 3f;
                    sonicboomeffectdata.rotation = direc * randomSpin;
                    sonicboomeffectdata.origin = modelTransform.position + modelTransform.forward * -2f + modelTransform.up * 0f;
                    RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);

                    // Debug.Log("Momentum:" + momentum.ToString());

                    hasSwung = true;

                }
            }

        }

        if (hasSwung && stopwatch <= this.attackDuration * 0.7f)
        {
            GameObject sonicboom = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/SonicBoomEffect");
            RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();



            Quaternion randomSpin = Quaternion.AngleAxis(UnityEngine.Random.Range(-100f, 100f), Vector3.forward);
            var direc = modelTransform.rotation;
            sonicboomeffectdata.scale = 3f;
            sonicboomeffectdata.rotation = direc * randomSpin;
            sonicboomeffectdata.origin = modelTransform.position + modelTransform.forward * -2f + modelTransform.up * 0f;
            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
            randomSpin = Quaternion.AngleAxis(UnityEngine.Random.Range(-100f, 100f), Vector3.forward);
            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
        }

        bool flag26 = base.inputBank.skill1.down && this.comboState != (EldritchFury.ComboState.Punch2) && this.stopwatch >= this.attackDuration * 0.65f;
        if (flag26 && base.isAuthority)
        {

            if (LordZot.LordZot.Gemdrain <= 0f)
            {
                AkSoundEngine.DynamicSequenceStop(mferofgod);
                AkSoundEngine.StopPlayingID(mferofgod);
                skillLocator.primary.Reset();
            }
            this.outer.SetNextStateToMain();
            LordZot.LordZot.timeRemaining = 6.0f;

        }
        bool flag2555 = base.inputBank.skill1.down && this.comboState != (EldritchFury.ComboState.Punch1) && this.stopwatch >= this.attackDuration * 0.61f;
        if (flag2555)
        {
            LordZot.LordZot.LastSwingArm = false;
            if (LordZot.LordZot.Gemdrain <= 0f)
            {
                AkSoundEngine.DynamicSequenceStop(mferofgod);
                AkSoundEngine.StopPlayingID(mferofgod);
                skillLocator.primary.Reset();
            }

            this.outer.SetNextStateToMain();
            LordZot.LordZot.timeRemaining = 6.0f;

        }
        bool flag27 = this.stopwatch >= this.attackDuration;
        if (flag27)
        {
            LordZot.LordZot.Busy = false;

            this.outer.SetNextStateToMain();

        }
        if (this.stopwatch >= this.attackDuration * 0.3f && modelAnimator.GetFloat("ShieldsIn") < 0.1f)
        {
            LordZot.LordZot.floatpower += 8f;
            GameObject original = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
            GameObject original2 = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
            EffectSettings7 = original.GetComponent<RoR2.EffectComponent>();
            EffectSettings7.applyScale = true;
           // EffectSettings7.soundName.IsNullOrWhiteSpace();
            EffectSettings7.parentToReferencedTransform = true;
            EffectSettings = original2.GetComponent<RoR2.EffectComponent>();
            EffectSettings.applyScale = true;
           // EffectSettings.soundName.IsNullOrWhiteSpace();
            EffectSettings.parentToReferencedTransform = true;
            RoR2.EffectData effectData = new RoR2.EffectData();
            effectData.origin = LordZot.LordZot.lefthand.position;
            effectData.scale = 1f;
            RoR2.EffectManager.SpawnEffect(original, effectData, true);

            RoR2.EffectData effectData2 = new RoR2.EffectData();
            effectData.origin = LordZot.LordZot.righthand.position;
            effectData.scale = 1f;
            RoR2.EffectManager.SpawnEffect(original2, effectData2, true);




            LordZot.LordZot.ticker2 = 2f;
            LordZot.LordZot.timeRemaining = 6.0f;

            modelAnimator.SetFloat("ShieldsIn", 0.12f);



        }
        if (this.stopwatch >= this.attackDuration * 0.3f && !this.hasSwung && base.isAuthority)
        {


            if (this.comboState == ComboState.Punch1)
            {
                RoR2.EffectData effectData22 = new RoR2.EffectData
                {
                    scale = 0.3f + (FuryTapPower * 0.01f),
                    origin = LordZot.LordZot.righthand.position,

                };

                GameObject gameObject5 = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
                EffectSettings2 = gameObject5.GetComponent<RoR2.EffectComponent>();
                EffectSettings3 = gameObject5.GetComponent<RoR2.ShakeEmitter>();


                EffectSettings3.enabled = false;
                EffectSettings2.applyScale = true;
                EffectSettings2.parentToReferencedTransform = false;
                RoR2.EffectManager.SpawnEffect(gameObject5, effectData22, true);


            }
            else
            {

                RoR2.EffectData effectData3 = new RoR2.EffectData
                {
                    scale = 0.5f + (FuryTapPower * 0.01f),

                    origin = LordZot.LordZot.lefthand.position
                };
                //    leftpunch.GetComponent<RoR2.EffectComponent>().applyScale = true;
                //    leftpunch.GetComponent<RoR2.ShakeEmitter>().enabled = false;
                RoR2.EffectManager.SpawnEffect(LordZot.LordZot.leftpunch, effectData3, true);
            };
        }

        bool inaitrue = LordZot.LordZot.inair is true;
        bool airq = characterMotor.isGrounded;
        if (airq && inaitrue)
        {

            if (this.stopwatch >= this.attackDuration * 0.1f)
            {
                if (isAuthority)
                {
                    switch (this.comboState)
                    {
                        case EldritchFury.ComboState.Punch1:
                            {
                                bool flageffect1 = this.stopwatch >= this.attackDuration * 0.1f && !this.hasSwung;
                                if (flageffect1)
                                {
                                    hasSwung = true;
                                    ChildLocator component = LordZot.LordZot.component;


                                    if (component)
                                    {


                                        GameObject original25 = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                                        GameObject original35 = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                                        GameObject original45 = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");
                                        Transform transform2 = component.FindChild("RightHand");

                                        if (transform2)
                                        {
                                            /*    Color32 color = new Color32();
                                                color.a = 255;
                                                color.r = 255;
                                                color.g = 0;
                                                color.b = 0;*/

                                            var position3 = base.characterBody.corePosition + (base.characterDirection.forward * 6f);
                                            RoR2.EffectData effectData = new RoR2.EffectData();
                                            effectData.origin = transform2.position;
                                            effectData.scale = 18f + (FuryTapPower * 0.3f);
                                            //  effectData.color = color;

                                            RoR2.EffectData effectData2 = new RoR2.EffectData();
                                            effectData2.origin = transform2.position;
                                            effectData2.scale = 3f + (FuryTapPower * 0.3f);
                                            // effectData2.color = color;
                                            RoR2.EffectManager.SpawnEffect(original25, effectData, true);
                                            RoR2.EffectManager.SpawnEffect(original35, effectData2, true);
                                            var position2 = base.characterBody.footPosition;
                                            RoR2.EffectData effectData3 = new RoR2.EffectData();
                                            effectData3.origin = position2;
                                            effectData3.scale = 6f + (FuryTapPower * 0.3f);
                                            RoR2.EffectManager.SpawnEffect(original45, effectData3, true);
                                        }
                                    };
                                };

                                break;
                            }
                        case EldritchFury.ComboState.Punch2:
                            {
                                bool flageffect1 = this.stopwatch >= this.attackDuration * 0.1f && !this.hasSwung;
                                if (flageffect1)
                                {

                                    ChildLocator component = LordZot.LordZot.component;

                                    hasSwung = true;
                                    if (component)
                                    {

                                        GameObject originaly = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                                        GameObject original22 = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                                        GameObject original44 = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");

                                        Transform transform = component.FindChild("LeftHand");

                                        if (transform)
                                        {
                                            var position3 = base.characterBody.corePosition + (base.characterDirection.forward * 6f);
                                            RoR2.EffectData effectData = new RoR2.EffectData();
                                            effectData.origin = transform.position;
                                            effectData.scale = 6f + (FuryTapPower * 0.3f);
                                            RoR2.EffectManager.SpawnEffect(originaly, effectData, true);

                                            RoR2.EffectData effectData2 = new RoR2.EffectData();
                                            effectData2.origin = transform.position;
                                            effectData2.scale = 18f + (FuryTapPower * 0.3f);

                                            RoR2.EffectManager.SpawnEffect(original22, effectData2, true);
                                            var position2 = base.characterBody.footPosition;
                                            RoR2.EffectData effectData3 = new RoR2.EffectData();
                                            effectData3.origin = position2;
                                            effectData3.scale = 6f + (FuryTapPower * 0.3f);
                                            RoR2.EffectManager.SpawnEffect(original44, effectData3, true);
                                        }
                                    };
                                };
                                break;
                            }
                    }
                };
                //  Debug.Log("Momentum:" + momentum.ToString());
                var position = base.characterBody.corePosition + (base.characterDirection.forward * 6f);
                RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();

                LandingBlast.losType = RoR2.BlastAttack.LoSType.NearestHit;
                LandingBlast.attacker = characterBody.gameObject;
                LandingBlast.inflictor = characterBody.gameObject;
                LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
                LandingBlast.baseDamage = (40 * 8f * FuryTapPower) + (LordZot.LordZot.momentum * 1f);
                LandingBlast.baseForce = (3500f + FuryTapPower * 15);
                LandingBlast.bonusForce = GetAimRay().direction + characterBody.characterDirection.forward * (LordZot.LordZot.momentum * 10);
                LandingBlast.position = position;
                LandingBlast.radius = 21f + (FuryTapPower * 0.3f);
                LandingBlast.crit = characterBody.RollCrit();
                LandingBlast.attackerFiltering = RoR2.AttackerFiltering.NeverHitSelf;

                LandingBlast.Fire();
                LordZot.LordZot.DisplayMomentum = true;

                this.shakeEmitter = transform.gameObject.AddComponent<RoR2.ShakeEmitter>();
                this.shakeEmitter.wave = new Wave
                {
                    amplitude = 0.4f,
                    frequency = 180f,
                    cycleOffset = 0f
                };
                this.shakeEmitter.duration = 0.4f;
                this.shakeEmitter.radius = 50f;
                this.shakeEmitter.amplitudeTimeDecay = true;
            };
            if (isAuthority)
            {
                LordZot.LordZot.inair = false;
                this.outer.SetNextStateToMain();
                return;
            }
        }
        if (!airq && base.isAuthority)
        { LordZot.LordZot.inair = true; };
    }
    bool flag17 = this.stopwatch >= this.attackDuration * 0.55f && !this.hasSwung;
    if (flag17)
    {
        this.hasSwung = true;
            LordZot.LordZot.momentum = 0f;
        if (LordZot.LordZot.Gemdrain <= 0f)
        {
            skillLocator.primary.Reset();
        }

    }
}



// Token: 0x06000070 RID: 112 RVA: 0x000069F0 File Offset: 0x00004BF0
public override InterruptPriority GetMinimumInterruptPriority()
{
    return InterruptPriority.Death;
}

// Token: 0x06000071 RID: 113 RVA: 0x00006A03 File Offset: 0x00004C03
public override void OnSerialize(UnityEngine.Networking.NetworkWriter writer)
{
    base.OnSerialize(writer);
    writer.Write((byte)this.comboState);
}

// Token: 0x06000072 RID: 114 RVA: 0x00006A1C File Offset: 0x00004C1C
public override void OnDeserialize(UnityEngine.Networking.NetworkReader reader)
{
    base.OnDeserialize(reader);
    this.comboState = (EldritchFury.ComboState)reader.ReadByte();
}


// Token: 0x04000093 RID: 147
public static float comboDamageCoefficient = 6f;

// Token: 0x04000096 RID: 150
public static float hitHopVelocity = 5f;

public static float hitPauseDuration = 0.15f;
public static float forceMagnitude = 16000f;
public static float attackspeedaltsoundthreshold = 3f;

// Token: 0x04000099 RID: 153
private float stopwatch;

// Token: 0x0400009A RID: 154
private float attackDuration = 1f;

// Token: 0x0400009B RID: 155
private float earlyExitDuration;

// Token: 0x0400009C RID: 156
private Animator animator;
public static float recoilAmplitude = 7f;
// Token: 0x0400009D RID: 157
private RoR2.OverlapAttack overlapAttack;

// Token: 0x0400009E RID: 158
private float hitPauseTimer;
public GameObject explodePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
// Token: 0x0400009F RID: 159
private bool isInHitPause;
private RoR2.ShakeEmitter shakeEmitter;
// Token: 0x040000A0 RID: 160
private bool hasSwung;
private RoR2.Tracer tracer;
// Token: 0x040000A1 RID: 161
private bool hasHit;
private RoR2.EffectData rightponch;
// Token: 0x040000A2 RID: 162
private bool hasHopped;
private Animator modelAnimator;
private Transform modelTransform;

// Token: 0x040000A3 RID: 163
public EldritchFury.ComboState comboState;
public static GameObject tracerEffectPrefab = EntityStates.GolemMonster.FireLaser.tracerEffectPrefab;
// Token: 0x040000A5 RID: 165
private BaseState.HitStopCachedState hitStopCachedState;

// Token: 0x040000A6 RID: 166
private GameObject swingEffectPrefab;

// Token: 0x040000A7 RID: 167
private GameObject hitEffectPrefab;

// Token: 0x040000A8 RID: 168
private string attackSoundString;


// Token: 0x02000025 RID: 37
public enum ComboState
{
    // Token: 0x040001C1 RID: 449
    Punch1,
    // Token: 0x040001C2 RID: 450
    Punch2,
}

// Token: 0x02000026 RID: 38
private struct ComboStateInfo
{
    // Token: 0x040001C4 RID: 452
    private string mecanimStateName;

    // Token: 0x040001C5 RID: 453
    private string mecanimPlaybackRateName;
}
public static float baseComboAttackDuration = 1f;

// Token: 0x0400317B RID: 12667
public static float baseEarlyExitDuration = 0.1f;
private RoR2.Tracer trac;
private TracerBehavior that;
private bool didntstartgemdrained;
private RoR2.EffectComponent EffectSettings2;
private RoR2.ShakeEmitter EffectSettings3;
private RoR2.EffectComponent EffectSettings;
private RoR2.EffectComponent EffectSettings7;
private uint soundID;
private uint mferofgod;
private bool soundplayed;
private uint mfer2;
    private float timeRemaining;

    public float FuryTapPower { get; private set; }

}