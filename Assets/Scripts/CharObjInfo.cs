//**************************************************************************************
// File: CharObjInfo.cs
//
// Purpose: This is the base class for all information and interactions 
// for character objects
// (i.e. Players, Monsters, Npcs, ect)
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;

public enum CharacterPartEnum
{
    ProjectileSpawner = 0,
    LeftHand = 1,
    RightHand = 2,
    Prop1 = 3,
    Prop2 = 4,
    Prop3 = 5,
    Prop4 = 6,
    Prop5 = 7,
    LeftFoot = 8,
    RightFoot = 9
}

[AdvancedInspector]
public abstract class CharObjInfo : DestructibleObjInfo
{
    #region Declarations
    protected string m_charName; //Name of the character to be displayed
    protected Loadout m_loadout; //Stores the character's loadout

    //Pointers to different classes
    #region Pointers
    [Inspect, Group("Pointers")]
    public PlayerController m_playerController;

    [Inspect, Group("Pointers")]
    public Transform m_carrierTransform;

    [Inspect, Group("Pointers")]
    public Transform[] m_attachablePartTransforms = new Transform[3]; //Parts of the character that can be targeted for attachment

    [Inspect, Group("Pointers")]
    public Sprite m_footLeftTrack;

    [Inspect, Group("Pointers")]
    public Sprite m_footRightTrack;
    #endregion

    //Basic stats
    #region Stats
    [Inspect(1), Group("Stats")]
    protected float m_stamina; //Current stamina
    [Inspect(1), Group("Stats")]
    protected int m_maxStamina; //Maximum stamina
    [Inspect(1), Group("Stats")]
    protected float m_staminaRegenTimer; //Time left before stamina can regenerate
    [Inspect(1), Group("Stats")]
    protected float m_staminaRegenDelayTime; //Delay time after spending stamina before it regenerates
    [Inspect(1), Group("Stats")]
    protected float m_staminaRegenDelayTimeLong; //Longer than normal delay time after spending stamina before it regenerates
    [Inspect(1), Group("Stats")]
    protected int m_staminaRegenRate; //How much stamina regenerates per second
    [Inspect(1), Group("Stats")]
    protected float m_sprintStaminaCost; //How much stamina will be spent per second while sprinting
    [Inspect(1), Group("Stats")]
    protected int m_dodgeCount; //Current dodges remaining
    [Inspect(1), Group("Stats")]
    protected float m_dodgeCooldownTimer; //Current dodge cooldown remaining
    [Inspect(1), Group("Stats")]
    protected float m_dodgeCooldown; //Dodge cooldown duration


    [Inspect(1), Group("Stats")]
    protected float m_mana; //Current mana
    [Inspect(1), Group("Stats")]
    protected int m_maxMana; //Maxmimum mana


    [Inspect(1), Group("Stats")]
    protected float m_poiseDamage; //Current poise damage build up
    [Inspect(1), Group("Stats")]
    protected int m_maxPoise; //Maximum poise
    [Inspect(1), Group("Stats")]
    protected float m_poiseRegenTimer; //Time left before poise can regenerate
    [Inspect(1), Group("Stats")]
    protected float m_poiseRegenDelayTime; //Delay time after spending poise before it regenerates
    [Inspect(1), Group("Stats")]
    protected int m_poiseRegenRate; //How much poise regenerates per second
    [Inspect(1), Group("Stats")]
    protected float m_poiseDamageImmunityCurrTimer; //Time left before poise damage immunity expires
    [Inspect(1), Group("Stats")]
    protected float m_poiseDamageImmunityTime; //Amount of time player will become immune to poise damage

    [Inspect(1), Group("Stats")]
    protected float m_cheatDeathTimer; //Cheat death time remaining
    [Inspect(1), Group("Stats")]
    protected float m_cheatDeathDuration; //Amount of time cheat death will last after dropping into final threshold
    #endregion

    //Movement stats
    #region Movement
    [Inspect, Group("Movement", 3)]
    public float m_walkSpeed = 20;
    [Inspect, Group("Movement")]
    public float m_runSpeed = 45;
    [Inspect, Group("Movement")]
    public float m_sprintSpeed = 120;
    [Inspect, Group("Movement")]
    public float m_turnSpeed = 15;
    #endregion

    //Stats related to the current action being taken (ex: StunGuard, temporary damage reduction, movement modifiers, ect)
    #region Action Stats
    [Inspect, Group("Action Stats")]
    public float m_poiseAnimMod = 0; //This acts like somewhat like stunguard in battlecon. This will be considered as part of your max poise (poise damage adds up, and breaks the poise threshold)
    [Inspect, Group("Action Stats")]
    public float m_armorAnimMod = 0; //This acts like soak in battlecon. Basic damage suffered will be reduced by this amount
    [Inspect, Group("Action Stats")]
    public float m_turnSpeedRightAnimMod = 1; //This controls how much of the turn speed is applied when turning toward the right (0 = 0%, 1 = 100%)
    [Inspect, Group("Action Stats")]
    public float m_turnSpeedLeftAnimMod = 1; //This controls how much of the turn speed is applied when turning toward the left (0 = 0%, 1 = 100%)
    #endregion
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    /// <param name="pCharName"></param>
    /// <param name="pLoadout"></param>
    /// <param name="pObjType"></param>
    public void Initialize(string pCharName, string pLoadout, string pObjType = "CharObjInfo")
    {
        base.Initialize(false, pObjType, new CharStatusEffectManager(this)); //TO DO: Implement object pooling for characters???

        m_maxStamina = 100;
        StaminaFloat = m_maxStamina;
        m_staminaRegenDelayTime = 0.8f;
        m_staminaRegenDelayTimeLong = 1.6f; // 2f;
        m_staminaRegenTimer = 0;
        m_staminaRegenRate = 40;
        m_sprintStaminaCost = 20f;

        m_maxMana = 100;
        ManaFloat = m_maxMana;

        m_maxPoise = 100;
        m_poiseDamage = 0;
        m_poiseRegenDelayTime = 0.8f;
        m_poiseRegenTimer = 0;
        m_poiseRegenRate = 40;
        m_poiseDamageImmunityCurrTimer = 0;
        m_poiseDamageImmunityTime = 1.1f; //TO DO: Possibly apply this time at the end of the stagger animation so that I don't need to manually sync it?
                                          //TO DO: Consider having immunity only apply after being staggered twice in a certain amount of time?

        m_cheatDeathTimer = 0;
        m_cheatDeathDuration = 0.3f;


        DodgeCount = 2;
        m_dodgeCooldown = 2;
        m_dodgeCooldownTimer = m_dodgeCooldown;


        m_loadout = new Loadout(this, gameObject, pLoadout);
        m_loadout.SwitchWeapons(1);


        //Point all the animator's scripts to this character object
        StateMachineBehaviourWithCharObjInfoPointer[] stateMachineBehaviorsWithCharObjInfo = m_animator.GetBehaviours<StateMachineBehaviourWithCharObjInfoPointer>();
        foreach (StateMachineBehaviourWithCharObjInfoPointer behavior in stateMachineBehaviorsWithCharObjInfo)
        {
            behavior.CharObjInfo = this;
        }

        //TO DO: REMOVE ME IF WE CONTINUE WITH HUDLESSHEIM
        //Initialize more of the floating HUD's details
        //((CharFloatingHUDController)m_floatingHUDController).InitializeSubBars();
    }

    /// <summary>
    /// Accepts an object type and returns true if one of the types
    /// that it is matches the type passed.
    /// i.e. PlayerObjInfo is also a CharObjInfo and a DestructibleObjInfo
    /// </summary>
    /// <param name="pObjType">Object Type string to check</param>
    /// <returns></returns>
    public override bool IsType(string pObjType)
    {
        if (pObjType == "ActorObjInfo" || pObjType == "DestructibleObjInfo" || pObjType == "CharObjInfo" || pObjType == ObjType)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Initializes all the interactions for this object
    /// </summary>
    protected override void InitializeInteractionData()
    {
        //Set default interaction audio clips
        m_interactAudioClips[(int)InteractionType.SharpLight] = AudioClipEnum.CharacterHit1;
        m_interactAudioClips[(int)InteractionType.SharpMedium] = AudioClipEnum.CharacterHit1;
        m_interactAudioClips[(int)InteractionType.SharpHeavy] = AudioClipEnum.CharacterHit1;
        m_interactAudioClips[(int)InteractionType.BluntLight] = AudioClipEnum.CharacterHit1;
        m_interactAudioClips[(int)InteractionType.BluntMedium] = AudioClipEnum.CharacterHit1;
        m_interactAudioClips[(int)InteractionType.BluntHeavy] = AudioClipEnum.CharacterHit1;
        m_interactAudioClips[(int)InteractionType.LightBurn] = AudioClipEnum.FireTest;

        //Set default interaction particle effects
        m_interactParticleEffects[(int)InteractionType.SharpLight] = ParticleEffectEnum.Blood;
        m_interactParticleEffects[(int)InteractionType.SharpMedium] = ParticleEffectEnum.Blood;
        m_interactParticleEffects[(int)InteractionType.SharpHeavy] = ParticleEffectEnum.Blood;
        m_interactParticleEffects[(int)InteractionType.BluntLight] = ParticleEffectEnum.HitDust;
        m_interactParticleEffects[(int)InteractionType.BluntMedium] = ParticleEffectEnum.HitDust;
        m_interactParticleEffects[(int)InteractionType.BluntHeavy] = ParticleEffectEnum.HitDust;
        m_interactParticleEffects[(int)InteractionType.LightBurn] = ParticleEffectEnum.LightBurn;
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    protected override void Start()
    {
        if (!m_initialized)
        {
            Initialize("FathErr", "0,0,0,0,0", "PlayerObjInfo");
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected override void Update()
    {
        base.Update();

        HandleStaminaRegen(Time.deltaTime);
        HandlePoiseRegen(Time.deltaTime);
        HandlePoiseDamageImmunity(Time.deltaTime);
        HandleCheatDeathTimer(Time.deltaTime);
    }

    /// <summary>
    /// Spawns a floating hud for this destructible obj info
    /// </summary>
    protected override void CreateFloatingHUD()
    {
        m_floatingHUDController = GUIFactory.CreateCharFloatingHUD(this, 0, new Vector2(0, 3));
    }

    /// <summary>
    /// Handles what happens when a destructible object runs out of health
    /// </summary>
    protected override void OnDeath()
    {
        m_animator.SetTrigger("Death");
        m_rigidbody.simulated = false;
    }

    /// <summary>
    /// This is used so that OnDeath can trigger an animation which
    /// can trigger OnDeathFinalAnim which will call OnDeathFinal
    /// </summary>
    protected override void OnDeathFinal()
    {
        m_loadout.CleanUp();
        DestroyMe();
    }

    /// <summary>
    /// Handles regenerating poise
    /// </summary>
    /// <param name="pDeltaTime"></param>
    private void HandlePoiseRegen(float pDeltaTime)
    {
        if (m_poiseRegenTimer > 0)
        {
            m_poiseRegenTimer -= pDeltaTime;
        }
        else if (m_poiseDamage > 0)
        {
            //This is here so that if you take poise damage over your normal m_maxPoise while having
            //additional poise resistance, you won't incur a longer poise regeneration time
            if (m_poiseDamage > PoiseDamageResistance)
            {
                m_poiseDamage = PoiseDamageResistance;
            }

            m_poiseDamage -= m_poiseRegenRate * pDeltaTime;

            if (m_poiseDamage < 0)
            {
                m_poiseDamage = 0;
            }
        }
    }

    /// <summary>
    /// Handles draining poise damage immunity
    /// </summary>
    /// <param name="pDeltaTime"></param>
    protected virtual void HandlePoiseDamageImmunity(float pDeltaTime)
    {
        if (m_poiseDamageImmunityCurrTimer > 0)
        {
            m_poiseDamageImmunityCurrTimer -= pDeltaTime;
        }
    }

    protected override void HandleHealthRegen(float pDeltaTime)
    {
        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Sprint"))
        {
            m_healthRegenTimer = m_healthRegenDelayTime;
            return;
        }

        base.HandleHealthRegen(pDeltaTime);
    }

    /// <summary>
    /// Handles cheat death timer
    /// </summary>
    /// <param name="pDeltaTime"></param>
    protected virtual void HandleCheatDeathTimer(float pDeltaTime)
    {
        if (m_cheatDeathTimer > 0)
        {
            m_cheatDeathTimer -= pDeltaTime;

            if (m_cheatDeathTimer < 0)
            {
                m_cheatDeathTimer = 0;
            }
        }
    }

    /// <summary>
    /// Handles regenerating stamina (or degenerating stamina, if the character is sprinting)
    /// </summary>
    /// <param name="pDeltaTime"></param>
    protected virtual void HandleStaminaRegen(float pDeltaTime)
    {
        if(DodgeCount < 2)
        {
            m_dodgeCooldownTimer -= pDeltaTime;
            if (m_dodgeCooldownTimer <= 0)
            {
                DodgeCount++;
                m_dodgeCooldownTimer = m_dodgeCooldown;
            }
        }

        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Sprint"))
        {
            StaminaFloat -= m_sprintStaminaCost * pDeltaTime;

            if (m_staminaRegenTimer < m_staminaRegenDelayTime)
            {
                m_staminaRegenTimer = m_staminaRegenDelayTime;
            }
        }
        else
        {
            if (m_staminaRegenTimer > 0)
            {
                m_staminaRegenTimer -= pDeltaTime;
            }
            else if (m_stamina < m_maxStamina)
            {
                StaminaFloat += m_staminaRegenRate * pDeltaTime;
            }
        }
    }

    /// <summary>
    /// Accepts a number for stamina spent and applies it
    /// </summary>
    /// <param name="pStamina"></param>
    public void SpendStamina(int pStamina)
    {
        StaminaFloat -= pStamina;

        if (m_staminaRegenTimer < m_staminaRegenDelayTime)
        {
            m_staminaRegenTimer = m_staminaRegenDelayTime;
        }

        CanDoActionResetScanner(); //Reset any triggers that are locked-in despite not having enough stamina to fire anymore
    }

    /// <summary>
    /// Accepts a number for stamina spent and applies it and
    /// if the stamina spent brings the stamina to a negative value, adds
    /// an extra "over stam" delay by applying the long delay time
    /// instead of the normal delay time
    /// </summary>
    /// <param name="pStamina"></param>
    public void SpendStaminaOverStam(int pStamina)
    {
        if (pStamina > m_stamina)
        {
            if (m_staminaRegenTimer < m_staminaRegenDelayTimeLong)
            {
                m_staminaRegenTimer = m_staminaRegenDelayTimeLong;
            }
        }
        else
        {
            if (m_staminaRegenTimer < m_staminaRegenDelayTime)
            {
                m_staminaRegenTimer = m_staminaRegenDelayTime;
            }
        }

        StaminaFloat -= pStamina;
    }

    /// <summary>
    /// Accepts a number for stamina spent and applies it and
    /// applies the long stamina delay instead of the normal delay
    /// </summary>
    /// <param name="pStamina"></param>
    public void SpendStaminaLongDelay(int pStamina)
    {
        //TODO: Probably remove stamina and fully replace with dodge system?
        DodgeCount -= 1;


        StaminaFloat -= pStamina;

        if (m_staminaRegenTimer < m_staminaRegenDelayTimeLong)
        {
            m_staminaRegenTimer = m_staminaRegenDelayTimeLong;
        }
    }

    /// <summary>
    /// Accepts an integer and adds mana to your mana pool
    /// </summary>
    /// <param name="pMana"></param>
    public void GainMana(int pMana)
    {
        //Debug.Log("Gain Mana Start");
        ManaFloat += pMana;
        //Debug.Log("Gain Mana End");
    }

    /// <summary>
    /// Accepts an integer for mana and spends that much mana
    /// </summary>
    /// <param name="pMana"></param>
    public void SpendMana(int pMana)
    {
        //Debug.Log("Gain Mana Start");
        ManaFloat -= pMana;

        CanDoActionResetScanner(); //Reset any triggers that are locked-in despite not having enough mana to fire anymore
        //Debug.Log("Gain Mana End");
    }

    /// <summary>
    /// Accepts an action name as a string and validates if it can
    /// be done.
    /// (i.e. checking if there's enough stamina to even attempt to initiate
    /// an attack.)
    /// Note: It might be better to just handle this in the animator
    /// state machine by updating parameters for stamina and such and then
    /// triggering some "exhausted" animation, for example.
    /// Although, this might still be necessary to use to enforce certain
    /// things that might be attempted that can't be handled that way?
    /// </summary>
    /// <param name="pActionName"></param>
    /// <returns></returns>
    public virtual bool CanDoAction(string pActionName)
    {
        switch (pActionName)
        {
            //TO DO: Fully implement for all actions
            case "AttackBasicLight":
                return Stamina > 0;
            case "AttackBasicHeavy":
                return Stamina > 0;
            case "AttackSpecialLight":
                return Stamina > 0;
            case "AttackSpecialHeavy":
                return Stamina > 0;
            case "Evade":
                return Stamina > 0 && !m_animator.GetBool("InWater");
            case "Interact":
                return true;
            case "SpecialAction1":
                return SpecialAbilityInfo1.IsAllowed();
            case "SpecialAction2":
                return SpecialAbilityInfo2.IsAllowed();
        }

        return false;
    }

    /// <summary>
    /// Scans through all the actions the player can take and
    /// checks if they can do them. If they can't it resets the pressed
    /// trigger for that action.
    ///
    /// This should be called anytime any of the follow stats are reduced:
    /// Health, Stamina, Mana
    ///
    /// NOTE: This can potentailly still cause one issue.... if you gain back
    /// enough of the resource to fire that trigger by the time it would have
    /// actually fired, then it should have been able to go off... this will
    /// prevent that
    /// </summary>
    protected void CanDoActionResetScanner()
    {
        if (!CanDoAction("AttackBasicLight"))
        {
            m_animator.ResetTrigger("AttackBasicLightPressed");
        }

        if (!CanDoAction("AttackBasicHeavy"))
        {
            m_animator.ResetTrigger("AttackBasicHeavyPressed");
        }

        if (!CanDoAction("AttackSpecialLight"))
        {
            m_animator.ResetTrigger("AttackSpecialLightPressed");
        }

        if (!CanDoAction("AttackSpecialHeavy"))
        {
            m_animator.ResetTrigger("AttackSpecialHeavyPressed");
        }

        if (!CanDoAction("Evade"))
        {
            m_animator.ResetTrigger("EvadeTapped");
        }

        if (!CanDoAction("Interact"))
        {
            m_animator.ResetTrigger("InteractPressed");
        }

        if (!CanDoAction("SpecialAction1"))
        {
            if (m_animator.GetInteger("SpecialActionMajorSlot") == 1)
            {
                m_animator.ResetTrigger("SpecialActionMajorPressed");
            }
            else if (m_animator.GetInteger("SpecialActionMinorSlot") == 1)
            {
                m_animator.ResetTrigger("SpecialActionMinorPressed");
            }
        }

        if (!CanDoAction("SpecialAction2"))
        {
            if (m_animator.GetInteger("SpecialActionMajorSlot") == 2)
            {
                m_animator.ResetTrigger("SpecialActionMajorPressed");
            }
            else if (m_animator.GetInteger("SpecialActionMinorSlot") == 2)
            {
                m_animator.ResetTrigger("SpecialActionMinorPressed");
            }
        }
    }

    /// <summary>
    /// Tells the loadout to toggle between weapon 1 and 2
    /// </summary>
    public void ToggleWeapons()
    {
        if (m_loadout.SelectedWeapon == 2)
        {
            m_loadout.SwitchWeapons(1);
        }
        else
        {
            m_loadout.SwitchWeapons(2);
        }
    }

    /// <summary>
    /// Checks for changes in poise damage resistance since last
    /// check and alters the current poise damage accordingly
    ///
    /// If poise damage resistance increases, current poise damage is
    /// unaffected. However, if poise damage resistance decreases and the
    /// current poise damage is greater than the poise damage resistance,
    /// then the current poise damage is reduced to just below the threhsold
    /// that would cause a stagger.
    /// </summary>
    protected void HandlePoiseDamageResistanceFlux()
    {
        //TO DO: actually write this code... including a backup variable...

        //TO DO: Do I actually need this? I can get the same affect by making it so that when the
        //your poise starts regenerating, it starts off by resetting it to PoiseDamageReistance - 1
        //(This effectively means that taking extra poise damage doesn't increase the time it takes for your poise to reset)
    }

    #region Suffering Effects
    /// <summary>
    /// Accepts a number for damage dealt and applies it
    /// </summary>
    /// <param name="pDamage"></param>
    public override void SufferDamage(int pDamage)
    {
        m_animator.SetBool("Flinch", true);


        //On dropping into critical health, enable cheat death (cannot drop below one health)
        if (HealthFloat > m_healthThresholds[0] && ((HealthFloat - pDamage) <= m_healthThresholds[0]))
        {
            m_cheatDeathTimer = m_cheatDeathDuration;
            SpawnProjectile(SpawnableObjectType.CheatDeathForcePulse);
        }

        //If about to take lethal damage while cheat death is enabled, drop to one health instead
        if (pDamage >= HealthFloat && m_cheatDeathTimer > 0)
        {
            HealthFloat = 1;
            base.SufferDamage(0); //Run DestructibleObjInfo.SufferDamage with 0 damage to process other suffer damage code
        }
        else
        {
            base.SufferDamage(pDamage);
        }

        CanDoActionResetScanner(); //Reset any triggers that are locked-in despite not having enough health to fire anymore
    }

    /// <summary>
    /// Accepts a number for poise damage dealt and applies it
    /// </summary>
    /// <param name="pPoiseDamage"></param>
    public override void SufferPoiseDamage(int pPoiseDamage)
    {
        if (m_poiseDamageImmunityCurrTimer > 0)
        {
            return;
        }

        m_poiseDamage += pPoiseDamage;

        if (m_poiseDamage >= PoiseDamageResistance)
        {
            m_poiseDamage = 0;

            SufferStagger();
        }

        m_poiseRegenTimer = m_poiseRegenDelayTime;
    }

    /// <summary>
    /// Accepts a number for poison damage dealt and applies it
    /// </summary>
    /// <param name="pPoison"></param>
    public override void SufferPoison(int pPoison)
    {

    }

    /// <summary>
    /// Have the character become staggered
    ///
    /// Note: This isn't like most suffer functions... the normal suffer
    /// functions are used to handle suffering a damage type...
    /// </summary>
    public void SufferStagger()
    {
        m_poiseDamageImmunityCurrTimer = m_poiseDamageImmunityTime;

        m_animator.SetTrigger("Stagger");
    }

    ///// <summary>
    ///// Forces the character to suffer a miniature stagger
    ///// </summary>
    //public override void SufferMiniStagger()
    //{
    //    m_statusEffectManager.Add(new MiniStaggerStatusEffect(null, null, null));
    //}

    /// <summary>
    /// This function is called when the actor enters water
    /// </summary>
    public override void EnterWater()
    {
        m_animator.SetBool("InWater", true);

        m_statusEffectManager.Add(new WetStatusEffect(null, null, this)); //TO DO: Replace null with some generic environmental object... or pass the water source?
    }

    /// <summary>
    /// This function is called when the actor exits water
    /// </summary>
    public override void ExitWater()
    {
        m_animator.SetBool("InWater", false);
    }
    #endregion

    //This is where I expose different mechanics (i.e. lunging, dashing, ect) to the animator
    #region Mechanics
    /// <summary>
    /// Causes the player to lunge with the passed power
    /// and at the set angle (relative to the character's facing direction)
    /// The power is passed as the floatParameter and the angle is
    /// passed as the intParameter.
    /// stringParamter is used to indicate whether or not to set the char's
    /// rotation to be equal to the movement direction before applying the
    /// lunge force
    /// </summary>
    /// <param name="pPowerAndAngle"></param>
    public void Lunge(AnimationEvent pPowerAndAngle)
    {
        //Handles setting the character's facing direction to the current movement direction before applying the force
        if (pPowerAndAngle.stringParameter == "1")
        {
            Vector2 dir = m_playerController.InputManager.Movement.Direction;
            if (dir.magnitude > 1)
            {
                dir.Normalize();
            }

            if (dir.magnitude > 0)
            {
                m_transform.rotation = Quaternion.Euler(0, 0, -1 * Mathf.Atan2(dir.x, dir.y) * 180 / Mathf.PI);
            }
        }

        //Stop the character's movement and angular movement
        m_rigidbody.velocity = Vector2.zero;
        m_rigidbody.angularVelocity = 0f;

        //Find the direction to lunge in by comparing the character's facing direction and the passed angle
        Vector2 tempLungeDir = ((Vector2)m_transform.up).Rotate(pPowerAndAngle.intParameter);

        //Apply the force in the character's facing direction, altered by the passed angle
        m_rigidbody.AddForce(tempLungeDir * pPowerAndAngle.floatParameter, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Clears all the details of this ObjInfo that should be
    /// cleared when a state is exited.
    /// </summary>
    public override void ClearStateInfo()
    {
        base.ClearStateInfo();

        //ResetAllInteractionTypes(); This was moved to the ClearAttackStateInfo function
        //ClearAllRecentCollisions(); This was moved to the ClearAttackStateInfo function
    }

    /// <summary>
    /// Clears all the details of this charObjInfo that should be
    /// cleared when an attack state is exited.
    /// (ex: Clearing all the RecentCollions for a weapon on a character)
    /// </summary>
    public virtual void ClearAttackStateInfo()
    {
        ResetAllInteractionTypes();
        ClearAllRecentCollisions();

    }

    /// <summary>
    /// Exposes the ClearAllRecentCollisions function of the
    /// equipped weapon
    /// </summary>
    public void ClearAllRecentCollisions()
    {
        EquippedWeaponInfo.ClearAllRecentCollisions();
    }

    /// <summary>
    /// Exposes the ClearRecentCollision function of the
    /// equipped weapon
    /// </summary>
    /// <param name="pRecentCollisionKeys"></param>
    public void ClearRecentCollision(string pRecentCollisionKeys)
    {
        EquippedWeaponInfo.ClearRecentCollision(pRecentCollisionKeys);
    }

    /// <summary>
    /// Exposes the SetInteractionType function of the
    /// equipped weapon
    /// </summary>
    /// <param name="pAnimationEvent"></param>
    public void SetInteractionTypeWeapon(AnimationEvent pAnimationEvent)
    {
        EquippedWeaponInfo.SetInteractionType(pAnimationEvent.stringParameter, (InteractionType)pAnimationEvent.intParameter);
    }

    /// <summary>
    /// Exposes the ResetInteractionType function of the
    /// equipped weapon
    /// </summary>
    /// <param name="pWeaponPartName"></param>
    public void ResetInteractionType(string pWeaponPartName)
    {
        EquippedWeaponInfo.ResetInteractionType(pWeaponPartName);
    }

    /// <summary>
    /// Exposes the ResetAllInteractionTypes function of the
    /// equipped weapon
    /// </summary>
    public void ResetAllInteractionTypes()
    {
        EquippedWeaponInfo.ResetAllInteractionTypes();
    }

    /// <summary>
    /// Spawns an object of the specified object type
    /// </summary>
    /// <param name="pSpawnableObjectType"></param>
    public void SpawnProjectile(SpawnableObjectType pSpawnableObjectType)
    {
        ObjectFactory.SpawnObject(this, this, m_attachablePartTransforms[(int)CharacterPartEnum.ProjectileSpawner].position, m_attachablePartTransforms[(int)CharacterPartEnum.ProjectileSpawner].up, pSpawnableObjectType);
    }

    /// <summary>
    /// Spawns an object of the specified type to the center of the Character
    /// </summary>
    /// <param name="pSpawnableObjectType"></param>
    public void SpawnObjectOnCenter(SpawnableObjectType pSpawnableObjectType)
    {
        ObjectFactory.SpawnObject(this, this, m_transform.position, m_transform.up, pSpawnableObjectType);
    }

    /// <summary>
    /// Causes the object to call a particle effect controller
    /// to play a particle effect for it at the location and in the
    /// orientation of the projectile spawner
    /// </summary>
    /// <param name="pParticleEffectEnum"></param>
    public void PlayParticleEffect_ProjectileSpawner(ParticleEffectEnum pParticleEffectEnum)
    {
        if (pParticleEffectEnum == ParticleEffectEnum.None)
        {
            return;
        }

        ParticleEffectManager.PlayParticleEffect(pParticleEffectEnum, m_attachablePartTransforms[(int)CharacterPartEnum.ProjectileSpawner].position, m_attachablePartTransforms[(int)CharacterPartEnum.ProjectileSpawner].up);
    }

    /// <summary>
    /// Plays a particle effect from the targeted character part
    /// </summary>
    /// <param name="pAnimationEvent"></param>
    public void PlayParticleEffect_TargetCharacterPart(AnimationEvent pAnimationEvent)
    {
        int tempParticleEffectEnum = -1;
        int tempCharacterPartEnum = -1;

        string[] tempSplitStr = pAnimationEvent.stringParameter.Split(',');

        tempParticleEffectEnum = System.Convert.ToInt32(tempSplitStr[0]);
        tempCharacterPartEnum = System.Convert.ToInt32(tempSplitStr[1]);

        if (tempParticleEffectEnum == -1 || tempCharacterPartEnum == -1)
        {
            return;
        }

        //TO DO: Add logic to get the float parameter to be interpreted as an angle of rotation for the particle effect

        ParticleEffectManager.PlayParticleEffect((ParticleEffectEnum)tempParticleEffectEnum, m_attachablePartTransforms[tempCharacterPartEnum]);
    }

    /// <summary>
    /// Exposes the EnableUnarmedDisableCurrent function of the
    /// loadout
    /// </summary>
    public void EnableUnarmedDisableCurrent()
    {
        m_loadout.EnableUnarmedDisableCurrent();
    }

    /// <summary>
    /// Exposes the EnableCurrentDisableUnarmed function of the
    /// loadout
    /// </summary>
    public void EnableCurrentDisableUnarmed()
    {
        m_loadout.EnableCurrentDisableUnarmed();
    }

    /// <summary>
    /// Accepts an integer for ammo and passes it to be spent by
    /// the special action which is currently indicated as "major" by
    /// the animator
    /// </summary>
    /// <param name="pAmmo"></param>
    public void SpendSpecialActionMajorAmmo(int pAmmo)
    {
        if (m_animator.GetInteger("SpecialActionMajorSlot") == 1)
        {
            SpecialAbilityInfo1.SpendAmmo(pAmmo);
        }
        else if (m_animator.GetInteger("SpecialActionMajorSlot") == 2)
        {
            SpecialAbilityInfo2.SpendAmmo(pAmmo);
        }
    }

    /// <summary>
    /// Accepts an integer for ammo and passes it to be spent by
    /// the special action which is currently indicated as "minor" by
    /// the animator
    /// </summary>
    /// <param name="pAmmo"></param>
    public void SpendSpecialActionMinorAmmo(int pAmmo)
    {
        if (m_animator.GetInteger("SpecialActionMinorSlot") == 1)
        {
            SpecialAbilityInfo1.SpendAmmo(pAmmo);
        }
        else if (m_animator.GetInteger("SpecialActionMinorSlot") == 2)
        {
            SpecialAbilityInfo2.SpendAmmo(pAmmo);
        }
    }

    /// <summary>
    /// Accepts an integer for ammo and passes it to be gained by
    /// the special action which is currently indicated as "major" by
    /// the animator
    /// </summary>
    /// <param name="pAmmo"></param>
    public void GainSpecialActionMajorAmmo(int pAmmo)
    {
        if (m_animator.GetInteger("SpecialActionMajorSlot") == 1)
        {
            SpecialAbilityInfo1.GainAmmo(pAmmo);
        }
        else if (m_animator.GetInteger("SpecialActionMajorSlot") == 2)
        {
            SpecialAbilityInfo2.GainAmmo(pAmmo);
        }
    }

    /// <summary>
    /// Accepts an integer for ammo and passes it to be gained by
    /// the special action which is currently indicated as "minor" by
    /// the animator
    /// </summary>
    /// <param name="pAmmo"></param>
    public void GainSpecialActionMinorAmmo(int pAmmo)
    {
        if (m_animator.GetInteger("SpecialActionMinorSlot") == 1)
        {
            SpecialAbilityInfo1.GainAmmo(pAmmo);
        }
        else if (m_animator.GetInteger("SpecialActionMinorSlot") == 2)
        {
            SpecialAbilityInfo2.GainAmmo(pAmmo);
        }
    }

    /// <summary>
    /// Allows animations to trigger effects for stepping
    /// </summary>
    public void StepRunLeft()
    {
        //TO DO: Play audio in here

        WetStatusEffect tempStatusEffect = (WetStatusEffect)m_statusEffectManager.GetStatusEffect(StatusEffectType.Wet);
        if (tempStatusEffect != null && tempStatusEffect.Dripping)
        {
            ObjectFactory.CreatePlayerTrack(m_footLeftTrack, m_attachablePartTransforms[(int)CharacterPartEnum.LeftFoot]);
        }
    }

    /// <summary>
    /// Allows animations to trigger effects for stepping
    /// </summary>
    public void StepRunRight()
    {
        WetStatusEffect tempStatusEffect = (WetStatusEffect)m_statusEffectManager.GetStatusEffect(StatusEffectType.Wet);
        if (tempStatusEffect != null && tempStatusEffect.Dripping)
        {
            ObjectFactory.CreatePlayerTrack(m_footRightTrack, m_attachablePartTransforms[(int)CharacterPartEnum.RightFoot]);
        }
    }

    /// <summary>
    /// Allows animations to trigger effects for stepping
    /// </summary>
    public void StepSprintLeft()
    {
        WetStatusEffect tempStatusEffect = (WetStatusEffect)m_statusEffectManager.GetStatusEffect(StatusEffectType.Wet);
        if (tempStatusEffect != null && tempStatusEffect.Dripping)
        {
            ObjectFactory.CreatePlayerTrack(m_footLeftTrack, m_attachablePartTransforms[(int)CharacterPartEnum.LeftFoot]);
        }
    }

    /// <summary>
    /// Allows animations to trigger effects for stepping
    /// </summary>
    public void StepSprintRight()
    {
        WetStatusEffect tempStatusEffect = (WetStatusEffect)m_statusEffectManager.GetStatusEffect(StatusEffectType.Wet);
        if (tempStatusEffect != null && tempStatusEffect.Dripping)
        {
            ObjectFactory.CreatePlayerTrack(m_footRightTrack, m_attachablePartTransforms[(int)CharacterPartEnum.RightFoot]);
        }
    }

    //Potentially deprecated Roll and Backstep pulse functions 
    /*
    //***********************************************************************
    // Method: RollPulse
    //
    // Purpose: When the player rolls, the animation will trigger this
    // as an event that will pulse tha player's velocity
    //***********************************************************************
    public void RollPulse()
    {
        Vector2 dir = m_playerController.InputManager.Movement.Direction;
        if (dir.magnitude > 1)
        {
            dir.Normalize();
        }

        if (dir.magnitude > 0)
        {
            //m_rigidbody.rotation = -1 * Mathf.Atan2(dir.x, dir.y) * 180 / Mathf.PI; (For some reason rotating the rigidbody instead of the transform doesn't work
            m_transform.rotation = Quaternion.Euler(0, 0, -1 * Mathf.Atan2(dir.x, dir.y) * 180 / Mathf.PI);
        }

        m_rigidbody.velocity = Vector2.zero;
        m_rigidbody.angularVelocity = 0f;

        m_rigidbody.AddForce(m_transform.up * m_rollPower, ForceMode2D.Impulse);
    }

    //***********************************************************************
    // Method: BackStepPulse
    //
    // Purpose: When the player back steps, the animation will trigger this
    // as an event that will pulse tha player's velocity
    //***********************************************************************
    public void BackStepPulse()
    {
        m_rigidbody.velocity = Vector2.zero;
        m_rigidbody.angularVelocity = 0f;

        m_rigidbody.AddForce(-1 * m_transform.up * m_backStepPower, ForceMode2D.Impulse);
    }
    */
    #endregion

    /// <summary>
    /// Used to instantly kill a character object by dealing damage
    /// equal to it's life
    /// </summary>
    [Inspect]
    public void Inspector_Kill()
    {
        HealthFloat = 0;
        SufferDamage(1);
    }



    #region Properties
    public float WalkSpeed
    {
        get
        {
            return m_walkSpeed;
        }
    }

    public float RunSpeed
    {
        get
        {
            return m_runSpeed;
        }
    }

    public float SprintSpeed
    {
        get
        {
            return m_sprintSpeed;
        }
    }


    public float TurnSpeed
    {
        get
        {
            return m_turnSpeed;
        }
        set
        {
            m_turnSpeed = value;
        }
    }

    public int DodgeCount
    {
        get
        {
            return m_dodgeCount;
        }
        set
        {
            m_dodgeCount = Mathf.Clamp(value, 0, 2);

            m_animator.SetInteger("DodgeCount", DodgeCount);
        }
    }

    public int Stamina
    {
        get
        {
            return (int)m_stamina;
        }
    }

    public float StaminaFloat
    {
        get
        {
            return m_stamina;
        }
        set
        {
            if(m_destroyed)
            {
                return;
            }

            //TO DO: Code below is temporarily in place to remove stamina
            m_stamina = Mathf.Clamp(value, m_maxStamina, m_maxStamina); //m_stamina = Mathf.Clamp(value, 0, m_maxStamina);

            m_animator.SetFloat("Stamina", m_stamina);
        }
    }

    public int MaxStamina
    {
        get
        {
            return m_maxStamina;
        }
    }

    public int Mana
    {
        get
        {
            return (int)m_mana;
        }
    }

    public float ManaFloat
    {
        get
        {
            return m_mana;
        }
        set
        {
            if (m_destroyed)
            {
                return;
            }

            m_mana = Mathf.Clamp(value, 0, m_maxMana);

            m_animator.SetFloat("Mana", m_mana);
        }
    }

    public int MaxMana
    {
        get
        {
            return m_maxMana;
        }
    }

    public int PoiseDamageResistance
    {
        get
        {
            return m_maxPoise + (int)m_poiseAnimMod;
        }
    }


    public float TurnSpeedRightAnimMod
    {
        get
        {
            return m_turnSpeedRightAnimMod;
        }
    }

    public float TurnSpeedLeftAnimMod
    {
        get
        {
            return m_turnSpeedLeftAnimMod;
        }
    }

    public Loadout Loadout
    {
        get
        {
            return m_loadout;
        }
    }

    public WeaponInfo EquippedWeaponInfo
    {
        get
        {
            return m_loadout.Weapon;
        }
    }

    public SpecialAbilityInfo SpecialAbilityInfo1
    {
        get
        {
            return m_loadout.SpecialAbility1;
        }
    }

    public SpecialAbilityInfo SpecialAbilityInfo2
    {
        get
        {
            return m_loadout.SpecialAbility2;
        }
    }

    //Overridden for turning off water where on exit would normally do that
    public override bool ActorFloorColliderEnabled
    {
        get
        {
            return base.ActorFloorColliderEnabled;
        }
        protected set
        {
            if (m_destroyed)
            {
                return;
            }

            if (!value)
            {
                m_animator.SetBool("InWater", false); //Make sure that InWater gets turned off if the character has their FloorCollider turned off
            }

            base.ActorFloorColliderEnabled = value;
        }
    }

    public Transform CarrierTransform
    {
        get
        {
            return m_carrierTransform;
        }
    }

    //TO DO: REMOVE ME
    [Inspect]
    public void Inspect_SpawnWaterRipple()
    {
        ObjectFactory.SpawnObject(this, this, m_transform.position, m_transform.up, SpawnableObjectType.WaterRipple);
    }
    #endregion
}