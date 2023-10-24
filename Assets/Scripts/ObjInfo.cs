//**************************************************************************************
// File: ObjInfo.cs
//
// Purpose: Base class for all information about an object
// This is the core class to look at with regards to interacting with an object
// externally (dealing damage, applying status effects, pulling levers, ect)
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

public enum InteractionType
{
    None = -1,
    SharpLight = 0,
    SharpMedium = 1,
    SharpHeavy = 2,
    BluntLight = 3,
    BluntMedium = 4,
    BluntHeavy = 5,
    WalkLight = 6,
    WalkMedium = 7,
    WalkHeavy = 8,
    RunLight = 9,
    RunMedium = 10,
    RunHeavy = 11,
    SprintLight = 12,
    SprintMedium = 13,
    SprintHeavy = 14,
    LightBurn = 15
}

[AdvancedInspector]
public abstract class ObjInfo : MonoBehaviour
{
    #region Declarations
    public const int GROUNDALL_LAYERMASK = 1536; //Only check against Ground and All
    public const int ACTORHAZARDHAZARD_LAYERMASK = 150994944; //ActorHazard and Hazard layerMask
    public const int PITFLOOR_LAYERMASK = 6144; //Pit and Floor layerMask
    public const int FLOOR_LAYERMASK = 2048; //Floor layerMask

    protected bool m_initialized = false; //Indicates if this object has been initialized    
    protected bool m_destroyed = false; //This object has been marked for destruction

    #region Pointers
    [Inspect, Group("Pointers", 1)]
    public Transform m_transform;
    #endregion

    #region Stats
    [Inspect, Group("Stats", 2)]
    public int m_bouncePower = 1; //Stores the amount of "bounce power" that will be applied to a weapon upon hitting this object
    [Inspect, Group("Stats")]
    public float m_manaGenerationFactor = 0; //Stores the percentage of mana that this object will allow a hazard to generate upon hitting it
    [Inspect, Group("Stats")]
    public int m_team = 0; //Indicates the "team" this object belongs to. 0 = none
    #endregion
    public static int INTERACTION_TYPE_COUNT = 16;

    //Defines the list of audio clips that will be played when different interactions happen
    protected AudioClipEnum[] m_interactAudioClips;

    //Defines the list of particle effects that will be played when different interactions happen
    protected ParticleEffectEnum[] m_interactParticleEffects;

    private bool m_pooled; //Indicates if this object uses the ObjectFactory's object pooling system
    private string m_objType;
    private bool m_preciseCollision;
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    /// <param name="pObjType">ObjType of this instance</param>
    /// <param name="pPreciseCollision">Uses precise collision</param>
    public virtual void Initialize(bool pPooled, string pObjType, bool pPreciseCollision = false)
    {
        m_pooled = pPooled;
        m_objType = pObjType;
        m_preciseCollision = pPreciseCollision;

        InitializeInteractionDataDefault();
        InitializeInteractionData();

        m_initialized = true;
    }

    /// <summary>
    /// This is the code that runs each time this object is spawned
    /// </summary>
    public virtual void Spawn(int pTeam)
    {
        Team = pTeam;

        if (Pooled)
        {
#if UNITY_EDITOR
            //If in editor mode, hide the item from the hierarchy to help make these objects less confusing(this isn't necessary in a build version)
            gameObject.hideFlags = HideFlags.None;
#endif

            m_destroyed = false;
            gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Destroys this object (or pretends to, if it's pooled)
    /// </summary>
    public virtual void DestroyMe()
    {
        m_destroyed = true; //This can potentially be set earlier in some cases, like in hazards, after they decay, while they're still lingering

        if (Pooled)
        {
#if UNITY_EDITOR
            //If in editor mode, hide the item from the hierarchy to help make these objects less confusing(this isn't necessary in a build version)
            gameObject.hideFlags = HideFlags.HideInHierarchy;
#endif
            ObjectFactory.PushObjInfoIntoPool(this);
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Accepts an object type and returns true if one of the types
    /// that it is matches the type passed.
    /// i.e. PlayerObjInfo is also a CharObjInfo and a DestructibleObjInfo
    /// </summary>
    /// <param name="pObjType">Object Type string to check</param>
    /// <returns></returns>
    public abstract bool IsType(string pObjType);

    /// <summary>
    /// Initializes all the interactions for this object
    /// </summary>
    protected virtual void InitializeInteractionData()
    {
    }

    /// <summary>
    /// Initializes all the interactions for this object and defaults them to none.
    /// </summary>
    protected void InitializeInteractionDataDefault()
    {
        //Initialize interaction audio clips
        m_interactAudioClips = new AudioClipEnum[INTERACTION_TYPE_COUNT];
        for (int i = 0; i < INTERACTION_TYPE_COUNT; i++)
        {
            m_interactAudioClips[i] = AudioClipEnum.None;
        }

        //Initialize interaction particle effects
        m_interactParticleEffects = new ParticleEffectEnum[INTERACTION_TYPE_COUNT];
        for (int i = 0; i < INTERACTION_TYPE_COUNT; i++)
        {
            m_interactParticleEffects[i] = ParticleEffectEnum.None;
        }
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    protected virtual void Start()
    {
        if (!m_initialized)
        {
            Initialize(false, "ObjInfo");
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected virtual void Update()
    {
    }

    /// <summary>
    /// Called when object is destroyed
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (!m_destroyed)
        {
            DestroyMe(); //Call this to run all the cleanup code before this object is destroyed by Unity (if it hasn't been done already)
        }
    }

    /// <summary>
    /// If this object is currently sleeping, wakes it up
    /// </summary>
    public virtual void Shake()
    {
    }

    #region Mechanics
    /// <summary>
    /// Causes the object to call an audio source controller to play
    /// an audio clip for it
    /// </summary>
    /// <param name="pAudioClip"></param>
    public void PlayAudio(AudioClipEnum pAudioClip)
    {
        if (pAudioClip == AudioClipEnum.None)
        {
            return;
        }

        AudioClipManager.PlayAudioClip(pAudioClip, m_transform);
    }

    /// <summary>
    /// Causes the object to call a particle effect controller
    /// to play a particle effect for it
    /// </summary>
    /// <param name="pParticleEffectEnum"></param>
    public void PlayParticleEffect(ParticleEffectEnum pParticleEffectEnum)
    {
        if (pParticleEffectEnum == ParticleEffectEnum.None)
        {
            return;
        }

        ParticleEffectManager.PlayParticleEffect(pParticleEffectEnum, transform, Vector3.zero);
    }

    /// <summary>
    /// Causes the object to call a particle effect controller
    /// to play a particle effect for it
    /// </summary>
    /// <param name="pParticleEffectEnum"></param>
    /// <param name="pDirection"></param>
    public void PlayParticleEffectWithDirection(ParticleEffectEnum pParticleEffectEnum, Vector3 pDirection)
    {
        if (pParticleEffectEnum == ParticleEffectEnum.None)
        {
            return;
        }

        ParticleEffectManager.PlayParticleEffect(pParticleEffectEnum, transform, pDirection);
    }

    /// <summary>
    /// Clears all the details of this ObjInfo that should be
    /// cleared when a state is exited. (ex: Clearing all the RecentCollions
    /// for a weapon on a character)
    /// </summary>
    public virtual void ClearStateInfo()
    {

    }
    #endregion

    /// <summary>
    /// Gets the interaction audio clip corresponding to the 
    /// interaction type.
    /// If the index is out of bounds, returns AudioClipEnum.None
    /// </summary>
    /// <param name="pInteractionType"></param>
    /// <returns></returns>
    public AudioClipEnum GetInteractionAudioClip(InteractionType pInteractionType)
    {
        int index = (int)pInteractionType;

        if (index > -1 && index < m_interactAudioClips.Length)
        {
            return m_interactAudioClips[index];
        }

        return AudioClipEnum.None;
    }

    /// <summary>
    /// Gets the interaction particle effect corresponding to
    /// the interaction type.
    /// If the index is out of bounds, returns ParticleEffectEnum.None
    /// </summary>
    /// <param name="pInteractionType"></param>
    /// <returns></returns>
    public ParticleEffectEnum GetInteractionParticleEffect(InteractionType pInteractionType)
    {
        int index = (int)pInteractionType;

        if (index > -1 && index < m_interactParticleEffects.Length)
        {
            return m_interactParticleEffects[index];
        }

        return ParticleEffectEnum.None;
    }

    /// <summary>
    /// Sets every sprite sorting layer and order to the passed values
    /// </summary>
    /// <param name="pAnimationEvent">SortingLayerName = StringParameter, SortingOrder = intParameter</param>
    public void SetSpriteSorting(AnimationEvent pAnimationEvent)
    {
        Component[] spriteRenderers = GetComponentsInChildren(typeof(SpriteRenderer), true);

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sortingLayerName = pAnimationEvent.stringParameter;
            spriteRenderer.sortingOrder = pAnimationEvent.intParameter;
        }
    }

    /// <summary>
    /// Resets every sprite sorting layer and order to their defaults
    /// </summary>
    public void ResetSpriteSorting()
    {
        Component[] spriteRenderers = GetComponentsInChildren(typeof(SpriteRenderer), true);

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sortingLayerName = "Default";
            spriteRenderer.sortingOrder = 0;
        }
    }

    /// <summary>
    /// Finds the ObjInfo component from the collider. First it if it has a ColliderInfo, if not, it tries to get it directly.
    /// </summary>
    /// <param name="pCollider"></param>
    /// <returns></returns>
    public static ObjInfo GetObjInfoFromCollider(Collider2D pCollider)
    {
        ColliderInfo tempColliderInfo = (ColliderInfo)pCollider.GetComponent(typeof(ColliderInfo));
        if (tempColliderInfo == null)
        {
            return pCollider.GetComponent<ObjInfo>();
        }

        return tempColliderInfo.ObjInfo;
    }

    /// <summary>
    /// Debug function used to print out a bunch of info about
    /// this ObjInfo
    /// </summary>
    [Inspect]
    public void PrintInfo()
    {
        Rigidbody2D tempRigidbody2D = GetComponent<Rigidbody2D>();

        Debug.Log(
            "Info for " + name +
            "\nWorld Position: " + m_transform.position.ToString("F8") +
            "\nWorld Rotation: " + transform.rotation.eulerAngles.ToString("F8") +
            "\nLocal Position: " + m_transform.localPosition.ToString("F8") +
            "\nLocal Rotation: " + m_transform.localRotation.eulerAngles.ToString("F8") +

            "\n\nForward: " + m_transform.forward.ToString("F8") +
            "\nUp: " + m_transform.up.ToString("F8") +
            "\nRight: " + m_transform.right.ToString("F8") +

            "\n\nVelocity: " + ((tempRigidbody2D == null) ? "null" : tempRigidbody2D.velocity.ToString("F8")) +
            "\nAngular Velocity: " + ((tempRigidbody2D == null) ? "null" : tempRigidbody2D.angularVelocity.ToString("F8")) +
            "\n\n");
    }

    #region Properties
    public string ObjType
    {
        get
        {
            return m_objType;
        }
    }

    public Transform Transform
    {
        get
        {
            return m_transform;
        }
    }

    public virtual Rigidbody2D RigidBody2D
    {
        get
        {
            return null;
        }
    }

    public bool PreciseCollision
    {
        get
        {
            return m_preciseCollision;
        }
    }

    public int BouncePower
    {
        get
        {
            return m_bouncePower;
        }
    }

    public float ManaGenerationFactor
    {
        get
        {
            return m_manaGenerationFactor;
        }
    }

    public bool Pooled
    {
        get
        {
            return m_pooled;
        }
    }

    public int Team
    {
        get
        {
            return m_team;
        }
        set
        {
            m_team = value;
        }
    }
    #endregion
}