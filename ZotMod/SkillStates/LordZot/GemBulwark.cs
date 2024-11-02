using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LordZot.SkillStates
{
    public class GemBulwark : BaseSkillState
    {
        // Token: 0x06003C55 RID: 15445 RVA: 0x000FB0B8 File Offset: 0x000F92B8
        public override void OnEnter()
        {
            LordZot.ChargedLaser = 0f;
            base.OnEnter();
            camadjusted = false;
            Debug.Log("ZotBulwark entered");
            LordZot.prevaim = base.characterBody.aimOriginTransform.localPosition;
            backhandhappened = false;
            // characterBody.aimOriginTransform.position = characterBody.aimOriginTransform.position + characterBody.aimOriginTransform.forward * 1.5f + characterBody.aimOriginTransform.up * 4.5f; ;
            characterBody.characterDirection.turnSpeed = 400f;
            this.animator = base.GetModelAnimator();
            Transform modelTransform = base.GetModelTransform();
            Vector3 prevcampos()
            {
                return base.cameraTargetParams.cameraParams.data.idealLocalCameraPos.value;


            }

            LordZot.LeftTrail.emitting = true;
            LordZot.RightTrail.emitting = true;


            if (isAuthority)
            {
                originalcampos = prevcampos();
            }
            if (!characterMotor.isGrounded && LordZot.ugh)
            {
                base.PlayAnimation("Body", "AscendDescend");
            };
            base.GetModelTransform().GetComponent<RoR2.AimAnimator>().enabled = true;




            //   EffectSettings = sonicboom.GetComponent<RoR2.EffectComponent>();
            // EffectSettings.applyScale = true;



            LordZot.Busy = true;
            LordZot.holdtime = false;
            LordZot.dontmove = false;




            this.stopwatch = 0.7f;
            this.stopwatch1 = 0f;
            this.stopwatch21 = 0f;
            this.stupidtime2 = 0f;

            stupidtime = 0f;
            bool flag = base.skillLocator;
            if (flag)
            {
                //   base.skillLocator.secondary.skillDef.activationStateMachineName = "Weapon";
            }




            this.laserOn = true;

            LordZot.floatpower += 8f;



            LordZot.timeRemaining = 6.0f;
            LordZot.ticker2 = 2f;
            Animator modelAnimator = base.GetModelAnimator();
            modelAnimator.SetFloat("ShieldsIn", 0.12f);


            this.duration = GemBulwark.baseDuration;

            this.chargePlayID = RoR2.Util.PlayAttackSpeedSound(EntityStates.GolemMonster.ChargeLaser.attackSoundString, base.gameObject, (1f / this.duration));
            if (modelTransform)
            {


                GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                GameObject original = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                GameObject laser = (EntityStates.GolemMonster.ChargeLaser.effectPrefab);
                GameObject laserfab = (EntityStates.GolemMonster.ChargeLaser.laserPrefab);
                ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                if (isAuthority)
                {
                    if (component)
                    {
                        Transform transform = component.FindChild("RightHand");
                        Transform transform2 = component.FindChild("LeftHand");
                        if (transform)
                        {
                            if (GemBulwark.effectPrefab)
                            {
                                this.chargeEffect = UnityEngine.Object.Instantiate<GameObject>(EntityStates.GolemMonster.ChargeLaser.effectPrefab, transform.position, transform.rotation);
                                this.chargeEffect.transform.parent = transform;
                                RoR2.ScaleParticleSystemDuration component2 = this.chargeEffect.GetComponent<RoR2.ScaleParticleSystemDuration>();
                                if (component2)
                                {
                                    component2.newDuration = this.duration;
                                }
                                RoR2.EffectData effectData = new RoR2.EffectData();
                                effectData.origin = transform.position;
                                effectData.scale = 1f;
                                if (LordZot.timeRemaining <= 0f)
                                { RoR2.EffectManager.SpawnEffect(original, effectData, true); }
                            }
                            if (GemBulwark.laserPrefab)
                            {
                                this.laserEffect = UnityEngine.Object.Instantiate<GameObject>(EntityStates.GolemMonster.ChargeLaser.laserPrefab, transform.position, transform.rotation);
                                this.laserEffect.transform.parent = transform;
                                this.laserLineComponent = this.laserEffect.GetComponent<LineRenderer>();
                            }
                            if (transform2)
                            {
                                RoR2.EffectData effectData = new RoR2.EffectData();
                                effectData.origin = transform2.position;
                                effectData.scale = 1f;
                                if (LordZot.timeRemaining <= 0f)
                                { RoR2.EffectManager.SpawnEffect(original2, effectData, true); }

                            }

                        }
                    }
                }
            }
            if (base.characterBody)
            {
                base.characterBody.SetAimTimer(this.duration);
                StartAimMode(this.duration);
            }


        }

        // Token: 0x06003C56 RID: 15446 RVA: 0x000FB208 File Offset: 0x000F9408
        public override void OnExit()
        {
            characterBody.characterDirection.turnSpeed = 150;

            LordZot.LeftTrail.emitting = false;
            LordZot.RightTrail.emitting = false;
            base.characterBody.SetAimTimer(0f);
            StartAimMode(0f);
            base.GetModelAnimator().SetBool("GodStop", true);
            base.GetModelTransform().GetComponent<RoR2.AimAnimator>().enabled = true;
            AkSoundEngine.StopPlayingID(this.chargePlayID);

            LordZot.ugh = false;
            LordZot.holdtime = false;
            LordZot.dontmove = false;
            if (isAuthority)
            {
                base.cameraTargetParams.cameraParams.data.idealLocalCameraPos.value = originalcampos;
            }
            LordZot.Busy = false;
            LordZot.Charging = false;
            // Destroy(da);
            if (this.chargeEffect)
            {
                EntityState.Destroy(this.chargeEffect);

            }
            if (this.laserEffect)
            {
                EntityState.Destroy(this.laserEffect);
            }
            PlayCrossfade("Bulwark", "NoBulwark", null, 1, 0.4f);
            base.OnExit();


        }


        // Token: 0x06003C57 RID: 15447 RVA: 0x000FB258 File Offset: 0x000F9458
        public override void Update()
        {
            base.Update();
            if (this.stopwatch21 > 0.41f)
            {
                Transform modelTransform = base.GetModelTransform();
                //    GameObject original = EntityStates.BrotherMonster.FistSlam.chargeEffectPrefab; 
                ChildLocator component = modelTransform.GetComponent<ChildLocator>();

                if (component)
                {
                    Transform transform = component.FindChild("RightHand");
                    if (transform)
                    {

                        RoR2.EffectData effectData = new RoR2.EffectData();
                        effectData.origin = transform.position;
                        effectData.scale = 2f + (LordZot.ChargedLaser * 0.4f);
                        //  RoR2.EffectManager.SpawnEffect(original, effectData, true);

                    }
                };




                if (laserEffect && laserLineComponent)
                {
                    if (LordZot.ChargedLaser < 20f)
                    {
                        float num = 1000000f;

                        Ray aimRay = GetAimRay();
                        // aimRay.origin += characterBody.aimOriginTransform.forward * 1.5f + characterBody.aimOriginTransform.up * 4.5f;
                        Vector3 position = laserEffect.transform.parent.position;
                        Vector3 point = aimRay.GetPoint(num);
                        this.laserDirection = point - position;

                        RaycastHit raycastHit;
                        if (Physics.Raycast(aimRay, out raycastHit, num, RoR2.LayerIndex.world.mask | RoR2.LayerIndex.entityPrecise.mask))
                        {
                            point = raycastHit.point;
                        }
                        laserLineComponent.SetPosition(0, position);
                        laserLineComponent.SetPosition(1, point);

                        laserLineComponent.startWidth = 1f + (LordZot.ChargedLaser * 0.21f);
                        laserLineComponent.endWidth = 1f + (LordZot.ChargedLaser * 0.21f);
                        GameObject end = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/ChargeOrbitalLaser");
                        GameObject end2 = (EntityStates.GolemMonster.ChargeLaser.effectPrefab);
                        // EffectSettings2 = end2.GetComponent<RoR2.EffectComponent>();
                        //   EffectSettings2.applyScale = true;
                        RoR2.EffectData effectData2 = new RoR2.EffectData();
                        effectData2.origin = point;
                        effectData2.scale = 1f + (0.5f * LordZot.ChargedLaser);
                        RoR2.EffectManager.SpawnEffect(end2, effectData2, true);

                        /*        {
                                    flicker.transform.position = point;
                                }*/
                    }
                    else
                    {
                        float num = 1000000f;
                        Ray aimRay = GetAimRay();

                        Vector3 position = laserEffect.transform.parent.position;
                        Vector3 point = aimRay.GetPoint(num);
                        this.laserDirection = point - position;

                        if (Physics.Raycast(aimRay, out raycastHit, num, RoR2.LayerIndex.world.mask | RoR2.LayerIndex.entityPrecise.mask))
                        {
                            point = raycastHit.point;
                        }
                        laserLineComponent.SetPosition(0, position);
                        laserLineComponent.SetPosition(1, point);

                        laserLineComponent.startWidth = 3.7f;
                        laserLineComponent.endWidth = 3.7f;
                        GameObject end = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/ChargeOrbitalLaser");
                        GameObject end2 = (EntityStates.GolemMonster.ChargeLaser.effectPrefab);
                        // EffectSettings2 = end2.GetComponent<RoR2.EffectComponent>();
                        //    EffectSettings2.applyScale = true;
                        RoR2.EffectData effectData2 = new RoR2.EffectData();
                        effectData2.origin = point;
                        effectData2.scale = 1f + (0.5f * LordZot.ChargedLaser);
                        RoR2.EffectManager.SpawnEffect(end2, effectData2, true);
                    };


                }
            }
        }
        // Token: 0x06003C58 RID: 15448 RVA: 0x000FB3B4 File Offset: 0x000F95B4
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.stopwatch21 += Time.fixedDeltaTime;
            this.updatewatch += Time.fixedDeltaTime;
            bool flag26 = base.inputBank.skill2.down;
            bool flag266 = base.inputBank.skill2.justReleased;
            bool flag27 = this.stopwatch1 >= this.duration;
            bool flag29 = this.updatewatch >= 1f;
            bool outofpower = LordZot.GemPowerVal <= 0f;
            this.duration = 100 + Time.fixedDeltaTime;
            stupidtime2 -= Time.fixedDeltaTime;

            LordZot.timeRemaining = 5f;




            if (this.stopwatch21 < 0.15f && !backhandhappened && !flag26)
            {

                Ray backhand = GetAimRay();
                /* RoR2.Util.CleanseBody(base.characterBody, false, false, false, false, true);*/
                Animator animator = GetModelAnimator();

                //animator.SetTrigger("Backhand");
                base.PlayAnimation("Gesture, Additive", "GemBulwark", "FireArrow.playbackRate", 0.8f);

                Util.PlaySound(EntityStates.BrotherMonster.SlideForwardState.soundString, base.gameObject);
                RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();

                LandingBlast.losType = BlastAttack.LoSType.NearestHit;
                LandingBlast.attacker = base.gameObject;
                LandingBlast.inflictor = base.gameObject;
                LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
                LandingBlast.baseDamage = (40 * 0.5f);
                LandingBlast.procCoefficient = 0f;
                LandingBlast.baseForce = 2500f + (LordZot.MaxGemPowerVal * 15);
                LandingBlast.bonusForce = (LandingBlast.baseForce * (backhand.direction) + (Vector3.up * LandingBlast.baseForce * 0.7f));
                LandingBlast.position = characterBody.corePosition + characterBody.characterDirection.forward * 1.5f;
                LandingBlast.radius = 20f;
                LandingBlast.crit = false;
                LandingBlast.falloffModel = RoR2.BlastAttack.FalloffModel.Linear;
                LandingBlast.attackerFiltering = AttackerFiltering.NeverHitSelf;
                LandingBlast.Fire();
                new RoR2.BlastAttack
                {

                    losType = BlastAttack.LoSType.NearestHit,
                    procCoefficient = 0f,
                    attacker = base.gameObject,
                    inflictor = base.gameObject,
                    teamIndex = RoR2.TeamComponent.GetObjectTeam(base.gameObject),
                    baseDamage = 11,
                    baseForce = 250f + (LordZot.MaxGemPowerVal * 4),
                    position = characterBody.corePosition + characterBody.characterDirection.forward * 1.5f,
                    radius = 55f + 0.03f * LordZot.MaxGemPowerVal,
                    falloffModel = RoR2.BlastAttack.FalloffModel.Linear,
                    crit = false
                }.Fire();
                new RoR2.BlastAttack
                {
                    losType = BlastAttack.LoSType.NearestHit,
                    procCoefficient = 0f,
                    attacker = base.gameObject,
                    inflictor = base.gameObject,
                    teamIndex = RoR2.TeamComponent.GetObjectTeam(base.gameObject),
                    baseDamage = 4,
                    baseForce = 250f + (LordZot.MaxGemPowerVal * 1),
                    position = characterBody.corePosition + characterBody.characterDirection.forward * 1.5f,
                    radius = 100f + 0.09f * LordZot.MaxGemPowerVal,
                    falloffModel = RoR2.BlastAttack.FalloffModel.Linear,
                    crit = false
                }.Fire();
                characterBody.characterDirection.turnSpeed = 250;
                Quaternion randomSpin = Quaternion.AngleAxis(UnityEngine.Random.Range(-300f, 300f), Vector3.forward);
                float randomx = (UnityEngine.Random.Range(-7f, 7f));
                var direc = Quaternion.LookRotation(base.GetAimRay().direction, base.characterBody.transform.up);
                GameObject sonicboom = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/SonicBoomEffect");
                RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();
                Transform modelTransform = base.GetModelTransform();
                sonicboomeffectdata.scale = 5f;
                sonicboomeffectdata.rotation = direc * randomSpin;
                sonicboomeffectdata.origin = modelTransform.position + modelTransform.forward * -1f + modelTransform.up * 2f + (LordZot.modeltransform.right * randomx);
                RoR2.EffectData sonicboomeffectdata2 = new RoR2.EffectData();

                sonicboomeffectdata2.scale = 5f;
                sonicboomeffectdata2.rotation = direc * randomSpin;
                sonicboomeffectdata2.origin = modelTransform.position + modelTransform.forward * -3f + modelTransform.up * 2f;

                RoR2.EffectData sonicboomeffectdata3 = new RoR2.EffectData();

                sonicboomeffectdata3.scale = 5f;
                sonicboomeffectdata3.rotation = direc * randomSpin;
                sonicboomeffectdata3.origin = modelTransform.position + modelTransform.forward * -3f + modelTransform.up * 2f + LordZot.model.transform.right * 4f;

                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata2, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata3, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata2, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata3, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");


                RoR2.EffectData effectData2 = new RoR2.EffectData();

                effectData2.origin = modelTransform.position + modelTransform.forward * 2f + (modelTransform.up * 4f);
                effectData2.scale = 1.2f + (LordZot.MaxGemPowerVal * 0.035f);
                GameObject original66 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                RoR2.EffectData effectData3 = new RoR2.EffectData();

                effectData3.origin = modelTransform.position + modelTransform.forward * 2f + (modelTransform.up * 4f) + new Vector3(0f, 0f, 0f);
                effectData3.scale = 4.5f + (LordZot.MaxGemPowerVal * 0.035f);
                RoR2.EffectManager.SpawnEffect(original66, effectData3, true);
                RoR2.EffectManager.SpawnEffect(original2, effectData2, true);
                backhandhappened = true;
            }
            if (this.stopwatch21 < 0.15f && backhandhappened)
            {
                Quaternion randomSpin = Quaternion.AngleAxis(UnityEngine.Random.Range(-300f, 300f), Vector3.forward);
                float randomx = (UnityEngine.Random.Range(-7f, 7f));
                var direc = Quaternion.LookRotation(base.GetAimRay().direction, base.characterBody.transform.up);
                GameObject sonicboom = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/SonicBoomEffect");
                RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();
                Transform modelTransform = base.GetModelTransform();
                sonicboomeffectdata.scale = 5f;
                sonicboomeffectdata.rotation = direc * randomSpin;
                sonicboomeffectdata.origin = modelTransform.position + modelTransform.forward * -1f + modelTransform.up * 2f + (LordZot.modeltransform.right * randomx);
                RoR2.EffectData sonicboomeffectdata2 = new RoR2.EffectData();

                sonicboomeffectdata2.scale = 5f;
                sonicboomeffectdata2.rotation = direc * randomSpin;
                sonicboomeffectdata2.origin = modelTransform.position + modelTransform.forward * -3f + modelTransform.up * 2f;

                RoR2.EffectData sonicboomeffectdata3 = new RoR2.EffectData();

                sonicboomeffectdata3.scale = 5f;
                sonicboomeffectdata3.rotation = direc * randomSpin;
                sonicboomeffectdata3.origin = modelTransform.position + modelTransform.forward * -3f + modelTransform.up * 2f + LordZot.model.transform.right * 4f;

                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata2, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata3, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata2, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata3, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
            }

            if (this.stopwatch21 > 0.41f && flag26)
            {
                if (base.isAuthority && !camadjusted)
                {
                    base.cameraTargetParams.cameraParams.data.idealLocalCameraPos.value.x = -4f;
                    camadjusted = true;
                    //  base.cameraTargetParams.cameraParams.standardLocalCameraPos.y = -3f;
                    //   base.cameraTargetParams.cameraParams.standardLocalCameraPos.z = -22f;
                }
                if (!LordZot.Charging)
                {
                    if (LordZot.GemPowerVal > 3f)
                    {
                        LordZot.GemPowerVal -= 3f;
                        LordZot.ChargedLaser += 3f;
                    }
                    else
                    {
                        LordZot.ChargedLaser += LordZot.GemPowerVal;
                        LordZot.GemPowerVal -= 3f;

                    }
                    LordZot.Charging = true;
                    GameObject original2 = EntityStates.GolemMonster.ChargeLaser.effectPrefab;
                    Transform modelTransform = base.GetModelTransform();
                    ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                    Transform transform2 = component.FindChild("RightHand");
                    this.chargePlayID = RoR2.Util.PlayAttackSpeedSound(EntityStates.GolemMonster.ChargeLaser.attackSoundString, base.gameObject, 1f);
                    GameObject laser = (EntityStates.GolemMonster.ChargeLaser.effectPrefab);
                    RoR2.EffectData effectData = new RoR2.EffectData();
                    effectData.origin = transform2.position;
                    effectData.scale = 1f + (1f * LordZot.ChargedLaser);
                    if (transform2 && isAuthority)
                    {

                        RoR2.EffectManager.SpawnEffect(original2, effectData, true);
                        RoR2.EffectManager.SpawnEffect(laser, effectData, true);
                    }
                };


                if (LordZot.Charging)
                {
                    LordZot.ChargedLaser += LordZot.GemPowerVal * 0.00053f;
                    if (updatewatch >= 1.5f)
                    {

                        if (LordZot.ChargedLaser > 15f)
                        {
                            RoR2.ShakeEmitter shakeEmitter;
                            shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                            shakeEmitter.wave = new Wave
                            {
                                amplitude = 0.001f,
                                frequency = 180f,
                                cycleOffset = 0f
                            };
                            shakeEmitter.duration = 1f;
                            shakeEmitter.radius = 50f;
                            shakeEmitter.amplitudeTimeDecay = false;
                            LordZot.timeRemaining = 6f;

                        }
                        else
                        {
                            RoR2.ShakeEmitter shakeEmitter;
                            shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                            shakeEmitter.wave = new Wave
                            {
                                amplitude = 0.0001f * LordZot.ChargedLaser,
                                frequency = 180f,
                                cycleOffset = 0f
                            };
                            shakeEmitter.duration = 1f;
                            shakeEmitter.radius = 50f;
                            shakeEmitter.amplitudeTimeDecay = false;
                            LordZot.timeRemaining = 6f;

                        };
                        if (LordZot.ChargedLaser > 15f)
                        {


                            GameObject original3 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
                            //  EffectSettings4 = original3.GetComponent<RoR2.EffectComponent>();
                            //  EffectSettings4.applyScale = true;
                            // EffectSettings4.parentToReferencedTransform = true;

                            RoR2.EffectData effectData2 = new RoR2.EffectData();
                            effectData2.origin = LordZot.righthand.position;
                            effectData2.scale = 0.2f;
                            RoR2.EffectManager.SpawnEffect(original3, effectData2, true);
                        }



                        if (LordZot.ChargedLaser > 10f)
                        {
                            Transform modelTransform = base.GetModelTransform();
                            ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                            Transform transform2 = component.FindChild("RightHand");
                            if (transform2)
                            {
                                GameObject laser2 = (EntityStates.GolemMonster.ChargeLaser.effectPrefab);
                                RoR2.EffectData effectData = new RoR2.EffectData();
                                effectData.origin = transform2.position;
                                effectData.scale = 1f + (1f * LordZot.ChargedLaser);
                                if (isAuthority)
                                { RoR2.EffectManager.SpawnEffect(laser2, effectData, true); };
                            }
                        };
                        if (LordZot.ChargedLaser > 30f)
                        {
                            Transform modelTransform = base.GetModelTransform();
                            ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                            Transform transform2 = component.FindChild("RightHand");

                            {
                                GameObject laser2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/artifactworldportalprespawneffect");
                                GameObject laser3 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/titanspawneffect");

                                if (effectPrefab)
                                {
                                    this.chargeEffect = UnityEngine.Object.Instantiate<GameObject>(laser2, transform2.position, LordZot.righthand.rotation);
                                    this.chargeEffect2 = UnityEngine.Object.Instantiate<GameObject>(laser3, LordZot.model.footPosition, Quaternion.identity);
                                    this.chargeEffect.transform.parent = transform;
                                    RoR2.ScaleParticleSystemDuration component2 = this.chargeEffect.GetComponent<RoR2.ScaleParticleSystemDuration>();
                                    if (component2)
                                    {
                                        component2.newDuration = 2f;
                                    }
                                    RoR2.EffectData effectData = new RoR2.EffectData();
                                    effectData.origin = transform2.position;
                                    effectData.scale = 0.5f + (0.2f * LordZot.ChargedLaser);
                                    RoR2.EffectManager.SpawnEffect(chargeEffect, effectData, true);
                                    RoR2.EffectData effectData2 = new RoR2.EffectData();
                                    effectData2.origin = LordZot.model.footPosition;
                                    effectData2.scale = 0.3f;
                                    RoR2.EffectManager.SpawnEffect(chargeEffect2, effectData2, true);
                                }



                                if (transform2)
                                {
                                    //   EffectSettings3 = laser2.GetComponent<RoR2.EffectComponent>();
                                    //  EffectSettings3.applyScale = true;
                                    //  EffectSettings3.parentToReferencedTransform = true;
                                    laser2.AddComponent<RoR2.DestroyOnTimer>().duration = 2.5f;

                                    RoR2.EffectData effectData = new RoR2.EffectData();
                                    effectData.origin = LordZot.righthand.position;
                                    effectData.scale = 1f + (1f * LordZot.ChargedLaser);
                                    RoR2.EffectManager.SpawnEffect(laser2, effectData, true);

                                }

                            };



                        }

                        if (LordZot.ChargedLaser > 40f)
                        {
                            GameObject laser2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/ArtifactworldPortalPrespawnEffect");
                            GameObject laser3 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/TitanSpawnEffect");

                            if (effectPrefab)
                            {
                                this.chargeEffect = UnityEngine.Object.Instantiate<GameObject>(laser2, LordZot.righthand.position, LordZot.righthand.rotation);
                                this.chargeEffect2 = UnityEngine.Object.Instantiate<GameObject>(laser3, LordZot.model.footPosition, Quaternion.identity);
                                this.chargeEffect.transform.parent = transform;
                                RoR2.ScaleParticleSystemDuration component2 = this.chargeEffect.GetComponent<RoR2.ScaleParticleSystemDuration>();
                                if (component2)
                                {
                                    component2.newDuration = 2.5f;
                                }
                                RoR2.ScaleParticleSystemDuration component1 = laser3.GetComponent<RoR2.ScaleParticleSystemDuration>();
                                if (component1)
                                {
                                    component1.newDuration = 2.5f;
                                }
                                RoR2.EffectData effectData = new RoR2.EffectData();
                                effectData.origin = transform.position;
                                effectData.scale = 0.5f;
                                RoR2.EffectManager.SpawnEffect(chargeEffect, effectData, true);
                                RoR2.EffectData effectData2 = new RoR2.EffectData();
                                effectData2.origin = LordZot.model.footPosition;
                                effectData2.scale = 0.3f;
                                RoR2.EffectManager.SpawnEffect(chargeEffect2, effectData2, true);
                            }
                        }
                        if (this.stopwatch21 > 1.1f && this.stopwatch21 < 1.15f)
                        {


                            GameObject original6 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                            RoR2.EffectData effectData = new RoR2.EffectData();

                            effectData.origin = characterBody.footPosition;
                            effectData.scale = 5f;
                            RoR2.EffectManager.SpawnEffect(original6, effectData, true);
                        };
                        this.updatewatch = 0f;
                    }



                };




                if (stupidtime >= 0)
                {
                    stupidtime -= Time.fixedDeltaTime;
                };
                if (stupidtime <= 0)
                {
                    LordZot.holdtime = false;
                };




                base.characterDirection.moveVector += base.inputBank.aimDirection;
            }
            if ((outofpower | (!flag26) && this.stopwatch21 >= 0.41f))
            {
                LordZot.Gemdrain = 1f;
                skillLocator.secondary.DeductStock(1);
                ZotLaser zotLaser = new ZotLaser();
                zotLaser.laserDirection = laserDirection;
                this.outer.SetNextState(zotLaser);
                PlayCrossfade("Bulwark", "NoBulwark", null, 1, 0.4f);
                return;
            };

            if (flag26 && this.stopwatch21 > 0.41f && !LordZot.holdtime)
            {
                LordZot.Charging = true;
                PlayCrossfade("Bulwark", "HoldBulwark", null, 1, 0.4f);
                this.modelanimator = GetModelAnimator();
                modelanimator.SetBool("GodStop", false);
                modelanimator.SetLayerWeight(4, 1);
                LordZot.holdtime = true;
                stupidtime = 10f;
                {

                }
            };
            if (stopwatch21 >= 0.4f && !flag26)
            {
                base.GetModelAnimator().SetBool("GodStop", true);
                PlayCrossfade("Bulwark", "NoBulwark", null, 1, 0.4f);
                characterBody.aimOriginTransform.localPosition = LordZot.prevaim;
                PlayCrossfade("Body", "Idle", null, 1f, 0.4f);
                skillLocator.secondary.AddOneStock();
                this.outer.SetNextStateToMain();
            };
        }

        // Token: 0x06003C59 RID: 15449 RVA: 0x0000CFF7 File Offset: 0x0000B1F7
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        // Token: 0x0400370E RID: 14094
        public static float baseDuration = 2f;

        // Token: 0x0400370F RID: 14095


        // Token: 0x04003710 RID: 14096
        public static GameObject effectPrefab = (EntityStates.GolemMonster.ChargeLaser.effectPrefab);

        // Token: 0x04003711 RID: 14097
        public static GameObject laserPrefab = (EntityStates.GolemMonster.ChargeLaser.laserPrefab);
        private Vector3 originalcampos;
        // Token: 0x04003712 RID: 14098
        public static string attackSoundString;
        private float stopwatch;

        // Token: 0x04003713 RID: 14099
        private float duration;

        // Token: 0x04003714 RID: 14100
        private uint chargePlayID;

        // Token: 0x04003715 RID: 14101
        private GameObject chargeEffect;
        private GameObject chargeEffect2;
        GameObject original2737 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/ArtifactworldPortalPrespawnEffect");
        // Token: 0x04003716 RID: 14102
        private GameObject laserEffect;
        private RaycastHit raycastHit;
        // Token: 0x04003717 RID: 14103
        private LineRenderer laserLineComponent;

        // Token: 0x04003718 RID: 14104
        private Vector3 laserDirection;

        // Token: 0x04003719 RID: 14105
        private Vector3 visualEndPosition;

        // Token: 0x0400371A RID: 14106
        private float flashTimer;

        // Token: 0x0400371B RID: 14107
        private bool laserOn;
        private float updatewatch;
        private float stopwatch1;
        private float stupidtime;
        private float stopwatch21;
        private Animator modelanimator;
        private float stupidtime2;
        // private GameObject da;
        private RoR2.EffectComponent EffectSettings3;

        private RoR2.EffectComponent EffectSettings;
        private RoR2.EffectComponent EffectSettings2;
        private RoR2.EffectComponent EffectSettings4;
        private bool backhandhappened;
        private Animator animator;
        private bool camadjusted;

        //  private Light flicker;

        public float ChargeDuration { get; private set; }
    }
}