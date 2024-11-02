using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace LordZot.SkillStates
{
    public class ZotLaser : BaseSkillState
    {
        // Token: 0x06003C64 RID: 15460 RVA: 0x000FB714 File Offset: 0x000F9914
        public override void OnEnter()
        {
            base.OnEnter();
            //  Debug.Log("ZotLaser entered");
            hasfired = false;
            this.duration = ZotLaser.baseDuration / this.attackSpeedStat;
            this.modifiedAimRay = base.GetAimRay();
            this.animator = base.GetModelAnimator();
            this.modifiedAimRay.direction = this.laserDirection;
            PlayCrossfade("Bulwark", "HoldBulwark", null, 1, 0.4f);
            LordZot.floatpower += LordZot.ChargedLaser;
            LordZot.ChargedLaser += 0.1f;
            LordZot.Busy = true;
            num = 1000000f;

            base.cameraTargetParams.cameraParams.data.idealLocalCameraPos.value.x = -4f;
            vector = base.GetAimRay().origin + base.GetAimRay().direction * num;
            if (Physics.Raycast(base.GetAimRay(), out raycastHit, num, RoR2.LayerIndex.world.mask | RoR2.LayerIndex.defaultLayer.mask | RoR2.LayerIndex.entityPrecise.mask))
            {
                vector = raycastHit.point;
            }
            impact2prefab = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/MajorAndMinorConstruct/LaserMajorConstruct.prefab").WaitForCompletion();

            base.GetModelAnimator().SetBool("GodStop", false);
            modelTransform = base.GetModelTransform();

            text = "RightHand";
            getinfo = true;
            LordZot.Charging = false;
            LordZot.timeRemaining = 6f;

            if (base.characterBody)
            {
                //  base.characterBody.SetAimTimer(1f);
            }

        }

        // Token: 0x06003C65 RID: 15461 RVA: 0x00032FA7 File Offset: 0x000311A7
        public override void OnExit()
        {
            base.OnExit();

            base.cameraTargetParams.cameraParams.data.idealLocalCameraPos.value.x = 0f;
            characterBody.aimOriginTransform.localPosition = LordZot.prevaim;
            LordZot.ChargedLaser = 0f;
            skillLocator.secondary.DeductStock(1);
            LordZot.Busy = false;
            base.PlayAnimation("Gesture, Additive", "BufferEmpty", null, 1f);
        }

        // Token: 0x06003C66 RID: 15462 RVA: 0x000FB969 File Offset: 0x000F9B69
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            /* if (base.fixedAge <= (this.duration * 0.7) && base.isAuthority)
             {
                 num = 1000000f;
                 vector = base.GetAimRay().origin + base.GetAimRay().direction * num;
                 if (Physics.Raycast(base.GetAimRay(), out raycastHit, num, RoR2.LayerIndex.world.mask | RoR2.LayerIndex.defaultLayer.mask | RoR2.LayerIndex.entityPrecise.mask))
                 {
                     vector = raycastHit.point;
                 }
                 if (tracerEffectPrefab)
                 {
                     impact2 = new RoR2.EffectData();
                     impact2.scale = 0.5f + (1f * LordZot.LordZot.ChargedLaser);
                     impact2.origin = raycastHit.point;
                     impact2.rotation = Quaternion.FromToRotation(raycastHit.point, (raycastHit.normal * 2));
                     GameObject original277674 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
                     GameObject original277675 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/PodGroundImpact");
                     RoR2.Util.PlaySound(EntityStates.ParentMonster.GroundSlam.attackSoundString, base.gameObject);
                     RoR2.EffectManager.SpawnEffect(original277674, impact2, true);
                     RoR2.EffectManager.SpawnEffect(original277675, impact2, true);
                     effectData2 = new RoR2.EffectData
                     {
                         scale = 0.5f + (1.9f * LordZot.LordZot.ChargedLaser),
                         origin = vector,
                         start = base.GetAimRay().origin
                     };
                     new RoR2.BlastAttack
                     {
                         losType = BlastAttack.LoSType.NearestHit,
                     procCoefficient = 0f,
                         attacker = base.gameObject,
                         inflictor = base.gameObject,
                         teamIndex = RoR2.TeamComponent.GetObjectTeam(base.gameObject),
                         baseDamage = 1 * 1.5f * LordZot.LordZot.ChargedLaser,
                         baseForce = 122 + 12f * LordZot.LordZot.ChargedLaser,
                         position = vector,
                         radius = 3f + 0.6f * LordZot.LordZot.ChargedLaser,
                         falloffModel = RoR2.BlastAttack.FalloffModel.None,
                         crit = false
                     }.Fire();

                     RoR2.EffectManager.SpawnEffect(tracerEffectPrefab, effectData2, true);
                 }
             }
 */
            if (base.isAuthority && !hasfired)
            {
                base.GetModelAnimator().SetLayerWeight(4, 1);
                PlayCrossfade("Bulwark", "NoBulwark", null, 1, 0.4f);
                if (effectPrefab)
                {
                    RoR2.EffectManager.SimpleMuzzleFlash(EntityStates.GolemMonster.FireLaser.effectPrefab, base.gameObject, text, true);
                }

                base.GetModelAnimator().SetBool("GodStop", true);
                RoR2.Util.PlaySound(EntityStates.GolemMonster.FireLaser.attackSoundString, base.gameObject);
                base.PlayAnimation("Gesture, Additive", "FireBulwark", null, 1f);

                if (LordZot.ChargedLaser > 40)
                {
                    RoR2.Util.PlaySound(EntityStates.VagrantMonster.FireMegaNova.novaSoundString, base.gameObject);
                }
                if (base.isAuthority)
                {




                    if (Physics.Raycast(base.GetAimRay(), out raycastHit, num, RoR2.LayerIndex.world.mask | RoR2.LayerIndex.defaultLayer.mask | RoR2.LayerIndex.entityPrecise.mask))
                    {
                        vector = raycastHit.point;
                    }
                    new RoR2.BlastAttack
                    {
                        losType = BlastAttack.LoSType.NearestHit,
                        procCoefficient = 0f,
                        attacker = base.gameObject,
                        inflictor = base.gameObject,
                        teamIndex = RoR2.TeamComponent.GetObjectTeam(base.gameObject),
                        baseDamage = 40 * 2.5f * LordZot.ChargedLaser,
                        baseForce = 1000 + 555f * LordZot.ChargedLaser,
                        position = vector,
                        radius = 6f + 0.9f * LordZot.ChargedLaser,
                        falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                        crit = false,
                        bonusForce = ZotLaser.force * base.GetAimRay().direction
                    }.Fire();
                    Vector3 origin = base.GetAimRay().origin;
                    if (modelTransform)
                    {
                        ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                        if (component)
                        {
                            Vector3 vector2 = characterBody.corePosition + characterDirection.forward * 1f;
                            TeamIndex teamIndex = characterBody ? characterBody.teamComponent.teamIndex : TeamIndex.None;
                            int childIndex = component.FindChildIndex(text);
                            if (tracerEffectPrefab)
                            {
                                RoR2.EffectData effectData = new RoR2.EffectData
                                {
                                    scale = 0.5f + (1.9f * LordZot.ChargedLaser),
                                    origin = vector,
                                    start = base.GetAimRay().origin
                                };
                                effectData.SetChildLocatorTransformReference(base.gameObject, childIndex);
                                GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                                GameObject original27 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/MagmaWormImpactExplosion");
                                GameObject original277 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/ArtifactShellExplosion");
                                GameObject original27767 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/MaulingRockImpact");
                                GameObject original27773 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/BrotherSunderWaveEnergizedExplosion");
                                GameObject original277671 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/SmokescreenEffect");
                                GameObject original277672 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/SonicBoomEffect");
                                GameObject titandeath = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/TitanDeathEffect");

                                GameObject original277674 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
                                GameObject original277675 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/PodGroundImpact");
                                GameObject original277676 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                                /*  EffectSettings = original2.GetComponent<RoR2.EffectComponent>();
                                  EffectSettings.applyScale = true;
                                  EffectSettings.parentToReferencedTransform = true;
                                  EffectSettings2 = original27.GetComponent<RoR2.EffectComponent>();
                                  EffectSettings2.applyScale = true;
                                  EffectSettings2.parentToReferencedTransform = true;
                                  EffectSettings4 = original277.GetComponent<RoR2.EffectComponent>();
                                  EffectSettings4.applyScale = true;
                                  EffectSettings4.parentToReferencedTransform = true;*/
                                GameObject original2737 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/ArtifactworldPortalSpawnEffect");
                                GameObject original2777 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/LaserTurbineBombExplosion");
                                /* EffectSettings5 = original2737.GetComponent<RoR2.EffectComponent>();
                                 EffectSettings5.applyScale = true;
                                 EffectSettings5.parentToReferencedTransform = true;

                                 EffectSettings3 = original2777.GetComponent<RoR2.EffectComponent>();
                                 EffectSettings3.applyScale = false;
                                 EffectSettings3.parentToReferencedTransform = true;*/
                                RoR2.EffectData impact = new RoR2.EffectData();
                                impact.scale = 0.5f + (1f * LordZot.ChargedLaser);
                                impact.origin = raycastHit.point;
                                impact.rotation = Quaternion.FromToRotation(raycastHit.point, (raycastHit.normal * 2));

                                RoR2.EffectData blast = new RoR2.EffectData();
                                blast.scale = 0.5f;
                                blast.origin = raycastHit.point;
                                RoR2.EffectData blast2 = new RoR2.EffectData();

                                ChildLocator component2 = modelTransform.GetComponent<ChildLocator>();
                                Transform transform = component2.FindChild("RightHand");

                                blast2.scale = 1f + (0.8f * LordZot.ChargedLaser);
                                blast2.origin = transform.position;
                                RoR2.EffectManager.SpawnEffect(original27767, impact, true);
                                RoR2.EffectManager.SpawnEffect(original277671, impact, true);
                                RoR2.EffectManager.SpawnEffect(original277672, impact, true);




                                RoR2.EffectManager.SpawnEffect(original277674, impact, true);
                                RoR2.EffectManager.SpawnEffect(original277675, impact, true);
                                RoR2.EffectManager.SpawnEffect(original277676, impact, true);
                                RoR2.EffectManager.SpawnEffect(original27, blast, true);
                                RoR2.EffectManager.SpawnEffect(original2, blast, true);
                                RoR2.EffectManager.SpawnEffect(original2, blast2, true);
                                RoR2.EffectManager.SpawnEffect(ZotLaser.tracerEffectPrefab, effectData, true);
                                RoR2.EffectManager.SpawnEffect(EntityStates.QuestVolatileBattery.CountDown.explosionEffectPrefab, effectData, true);
                                if (LordZot.ChargedLaser > 40)
                                {

                                    RoR2.EffectManager.SpawnEffect(original277, blast, true);

                                    RoR2.EffectManager.SpawnEffect(titandeath, impact, true);
                                    titandeath.transform.GetChild(2).gameObject.SetActive(false);
                                    RoR2.EffectManager.SpawnEffect(original2737, blast, true);
                                    RoR2.EffectManager.SpawnEffect(original2777, blast, true);
                                    RoR2.EffectManager.SpawnEffect(original27773, impact, true);
                                    LordZot.timeRemaining = 6f;
                                };


                                if (LordZot.ChargedLaser < 240)
                                {
                                    RoR2.ShakeEmitter shakeEmitter;
                                    shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                                    shakeEmitter.wave = new Wave
                                    {
                                        amplitude = 0.005f * LordZot.ChargedLaser,
                                        frequency = 180f,
                                        cycleOffset = 0f
                                    };
                                    shakeEmitter.duration = 0.02f * LordZot.ChargedLaser;
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
                                        amplitude = 1f,
                                        frequency = 180f,
                                        cycleOffset = 0f
                                    };
                                    shakeEmitter.duration = 2.5f;
                                    shakeEmitter.radius = 50f;
                                    shakeEmitter.amplitudeTimeDecay = true;
                                    LordZot.timeRemaining = 6f;
                                };

                                hasfired = true;

                            }
                        }
                    }
                }
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        // Token: 0x06003C67 RID: 15463 RVA: 0x0000CFF7 File Offset: 0x0000B1F7
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
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
        private RaycastHit raycastHit;
        // Token: 0x0400372E RID: 14126
        public static float minSpread;

        // Token: 0x0400372F RID: 14127
        public static float maxSpread;
        public static RoR2.EffectData impact2;
        // Token: 0x04003730 RID: 14128
        public static int bulletCount;

        // Token: 0x04003731 RID: 14129
        public static float baseDuration = 0.9f;

        // Token: 0x04003732 RID: 14130
        public static string attackSoundString;

        // Token: 0x04003733 RID: 14131
        public Vector3 laserDirection;

        // Token: 0x04003734 RID: 14132
        private float duration;

        // Token: 0x04003735 RID: 14133
        private Ray modifiedAimRay;
        private Animator animator;
        private RoR2.EffectComponent EffectSettings;
        private RoR2.EffectComponent EffectSettings2;
        private RoR2.EffectComponent EffectSettings4;
        private RoR2.EffectComponent EffectSettings3;
        private RoR2.EffectComponent EffectSettings5;
        private bool hitwall;
        private bool hasfired;
        private bool getinfo;
        private Transform modelTransform;
        private EffectData impact;
        private string text;
        private float num;
        private Vector3 vector;
        private GameObject impact2prefab;

        public static GameObject tracerEffectPrefab2 = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/MajorAndMinorConstruct/LaserMajorConstruct.prefab").WaitForCompletion();

        public EffectData effectData2 { get; private set; }
    }
}
