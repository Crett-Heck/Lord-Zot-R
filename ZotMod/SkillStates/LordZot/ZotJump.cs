using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using UnityEngine;

namespace LordZot.SkillStates
{
    class ZotJump : BaseSkillState
    {
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            //  Debug.Log("ZotJump entered");
            LordZot.Charged = 0;
            LordZot.Chargingjump = true;
            this.stopwatch = 0f;
            this.Duration = this.stopwatch + 1f;
            LordZot.jumpcooldown = 5f;
            var position = characterBody.footPosition;
            LordZot.timeRemainingstun = 0f;

            RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();

            LandingBlast.losType = BlastAttack.LoSType.NearestHit;
            LandingBlast.attacker = base.gameObject;
            LandingBlast.inflictor = base.gameObject;
            LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
            LandingBlast.baseDamage = 40 * 1f;
            LandingBlast.baseForce = 1000f + (LordZot.mass);
            LandingBlast.bonusForce = new Vector3(0f, 2500f + LordZot.mass * 2, 0f);
            LandingBlast.position = position;
            LandingBlast.procCoefficient = 0f;
            LandingBlast.radius = 25f;
            LandingBlast.crit = base.RollCrit();
            LandingBlast.attackerFiltering = AttackerFiltering.NeverHitSelf;

            LandingBlast.Fire();

            GameObject original6 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
            RoR2.EffectData effectData2 = new RoR2.EffectData();

            effectData2.origin = position;
            effectData2.scale = 25f;
            RoR2.EffectManager.SpawnEffect(original6, effectData2, true);

            this.modelAnimator = base.GetModelAnimator();

            this.modelTransform = base.GetModelTransform();
            GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");
            RoR2.EffectManager.SimpleEffect(original2, characterBody.footPosition, Quaternion.Euler(0f, 0f, 0f), true);
            /*  if (base.isAuthority);
              { }*/

        }

        public override void OnExit()
        {
            base.PlayAnimation("Body", "Idle", null, 0f);
            LordZot.Chargingjump = false;
            base.OnExit();
            // characterMotor.enabled = true;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.PlayAnimation("Body", "ChargeJump", "JumpChargeDuration", 100f);
            this.stopwatch += Time.fixedDeltaTime;
            this.updatewatch += Time.fixedDeltaTime;
            bool outofpower = LordZot.GemPowerVal <= 0;
            bool flag26 = base.inputBank.jump.down;
            bool flag266 = base.inputBank.jump.justReleased;
            bool flag27 = this.stopwatch >= this.Duration;
            bool flag29 = this.updatewatch >= 0.3f;
            this.Duration = this.stopwatch + 100;
            if (LordZot.floatpower < LordZot.Charged)
            { LordZot.floatpower += LordZot.Charged * 0.02f; };
            LordZot.floatpower += Time.fixedDeltaTime;
            LordZot.jumpcooldown = this.Duration;
            if (flag29)
            {

                RoR2.ShakeEmitter shakeEmitter;
                shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                shakeEmitter.wave = new Wave
                {
                    amplitude = 0.009f * Charged,
                    frequency = 180f,
                    cycleOffset = 0f
                };
                shakeEmitter.duration = 0.3f;
                shakeEmitter.radius = 50f;
                shakeEmitter.amplitudeTimeDecay = false;
                this.updatewatch = 0f;
                GameObject original6 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");

                RoR2.EffectData effectData = new RoR2.EffectData();

                effectData.origin = characterBody.footPosition;
                effectData.scale = 0.3f * this.stopwatch + 1f;


                RoR2.EffectManager.SpawnEffect(original6, effectData, true);
                RoR2.EffectManager.SpawnEffect(original6, effectData, true);

            }

            if (characterMotor.isGrounded)
            {
                //  characterMotor.enabled = false;
                characterMotor.velocity = Vector3.zero;
            };

            if (LordZot.Charged > 5f && flag29)
            {

                GameObject original6 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");
                new RoR2.BlastAttack
                {
                    losType = BlastAttack.LoSType.NearestHit,
                    attacker = base.gameObject,
                    baseDamage = 1 * 0.008f * LordZot.Charged,
                    baseForce = 75 * LordZot.Charged,
                    bonusForce = new Vector3(0f, 150f * LordZot.Charged, 0f),
                    crit = false,
                    damageType = DamageType.Generic,
                    falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                    procCoefficient = 0,
                    radius = 13f + LordZot.Charged * 0.09f,
                    position = LordZot.model.corePosition,
                    attackerFiltering = AttackerFiltering.NeverHitSelf,
                    teamIndex = base.teamComponent.teamIndex
                }.Fire();
                RoR2.EffectData effectData = new RoR2.EffectData();

                effectData.origin = characterBody.footPosition;
                effectData.scale = 0.9f * this.stopwatch;
                RoR2.EffectManager.SpawnEffect(original2, effectData, true);
                RoR2.EffectManager.SpawnEffect(original6, effectData, true);

            }

            if (LordZot.Charged > 10f && flag29)
            {
                new RoR2.BlastAttack
                {
                    losType = BlastAttack.LoSType.NearestHit,
                    attacker = base.gameObject,
                    baseDamage = 1 * 0.88f * LordZot.Charged,
                    baseForce = 250,
                    bonusForce = new Vector3(0f, 150f, 0f),
                    crit = false,
                    damageType = DamageType.Generic,
                    falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                    procCoefficient = 0,
                    radius = 13f + LordZot.Charged * 0.09f,
                    position = LordZot.model.corePosition,
                    attackerFiltering = AttackerFiltering.NeverHitSelf,
                    teamIndex = base.teamComponent.teamIndex
                }.Fire();
                GameObject original7 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                GameObject original6 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/TitanSpawnEffect");
                GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");

                RoR2.EffectData effectData = new RoR2.EffectData();

                effectData.origin = characterBody.footPosition;
                effectData.scale = 0.15f * this.stopwatch;
                RoR2.EffectManager.SpawnEffect(original7, effectData, true);
                RoR2.EffectManager.SpawnEffect(original2, effectData, true);
                RoR2.EffectManager.SpawnEffect(original6, effectData, true);

            }

            if (outofpower && flag26)
            {
                LordZot.Gemdrain = 1f;
                LordZot.Chargingjump = false;
                LordZot.jumpcooldown = 0.4f;
                BaseLeap zotLeap = new BaseLeap();

                { LordZot.Charged += this.stopwatch * 1; }

                this.outer.SetNextState(zotLeap);

                return;
            }
            if (flag27)
            {
                PlayCrossfade("Body", "Idle", null, 1f, 0.2f);

                LordZot.jumpcooldown = 0f;
                LordZot.Chargingjump = false;
                BaseLeap zotLeap = new BaseLeap();
                { LordZot.Charged += this.stopwatch * 1; }
                this.outer.SetNextState(zotLeap);

                return;

            }
            if (this.stopwatch > 1f && this.stopwatch < 1.1f)
            {


                GameObject original6 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                RoR2.EffectData effectData = new RoR2.EffectData();

                effectData.origin = characterBody.footPosition;
                effectData.scale = 5f;
                RoR2.EffectManager.SpawnEffect(original6, effectData, true);
            }
            if (this.stopwatch > 5f && this.stopwatch < 5.1f)
            {
                new RoR2.BlastAttack
                {
                    losType = BlastAttack.LoSType.NearestHit,
                    attacker = base.gameObject,
                    baseDamage = 15 * 1f * LordZot.Charged,
                    baseForce = 880 + (30f * LordZot.Charged),
                    bonusForce = new Vector3(0f, 300f * LordZot.Charged, 0f),
                    crit = false,
                    damageType = DamageType.Generic,
                    falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                    procCoefficient = 0,
                    radius = 13f + LordZot.Charged * 0.09f,
                    position = LordZot.model.corePosition,
                    attackerFiltering = AttackerFiltering.NeverHitSelf,
                    teamIndex = base.teamComponent.teamIndex
                }.Fire();
                GameObject original6 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");

                RoR2.EffectData effectData = new RoR2.EffectData();

                effectData.origin = characterBody.footPosition;
                effectData.scale = 1f * this.stopwatch;
                RoR2.EffectManager.SpawnEffect(original2, effectData, true);
                RoR2.EffectManager.SpawnEffect(original6, effectData, true);
                RoR2.EffectManager.SpawnEffect(original6, effectData, true);

            }
            if (flag266 && this.stopwatch >= 1f)
            {
                LordZot.Chargingjump = false;
                LordZot.timeRemainingstun = 0f;
                BaseLeap zotLeap = new BaseLeap();
                { LordZot.Charged += this.stopwatch * 1; }
                this.outer.SetNextState(zotLeap);
            }

            if (flag266 && this.stopwatch < 1f)
            {

                LordZot.jumpcooldown = 0f;
                LordZot.Chargingjump = false;
                BaseLeap zotLeap = new BaseLeap();
                {
                    LordZot.Charged += this.stopwatch * 1;
                }
                this.outer.SetNextState(zotLeap);

                return;
            }



        }









        private float stopwatch;
        private Animator modelAnimator;
        private float Duration;
        private Transform modelTransform;
        private float updatewatch;

        public float Charged { get; private set; }
    }
}
