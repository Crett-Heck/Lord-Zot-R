using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using UnityEngine;
namespace LordZot.SkillStates
{
    class ZotSlam : BaseSkillState
    {
        // Token: 0x06003C64 RID: 15460 RVA: 0x000FB714 File Offset: 0x000F9914
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("ZotSlam entered");
            LordZot.RightTrail.emitting = true;
            facingdown = false;
            hasswung = false;
            this.animator = base.GetModelAnimator();
            stopwatch21 = 0f;
            LordZot.Busy = true;

            LordZot.floatpower += LordZot.ChargedSlam;
            this.modifiedAimRay = base.GetAimRay();
            string aimraydirection = modifiedAimRay.direction.y.ToString();
            this.modifiedAimRay.direction = this.laserDirection;
            LordZot.Charging = false;
            LordZot.slammingair = false;
            // Debug.Log("aimray direction y is" + aimraydirection);
            RoR2.Util.PlaySound(EntityStates.VagrantMonster.ChargeTrackingBomb.chargingSoundString, base.gameObject);
            //animator.SetTrigger("Fury1");
            PlayCrossfade("Gesture, Additive", "FireArrow", "FireArrow.playbackRate", 1f, 0.02f);
            base.GetModelAnimator().SetBool("GodStop", true);
            //  base.GetModelAnimator().SetLayerWeight(4, 1);
            if (base.GetAimRay().direction.y < -0.7f)
            {
                facingdown = true;
            };
            if (base.GetAimRay().direction.y > -0.7)
            {
                facingdown = false;
            };

            //animator.SetTrigger("Fury1");


            string facedownornot = facingdown.ToString();
            //   Debug.Log("facing down is " + facedownornot);
        }

        // Token: 0x06003C65 RID: 15461 RVA: 0x00032FA7 File Offset: 0x000311A7
        public override void OnExit()
        {
            base.OnExit();
            LordZot.momentum = 0f;
            LordZot.RightTrail.emitting = false;
            LordZot.ChargedSlam = 0f;
            LordZot.Busy = false;
        }

        // Token: 0x06003C66 RID: 15462 RVA: 0x000FB969 File Offset: 0x000F9B69
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            string facedownornot = facingdown.ToString();
            //  Debug.Log("facing down is " + facedownornot);
            if (base.GetAimRay().direction.y < -0.7f)
            {
                facingdown = true;

            };


            if (!facingdown | hasswung)
            { this.stopwatch21 += Time.fixedDeltaTime; }
            if (facingdown & !LordZot.motor.isGrounded)
            {
                GetModelAnimator().SetFloat("FireArrow.playbackRate", 0.03f);
                LordZot.motor.velocity.y -= 1;
            };

            if (stopwatch21 < 0.31f && !hasswung)
            {
                GameObject laser2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
                /* EffectSettings2 = laser2.GetComponent<RoR2.EffectComponent>();
                 EffectSettings2.applyScale = true;
                 EffectSettings2.parentToReferencedTransform = true;*/
                RoR2.EffectData effectData = new RoR2.EffectData();
                effectData.origin = LordZot.righthand.position;
                effectData.scale = 1f;
                RoR2.EffectManager.SpawnEffect(laser2, effectData, true);
            }


            if (stopwatch21 >= 0.31f && !hasswung && !facingdown)
            {

                hasswung = true;
                RoR2.Util.PlayAttackSpeedSound(EntityStates.VagrantMonster.FireTrackingBomb.fireBombSoundString, base.gameObject, 0.87f);
                Transform modelTransform = base.GetModelTransform();
                RoR2.Util.PlaySound(EntityStates.GolemMonster.FireLaser.attackSoundString, base.gameObject);
                string text = "RightHand";
                if (base.characterBody)
                {
                    //  base.characterBody.SetAimTimer(1f);
                }
                if (effectPrefab)
                {
                    RoR2.EffectManager.SimpleMuzzleFlash(EntityStates.GolemMonster.FireLaser.effectPrefab, base.gameObject, text, true);
                }
                if (base.isAuthority)
                {
                    // Debug.Log("Momentum:" + momentum.ToString());
                    new RoR2.BlastAttack
                    {
                        losType = BlastAttack.LoSType.NearestHit,
                        attacker = base.gameObject,
                        inflictor = base.gameObject,
                        teamIndex = RoR2.TeamComponent.GetObjectTeam(base.gameObject),
                        baseDamage = 40 * 3.5f + (40 * 3.8f * LordZot.ChargedSlam) + (LordZot.momentum * 1f),
                        baseForce = 7000f,
                        position = characterBody.corePosition + characterDirection.forward * 4f + modelTransform.up * 2f,
                        radius = 9f + 0.4f * LordZot.ChargedSlam,
                        falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                        crit = characterBody.RollCrit(),
                        bonusForce = base.GetAimRay().direction + characterBody.characterDirection.forward * ((2500 * LordZot.ChargedSlam) + (LordZot.momentum * 10))
                    }.Fire();
                    LordZot.DisplayMomentum = true;

                    Vector3 origin = base.GetAimRay().origin;
                    if (modelTransform)
                    {
                        ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                        if (component)
                        {
                            int childIndex = component.FindChildIndex(text);
                            if (tracerEffectPrefab)
                            {
                                RoR2.EffectData effectData = new RoR2.EffectData
                                {
                                    scale = 1f + (2.9f * LordZot.ChargedSlam),
                                    origin = characterBody.corePosition + characterDirection.forward * 4f,
                                    start = base.GetAimRay().origin
                                };
                                effectData.SetChildLocatorTransformReference(base.gameObject, childIndex);
                                GameObject laser3 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/VagrantNovaExplosion");
                                GameObject laser2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
                                GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                                GameObject original27 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/MagmaWormImpactExplosion");
                                GameObject original277 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/ArtifactShellExplosion");
                                GameObject original2737 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/ArtifactworldPortalSpawnEffect");
                                GameObject original2777 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/BrotherSunderWaveEnergizedExplosion");
                                GameObject original66 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                                GameObject original26 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/ScavSitImpact");
                                GameObject original25 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");



                                /*   EffectSettings = original2.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings.applyScale = true;
                                   EffectSettings2 = original27.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings2.applyScale = true;
                                   EffectSettings4 = original277.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings4.applyScale = true;
                                   EffectSettings5 = original2737.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings5.applyScale = true;
                                   EffectSettings3 = original2777.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings3.applyScale = true;
   */
                                Quaternion randomSpin = Quaternion.AngleAxis(UnityEngine.Random.Range(-300f, 300f), Vector3.forward);
                                var direc = Quaternion.LookRotation(base.GetAimRay().direction);

                                GameObject sonicboom = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/SonicBoomEffect");

                                RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();

                                sonicboomeffectdata.scale = 3f;
                                sonicboomeffectdata.rotation = direc;
                                sonicboomeffectdata.origin = modelTransform.position + modelTransform.forward * 2f + modelTransform.up * 2f;

                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);

                                RoR2.EffectData blast = new RoR2.EffectData();
                                blast.scale = 5f + (0.5f * LordZot.ChargedSlam);
                                blast.rotation = direc;
                                blast.origin = characterBody.corePosition + characterDirection.forward * 5f + modelTransform.up * 4f;
                                RoR2.EffectData blast3 = new RoR2.EffectData();
                                blast3.scale = 10f + (0.5f * LordZot.ChargedSlam);
                                blast3.rotation = direc;
                                blast3.origin = characterBody.footPosition;
                                RoR2.EffectData blast2 = new RoR2.EffectData();
                                ChildLocator component2 = modelTransform.GetComponent<ChildLocator>();
                                Transform transform = component2.FindChild("RightHand");

                                if (LordZot.momentum > 100)
                                {
                                    var direc2 = transform.rotation * Quaternion.Euler(90f, 0f, 0);
                                    GameObject sonicboom2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/parentblinkeffect");
                                    RoR2.EffectData sonicboomeffectdata2 = new RoR2.EffectData();

                                    sonicboomeffectdata2.scale = 3f;
                                    sonicboomeffectdata2.rotation = direc2;
                                    sonicboomeffectdata2.origin = transform.position;
                                    RoR2.EffectManager.SpawnEffect(sonicboom2, sonicboomeffectdata2, true);
                                }

                                blast2.scale = 5 + (0.3f * LordZot.ChargedSlam);
                                blast2.origin = characterBody.corePosition + characterDirection.forward * 5f + modelTransform.up * 4f;
                                blast2.rotation = direc;

                                RoR2.EffectManager.SpawnEffect(laser2, blast, true);
                                RoR2.EffectManager.SpawnEffect(original66, blast, true);
                                RoR2.EffectManager.SpawnEffect(original2, blast, true);
                                RoR2.EffectManager.SpawnEffect(original2, blast2, true);
                                RoR2.EffectManager.SpawnEffect(original27, blast, true);
                                RoR2.EffectManager.SpawnEffect(original25, blast3, true);
                                RoR2.EffectManager.SpawnEffect(original26, blast3, true);
                                if (LordZot.ChargedSlam > 4)
                                {
                                    RoR2.EffectManager.SpawnEffect(ZotSlam.tracerEffectPrefab, effectData, true);
                                    RoR2.EffectManager.SpawnEffect(EntityStates.QuestVolatileBattery.CountDown.explosionEffectPrefab, effectData, true);

                                };


                                if (LordZot.ChargedSlam > 40)
                                {
                                    RoR2.Util.PlaySound(EntityStates.VagrantMonster.FireMegaNova.novaSoundString, base.gameObject);
                                    RoR2.EffectManager.SpawnEffect(laser3, blast, true);
                                    RoR2.EffectManager.SpawnEffect(original277, blast, true);
                                    RoR2.EffectManager.SpawnEffect(original2737, blast, true);
                                    RoR2.EffectManager.SpawnEffect(original2777, blast, true);
                                };


                                if (LordZot.ChargedSlam < 150)
                                {
                                    RoR2.ShakeEmitter shakeEmitter;
                                    shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                                    shakeEmitter.wave = new Wave
                                    {
                                        amplitude = 1f + 0.003f * LordZot.ChargedSlam,
                                        frequency = 180f,
                                        cycleOffset = 0f
                                    };
                                    shakeEmitter.duration = 1f + 0.009f * LordZot.ChargedSlam;
                                    shakeEmitter.radius = 50f;
                                    shakeEmitter.amplitudeTimeDecay = true;
                                    LordZot.timeRemaining = 6f;
                                }
                                else
                                {
                                    RoR2.ShakeEmitter shakeEmitter;
                                    shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                                    shakeEmitter.wave = new Wave
                                    {
                                        amplitude = 4f,
                                        frequency = 180f,
                                        cycleOffset = 0f
                                    };
                                    shakeEmitter.duration = 3.5f;
                                    shakeEmitter.radius = 50f;
                                    shakeEmitter.amplitudeTimeDecay = true;
                                    LordZot.timeRemaining = 6f;
                                }

                            }
                        }
                    }
                }
            }
            if (!hasswung && facingdown)
            {





                if (!isGrounded)
                {
                    LordZot.motor.velocity += GetAimRay().direction * 15;

                    GetModelAnimator().SetFloat("FireArrow.playbackRate", 0f);
                    base.PlayAnimation("Body", "AscendDescend", null, 1f); ;

                };

                if (isGrounded)
                {
                    base.PlayCrossfade("Gesture, Additive", "BufferEmpty", null, 1, 0.02f);
                    GetModelAnimator().SetFloat("FireArrow.playbackRate", 1f);
                    base.PlayAnimation("Body", "AscendDescend 2", null, 1f);
                    hasswung = true;
                    Transform transform1 = base.GetModelTransform();

                    RoR2.Util.PlaySound(EntityStates.GolemMonster.FireLaser.attackSoundString, base.gameObject);
                    const string V = "RightHand";
                    RoR2.Util.PlayAttackSpeedSound(EntityStates.VagrantMonster.FireTrackingBomb.fireBombSoundString, base.gameObject, 0.87f);
                    if (transform1)
                    {
                        ChildLocator component = transform1.GetComponent<ChildLocator>();
                        if (component)
                        {
                            int childIndex = component.FindChildIndex(V);
                            if (tracerEffectPrefab)
                            {
                                RoR2.EffectData effectData = new RoR2.EffectData
                                {
                                    scale = 1f + (2.9f * LordZot.ChargedSlam),
                                    origin = characterBody.corePosition,
                                    start = this.modifiedAimRay.origin
                                };
                                effectData.SetChildLocatorTransformReference(base.gameObject, childIndex);
                                GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                                GameObject original27 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/MagmaWormImpactExplosion");
                                GameObject original277 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/ArtifactShellExplosion");
                                GameObject original2737 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/ArtifactworldPortalSpawnEffect");
                                GameObject original2777 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/GrandparentDeathEffectLightShafts");
                                GameObject original66 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                                GameObject original26 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/ScavSitImpact");
                                GameObject original25 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                                Quaternion randomSpin = Quaternion.AngleAxis(UnityEngine.Random.Range(-300f, 300f), Vector3.forward);
                                var direc = Quaternion.LookRotation(this.modifiedAimRay.direction);
                                GameObject sonicboom = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/SonicBoomEffect");
                                RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();

                                sonicboomeffectdata.scale = 3f;
                                sonicboomeffectdata.rotation = Quaternion.FromToRotation(characterBody.corePosition, (characterBody.corePosition + Vector3.up * 5)) * randomSpin;
                                sonicboomeffectdata.origin = transform1.position + transform1.forward * -1f + transform1.up * 0f;
                                /*   EffectSettings = original2.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings.applyScale = true;
                                   EffectSettings2 = original27.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings2.applyScale = true;
                                   EffectSettings4 = original277.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings4.applyScale = true;
                                   EffectSettings5 = original2737.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings5.applyScale = true;
                                   EffectSettings3 = original2777.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings3.applyScale = true;
   */

                                GameObject original27767 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/MaulingRockImpact");

                                //   GameObject original277671 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/SmokescreenEffect");
                                GameObject original277672 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/SonicBoomEffect");
                                GameObject original277673 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/TitanDeathEffect");
                                GameObject original277674 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
                                GameObject original277675 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/PodGroundImpact");
                                GameObject original277676 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");

                                RoR2.EffectData impact = new RoR2.EffectData();
                                impact.scale = 2f + (1.8f * LordZot.ChargedSlam);
                                impact.origin = LordZot.model.corePosition + Vector3.up * 2;
                                impact.rotation = Quaternion.FromToRotation(LordZot.model.corePosition, LordZot.model.corePosition + Vector3.up);
                                RoR2.EffectManager.SpawnEffect(original27767, impact, true);
                                // RoR2.EffectManager.SpawnEffect(original277671, impact, true);
                                RoR2.EffectManager.SpawnEffect(original277672, impact, true);
                                RoR2.EffectManager.SpawnEffect(original277673, impact, true);
                                RoR2.EffectManager.SpawnEffect(original277674, impact, true);
                                RoR2.EffectManager.SpawnEffect(original277675, impact, true);
                                RoR2.EffectManager.SpawnEffect(original277676, impact, true);


                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectData blast = new RoR2.EffectData();
                                blast.scale = 2f + (2.8f * LordZot.ChargedSlam);
                                blast.origin = characterBody.corePosition;
                                RoR2.EffectData blast2 = new RoR2.EffectData();

                                ChildLocator component2 = transform1.GetComponent<ChildLocator>();
                                Transform transform = component2.FindChild("RightHand");

                                blast2.scale = 1f + (0.9f * LordZot.ChargedSlam);
                                blast2.origin = characterBody.corePosition;
                                blast2.rotation = direc;

                                RoR2.EffectManager.SpawnEffect(original66, blast, true);
                                RoR2.EffectManager.SpawnEffect(original2, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(original2, blast2, true);
                                RoR2.EffectManager.SpawnEffect(original27, blast, true);
                                RoR2.EffectManager.SpawnEffect(original25, blast, true);
                                RoR2.EffectManager.SpawnEffect(original26, blast, true);
                                if (LordZot.ChargedSlam > 4)
                                {
                                    RoR2.EffectManager.SpawnEffect(ZotSlam.tracerEffectPrefab, effectData, true);
                                    RoR2.EffectManager.SpawnEffect(EntityStates.QuestVolatileBattery.CountDown.explosionEffectPrefab, effectData, true);

                                };
                                if (LordZot.ChargedSlam > 40)
                                {
                                    RoR2.Util.PlaySound(EntityStates.VagrantMonster.FireMegaNova.novaSoundString, base.gameObject);
                                    RoR2.EffectManager.SpawnEffect(original277, blast, true);
                                    RoR2.EffectManager.SpawnEffect(original2737, blast, true);
                                    RoR2.EffectManager.SpawnEffect(original2777, blast, true);
                                };


                                if (LordZot.ChargedSlam < 150)
                                {
                                    RoR2.ShakeEmitter shakeEmitter;
                                    shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                                    shakeEmitter.wave = new Wave
                                    {
                                        amplitude = 1f + 0.003f * LordZot.ChargedSlam,
                                        frequency = 180f,
                                        cycleOffset = 0f
                                    };
                                    shakeEmitter.duration = 1f + 0.009f * LordZot.ChargedSlam;
                                    shakeEmitter.radius = 50f;
                                    shakeEmitter.amplitudeTimeDecay = true;
                                    LordZot.timeRemaining = 6f;
                                }
                                else
                                {
                                    RoR2.ShakeEmitter shakeEmitter;
                                    shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                                    shakeEmitter.wave = new Wave
                                    {
                                        amplitude = 3f,
                                        frequency = 180f,
                                        cycleOffset = 0f
                                    };
                                    shakeEmitter.duration = 3.5f;
                                    shakeEmitter.radius = 50f;
                                    shakeEmitter.amplitudeTimeDecay = true;
                                    LordZot.timeRemaining = 6f;
                                }

                                if (base.isAuthority)
                                {
                                    new RoR2.BlastAttack
                                    {
                                        losType = BlastAttack.LoSType.NearestHit,
                                        attacker = base.gameObject,
                                        inflictor = base.gameObject,
                                        teamIndex = RoR2.TeamComponent.GetObjectTeam(base.gameObject),
                                        baseDamage = 40 * 1.5f + (40 * 2.3f * LordZot.ChargedSlam),
                                        baseForce = 15000f,
                                        position = characterBody.corePosition,
                                        radius = 25f + 2f * LordZot.ChargedSlam,
                                        falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                                        crit = characterBody.RollCrit(),
                                        bonusForce = Vector3.up * (9000 + 4555f * LordZot.ChargedSlam)
                                    }.Fire();
                                    Vector3 origin1 = this.modifiedAimRay.origin;


                                    hasswung = true;


                                }
                            }
                        }
                    }




                }

            };
            if (hasswung)
            {
                GetModelAnimator().SetFloat("FireArrow.playbackRate", 0.3f);
                Quaternion randomSpin = Quaternion.AngleAxis(UnityEngine.Random.Range(-300f, 300f), Vector3.forward);
                var direc = Quaternion.LookRotation(this.modifiedAimRay.direction);
                GameObject sonicboom = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/SonicBoomEffect");
                RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();

                sonicboomeffectdata.scale = 3f;
                sonicboomeffectdata.rotation = direc * randomSpin;
                sonicboomeffectdata.origin = base.GetModelTransform().position + base.GetModelTransform().forward * 2f + base.GetModelTransform().up * 2f;


                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
            }

            if (stopwatch21 >= 0.61f)
            {
                GetModelAnimator().SetFloat("FireArrow.playbackRate", 1f);
                this.outer.SetNextStateToMain();
                return;
            }
        }

        // Token: 0x06003C67 RID: 15463 RVA: 0x0000CFF7 File Offset: 0x0000B1F7
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        // Token: 0x04003728 RID: 14120
        public static GameObject effectPrefab = EntityStates.GolemMonster.FireLaser.effectPrefab;

        // Token: 0x04003729 RID: 14121
        public static GameObject hitEffectPrefab = EntityStates.GolemMonster.FireLaser.hitEffectPrefab;

        // Token: 0x0400372A RID: 14122
        public static GameObject tracerEffectPrefab = EntityStates.GolemMonster.FireLaser.tracerEffectPrefab;

        // Token: 0x0400372B RID: 14123
        public static float damageCoefficient;

        // Token: 0x0400372C RID: 14124
        public static float blastRadius;

        // Token: 0x0400372D RID: 14125
        public static float force;

        // Token: 0x0400372E RID: 14126
        public static float minSpread;

        // Token: 0x0400372F RID: 14127
        public static float maxSpread;

        // Token: 0x04003730 RID: 14128
        public static int bulletCount;

        // Token: 0x04003731 RID: 14129
        public static float baseDuration = 1f;

        // Token: 0x04003732 RID: 14130
        public static string attackSoundString;

        // Token: 0x04003733 RID: 14131
        public Vector3 laserDirection;

        // Token: 0x04003734 RID: 14132
        private float duration;

        // Token: 0x04003735 RID: 14133
        private Ray modifiedAimRay;
        private RoR2.EffectComponent EffectSettings;
        private RoR2.EffectComponent EffectSettings2;
        private RoR2.EffectComponent EffectSettings4;
        private RoR2.EffectComponent EffectSettings5;
        private RoR2.EffectComponent EffectSettings3;
        private bool hasswung;
        private Animator animator;
        private float stopwatch21;
        private bool facingdown;
        private uint mferofgod;




    }



}
