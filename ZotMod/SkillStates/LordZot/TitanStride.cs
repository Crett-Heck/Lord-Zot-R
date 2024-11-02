using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.Navigation;
using BepInEx;
using R2API;
using EntityStates;
using R2API.Utils;
using RoR2;


namespace LordZot.SkillStates
{
    class TitanStride : BaseSkillState
    {

        public override void OnEnter()
        {
            base.OnEnter();
            // Debug.Log("ZotBlink entered");
            if (LordZot.GemPowerVal < LordZot.mass * 0.02f)
            {

                didntstartgemdrained = false;
                LordZot.Gemdrain = 1f;
                this.outer.SetNextStateToMain();
            }
            else
            {


                if (characterMotor.isGrounded)
                {
                    nottoofar = true;
                    castfromground = true;
                }
                else
                if
                (LordZot.GemPowerVal < LordZot.mass * 0.02f)
                {
                    didntstartgemdrained = false;
                    LordZot.Gemdrain = 1f;
                    this.outer.SetNextStateToMain();
                }
                else
                {
                    castfromground = false;
                    LordZot.GemPowerVal -= LordZot.mass * 0.02f;
                };
                didntstartgemdrained = true;
                LordZot.momentum = (LordZot.mass * 6);
                RoR2.Util.PlaySound(EntityStates.BrotherMonster.BaseSlideState.soundString, base.gameObject);
                RoR2.Util.PlaySound(EntityStates.BrotherMonster.ExitSkyLeap.soundString, zotPrefab);
                this.modelTransform = base.GetModelTransform();
                LordZot.GemPowerVal -= 2f;

                if (base.inputBank.moveVector.magnitude > 0)
                {

                    b = base.inputBank.moveVector * this.blinkDistance;
                }
                else
                {

                    b = GetAimRay().direction * (this.blinkDistance * 0.5f);
                }
                characterBody.characterDirection.turnSpeed = 200000f;
                this.blinkDestination = base.transform.position;
                this.blinkStart = base.transform.position;
                /*  if (tracker)
                  {
                      if (tracker.GetTrackingTarget() != null)
                      {
                          Debug.Log("attempt");
                          NodeGraph groundNodes = RoR2.SceneInfo.instance.groundNodes;
                          NodeGraph.NodeIndex nodeIndex = groundNodes.FindClosestNode(tracker.GetTrackingTarget().gameObject.transform.position - (base.transform.position + base.inputBank.moveVector), base.characterBody.hullClassification);
                          groundNodes.GetNodePosition(nodeIndex, out this.blinkDestination);
                      }
                      else
                      {*/
                //  Debug.Log("fail");
                NodeGraph groundNodes = RoR2.SceneInfo.instance.groundNodes;
                NodeGraph.NodeIndex nodeIndex = groundNodes.FindClosestNode(base.transform.position + b, base.characterBody.hullClassification);
                groundNodes.GetNodePosition(nodeIndex, out this.blinkDestination);
                /*     }

                 }*/



                this.blinkDestination += base.transform.position - base.characterBody.footPosition + new Vector3(0f, 0.1f, 0f);
                /* if (blinkDestination.magnitude > 350)
                 { nottoofar = true; };*/
                /* if (blinkDestination.magnitude < 350)
                 { nottoofar = false; };*/

                this.CreateBlinkEffect(RoR2.Util.GetCorePosition(base.gameObject));


                if (this.modelTransform)
                {
                    this.characterModel = this.modelTransform.GetComponent<RoR2.CharacterModel>();
                }
                if (this.characterModel)
                {


                    foreach (RoR2.CharacterModel.RendererInfo renderInfo in characterModel.baseRendererInfos)
                    {

                        {
                            Material mat = renderInfo.defaultMaterial;
                            mat.shader = LordZot.distortion;
                            mat.shader.SetPropertyValue("_DistortionAmount", 255f);
                        }
                    }
                }

                if (this.hurtboxGroup)
                {
                    RoR2.HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
                    int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter = 1;
                    hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
                }


                /* if (NetworkServer.active)
                 {
                     RoR2.Util.CleanseBody(base.characterBody, true, false, false, false, false);
                 }*/
                if (EntityStates.BrotherMonster.BaseSlideState.slideEffectPrefab && base.characterBody)
                {
                    Vector3 position = base.characterBody.corePosition;
                    Quaternion rotation = Quaternion.identity;
                    RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();
                    sonicboomeffectdata.scale = 5f + (LordZot.mass * 0.01f);
                    sonicboomeffectdata.rotation = rotation;
                    sonicboomeffectdata.origin = position;

                    RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                    RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                    RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);

                    Transform transform = base.FindModelChild(EntityStates.BrotherMonster.BaseSlideState.slideEffectMuzzlestring);

                    if (transform)
                    {
                        position = transform.position;
                    }
                    if (base.characterDirection)
                    {
                        rotation = RoR2.Util.QuaternionSafeLookRotation(this.slideRotation * base.characterDirection.forward, Vector3.up);
                    }
                    RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();
                    LandingBlast.attacker = characterBody.gameObject;

                    LandingBlast.losType = BlastAttack.LoSType.NearestHit;
                    LandingBlast.inflictor = characterBody.gameObject;
                    LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
                    LandingBlast.baseDamage = 10 + (LordZot.mass * 0.2f);
                    LandingBlast.baseForce = 1000f + (LordZot.mass * 2f);
                    LandingBlast.bonusForce = characterDirection.moveVector * LordZot.mass;
                    LandingBlast.position = position;
                    LandingBlast.procCoefficient = 0f;
                    LandingBlast.radius = 35f + (LordZot.mass * 0.02f);
                    LandingBlast.crit = characterBody.RollCrit();
                    LandingBlast.Fire();

                    RoR2.EffectData sonicboomeffectdata3 = new RoR2.EffectData();
                    sonicboomeffectdata3.scale = 5f + (LordZot.mass * 0.01f);
                    sonicboomeffectdata3.rotation = rotation;
                    sonicboomeffectdata3.origin = position;

                    RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata3, true);
                    RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata3, true);
                    RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata3, true);
                    GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");
                    RoR2.EffectManager.SpawnEffect(original2, sonicboomeffectdata3, true);
                    RoR2.EffectManager.SimpleEffect(EntityStates.BrotherMonster.BaseSlideState.slideEffectPrefab, position, rotation, true);
                    this.shakeEmitter = zotPrefab.gameObject.AddComponent<RoR2.ShakeEmitter>();
                    this.shakeEmitter.wave = new Wave
                    {
                        amplitude = 0.5f,
                        frequency = 180f,
                        cycleOffset = 0f
                    };
                    this.shakeEmitter.duration = 0.6f;
                    this.shakeEmitter.radius = 50f;
                    this.shakeEmitter.amplitudeTimeDecay = true;

                    if (!characterMotor.isGrounded)
                    {
                        castfromground = false;
                        RoR2.EffectManager.SpawnEffect(EntityStates.ParentMonster.LoomingPresence.blinkPrefab, sonicboomeffectdata, true);
                        RoR2.EffectManager.SpawnEffect(EntityStates.ParentMonster.LoomingPresence.blinkPrefab, sonicboomeffectdata, true);
                        RoR2.EffectManager.SpawnEffect(EntityStates.ParentMonster.LoomingPresence.blinkPrefab, sonicboomeffectdata, true);
                    };



                }
            };
        }
        private void CreateBlinkEffect(Vector3 origin)
        {
            RoR2.EffectData effectData = new RoR2.EffectData();
            effectData.rotation = RoR2.Util.QuaternionSafeLookRotation(this.blinkDestination - this.blinkStart);
            effectData.origin = origin;
            RoR2.EffectData effectDat2a = new RoR2.EffectData();
            effectDat2a.rotation = RoR2.Util.QuaternionSafeLookRotation(this.blinkStart - this.blinkDestination);
            effectDat2a.origin = this.blinkDestination;
            RoR2.EffectManager.SpawnEffect(EntityStates.ParentMonster.LoomingPresence.blinkPrefab, effectDat2a, true);
            RoR2.EffectManager.SpawnEffect(EntityStates.ParentMonster.LoomingPresence.blinkPrefab, effectData, true);
            RoR2.EffectManager.SpawnEffect(EntityStates.ParentMonster.LoomingPresence.blinkPrefab, effectData, true);
            RoR2.EffectManager.SpawnEffect(EntityStates.ParentMonster.LoomingPresence.blinkPrefab, effectData, true);
        }
        private void SetPosition(Vector3 newPosition)
        {
            if (base.characterMotor)
            {
                base.characterMotor.Motor.SetPositionAndRotation(newPosition, Quaternion.identity, true);
            }
        }
        // Token: 0x06003FBF RID: 16319 RVA: 0x0010AE40 File Offset: 0x00109040
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (didntstartgemdrained)
            {
                if (base.isAuthority)
                {
                    stopwatch += Time.fixedDeltaTime;
                    base.FixedUpdate();
                    if (characterMotor.isGrounded && castfromground)

                    {

                        if (base.characterMotor && base.characterDirection)
                        {
                            base.characterMotor.velocity = Vector3.zero;
                        }
                        if (nottoofar)
                        {
                            this.SetPosition(Vector3.Lerp(this.blinkStart, this.blinkDestination, stopwatch / 0.5f));
                        }
                        else
                        {
                            Vector3 a = Vector3.zero;
                            Vector3 c = Vector3.zero;
                            if (base.inputBank && base.characterDirection)
                            {
                                a = GetAimRay().direction;

                            }
                            if (base.characterMotor)
                            {
                                Vector3 position = base.characterBody.corePosition;
                                Quaternion rotation = Quaternion.identity;
                                RoR2.EffectData sonicboomeffectdata2 = new RoR2.EffectData();
                                sonicboomeffectdata2.scale = 5f + (LordZot.mass * 0.01f);
                                sonicboomeffectdata2.rotation = rotation;
                                sonicboomeffectdata2.origin = position;

                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata2, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata2, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata2, true);
                                float num = EntityStates.BrotherMonster.BaseSlideState.speedCoefficientCurve.Evaluate(base.fixedAge / TitanStride.duration);
                                base.characterMotor.rootMotion += this.slideRotation * (num * (35f) * (a + base.characterMotor.moveDirection) * Time.fixedDeltaTime);


                            }
                        }
                    }
                    else
                    {
                        if (characterMotor.isGrounded && !castfromground)

                        {

                            if (base.characterMotor && base.characterDirection)
                            {

                                base.characterMotor.velocity = Vector3.zero;
                            }

                        }
                        else
                        {

                            {
                                Vector3 a = Vector3.zero;
                                Vector3 c = Vector3.zero;
                                if (base.inputBank && base.characterDirection)
                                {
                                    a = GetAimRay().direction;
                                }
                                if (base.characterMotor)
                                {
                                    if (!castfromground && !characterMotor.isGrounded)
                                    {
                                        float num = EntityStates.BrotherMonster.BaseSlideState.speedCoefficientCurve.Evaluate(base.fixedAge / TitanStride.duration);
                                        base.characterMotor.rootMotion += this.slideRotation * (num * (35f) * a * Time.fixedDeltaTime);
                                    }

                                }

                            }

                        }
                    }




                    if (base.fixedAge >= TitanStride.duration)
                    {
                        this.outer.SetNextStateToMain();
                    }
                    if (!inputBank.skill3.down && isGrounded)
                    {
                        this.outer.SetNextStateToMain();
                    }
                }
            }
        }

        // Token: 0x06003FC0 RID: 16320 RVA: 0x0010AEFF File Offset: 0x001090FF
        public override void OnExit()
        {
            this.modelTransform = base.GetModelTransform();
            characterBody.characterDirection.turnSpeed = 150f;
            if (LordZot.Gemdrain <= 0)
            {
                skillLocator.utility.Reset();

                if (this.modelTransform)
                {
                    LordZot.floatpower += 19f;
                    /* RoR2.TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<RoR2.TemporaryOverlay>();
                     temporaryOverlay.duration = 1f;
                     temporaryOverlay.destroyComponentOnEnd = true;
                     temporaryOverlay.originalMaterial = Resources.Load<Material>("materials/matEcho");
                     temporaryOverlay.inspectorCharacterModel = this.modelTransform.gameObject.GetComponent<RoR2.CharacterModel>();
                     temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                     temporaryOverlay.animateShaderAlpha = true;*/
                };


            };


            if (this.characterModel)
            {
                foreach (RoR2.CharacterModel.RendererInfo renderInfo in characterModel.baseRendererInfos)
                {

                    {
                        Material mat = renderInfo.defaultMaterial;
                        mat.shader = LordZot.hopoo;
                    }
                }
            }
            if (this.hurtboxGroup)
            {
                RoR2.HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
                int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter = 0;
                hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
            }

            base.OnExit();



        }

        // Token: 0x06003FC2 RID: 16322 RVA: 0x0000D472 File Offset: 0x0000B672
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }





        // Token: 0x04003B7E RID: 15230
        public static float duration = 0.4f;
        private Transform modelTransform;
        // Token: 0x04003B7F RID: 15231
        public static AnimationCurve speedCoefficientCurve;

        // Token: 0x04003B80 RID: 15232
        public static AnimationCurve jumpforwardSpeedCoefficientCurve;
        private RoR2.ShakeEmitter shakeEmitter;
        // Token: 0x04003B81 RID: 15233
        public static string soundString;

        // Token: 0x04003B82 RID: 15234
        public static GameObject slideEffectPrefab;

        // Token: 0x04003B83 RID: 15235
        public static string slideEffectMuzzlestring;

        // Token: 0x04003B84 RID: 15236
        protected Vector3 slideVector;

        // Token: 0x04003B85 RID: 15237
        protected Quaternion slideRotation;
        private RoR2.CharacterModel characterModel;
        public static float destealthDuration = 0.7f;
        private RoR2.HurtBoxGroup hurtboxGroup;
        public static GameObject zotPrefab;
        public static GameObject slamImpactEffect;
        public static GameObject slamEffectPrefab;
        public GameObject sonicboom = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/SonicBoomEffect");
        private Vector3 blinkDestination = Vector3.zero;

        private Vector3 blinkStart = Vector3.zero;
        private float blinkDistance = 28f;
        private static float stopwatch;
        private bool castfromground;

        public bool nottoofar { get; private set; }


        private bool didntstartgemdrained;
        private Vector3 b;
    }
}
