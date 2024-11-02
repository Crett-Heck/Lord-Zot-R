using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using UnityEngine;
using EntityStates.ParentMonster;
namespace LordZot.SkillStates
{
    class BaseLeap : BaseSkillState
    {
        public override void OnEnter()
        {
            // Debug.Log("BaseLeap entered");

            base.OnEnter();
            base.characterDirection.moveVector += base.inputBank.moveVector;
            base.characterMotor.moveDirection += base.inputBank.moveVector;
            applyx = false;
            LordZot.Chargingjump = false;
            LordZot.floatpower += LordZot.Charged;
            LordZot.flightcooldown = 0.1f;
            LordZot.timeRemainingstun = 0f;
            this.animator = base.GetModelAnimator();
            this.modelTransform = base.GetModelTransform();
            this.stopwatch = 0f;
            this.counte = 0f;

            this.previousaccel = 700;
            LordZot.ticker2 = 2f;

            BaseLeap.previous = 3f;

            Vector3 direction = base.GetAimRay().direction;





            direction.y = Mathf.Max(direction.y, BaseLeap.minimumY);
            Vector3 b = Vector3.up * LordZot.Charged * 3f;
            Vector3 b2 = Vector3.up * LordZot.Charged * 4.5f;

            Vector3 b3 = new Vector3(base.characterDirection.moveVector.x, 0f, base.characterDirection.moveVector.z) * (LordZot.Charged * 10);

            base.characterMotor.Motor.ForceUnground();

            base.characterMotor.velocity += b + b2 + (Vector3.up * 22f);


            base.GetModelTransform().GetComponent<RoR2.AimAnimator>().enabled = true;
            RoR2.Util.PlaySound(BaseLeap.leapSoundString, base.gameObject);

            base.PlayAnimation("Body", "TitanJump", "forwardSpeed", 1f);

            RoR2.Util.PlaySound(BaseLeap.soundLoopStartEvent, base.gameObject);
            Vector3 footPosition = base.characterBody.footPosition;
            new RoR2.BlastAttack
            {
                losType = BlastAttack.LoSType.NearestHit,
                attacker = base.gameObject,
                baseDamage = 40 * 0.08f * LordZot.Charged,
                baseForce = 0,
                bonusForce = new Vector3(0f, 150f * LordZot.Charged, 0f),
                crit = this.isCritAuthority,
                damageType = DamageType.Generic,
                falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                procCoefficient = 1,
                radius = 13f + LordZot.Charged * 0.09f,
                position = footPosition,
                attackerFiltering = AttackerFiltering.NeverHitSelf,
                teamIndex = base.teamComponent.teamIndex
            }.Fire();
            var position = this.characterBody.footPosition;
            RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();

            sonicboomeffectdata.scale = 3f + LordZot.Charged;
            sonicboomeffectdata.rotation = Quaternion.LookRotation(LordZot.motor.Motor.CharacterUp);
            sonicboomeffectdata.origin = position;
            GameObject sonicboom = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/SonicBoomEffect");
            RoR2.EffectManager.SpawnEffect(LoomingPresence.blinkPrefab, sonicboomeffectdata, true);
            RoR2.EffectManager.SpawnEffect(LoomingPresence.blinkPrefab, sonicboomeffectdata, true);
            RoR2.EffectManager.SpawnEffect(LoomingPresence.blinkPrefab, sonicboomeffectdata, true);
            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);


            GameObject original22 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");
            GameObject original6 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
            GameObject original = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/PodGroundImpact");
            GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/ScavSitImpact");
            //  EffectSettings1 = original22.GetComponent<RoR2.EffectComponent>();
            //  EffectSettings1.applyScale = true;
            //  EffectSettings2 = original2.GetComponent<RoR2.EffectComponent>();
            // EffectSettings2.applyScale = true;

            //  EffectSettings3 = original6.GetComponent<RoR2.EffectComponent>();
            //  EffectSettings3.applyScale = true;
            //  EffectSettings4 = original.GetComponent<RoR2.EffectComponent>();
            //  EffectSettings4.applyScale = true;
            RoR2.EffectData effectData = new RoR2.EffectData();

            effectData.origin = position;
            effectData.scale = 0.5f * LordZot.Charged;
            RoR2.EffectManager.SpawnEffect(EntityStates.LemurianBruiserMonster.SpawnState.spawnEffectPrefab, effectData, true);

            RoR2.EffectManager.SpawnEffect(original2, effectData, true);
            RoR2.EffectManager.SpawnEffect(original6, effectData, true);
            RoR2.EffectManager.SpawnEffect(original22, effectData, true);

            if (LordZot.Charged > 4.5f)
            {
                RoR2.EffectManager.SpawnEffect(EntityStates.TitanMonster.DeathState.initialEffect, effectData, true);
                RoR2.EffectManager.SpawnEffect(original, effectData, true);
            }

        }


        // Token: 0x06003DF6 RID: 15862 RVA: 0x0010242C File Offset: 0x0010062C
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.characterMotor)
            {
                counte += Time.fixedDeltaTime;

                bool utilityleap = inputBank.skill3.down;
                if (utilityleap)
                {
                    LordZot.jumpcooldown = 0f;

                    TitanStride zotblinkleap = new TitanStride();
                    this.outer.SetState(zotblinkleap);

                };



                if (this.stopwatch >= 0)
                { this.stopwatch -= Time.fixedDeltaTime; };

                if (/*LordZot.LordZot.timeRemainingstun <= 0 &&*/ !LordZot.wellhefell)
                {

                    bool primaryleap = inputBank.skill1.down;
                    bool secondaryleap = inputBank.skill2.down;
                    bool specialleap = inputBank.skill4.down;
                    if (secondaryleap)
                    {
                        LordZot.timeRemaining = 6f;
                        Animator modelAnimator = base.GetModelAnimator();
                        modelAnimator.SetFloat("ShieldsIn", 0.12f);
                        this.modelTransform = base.GetModelTransform();
                        ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();


                        LordZot.jumpcooldown = 0f;

                        LordZot.ugh = true;
                        GemBulwark bulwarkleap = new GemBulwark();
                        this.outer.SetState(bulwarkleap);


                    };

                    if (specialleap && !LordZot.Busy)
                    {
                        LordZot.timeRemaining = 6f;
                        Animator modelAnimator = base.GetModelAnimator();
                        modelAnimator.SetFloat("ShieldsIn", 0.12f);
                        this.modelTransform = base.GetModelTransform();
                        ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();


                        LordZot.jumpcooldown = 0f;

                        EldritchSlam eldritchslamleap = new EldritchSlam();

                        this.outer.SetState(eldritchslamleap);



                    };

                    if (!specialleap && LordZot.slammingair)
                    {
                        base.PlayCrossfade("Gesture, Additive", "FireArrow", "FireArrow.playbackRate", 0.4f, 0.1f);

                        LordZot.timeRemaining = 6f;
                        Animator modelAnimator = base.GetModelAnimator();
                        modelAnimator.SetFloat("ShieldsIn", 0.12f);
                        this.modelTransform = base.GetModelTransform();
                        ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();


                        LordZot.jumpcooldown = 0f;

                        SkillStates.ZotSlam slamleap = new ZotSlam();
                        this.outer.SetState(slamleap);






                    };

                    if (primaryleap)
                    {
                        LordZot.timeRemaining = 6f;
                        Animator modelAnimator = base.GetModelAnimator();
                        modelAnimator.SetFloat("ShieldsIn", 0.12f);
                        this.modelTransform = base.GetModelTransform();
                        ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();


                        LordZot.jumpcooldown = 0f;

                        EldritchFury eldritchfuryleap = new EldritchFury();
                        this.outer.SetState(eldritchfuryleap);



                    };

                }
                if (!characterMotor.isGrounded)
                {
                    if (counte < 0.1f)
                    {
                        base.PlayAnimation("Body", "TitanJump", "forwardSpeed", 1f);

                    }
                    if (counte > 1f)
                    {
                        if (LordZot.Charged > 10)
                        {
                            // base.characterMotor.moveDirection += base.inputBank.moveVector * 1f;
                            characterMotor.velocity += base.inputBank.moveVector * LordZot.Charged * 0.5f;
                        }
                        else
                        {

                            //  base.characterMotor.moveDirection += base.inputBank.moveVector * 1f;
                            characterMotor.velocity += base.inputBank.moveVector * LordZot.Charged * 0.25f;
                        }

                    }
                    else
                    {
                        if (LordZot.Charged > 10)
                        {
                            // base.characterMotor.moveDirection += base.inputBank.moveVector * 1f;
                            characterMotor.velocity += base.inputBank.moveVector * 3f;
                        }
                        else
                        {

                            // base.characterMotor.moveDirection += base.inputBank.moveVector * 1f;
                            characterMotor.velocity += base.inputBank.moveVector * LordZot.Charged * 0.25f;
                        }
                    };




                };
                /*  if (characterBody.baseAcceleration > 35)

                  { base.characterBody.baseAcceleration = base.characterBody.baseAcceleration * 0.97f; };
                  if (characterBody.baseAcceleration < 100 && !LordZot.LordZot.flight && !characterMotor.isGrounded)
                  { characterMotor.velocity.y -= 1.2f; };*/
                if (LordZot.flight)
                {

                    LordZot.Charged = 0f;
                    this.outer.SetNextStateToMain();
                };

                if (base.fixedAge >= BaseLeap.minimumDuration && ((base.characterMotor.Motor.GroundingStatus.IsStableOnGround && !base.characterMotor.Motor.LastGroundingStatus.IsStableOnGround)))
                {


                    if (!LordZot.wellhefell)
                    {
                        if (!LordZot.Busy)
                        {
                            base.PlayAnimation("Body", "AscendDescend 2", null, 1f);
                        };
                        LordZot.wellhefell = true;
                        this.stopwatch = 1.6f;
                        base.characterMotor.velocity = Vector3.zero;
                        //    base.characterMotor.Motor.enabled = false;
                        //    characterMotor.enabled = false;
                    };
                    LordZot.jumpcooldown = 0f;

                    LordZot.Charged = 0f;





                }
                if ((this.stopwatch <= 0 && LordZot.wellhefell) | base.inputBank.jump.down && LordZot.wellhefell)
                {
                    //  Debug.Log("he fell");
                    //   base.characterMotor.Motor.enabled = true;
                    LordZot.wellhefell = false;
                    //    LordZot.LordZot.fallstun = false;

                    LordZot.jumpcooldown = 0f;

                    base.characterMotor.velocity = Vector3.zero;

                    LordZot.Charged = 0f;
                    //   characterMotor.enabled = true;
                    if (LordZot.slammingair)
                    {

                        LordZot.Charging = false;

                        ZotSlam zotSlam = new ZotSlam();

                        zotSlam.laserDirection = base.GetAimRay().direction;
                        this.outer.SetNextState(zotSlam);

                        return;

                    }
                    else
                    {
                        this.outer.SetNextStateToMain();
                    }
                };


            }
        }




        public override void OnExit()
        {
            RoR2.Util.PlaySound(BaseLeap.soundLoopStopEvent, base.gameObject);


            LordZot.wellhefell = false;
            //  LordZot.LordZot.fallstun = false;


            LordZot.wellhefell = false;
            //   LordZot.LordZot.fallstun = false;


            LordZot.jumpcooldown = 0f;
            applyx = false;
            LordZot.Charged = 0f;
            base.OnExit();
        }

        // Token: 0x06003DFB RID: 15867 RVA: 0x0000D472 File Offset: 0x0000B672
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        // Token: 0x040038F8 RID: 14584
        public static float minimumDuration = 0.0f;
        private float previousaccel = 700;
        public static float previous = 3f;
        // Token: 0x040038F9 RID: 14585
        public static float blastRadius;

        // Token: 0x040038FA RID: 14586
        public static float blastProcCoefficient;

        // Token: 0x040038FB RID: 14587
        [SerializeField]
        public float blastDamageCoefficient;

        // Token: 0x040038FC RID: 14588
        [SerializeField]
        public float blastForce;

        // Token: 0x040038FD RID: 14589
        public static string leapSoundString;

        // Token: 0x040038FE RID: 14590
        public static GameObject projectilePrefab;

        // Token: 0x040038FF RID: 14591
        [SerializeField]
        public Vector3 blastBonusForce;

        // Token: 0x04003900 RID: 14592
        [SerializeField]
        public GameObject blastImpactEffectPrefab;

        // Token: 0x04003901 RID: 14593
        [SerializeField]
        public GameObject blastEffectPrefab;

        // Token: 0x04003902 RID: 14594
        public static float airControl;

        // Token: 0x04003903 RID: 14595
        public static float aimVelocity;

        // Token: 0x04003904 RID: 14596
        public static float upwardVelocity;

        // Token: 0x04003905 RID: 14597
        public static float forwardVelocity;

        // Token: 0x04003906 RID: 14598
        public static float minimumY;

        // Token: 0x04003907 RID: 14599
        public static float minYVelocityForAnim;

        // Token: 0x04003908 RID: 14600
        public static float maxYVelocityForAnim;

        // Token: 0x04003909 RID: 14601
        public static float knockbackForce;

        // Token: 0x0400390A RID: 14602
        [SerializeField]
        public GameObject fistEffectPrefab;

        // Token: 0x0400390B RID: 14603
        public static string soundLoopStartEvent;

        // Token: 0x0400390C RID: 14604
        public static string soundLoopStopEvent;

        // Token: 0x0400390D RID: 14605
        public static RoR2.NetworkSoundEventDef landingSound;
        private Animator animator;
        private float stopwatch;
        private float counte;

        // Token: 0x0400390E RID: 14606
        private float previousAirControl = 1f;

        // Token: 0x0400390F RID: 14607
        private GameObject leftFistEffectInstance;

        // Token: 0x04003910 RID: 14608
        private GameObject rightFistEffectInstance;

        // Token: 0x04003911 RID: 14609
        protected bool isCritAuthority;


        // Token: 0x04003913 RID: 14611
        private bool detonateNextFrame;
        private Transform modelTransform;
        private RoR2.EffectComponent EffectSettings1;
        private RoR2.EffectComponent EffectSettings2;
        private RoR2.EffectComponent EffectSettings3;
        private RoR2.EffectComponent EffectSettings4;
        private Vector3 m2;
        private bool applyx;
    }
}
