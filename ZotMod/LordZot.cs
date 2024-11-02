using BepInEx;
using LordZot.Modules.Survivors;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using UnityEngine;


using R2API;
using EntityStates;

using EntityStates.Mage;
using RoR2.ContentManagement;
using RoR2.Skills;
using UnityEngine.Networking;
using RoR2.Projectile;

using KinematicCharacterController;
using EntityStates.Merc;
using EntityStates.GolemMonster;
using EntityStates.BrotherMonster;
using EntityStates.ParentMonster;
using EntityStates.Loader;
using EntityStates.LemurianBruiserMonster;
using EntityStates.QuestVolatileBattery;
using EntityStates.TitanMonster;
using EntityStates.BeetleGuardMonster;
using EntityStates.GrandParentBoss;
using EntityStates.NewtMonster;
using EntityStates.VagrantMonster;
using System.Collections;
using RoR2.Navigation;
using RoR2.UI;
using UnityEngine.UI;
using UnityEngine.TextCore;
using TMPro;
using RoR2.Audio;
using Rewired.Data;
using Rewired;
using JetBrains.Annotations;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using UnityEngine.AddressableAssets;
using BepInEx.Configuration;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

//rename this namespace
namespace LordZot
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
        "UnlockableAPI"
    })]
    
    public class LordZot : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "jot.LordZot";
        public const string MODNAME = "LordZot";
        public const string MODVERSION = "1.4.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEVELOPER_PREFIX = "JOT";
        public static bool Busy;
        public static LordZot instance;
        internal static Animator animator;
        internal static ChildLocator component;
        internal static float momentum;
        internal static float timeRemaining;
        internal static float floatpower;
        internal static Transform lefthand;
        internal static Transform righthand;
        internal static bool inair;
        internal static float ticker2;
        internal static GameObject leftpunch;
        public static Material commandoMat;
        public static GameObject Gem;


        public static RoR2.CharacterMaster GemMaster { get; private set; }
        public static GameObject GemMaster2 { get; private set; }
        internal static List<SkillFamily> skillFamilies = new List<SkillFamily>();
        internal static List<SurvivorDef> survivorDefinitions = new List<SurvivorDef>();
        internal static List<SkillDef> skillDefs = new List<SkillDef>();
        public static GameObject zotPrefab; // the survivor body prefab
        public GameObject characterDisplay; // the prefab used for character select
        public GameObject doppelganger; // umbra sTuff
        public static float Charged;
        internal static List<GameObject> bodyPrefabs = new List<GameObject>();
        public static GameObject arrowProjectile; // prefab for our survivor's primary attack projectile
        private static readonly Color characterColor = new Color(0.55f, 0.15f, 0.55f); // color used for the survivor

        public static float timeRemainingstun;
        public static float jumpcooldown;
        public static float mass = 100;
        public static bool fallstun;
        private KeyCode jump;
        private bool jumpInputReceived;
        private RoR2.CharacterModel modela;
        public static RoR2.PlayerCharacterMasterController master;
        public static RoR2.CharacterBody model;

        public static bool wellhefell;
        public static float fallspeed;
        public static float fastestfallspeed;
        public static bool holdtime = false;
        public static float ChargedLaser;
        public static bool ugh;
        public static bool flight = false;
        public static Shader distortion = LegacyResourcesAPI.Load<Shader>("shaders/fx/hgdistortion");
        public static Shader cloudremap = LegacyResourcesAPI.Load<Shader>("shaders/fx/cloudremap");
        public static Vector3 prevaim;
        public static Shader cloudintersectionremap = LegacyResourcesAPI.Load<Shader>("shaders/fx/cloudintersectionremap");
        internal static List<RoR2.BuffDef> buffDefs = new List<RoR2.BuffDef>();
        public static Shader opaquecloudremap = LegacyResourcesAPI.Load<Shader>("shaders/fx/opaquecloudremap");
        public static float Gemdrain = 0f;
        public static Shader hopoo = LegacyResourcesAPI.Load<Shader>("shaders/deferred/hgstandard");
        public static float flightcooldown = 0.1f;
        public static bool dontmove = false;
        public static KeyCode Floatbutton = UnityEngine.KeyCode.V;
        public static KeyCode Gempowercheat1 = UnityEngine.KeyCode.L;
        public static bool slammingair;
        public static KeyCode Gempowercheat2 = UnityEngine.KeyCode.LeftControl;
        public static float ticker = 0f;
        private static RoR2.NetworkUser zotuser;
        public GameObject RedOrb { get; private set; }
        public static GameObject RedTrail { get; private set; }
        public static bool DisplayMomentum;
        public static Vector3 Leftfootpos;
        public static Vector3 Righthandpos;
        public static Vector3 Lefthandpos;
        public static Vector3 Rightfootpos;
        public static GameObject originalag = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/OmniImpactVFXLightning");
        public static GameObject originalug = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFXQuick");

        public static RoR2.CharacterMotor motor;

        public RoR2.CharacterModel actualmodel { get; private set; }

        private bool zotisingame = false;
        internal static List<GameObject> projectilePrefabs = new List<GameObject>();
        public float gauntletsfloat { get; private set; }
        public TextMeshProUGUI MomentumDamageDisplay { get; private set; }
        public static float MomentumDamageDisplaytime;
        public static UnityEngine.Transform modeltransform;

        public float GemstoAllocate { get; private set; }

        private bool zotcheck = true;
        public static GameObject trail1;
        public static RoR2.Tracer trailtrace1;
        public static GameObject trail2;
        public static RoR2.Tracer trailtrace2;
        public static RoR2.EffectData lefttraileffect;
        public static GameObject original111 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/tracers/tracersmokeline/TracerMageLightningLaser");
        public static GameObject original222 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/tracers/tracersmokeline/TracerMageFireLaser");
        public static RoR2.EffectData righttraileffect;

        public static Transform rightshield;
        public static Transform leftshield;

        public static Transform leftfoot;
        public static Transform rightfoot;

        public static GameObject rightpunch = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/tracers/TracerGolem");
        public static int zotlevel;
        public static float zotlevels;
        public static RoR2.SkillLocator zotskills;
       
        public static float GemPowerVal = 20f;
        public static string GemPowerValString;
        public static float MaxGemPowerVal = 39f;
        public RoR2.UI.HGTextMeshProUGUI textMesh;
        public static float ChargedSlam { get; internal set; }
        public float GemStack { get; private set; }
        public static List<RoR2.CharacterBody> ZotWhacked;
        public static RoR2.CharacterBody ZotWhackmaster;
        public float ZotWhackDuration;
        public static Material PleaseWork;
        public float Inevitable = 0f;
        public static Slider GemBarUIReal;
        private RoR2.UI.HUD hud = null;
        public static Material emiss;
        public GameObject GameObjectReference2 { get; private set; }
        public RectTransform rectTransformback { get; private set; }
        public float percent { get; private set; }
        public static bool LastSwingArm { get; internal set; }
        public RoR2.UI.HUD hud2 { get; private set; }
        public bool Zotchosensurvivor;
        public static Material Emission { get; private set; }

        public static Material Thematerial { get; private set; }
        public float GempowerValpercent { get; private set; }
        public float ticker22 { get; private set; }
        public object hudclone { get; private set; }
        internal static RoR2.BuffDef ZotWhack { get; private set; }
        internal static RoR2.BuffDef ZotJumping { get; private set; }
        internal static RoR2.BuffDef ZotFlight { get; private set; }
        public RoR2.Tracer RedTracer { get; private set; }
        public float timeinair { get; private set; }
        public float gemtick { get; private set; }
        public static RoR2.PlayerCharacterMasterController playerCharacterMaster;
        public Material mat;
        public bool Superjump;
        public float superjumptogglecooldown;
        public static bool Chargingjump = false;

        public static float Followfloatpower;

        private bool hudinit = false;

        public static bool Charging = false;
        public GameObject GameObjectReference;
        public TextMeshProUGUI GemBarText;

        public TextMeshProUGUI MISCINFO { get; private set; }

        public static string GemPowerDisplayValue;

        public Slider GempowerSlider { get; private set; }

        public RoR2.UI.HGTextMeshProUGUI GemValText;

        public RectTransform GempowerSlider2 { get; private set; }

        private List<RoR2.CharacterBody> instancesList;
        private List<RoR2.CharacterBody> list;
        private RoR2.CharacterBody characterBody2;
        public RectTransform rectTransformfill;
        private RoR2.EffectComponent EffectSettings;
        private RoR2.EffectComponent EffectSettings2;
        //rivate <RoR2.ShakeEmitter> Stopshaking;
        private RoR2.EffectComponent EffectSettings3;
        private RoR2.EffectComponent EffectSettings4;
        private RoR2.EffectComponent EffectSettings5;
        public static GameObject RedTrailPrefab;
        public static Vector3 nextgem;
        private int gemtickbacklog;
        public Vector3 footposition { get; private set; }
        public static RoR2.SpawnCard GemCard;
        public GameObject Gemmasterprefab { get; private set; }
        public RoR2.CharacterMaster gemcomponent { get; private set; }
        public static GameObject Gembody;
        public RoR2.DirectorPlacementRule GemRule { get; private set; }
        public TeamIndex? teamIndexOverride { get; private set; }
        public float GemsActive { get; private set; }
        public float GemsCollected { get; private set; }
        public static TrailRenderer LeftTrail { get; private set; }
        public static TrailRenderer RightTrail { get; private set; }
        public float ticker222 { get; private set; }
        public static EntityStateMachine component3 { get; private set; }
        public TextMeshProUGUI MISCINFO2 { get; private set; }
        public CharacterBody Bodies { get; private set; }
        public static NetworkStateMachine networkStatemacjh { get; private set; }
        public EntityStateMachine slideStateMachine { get; private set; }
        public EntityStateMachine BodyStateMachine { get; private set; }
        public CharacterBody ThatsZot { get; private set; }
        public ConfigFile ZotConfig { get; private set; }

        private ConfigEntry<string> canzotpickupitems;
        private static EntityStateMachine[] stuffmachines;
        public bool readconfig = false;

        public static RoR2.HuntressTracker tracker;

        public static float leftlegik;
        public static float rightlegik;
        private RaycastHit raycastHitleft;
        private RaycastHit raycastHitright;
        private float leftikfollow = 0;
        private float rightikfollow = 0;
        private string logmass;

        public static Texture emissioncolor;

        public static GameObject Gempowabarasset;

        public static Texture charPortrait;
        public static Texture icon1portrait;
        public static Sprite iconP;
        public static Sprite fill;
        public static Sprite bar;
        public static Sprite icon1;
        public static Sprite icon2;
        public static Sprite icon3;
        public static Sprite gameboypunch;
        public static Sprite KayleStride;
        public static Sprite gempower;
        public static Sprite gempower2;
        public static Texture gempowa;
        public static Material Zotbodmat;
        public static Material Material015;

        private void Awake()
        {
            instance = this;

            Log.Init(Logger);
            Modules.Assets.Initialize(); // load assets and read config
            Modules.Config.ReadConfig();
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            // survivor initialization
            new MyCharacter().Initialize();

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            Hook();


        }

        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            On.RoR2.Stage.Start += zotcheck2;
            On.RoR2.UI.HUD.Awake += GemHud;
            On.RoR2.CharacterBody.Start += Gemeffect;
            On.RoR2.CharacterBody.OnSprintStart += ZotNoSprint;
            On.RoR2.CharacterMotor.OnHitGroundServer += Landing2;
            On.RoR2.CharacterMotor.OnMovementHit += Knockbackdamage2;
            On.RoR2.HealthComponent.TakeDamage += VoidImmune;
            On.RoR2.UI.MainMenu.BaseMainMenuScreen.OnEnter += ResetZot;
        }
        private void VoidImmune(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (damageInfo.damageColorIndex is DamageColorIndex.Void && self.body.baseNameToken is "ZotBody")
            {
                damageInfo.damage = 1;
            }
            else
            { orig(self, damageInfo); };
        }
        private void Gemeffect(On.RoR2.CharacterBody.orig_Start orig, CharacterBody self)
        {
            if (self.baseNameToken is "TIMECRYSTAL_BODY_NAME")
            {
                GameObject original2738 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/VagrantSpawnEffect");
                RoR2.EffectData blast = new RoR2.EffectData();
                blast.scale = 3f;
                blast.origin = self.corePosition;
                RoR2.EffectManager.SpawnEffect(original2738, blast, true);
            }
            orig(self);
        }
        private void InitConfig()
        {
            ZotConfig = new ConfigFile(Paths.ConfigPath + "\\ZotConfig.cfg", true);
            canzotpickupitems = ZotConfig.Bind<string>(
                "Settings",
                "Can Zot Pick Up Items",
                "False",
                "While Zot is not designed with items in mind, some wish to defy this in search of even greater power. " +
                "Wackiness may ensue. Set to True if you wish to pursue this dark path."
                );

        }
        private void ReadConfig()
        {
            if (!readconfig)
            {
                if (canzotpickupitems.Value is "False")
                {
                    On.RoR2.GenericPickupController.AttemptGrant += ZotNoItem;
                }
                readconfig = true;
            }
        }
        private void ZotNoItem(On.RoR2.GenericPickupController.orig_AttemptGrant orig, GenericPickupController self, CharacterBody body)
        {
            if (body.baseNameToken is "ZotBody")
            { }
            else
            {
                orig(self, body);
            }
        }
        private void Landing2(On.RoR2.CharacterMotor.orig_OnHitGroundServer orig, RoR2.CharacterMotor self, RoR2.CharacterMotor.HitGroundInfo hitGroundInfo)
        {
            timeinair = 0f;
            var characterBody = self.GetComponentInParent<RoR2.CharacterBody>();
            var magnitude = hitGroundInfo.velocity.magnitude;
            string magdebug = magnitude.ToString();
            //   Debug.Log("magnitude");
            //  Debug.Log(magdebug);
            {
                orig(self, hitGroundInfo);
            }
            if (characterBody.baseNameToken is "ZotBody")
            {
                LordZot.flight = false;



                var position = characterBody.footPosition;


                RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();

                LandingBlast.attacker = characterBody.gameObject;
                LandingBlast.inflictor = characterBody.gameObject;
                LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
                LandingBlast.baseDamage = (0.5f + ((40 * 0.1f) * magnitude)) * (mass * 0.003f);
                LandingBlast.baseForce = 1000f + (magnitude * 20f) * (mass * 0.01f);
                LandingBlast.procCoefficient = 0f;
                LandingBlast.bonusForce = new Vector3(0f, 1000f + (magnitude * 50f * (mass * 0.01f)), 0f);
                LandingBlast.position = position;
                LandingBlast.radius = 7f + (magnitude * 0.1f) + (mass * 0.06f);
                LandingBlast.crit = characterBody.RollCrit();
                LandingBlast.losType = BlastAttack.LoSType.NearestHit;
                LandingBlast.falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot;
                LandingBlast.attackerFiltering = AttackerFiltering.NeverHitSelf;
                LandingBlast.Fire();


                timeRemainingstun = 1.6f;

                characterBody.characterMotor.velocity = Vector3.zero;


                GameObject original = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/PodGroundImpact");
                GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/ScavSitImpact");
                GameObject original25 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                GameObject original66 = EntityStates.TitanMonster.DeathState.initialEffect;
                GameObject original666 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/parentslameffect");
                //EffectSettings2 = original.GetComponent<RoR2.EffectComponent>();
                // EffectSettings2.applyScale = true;
                // EffectSettings = original2.GetComponent<RoR2.EffectComponent>();
                //EffectSettings.applyScale = true;
                //  EffectSettings3 = original666.GetComponent<RoR2.EffectComponent>();
                // EffectSettings3.applyScale = true;
                // EffectSettings4 = original66.GetComponent<RoR2.EffectComponent>();
                //EffectSettings4.applyScale = true;
                // EffectSettings5 = original25.GetComponent<RoR2.EffectComponent>();
                // EffectSettings5.applyScale = true;

                RoR2.EffectData effectData = new RoR2.EffectData();

                effectData.origin = position + new Vector3(0, 6f, 0);
                effectData.scale = 2f + (0.25f * magnitude) + (mass * 0.01f);

                RoR2.EffectManager.SpawnEffect(EntityStates.BeetleGuardMonster.GroundSlam.slamEffectPrefab, effectData, true);
                RoR2.EffectManager.SpawnEffect(EntityStates.LemurianBruiserMonster.SpawnState.spawnEffectPrefab, effectData, true);
                RoR2.EffectManager.SpawnEffect(original, effectData, true);
                RoR2.EffectManager.SpawnEffect(original25, effectData, true);
                RoR2.EffectManager.SpawnEffect(original2, effectData, true);

                RoR2.EffectData effectData2 = new RoR2.EffectData();

                effectData2.origin = position + new Vector3(0, 6f, 0);
                effectData2.scale = 1f + (0.05f * magnitude) + (mass * 0.01f);
                RoR2.EffectManager.SpawnEffect(original2, effectData2, true);
                if (magnitude + mass > 500)
                {
                    RoR2.Util.PlaySound(ExitSkyLeap.soundString, characterBody.gameObject);
                    RoR2.EffectManager.SpawnEffect(original66, effectData2, true);
                    RoR2.EffectManager.SpawnEffect(original666, effectData, true);
                }
                RoR2.EffectManager.SpawnEffect(original25, effectData2, true);

                RoR2.ShakeEmitter shakeEmitter;
                shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                shakeEmitter.wave = new Wave
                {
                    amplitude = 0.5f,
                    frequency = 180f,
                    cycleOffset = 0f
                };
                shakeEmitter.duration = 1.5f;
                shakeEmitter.radius = 50f;
                shakeEmitter.amplitudeTimeDecay = true;


                // RoR2.EffectManager.SpawnEffect(EntityStates.VagrantMonster.FireMegaNova.novaEffectPrefab, effectData, false);
                // RoR2.Util.PlaySound(FireMegaNova.novaSoundString, base.gameObject);
                RoR2.Util.PlaySound(ExitSkyLeap.soundString, base.gameObject);// RoR2.Util.PlaySound(FireMegaNova.novaSoundString, characterBody.gameObject);
                if (!Busy)
                {
                    SkillStates.ZotLand zotLand = new SkillStates.ZotLand();

                    foreach (EntityStateMachine entityStateMachine in base.gameObject.GetComponents<EntityStateMachine>())
                    {
                        bool flag = entityStateMachine;
                        if (flag)
                        {
                            bool flag2 = entityStateMachine.customName == "Body";
                            if (flag2)
                            {
                                BodyStateMachine = entityStateMachine;
                            }
                        }
                    }
                    if (BodyStateMachine.networkIdentity.localPlayerAuthority)
                    {
                        BodyStateMachine.SetNextState(zotLand);
                    }
                }
            };


        }
        private void ZotNoSprint(On.RoR2.CharacterBody.orig_OnSprintStart orig, RoR2.CharacterBody self)
        {
            if (self.baseNameToken is "ZotBody")
            {
                self.isSprinting = false;
                if (timeRemaining <= 0.1f)
                {

                    floatpower += 8f;
                    GameObject original = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                    GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");

                    //   EffectSettings.parentToReferencedTransform = true;
                    RoR2.EffectData effectData = new RoR2.EffectData();
                    effectData.origin = lefthand.position;
                    effectData.scale = 1f;
                    RoR2.EffectManager.SpawnEffect(original, effectData, true);

                    RoR2.EffectData effectData2 = new RoR2.EffectData();
                    effectData.origin = righthand.position;
                    effectData.scale = 1f;
                    RoR2.EffectManager.SpawnEffect(original2, effectData, true);




                    ticker2 = 2f;


                    animator.SetFloat("ShieldsIn", 0.12f);


                }
                if (timeRemaining >= 0)
                {

                    timeRemaining = 6f;
                };


            }
            else
            {
                orig(self);
            }

        }
        private void GemHud(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self)
        {

            orig.Invoke(self);

            if (NetworkServer.active)
            {
                hud = self;
                this.SetupHUD(self.transform);
            }

        }
        private void SetupHUD([NotNull] Transform selftransform)
        {
            if (GemBarUIReal == null && NetworkServer.active)
            {
                //   Debug.Log("Generating UI");
                if (PlayerCharacterMasterController.instances[0].body.baseNameToken is "ZotBody")
                {
                    ThatsZot = PlayerCharacterMasterController.instances[0].body;
                }


                if (ThatsZot != null)
                {
                    Transform transform = this.hud.mainContainer.transform;


                    GemBarUIReal = Instantiate<GameObject>(Gempowabarasset, transform, false).GetComponent<Slider>();
                    GemBarText = GemBarUIReal.GetComponentInChildren<TextMeshProUGUI>();
                    MISCINFO = Instantiate<TextMeshProUGUI>(GemBarUIReal.GetComponentInChildren<TextMeshProUGUI>(), GemBarUIReal.transform);
                    MISCINFO.transform.position += new Vector3(-25, 45, 0);
                    MISCINFO2 = Instantiate<TextMeshProUGUI>(GemBarUIReal.GetComponentInChildren<TextMeshProUGUI>(), GemBarUIReal.transform);
                    MISCINFO2.transform.position += new Vector3(-160, 790, 0);
                    MomentumDamageDisplay = Instantiate<TextMeshProUGUI>(GemBarUIReal.GetComponentInChildren<TextMeshProUGUI>(), GemBarUIReal.transform);
                    MomentumDamageDisplay.text = "";
                    MomentumDamageDisplay.color = new Color(255, 55, 55, 255);
                    MomentumDamageDisplay.transform.position += new Vector3(-145, 770, 0);
                    // GemBarText.text = GemPowerDisplayValue;
                    bool flag = base.transform;
                    if (flag)
                    {


                        /* this.GemBarText.transform.SetParent(transform, false);
                         this.GemValText.color = new Color(255f, 255f, 255f, 255f);
                         this.GemValText.fontSize = 33f;
                         this.GemValText.outlineColor = new Color(0f, 0f, 0f);
                         this.GemValText.outlineWidth = 0.5f;
                         this.GemValText.text = "";
                         this.GemValText.faceColor = Color.white;
                         rectTransform.anchoredPosition = new Vector2(25f, 0f);
                         rectTransform.anchorMin = new Vector2(0.33f, 0.1f);
                         rectTransform.anchorMax = new Vector2(0.33f, 0.1f);
                         rectTransform.sizeDelta = new Vector2(40f, 40f);
                         this.GemValText.enableWordWrapping = false;
 */
                    }


                }



                hudinit = true;
                //  Debug.Log("Gem Power UI Initialized");
            }
        }
        private void Knockbackdamage2(On.RoR2.CharacterMotor.orig_OnMovementHit orig, RoR2.CharacterMotor self, Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            orig(self, hitCollider, hitNormal, hitPoint, ref hitStabilityReport);

            if (zotisingame)
            {


                if (((Mathf.Abs(self.velocity.magnitude) > 45f) | (Mathf.Abs(self.velocity.magnitude) > 25f && self.mass > 100)) && self.GetComponent<RoR2.CharacterBody>().teamComponent.teamIndex is TeamIndex.Monster && !self.isGrounded)
                {
                    RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();

                    LandingBlast.attacker = model.gameObject;
                    LandingBlast.inflictor = model.gameObject;
                    LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(model.gameObject);
                    LandingBlast.baseDamage = ((self.mass * 3) * (self.velocity.magnitude * 0.27f) * 0.025f) + (((self.mass * 3) * (self.velocity.magnitude * 0.27f) * 0.025f) * (40 * 0.005f)) * (1 + self.GetComponentInParent<RoR2.CharacterBody>().level * 0.3f);
                    LandingBlast.baseForce = 15 * self.velocity.magnitude + self.mass;
                    LandingBlast.procCoefficient = 0f;

                    LandingBlast.losType = BlastAttack.LoSType.NearestHit;
                    LandingBlast.position = hitPoint;
                    LandingBlast.radius = 1 + self.velocity.magnitude * 0.2f + (self.mass * 0.001f);
                    LandingBlast.falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot;
                    LandingBlast.attackerFiltering = AttackerFiltering.NeverHitSelf;
                    LandingBlast.Fire();

                    GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/ScavSitImpact");
                    GameObject original25 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                    GameObject original66 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                    GameObject original2737 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/OmniImpactVFXLarge");
                    GameObject original27337 = EntityStates.BeetleGuardMonster.GroundSlam.slamEffectPrefab;
                    RoR2.EffectData blast = new RoR2.EffectData();
                    blast.scale = self.velocity.magnitude * 0.13f + (self.mass * 0.0005f);
                    blast.origin = hitPoint;
                    blast.rotation = Quaternion.FromToRotation(hitPoint, hitNormal);
                    RoR2.EffectData blast2 = new RoR2.EffectData();
                    blast2.scale = self.velocity.magnitude * 0.13f + (self.mass * 0.0005f); ;
                    blast2.origin = hitPoint;

                    RoR2.EffectManager.SpawnEffect(original66, blast2, true);
                    RoR2.EffectManager.SpawnEffect(original2, blast, true);
                    RoR2.EffectManager.SpawnEffect(original2737, blast, true);
                    RoR2.EffectManager.SpawnEffect(original25, blast, true);
                    if (Mathf.Abs(self.velocity.magnitude) > 180f | self.mass > 600)
                    {
                        RoR2.Util.PlaySound(EntityStates.BrotherMonster.ExitSkyLeap.soundString, self.gameObject);
                        RoR2.EffectManager.SpawnEffect(original27337, blast, true);
                        RoR2.EffectManager.SpawnEffect(EntityStates.ParentMonster.GroundSlam.slamImpactEffect, blast, true);
                        RoR2.EffectManager.SpawnEffect(EntityStates.BeetleGuardMonster.GroundSlam.slamEffectPrefab, blast, true);
                    }
                    self.velocity = self.velocity * 0.05f;
                }

            }
        }
        private void ResetZot(On.RoR2.UI.MainMenu.BaseMainMenuScreen.orig_OnEnter orig, RoR2.UI.MainMenu.BaseMainMenuScreen self, RoR2.UI.MainMenu.MainMenuController mainMenuController)
        {
            On.RoR2.FootstepHandler.Footstep_AnimationEvent -= FootstepBlast;
            GemsActive = 0f;
            GemStack = 0f;
            hudinit = false;
            Superjump = false;
            zotcheck = false;
            Destroy(GemBarUIReal);
            On.RoR2.ExperienceManager.AwardExperience -= ZotNoEXP;
            orig(self, mainMenuController);
            On.RoR2.CharacterBody.OnKilledOtherServer -= GemCollection;
            GemsCollected = 0f;
            mass = 100f;
        }
        private void GemCollection(On.RoR2.CharacterBody.orig_OnKilledOtherServer orig, RoR2.CharacterBody self, RoR2.DamageReport damageReport)
        {
            if (damageReport.victimBody.baseNameToken is "TIMECRYSTAL_BODY_NAME" /*&& damageReport.attackerBody.baseNameToken is "ZOT_NAME"*/)
            {
                GemStack += 4f;
                mass += 3f + (GemStack * 0.25f);
                logmass = mass.ToString();
                Debug.Log("Mass: " + logmass);
                model.baseMaxHealth = 800 + ((GemStack * 100 * GemStack * 0.0022f)) + (GemStack * 50);
                model.healthComponent.Heal((model.healthComponent.combinedHealth * 0.07f) + 180, default(RoR2.ProcChainMask), true);
                floatpower = 1000f;
                MaxGemPowerVal = 90f + (GemStack * 1.4f) + (GemStack * GemStack * 0.035f);
                GemPowerVal += MaxGemPowerVal * 0.09f;
                GemsActive -= 1f;
                GemsCollected += 0.5f;
            };
            // if (damageReport.victimBody.baseNameToken is "TIMECRYSTAL_BODY_NAME" && damageReport.attackerBody.baseNameToken != "ZOT_NAME")
            // { damageReport.victimBody.master.Respawn(damageReport.victimBody.corePosition, Quaternion.identity); };
            //     orig(self, damageReport);

        }
        private void DoYouKnowWhoIAm(On.RoR2.HealthComponent.orig_Suicide orig, RoR2.HealthComponent self, GameObject killerOverride, GameObject inflictorOverride, DamageType damageType)
        {
            if (self.body.baseNameToken is "ZotBody")
            {
                GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                GameObject original27 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/MagmaWormImpactExplosion");
                GameObject original277 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/ArtifactShellExplosion");


                GameObject original2737 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/ArtifactworldPortalSpawnEffect");
                GameObject original2777 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/GrandparentDeathEffectLightShafts");
                GameObject original2738 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/VagrantNovaExplosion");
                RoR2.EffectData blast = new RoR2.EffectData();
                blast.scale = 10f;
                blast.origin = self.body.corePosition;
                RoR2.EffectManager.SpawnEffect(original27, blast, true);
                RoR2.EffectManager.SpawnEffect(original2738, blast, true);
                RoR2.EffectManager.SpawnEffect(original2, blast, true);

                RoR2.EffectManager.SpawnEffect(EntityStates.QuestVolatileBattery.CountDown.explosionEffectPrefab, blast, true);

                RoR2.EffectManager.SpawnEffect(original277, blast, true);
                RoR2.EffectManager.SpawnEffect(original2737, blast, true);
                RoR2.EffectManager.SpawnEffect(original2777, blast, true);





                RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();
                LandingBlast.attacker = self.body.gameObject;
                LandingBlast.inflictor = self.body.gameObject;
                LandingBlast.teamIndex = TeamIndex.Monster;
                LandingBlast.baseDamage = 1000;
                LandingBlast.baseForce = 300f;
                LandingBlast.position = self.body.corePosition;
                LandingBlast.radius = 50f;

                LandingBlast.losType = BlastAttack.LoSType.NearestHit;
                LandingBlast.falloffModel = RoR2.BlastAttack.FalloffModel.None;
                LandingBlast.attackerFiltering = AttackerFiltering.NeverHitSelf;
                LandingBlast.Fire();
                RoR2.BlastAttack LandingBlast2 = new RoR2.BlastAttack();
                LandingBlast2.attacker = self.body.gameObject;
                LandingBlast2.inflictor = self.body.gameObject;
                LandingBlast2.teamIndex = TeamIndex.Player;
                LandingBlast2.baseDamage = 1000;
                LandingBlast2.baseForce = 111500f;
                LandingBlast2.position = self.body.corePosition;
                LandingBlast2.radius = 50f;

                LandingBlast2.losType = BlastAttack.LoSType.NearestHit;
                LandingBlast2.falloffModel = RoR2.BlastAttack.FalloffModel.None;
                LandingBlast2.attackerFiltering = AttackerFiltering.NeverHitSelf;
                LandingBlast2.Fire();


            }
            else
                orig(self, killerOverride, inflictorOverride, damageType);
        }
        private void zotcheck2(On.RoR2.Stage.orig_Start orig, RoR2.Stage self)
        {
            On.RoR2.FootstepHandler.Footstep_AnimationEvent -= FootstepBlast;
            GemstoAllocate = 0f;

            GemstoAllocate = GemsActive;
            GemsActive = 0f;
            zotcheck = false;
            Debug.Log("Gems to allocate: " + GemstoAllocate.ToString());
            Inevitable += 7 * GemstoAllocate;

            GemstoAllocate = 0f;
            Inevitable += 21;
            Debug.Log("Gems Active:" + GemsActive.ToString());
            if (GemPowerVal < MaxGemPowerVal / 1.8f)
            { GemPowerVal = MaxGemPowerVal / 1.8f; };

            orig(self);
        }
        void FixedUpdate()
        {

            if (!zotcheck)
            {

                // Debug.Log("Zot checking.");
                if (PlayerCharacterMasterController.instances[0].body.baseNameToken is "ZotBody")
                {
                    model = PlayerCharacterMasterController.instances[0].body;
                }





                if (model != null)
                {
                    //  Debug.Log("We got a Zot");

                    modeltransform = model.modelLocator.modelTransform;
                    motor = model.characterMotor;
                    actualmodel = modeltransform.GetComponent<RoR2.CharacterModel>();
                    zotisingame = true;
                    On.RoR2.FootstepHandler.Footstep_AnimationEvent += FootstepBlast;
                    // model.inventory.enabled = true;

                    zotcheck = true;
                    On.RoR2.CharacterBody.OnKilledOtherServer += GemCollection;
                    On.RoR2.ExperienceManager.AwardExperience += ZotNoEXP;

                    animator = model.modelLocator.modelTransform.GetComponent<Animator>();

                    component = model.modelLocator.modelTransform.GetComponent<ChildLocator>();
                    model.baseMaxHealth = 800 + ((GemStack * 100 * GemStack * 0.0022f)) + (GemStack * 50);
                    if (component)
                    {
                        rightshield = component.FindChild("RightShield");
                        leftshield = component.FindChild("LeftShield");
                        leftfoot = component.FindChild("LeftFoot");
                        rightfoot = component.FindChild("RightFoot");
                        righthand = component.FindChild("RightHand");
                        lefthand = component.FindChild("LeftHand");

                        if (leftshield)
                        {
                            LeftTrail = leftshield.GetComponent<TrailRenderer>();
                            LeftTrail.emitting = false;

                        }
                        if (rightshield)
                        {
                            RightTrail = rightshield.GetComponent<TrailRenderer>();
                            RightTrail.emitting = false;
                        }
                    }



                    else
                    {
                        //  Debug.Log("There is no Zot. How could this happen?");


                        zotisingame = false;
                        zotcheck = true;
                    };


                    //  Debug.Log("Done checking for Zots");

                }

            }

            if (zotisingame)
            {

                if (GempowerSlider)
                {
                    GempowerSlider.maxValue = MaxGemPowerVal;
                    GempowerSlider.value = GemPowerVal;
                    //GemBarText.text = GemPowerDisplayValue;
                }

                if (superjumptogglecooldown > 0f)
                { superjumptogglecooldown -= Time.fixedDeltaTime; };


                if (model.baseNameToken is "ZotBody")
                {

                    if (Input.GetKey(KeyCode.U) && Input.GetKey(KeyCode.LeftControl) && superjumptogglecooldown <= 0)
                    {
                        GemStack += 10;
                        GemPowerVal = MaxGemPowerVal;
                        floatpower = 1000f;
                        superjumptogglecooldown = 0.2f;

                    }
                    if (Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.LeftControl) && superjumptogglecooldown <= 0)
                    {
                        //  On.RoR2.GenericPickupController.GrantItem -= ZotNoItems;
                        //  On.RoR2.ExperienceManager.AwardExperience -= ZotNoEXP;

                    }



                    if (Input.GetKey(KeyCode.L) && superjumptogglecooldown <= 0 && !Superjump && Input.GetKey(KeyCode.LeftControl))
                    {
                        Superjump = true;
                        superjumptogglecooldown = 1f;
                        GameObject original2777 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/BrotherSunderWaveEnergizedExplosion");
                        RoR2.EffectData blast = new RoR2.EffectData();
                        blast.scale = 10f;
                        blast.origin = model.corePosition;
                        RoR2.EffectManager.SpawnEffect(original2777, blast, true);
                    };

                    if (Input.GetKey(KeyCode.L) && superjumptogglecooldown <= 0 && Superjump && Input.GetKey(KeyCode.LeftControl))
                    {
                        Superjump = false;
                        superjumptogglecooldown = 1f;
                        GameObject original2777 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/BrotherSunderWaveEnergizedExplosion");
                        RoR2.EffectData blast = new RoR2.EffectData();
                        blast.scale = 10f;
                        blast.origin = model.corePosition;
                        RoR2.EffectManager.SpawnEffect(original2777, blast, true);
                    };


                    if (model.inputBank.sprint.down && superjumptogglecooldown <= 0)
                    {
                        model.isSprinting = false;
                        if (timeRemaining <= 0.1f)
                        {
                            floatpower += 8f;
                            GameObject original = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                            GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");

                            //   EffectSettings.parentToReferencedTransform = true;
                            RoR2.EffectData effectData = new RoR2.EffectData();
                            effectData.origin = lefthand.position;
                            effectData.scale = 1f;
                            RoR2.EffectManager.SpawnEffect(original, effectData, true);

                            RoR2.EffectData effectData2 = new RoR2.EffectData();
                            effectData.origin = righthand.position;
                            effectData.scale = 1f;
                            RoR2.EffectManager.SpawnEffect(original2, effectData, true);




                            ticker2 = 2f;


                            animator.SetFloat("ShieldsIn", 0.12f);


                        }
                        if (timeRemaining >= 0)
                        {

                            timeRemaining = 6f;
                        };




                    };

                    if (GemPowerVal > MaxGemPowerVal)
                    { GemPowerVal = MaxGemPowerVal; };
                    if (GemPowerVal < 0)
                    { GemPowerVal = 0; };
                    MaxGemPowerVal = 90f + (GemStack * 1.4f) + (GemStack * GemStack * 0.035f);
                    if (GemPowerVal >= 1)
                    {
                        model.skillLocator.ResetSkills();
                        Gemdrain = 0f;
                    };

                    if (GemPowerVal < MaxGemPowerVal)
                    { GemPowerVal += MaxGemPowerVal * 0.000045f; };
                    if (Superjump)
                    {
                        if (Chargingjump)
                        {
                            Charged += GemPowerVal * 0.0027f;
                            GemPowerVal -= Time.fixedDeltaTime * 1.4f;
                            GemPowerVal -= GemPowerVal * 0.0027f;
                            Charged += Time.fixedDeltaTime * 2;
                        }
                    };
                    if (Chargingjump)
                    {

                        GemPowerVal -= Time.fixedDeltaTime * 1.4f;
                        Charged += Time.fixedDeltaTime * 2.8f;
                    }
                    if (Charging)
                    {
                        GemPowerVal -= GemPowerVal * 0.0007f;
                        ChargedSlam += GemPowerVal * 0.0007f;
                        GemPowerVal -= Time.fixedDeltaTime * 1.4f;
                        ChargedSlam += Time.fixedDeltaTime * 2.8f;

                    };


                };
                if (DisplayMomentum)
                {

                    MomentumDamageDisplay.text = (((int)momentum).ToString() + "!");
                    MomentumDamageDisplay.color = new Color(255f, 55f, 55f, 255f);
                    MomentumDamageDisplaytime = 3f;
                    momentum = 0f;

                    DisplayMomentum = false;
                }

                //  if (MomentumDamageDisplaytime <= 0f)
                // { UnityEngine.Object.Destroy(MomentumDamageDisplay); };
                if (MomentumDamageDisplay)
                {// MomentumDamageDisplay.transform.position += new Vector3(0, 1, 0);
                 // MomentumDamageDisplay.alpha -= 2f;
                }


                if (model.isSprinting)
                {
                    model.isSprinting = false;
                };


                if (flight)
                {

                    GemPowerVal -= Time.fixedDeltaTime * 1.5f;
                    /* RoR2.ShakeEmitter shakeEmitter;
                     shakeEmitter = model.gameObject.AddComponent<RoR2.ShakeEmitter>();
                     shakeEmitter.wave = new Wave
                     {
                         amplitude = 0.05f,
                         frequency = 180f,
                         cycleOffset = 0f
                     };
                     shakeEmitter.duration = 0.1f;
                     shakeEmitter.radius = 50f;
                     shakeEmitter.amplitudeTimeDecay = false;
 */


                    model.characterMotor.moveDirection += model.inputBank.moveVector;
                    if (motor.velocity.magnitude <= 50)
                    {
                        model.characterMotor.velocity += model.inputBank.moveVector * 1.5f;
                    }

                };



                if (flight && model.inputBank.jump.down)
                {




                    model.characterMotor.velocity += new Vector3(0, 0.5f, 0);



                };
                if (flight && model.inputBank.sprint.down)
                {




                    model.characterMotor.velocity += new Vector3(0, -0.5f, 0);



                };





                if (Gemdrain > 0f)
                {
                    Gemdrain -= Time.fixedDeltaTime;

                };






                if (ticker22 <= 0f)
                {
                    if (leftikfollow < leftlegik && (leftlegik - leftikfollow) > 0.05)
                    {
                        leftikfollow += leftlegik * 0.01f;
                    };

                    if (leftikfollow > leftlegik && leftikfollow > 0.1f && (leftikfollow - leftlegik) > 0.02)
                    {

                        leftikfollow -= leftikfollow * 0.01f; ;
                    };

                    if (rightikfollow < rightlegik && (rightlegik - rightikfollow) > 0.05)
                    {
                        rightikfollow += rightlegik * 0.01f;
                    };

                    if (rightikfollow > rightlegik && rightikfollow > 0.1f && (rightikfollow - rightlegik) > 0.02)
                    {
                        rightikfollow -= rightikfollow * 0.01f;
                    };
                    if (flight && ticker222 <= 0.1f)
                    {
                        /* ChildLocator component = model.modelLocator.modelTransform.GetComponent<ChildLocator>();

                         if (component)
                         {


                             Transform transform = component.FindChild("LeftFoot");
                             Transform transform2 = component.FindChild("RightFoot");
                             if (transform)
                             {
                                 RoR2.EffectData effectData = new RoR2.EffectData();
                                 effectData.origin = leftfoot.position;

                                 effectData.scale = 5f;


                                 //    RoR2.EffectManager.SpawnEffect(originalag, effectData, false);
                                 RoR2.EffectManager.SpawnEffect(leftpunch, effectData, true);
                             };
                             if (transform2)
                             {
                                 RoR2.EffectData effectData = new RoR2.EffectData();
                                 effectData.origin = rightfoot.position;
                                 effectData.scale = 5f;


                                 //   RoR2.EffectManager.SpawnEffect(originalag, effectData, false);
                                 RoR2.EffectManager.SpawnEffect(leftpunch, effectData, true);
                             };
                         };*/

                        ticker22 = 0.01f;
                        ticker222 = 0.06f;

                    }


                };
                gemtick -= Time.fixedDeltaTime;
                ticker22 -= Time.fixedDeltaTime;
                ticker222 -= Time.fixedDeltaTime;
                if (Inevitable >= 7f)
                {


                    Inevitable -= 7f;
                    GemsActive += 1f;
                    GemCard = LegacyResourcesAPI.Load<RoR2.SpawnCard>("SpawnCards/CharacterSpawnCards/cscLunarGolem");
                    GemCard = UnityEngine.Object.Instantiate<RoR2.SpawnCard>(GemCard);
                    Gemmasterprefab = GemCard.prefab;
                    Gemmasterprefab = R2API.PrefabAPI.InstantiateClone(Gemmasterprefab, Gemmasterprefab.name, true);

                    Gemmasterprefab.GetComponent<RoR2.CharacterAI.BaseAI>().enabled = false;
                    RoR2.CharacterMaster component = Gemmasterprefab.GetComponent<RoR2.CharacterMaster>();
                    component.GetComponent<RoR2.CharacterAI.BaseAI>().enabled = false;
                    Gembody = LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/TimeCrystalBody");
                    Gembody = R2API.PrefabAPI.InstantiateClone(Gembody, Gembody.name, true);

                    component.bodyPrefab = Gembody;

                    component.GetComponent<RoR2.CharacterAI.BaseAI>().enabled = false;
                    GemCard.prefab = Gemmasterprefab;




                    PrefabAPI.RegisterNetworkPrefab(Gemmasterprefab);
                    // Debug.Log("masterfab");
                    PrefabAPI.RegisterNetworkPrefab(Gembody);
                    GemCard.directorCreditCost = 0;

                    // Debug.Log("1");
                    NodeGraph groundNodes = RoR2.SceneInfo.instance.groundNodes;
                    // Debug.Log("2");
                    NodeGraph.NodeIndex nodeIndex = groundNodes.FindClosestNode(model.corePosition + new Vector3(UnityEngine.Random.Range(15, 1000), UnityEngine.Random.Range(15, 10000), UnityEngine.Random.Range(15, 10000)), model.hullClassification, float.PositiveInfinity);
                    groundNodes.GetNodePosition(nodeIndex, out nextgem);
                    //  Debug.Log("3");
                    //RoR2.EffectManager.SimpleEffect(EntityStates.VagrantMonster.FireMegaNova.novaEffectPrefab, nextgem, Quaternion.identity, true);
                    //  Debug.Log("4");
                    string nextgemoutput = nextgem.ToString();
                    //  var rand = new System.Random();
                    //   var node = nodeIndex[rand.Next(1)];
                    //  Debug.Log(nextgemoutput);
                    gemtick = UnityEngine.Random.Range((15), (45));






                    if (NetworkServer.active)
                    {


                        RoR2.DirectorPlacementRule placementRule = new RoR2.DirectorPlacementRule
                        {
                            placementMode = RoR2.DirectorPlacementRule.PlacementMode.Random,
                            minDistance = 15f,
                            maxDistance = 5555f,
                            position = model.transform.position
                        };
                        RoR2.DirectorSpawnRequest GemSpawn = new RoR2.DirectorSpawnRequest(GemCard, placementRule, RoR2.RoR2Application.rng);

                        GemSpawn.teamIndexOverride = new RoR2.TeamIndex?(RoR2.TeamIndex.Neutral);

                        RoR2.DirectorCore.instance.TrySpawnObject(GemSpawn);


                    }



                };




                if (ticker <= 0)
                {
                    model.baseMaxHealth = 800 + ((GemStack * 100 * GemStack * 0.0022f)) + (GemStack * 50);
                    ticker = 1f;
                    /*
                                        RedOrb = Resources.Load<GameObject>("Prefabs/Effects/OrbEffects/FlurryArrowOrbEffect");
                                        RedOrb.GetComponent<RoR2.Orbs.OrbEffect>().parentObjectTransform = component.FindChild("RightHand");

                                        RoR2.EffectManager.SpawnEffect(RedOrb, righttraileffect, true);
                    */


                    //ChildLocator componentyy = model.modelLocator.modelTransform.GetComponent<ChildLocator>();

                    // ProjectileManager.instance.FireProjectile(RedTrailPrefab, componentyy.FindChild("RightHand").position + Vector3.up * 2, Quaternion.identity, model.gameObject, 0, 0, false, DamageColorIndex.Default, component.FindChild("RightHand").gameObject, 1f);




                };
                ticker -= Time.fixedDeltaTime;
                model.healthComponent.Heal(model.healthComponent.missingCombinedHealth * 0.000035f, default(RoR2.ProcChainMask), false);







                if (model.characterMotor.isGrounded)
                {
                    timeinair = 0f;
                    flight = false;
                    model.RemoveBuff(ZotFlight);
                    RoR2.CharacterFlightParameters flightparameters = model.characterMotor.flightParameters;
                    flightparameters.channeledFlightGranterCount = -2;
                    model.characterMotor.flightParameters = flightparameters;
                    RoR2.CharacterGravityParameters gravparams = model.characterMotor.gravityParameters;
                    gravparams.channeledAntiGravityGranterCount = -2;
                    model.characterMotor.gravityParameters = gravparams;



                    /*                      string layer10 = leftikfollow.ToString();
                                         string layer11 = rightikfollow.ToString();
                                        string layer14 = animator.GetFloat("rightdist").ToString();
                                        string layer15 = animator.GetFloat("rightdist").ToString();
                                        string layer12 = leftlegik.ToString();
                                        string layer13 = rightlegik.ToString();
                                        Debug.Log(layer11);
                                        Debug.Log(layer10);
                                        Debug.Log(layer13);
                                        Debug.Log(layer12);
                                        Debug.Log(layer14);
                                        Debug.Log(layer15);*/

                }

                footposition = motor.transform.position;

                leftlegik = (Physics.Raycast(new Ray(leftfoot.position + (model.characterDirection.forward * 3), model.transform.up * -1), out raycastHitleft, 1f, RoR2.LayerIndex.world.mask, QueryTriggerInteraction.Ignore) ? Mathf.Clamp01(1f - (raycastHitleft.distance) / 3f) : 0f);

                rightlegik = (Physics.Raycast(new Ray(rightfoot.position + (model.characterDirection.forward * 3), model.transform.up * -1), out raycastHitright, 1f, RoR2.LayerIndex.world.mask, QueryTriggerInteraction.Ignore) ? Mathf.Clamp01(1f - (raycastHitright.distance) / 3f) : 0f);




                animator.SetFloat("rightdist", rightikfollow);
                animator.SetFloat("leftdist", leftikfollow);
                /*         if (model.inputBank.moveVector.magnitude > 0 && (leftikfollow > 0.28 | rightikfollow > 0.28))
                         {
                             motor.velocity.x += model.inputBank.moveVector.x * 0.1f;
                             motor.velocity.z += model.inputBank.moveVector.z * 0.1f;
                         };*/
                if (jumpcooldown > 0)
                { jumpcooldown -= Time.fixedDeltaTime; };

                if (flightcooldown > 0)
                { flightcooldown -= Time.fixedDeltaTime; };
                if (momentum > 1f)
                { momentum -= (momentum * 0.01f); };

                if (model.inputBank.jump.down && jumpcooldown <= 0 && model.baseNameToken is "ZotBody" && model.characterMotor.isGrounded && !Busy)
                {
                    {

                        SkillStates.ZotJump zotJump = new SkillStates.ZotJump();

                        foreach (EntityStateMachine entityStateMachine in model.GetComponents<EntityStateMachine>())
                        {
                            bool flag = entityStateMachine;
                            if (flag)
                            {
                                bool flag2 = entityStateMachine.customName == "Body";
                                if (flag2)
                                {
                                    BodyStateMachine = entityStateMachine;
                                }
                            }
                        }
                        // entityStateMachine.Name = "Jump";
                        if (BodyStateMachine.networkIdentity.localPlayerAuthority)
                        {
                            BodyStateMachine.SetNextState(zotJump);
                        }
                    };
                };
                if (zotcheck && model.healthComponent.alive == false)
                { zotcheck = false; }

                /*                if (model.inputBank.skill1.down && model.baseNameToken is "LordZot_Body" && !Busy)
                                {
                                    {



                                        foreach (EntityStateMachine entityStateMachine in model.GetComponents<EntityStateMachine>())
                                        {
                                            bool flag = entityStateMachine;
                                            if (flag)
                                            {
                                                bool flag2 = entityStateMachine.customName == "Weapon";
                                                if (flag2)
                                                {
                                                    BodyStateMachine = entityStateMachine;
                                                }
                                            }
                                        }
                                        // entityStateMachine.Name = "Jump";
                                        if (BodyStateMachine.networkIdentity.localPlayerAuthority)
                                        {
                                            BodyStateMachine.SetNextState(new EldritchFury());
                                        }
                                    };
                                };
                                if (model.inputBank.skill4.down && model.baseNameToken is "LordZot_Body" && !Busy)
                                {
                                    {



                                        foreach (EntityStateMachine entityStateMachine in model.GetComponents<EntityStateMachine>())
                                        {
                                            bool flag = entityStateMachine;
                                            if (flag)
                                            {
                                                bool flag2 = entityStateMachine.customName == "Weapon";
                                                if (flag2)
                                                {
                                                    BodyStateMachine = entityStateMachine;
                                                }
                                            }
                                        }
                                        // entityStateMachine.Name = "Jump";
                                        if (BodyStateMachine.networkIdentity.localPlayerAuthority)
                                        {
                                            BodyStateMachine.SetNextState(new EldritchSlam());
                                        }
                                    };
                                };
                                if (model.inputBank.skill2.down && model.baseNameToken is "LordZot_Body" && !Busy)
                                {
                                    {



                                        foreach (EntityStateMachine entityStateMachine in model.GetComponents<EntityStateMachine>())
                                        {
                                            bool flag = entityStateMachine;
                                            if (flag)
                                            {
                                                bool flag2 = entityStateMachine.customName == "Weapon";
                                                if (flag2)
                                                {
                                                    BodyStateMachine = entityStateMachine;
                                                }
                                            }
                                        }
                                        // entityStateMachine.Name = "Jump";
                                        if (BodyStateMachine.networkIdentity.localPlayerAuthority)
                                        {
                                            BodyStateMachine.SetNextState(new ZotBulwark());
                                        }
                                    };
                                };
                                if (model.inputBank.skill3.down && model.baseNameToken is "LordZot_Body" && !Busy)
                                {
                                    {



                                        foreach (EntityStateMachine entityStateMachine in model.GetComponents<EntityStateMachine>())
                                        {
                                            bool flag = entityStateMachine;
                                            if (flag)
                                            {
                                                bool flag2 = entityStateMachine.customName == "Body";
                                                if (flag2)
                                                {
                                                    BodyStateMachine = entityStateMachine;
                                                }
                                            }
                                        }
                                        // entityStateMachine.Name = "Jump";
                                        if (BodyStateMachine.networkIdentity.localPlayerAuthority)
                                        {
                                            BodyStateMachine.SetNextState(new ZotBlink());
                                        }
                                    };
                                };*/

                /*  if (model.inputBank.skill1.down && model.baseNameToken is "LordZot_Body" && !Busy)
                  {
                      {

                          EldritchFury eldritchFury = new EldritchFury();
                          RoR2.EntityStateMachine entityStateMachine = model.GetComponent<RoR2.EntityStateMachine>();
                          if (entityStateMachine.networkIdentity.localPlayerAuthority)
                          {
                              entityStateMachine.SetNextState(eldritchFury);
                          }
                      };
                  };*/
                if (Input.GetKey(LordZot.Floatbutton) && model.baseNameToken is "ZotBody" && !model.characterMotor.isGrounded && !flight && flightcooldown <= 0f)
                {

                    model.AddBuff(ZotFlight);
                    RoR2.ShakeEmitter shakeEmitter;
                    shakeEmitter = model.gameObject.AddComponent<RoR2.ShakeEmitter>();
                    shakeEmitter.wave = new Wave
                    {
                        amplitude = 0.3f,
                        frequency = 180f,
                        cycleOffset = 0f
                    };
                    shakeEmitter.duration = 0.5f;
                    shakeEmitter.radius = 50f;
                    shakeEmitter.amplitudeTimeDecay = true;


                    RoR2.TemporaryOverlay temporaryOverlay = modeltransform.gameObject.AddComponent<RoR2.TemporaryOverlay>();
                    temporaryOverlay.duration = 0.6f;
                    temporaryOverlay.animateShaderAlpha = true;
                    temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    temporaryOverlay.destroyComponentOnEnd = true;
                    temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("materials/matVagrantEnergized");
                    temporaryOverlay.AddToCharacerModel(modeltransform.GetComponent<RoR2.CharacterModel>());
                    GameObject original = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/HuntressBlinkEffect");
                    RoR2.EffectData effectData = new RoR2.EffectData();
                    effectData.origin = model.corePosition;
                    effectData.scale = 15f;
                    RoR2.EffectManager.SpawnEffect(original, effectData, true);


                    RoR2.CharacterFlightParameters flightparameters = model.characterMotor.flightParameters;
                    flightparameters.channeledFlightGranterCount = 2;
                    model.characterMotor.flightParameters = flightparameters;
                    RoR2.CharacterGravityParameters gravparams = model.characterMotor.gravityParameters;
                    gravparams.channeledAntiGravityGranterCount = 2;
                    model.characterMotor.gravityParameters = gravparams;

                    model.characterMotor.velocity = Vector3.zero;
                    LordZot.flight = true;
                    flightcooldown = 0.5f;

                };
                if ((Input.GetKey(LordZot.Floatbutton) | GemPowerVal <= 0.1f) && model.baseNameToken is "ZotBody" && !model.characterMotor.isGrounded && flight && flightcooldown <= 0f)
                {

                    model.RemoveBuff(ZotFlight);
                    RoR2.TemporaryOverlay temporaryOverlay = modeltransform.gameObject.AddComponent<RoR2.TemporaryOverlay>();
                    temporaryOverlay.duration = 0.5f;
                    temporaryOverlay.animateShaderAlpha = true;
                    temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    temporaryOverlay.destroyComponentOnEnd = true;
                    temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("materials/matVagrantEnergized");
                    temporaryOverlay.AddToCharacerModel(modeltransform.GetComponent<RoR2.CharacterModel>());
                    GameObject original = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/HuntressBlinkEffect");
                    RoR2.EffectData effectData = new RoR2.EffectData();
                    effectData.origin = model.corePosition;
                    effectData.scale = 8f;
                    //   RoR2.EffectManager.SpawnEffect(original, effectData, true);


                    RoR2.CharacterFlightParameters flightparameters = model.characterMotor.flightParameters;
                    flightparameters.channeledFlightGranterCount = -2;
                    model.characterMotor.flightParameters = flightparameters;
                    RoR2.CharacterGravityParameters gravparams = model.characterMotor.gravityParameters;
                    gravparams.channeledAntiGravityGranterCount = -2;
                    model.characterMotor.gravityParameters = gravparams;

                    flightcooldown = 0.5f;
                    LordZot.flight = false;
                };

                if (Busy && motor.velocity.y < 0 && !motor.isGrounded)
                { motor.velocity.y += 0.09f; };
                if (motor.velocity.y > 500)
                { motor.velocity.y -= motor.velocity.y * 0.04f; };

                if (Charged > 0f && !motor.isGrounded)
                {
                    Charged -= Charged * 0.003f;
                    //  Debug.Log(Charged.ToString());






                    string debug2 = fallspeed.ToString();
                    //    Debug.Log("speed");
                    //    Debug.Log(debug2);
                    bool isfaster = fallspeed >= fastestfallspeed;
                    if (isfaster)
                    {
                        fastestfallspeed = motor.velocity.y;
                        string debug = fastestfallspeed.ToString();
                        // Debug.Log("fastest");
                        //Debug.Log(debug);
                    }

                    /*if (Busy)
                    { motor.velocity.y = motor.velocity.y * 0.99f; };*/



                };
                if (!Busy && !motor.isGrounded && !flight)
                {
                    fallspeed = motor.velocity.y;
                    motor.velocity.y -= 0.0055f + timeinair;
                    timeinair += 0.0335f;
                }


                if (!motor.isGrounded && !flight && Charged > 1)
                {


                };



                if (timeRemainingstun > 0 && motor.isGrounded)
                {
                    //    model.characterMotor.Motor.enabled = true;
                    timeinair = 0f;
                    fastestfallspeed = 0f;
                    timeRemainingstun -= Time.fixedDeltaTime;
                    motor.velocity = Vector3.zero;
                    motor.moveDirection = Vector3.zero;


                }
                else

                {

                    string debug = fastestfallspeed.ToString();

                };

                if (Followfloatpower < floatpower)
                {
                    Followfloatpower += floatpower * 0.01f;
                    Followfloatpower += Time.fixedDeltaTime * 2;
                }
                else
                {
                    Followfloatpower -= Followfloatpower * 0.01f;
                    Followfloatpower -= Time.fixedDeltaTime * 1;
                };
                if (floatpower < 0)
                { floatpower = 0; };
                if (floatpower > 0f)
                {
                    floatpower -= Time.fixedDeltaTime * 3;


                };



                if (floatpower > 10f)
                {
                    floatpower -= floatpower * 0.015f;
                    Followfloatpower -= Followfloatpower * 0.015f;
                };
                if (mat != null)
                {
                    mat.SetFloat("_EmPower", Followfloatpower);
                }
                if (model.modelLocator.modelTransform.GetComponent<RoR2.CharacterModel>())
                {
                    foreach (RoR2.CharacterModel.RendererInfo renderInfo in model.modelLocator.modelTransform.GetComponent<RoR2.CharacterModel>().baseRendererInfos)
                    {

                        {
                            mat = renderInfo.defaultMaterial;

                        }
                    }
                }

                if ((floatpower < ChargedSlam | floatpower < ChargedLaser) && Busy)
                {
                    floatpower += Time.fixedDeltaTime * 55;
                    Followfloatpower += Time.fixedDeltaTime * 55;
                };
                if (Followfloatpower < 0f)
                { Followfloatpower = 0f; };


            }
        }
        private void ZotNoEXP(On.RoR2.ExperienceManager.orig_AwardExperience orig, RoR2.ExperienceManager self, Vector3 origin, RoR2.CharacterBody body, ulong amount)
        {

            if (body.baseNameToken is "ZotBody")
            {
                amount = 0;

            }


            orig(self, origin, body, amount);

        }

        /* private void ZotNoItems(On.RoR2.GenericPickupController.orig_GrantItem orig, RoR2.GenericPickupController self, RoR2.CharacterBody body, RoR2.Inventory inventory)
         {
             if (body = model)
             { inventory.enabled = false; };

                orig(self, body, inventory);

         }
 */
        void Update()
        {
            if (zotisingame)
            {
                /*    if (Inevitable >= 10)
                    {
                        Inevitable = 0f;




                    }*/
                //  GempowerValpercent = Math.Clamp((MaxGemPowerVal / GemPowerVal), 0.35f, 0.6f);
                //   rectTransformfill.anchorMax = new Vector2(GempowerValpercent, 0.09f);
                if (!RoR2.PauseManager.isPaused)
                {


                    Inevitable += Time.fixedDeltaTime * 0.14f;

                    if (GemBarUIReal)
                    {
                        //GemBarUIReal.maxValue = Mathf.Max(GemPowerVal, GemBarUIReal.maxValue);
                        GemBarUIReal.value = Mathf.Clamp((GemPowerVal / MaxGemPowerVal), 0, 1);
                        //  GemBarUIReal.value = 0.5f;
                    }

                }
                GemPowerDisplayValue = (GemPowerVal.ToString("0.0") + "/" + MaxGemPowerVal.ToString("0"));
                MISCINFO2.text = ("Mass:" + mass.ToString() + " Momentum:" + momentum.ToString());
                // + "|Gems Collected:" + GemsCollected.ToString("0")
                GemBarText.text = GemPowerDisplayValue;





                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;

                }
                else
                {
                    var player = RoR2.LocalUserManager.GetFirstLocalUser();

                    var model = player.cachedBody.modelLocator.modelTransform;
                    var animator = model.GetComponent<Animator>();

                    animator.SetFloat("ShieldsIn", 0f);

                    ChildLocator component = model.GetComponent<ChildLocator>();


                    if (component)
                    {

                        GameObject original = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                        GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                        //   EffectSettings = original2.GetComponent<RoR2.EffectComponent>();
                        //   EffectSettings.applyScale = true;
                        //   EffectSettings.soundName.IsNullOrWhiteSpace();
                        //   EffectSettings2 = original.GetComponent<RoR2.EffectComponent>();
                        //   EffectSettings2.applyScale = true;
                        //   EffectSettings2.soundName.IsNullOrWhiteSpace();
                        Transform transform = component.FindChild("LeftHand");
                        Transform transform2 = component.FindChild("RightHand");
                        if (transform)
                        {
                            RoR2.EffectData effectData = new RoR2.EffectData();
                            effectData.origin = transform.position;
                            effectData.scale = 1f;
                            RoR2.EffectManager.SpawnEffect(original, effectData, true);
                        }
                        if (transform2)
                        {
                            RoR2.EffectData effectData = new RoR2.EffectData();
                            effectData.origin = transform2.position;
                            effectData.scale = 1f;
                            RoR2.EffectManager.SpawnEffect(original2, effectData, true);
                        }
                    }
                    timeRemaining = 1000f;
                    //   RoR2.EntityStateMachine entityStateMachine = new RoR2.EntityStateMachine();
                    //  entityStateMachine.SetState(zotunSummonShield);

                    //   Debug.Log("statemachine");

                    //  Debug.Log("setstate");

                }


            }

        }
        private void FootstepBlast(On.RoR2.FootstepHandler.orig_Footstep_AnimationEvent orig, RoR2.FootstepHandler self, AnimationEvent animationEvent)
        {
            {
                orig(self, animationEvent);
            }
            bool flag3 = self.body.baseNameToken is "ZotBody";
            if (flag3)
            {


                bool flag = animationEvent.stringParameter is "LeftFoot";
                bool flag2 = animationEvent.stringParameter is "RightFoot";

                if (mass > 10)
                {
                    var player = playerCharacterMaster;

                    var bod = model;
                    var modely = bod.modelLocator.modelTransform;

                    var animaty = modely.GetComponent<Animator>();
                    RoR2.ShakeEmitter shakeEmitter = zotPrefab.gameObject.AddComponent<RoR2.ShakeEmitter>();
                    shakeEmitter.wave = new Wave
                    {
                        amplitude = 1.5f,
                        frequency = 180f,
                        cycleOffset = 0f
                    };
                    shakeEmitter.duration = 1.6f;
                    shakeEmitter.radius = 50f;
                    shakeEmitter.amplitudeTimeDecay = true;
                }

                if (flag)
                {

                    var bod2 = model;
                    var model2 = bod2.modelLocator.modelTransform;

                    ChildLocator component = model2.GetComponent<ChildLocator>();
                    if (component)
                    {
                        Transform transform = component.FindChild("LeftFoot");

                        if (transform)
                        {

                            var position = transform.position + new Vector3(0f, 0f, 0f);

                            new RoR2.BlastAttack
                            {
                                procCoefficient = 0f,
                                attacker = model.gameObject,
                                inflictor = model.gameObject,
                                teamIndex = RoR2.TeamComponent.GetObjectTeam(model.gameObject),
                                baseDamage = 100 + mass,
                                baseForce = 1 + (mass * 0.01f),
                                position = position,
                                radius = 3f,

                                losType = BlastAttack.LoSType.NearestHit,
                                falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                                crit = false
                            }.Fire();
                            RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();
                            LandingBlast.attacker = bod2.gameObject;
                            LandingBlast.inflictor = bod2.gameObject;
                            LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
                            LandingBlast.baseDamage = (mass * 0.10f);
                            LandingBlast.baseForce = 15f;

                            LandingBlast.losType = BlastAttack.LoSType.NearestHit;
                            LandingBlast.bonusForce = new Vector3(0f, 340f + (mass * 0.3f), 0f);
                            LandingBlast.position = position;
                            LandingBlast.radius = 13f + (mass * 0.005f);
                            LandingBlast.procCoefficient = 0f;
                            LandingBlast.crit = false;
                            LandingBlast.falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot;
                            LandingBlast.attackerFiltering = AttackerFiltering.NeverHitSelf;
                            LandingBlast.Fire();



                            GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");


                            RoR2.EffectData effectData2 = new RoR2.EffectData();

                            effectData2.origin = position;
                            effectData2.scale = 1.2f + (mass * 0.005f);
                            GameObject original66 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                            RoR2.EffectData effectData3 = new RoR2.EffectData();

                            effectData3.origin = position + new Vector3(0f, -1f, 0f);
                            effectData3.scale = 2.5f + (mass * 0.02f);
                            RoR2.EffectManager.SpawnEffect(original66, effectData3, true);
                            RoR2.EffectManager.SpawnEffect(original2, effectData2, true);
                            //  GameObject originaly = Resources.Load<GameObject>("prefabs/effects/impacteffects/engiconcussionexplosion");

                            // RoR2.EffectManager.SpawnEffect(originaly, effectData2, true);
                            //  RoR2.EffectManager.SpawnEffect(original3, effectData2, false);
                        }
                    }
                }



                if (flag2)
                {


                    var bod2 = model;
                    var model2 = bod2.modelLocator.modelTransform;

                    ChildLocator component = model2.GetComponent<ChildLocator>();
                    if (component)
                    {
                        Transform transform = component.FindChild("RightFoot");

                        if (transform)
                        {

                            var position = transform.position + new Vector3(0f, 0f, 0f);
                            new RoR2.BlastAttack
                            {
                                procCoefficient = 0f,
                                attacker = model.gameObject,
                                inflictor = model.gameObject,
                                teamIndex = RoR2.TeamComponent.GetObjectTeam(model.gameObject),
                                baseDamage = 100 + mass,
                                baseForce = 1 + (mass * 0.01f),

                                losType = BlastAttack.LoSType.NearestHit,
                                position = position,
                                radius = 3f,
                                falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                                crit = false
                            }.Fire();
                            RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();
                            LandingBlast.attacker = bod2.gameObject;

                            LandingBlast.losType = BlastAttack.LoSType.NearestHit;
                            LandingBlast.inflictor = bod2.gameObject;
                            LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
                            LandingBlast.baseDamage = (mass * 0.10f);
                            LandingBlast.baseForce = 15f;
                            LandingBlast.bonusForce = new Vector3(0f, 340f + (mass * 1f), 0f);
                            LandingBlast.position = position;
                            LandingBlast.radius = 17f + (mass * 0.003f);
                            LandingBlast.procCoefficient = 0f;
                            LandingBlast.crit = false;
                            LandingBlast.falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot;
                            LandingBlast.attackerFiltering = AttackerFiltering.NeverHitSelf;
                            LandingBlast.Fire();

                            GameObject original2 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                            RoR2.EffectData effectData2 = new RoR2.EffectData();

                            effectData2.origin = position;
                            effectData2.scale = 1.2f + (mass * 0.005f);
                            GameObject original66 = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                            RoR2.EffectData effectData3 = new RoR2.EffectData();

                            effectData3.origin = position + new Vector3(0f, -1f, 0f);
                            effectData3.scale = 2.5f + (mass * 0.02f);
                            RoR2.EffectManager.SpawnEffect(original66, effectData3, true);
                            RoR2.EffectManager.SpawnEffect(original2, effectData2, true);
                            //  GameObject originaly = Resources.Load<GameObject>("prefabs/effects/impacteffects/fmjimpact");

                            //  RoR2.EffectManager.SpawnEffect(originaly, effectData2, true);
                            //  RoR2.EffectManager.SpawnEffect(original3, effectData2, false);
                        }
                    }

                }
            }
        }


        }

    }