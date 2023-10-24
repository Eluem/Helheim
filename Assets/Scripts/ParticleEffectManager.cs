//**************************************************************************************
// File: ParticleEffectManager.cs
//
// Purpose: Contains and organizes all the particle effects for other objects to call
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;

public enum ParticleEffectEnum
{
    None = -1,
    Blood = 0,
    HitDust = 1,
    LavaHit = 2,
    LavaBurn = 3,
    UltraGreatswordExplosion = 4,
    DeathSmoke = 5,
    ShurikenSpawn = 6,
    FireBallWindUp = 7,
    FireBallChargeUp = 8,
    LightBurn = 9,
    HitSparks = 10
}

[AdvancedInspector]
public class ParticleEffectManager : MonoBehaviour
{
    #region Declarations
    protected static ParticleEffectManager m_instance; //Used to allow static referencing of non-static values

    [Inspect]
    public const int PARTICLE_EFFECT_TYPE_COUNT = 11;
    public const int MAX_PARTICLE_EFFECTS = 30; //TO DO: Make this dynamically based on some performance settings

    [Inspect]
    public GameObject[] m_particleEffectPrefabs = new GameObject[PARTICLE_EFFECT_TYPE_COUNT];

    //private ParticleEffectController[,] m_particleEffectControllers = new ParticleEffectController[PARTICLE_EFFECT_TYPE_COUNT, MAX_PARTICLE_EFFECTS];

    private Stack<ParticleEffectController>[] m_availableParticleEffectControllers;
    #endregion

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
        m_instance = this;

        //Build the stack of available particle effect controllers
        m_availableParticleEffectControllers = new Stack<ParticleEffectController>[PARTICLE_EFFECT_TYPE_COUNT];

        //Initialize each stack
        for (int i = 0; i < PARTICLE_EFFECT_TYPE_COUNT; i++)
        {
            m_availableParticleEffectControllers[i] = new Stack<ParticleEffectController>();
        }

        //Generate prefabs and fill the stacks with pointers to their controllers
        for (int i = 0; i < PARTICLE_EFFECT_TYPE_COUNT; i++)
        {
            for (int j = 0; j < MAX_PARTICLE_EFFECTS; j++)
            {
                GameObject tempObj = (GameObject)Object.Instantiate(m_particleEffectPrefabs[i], Vector3.zero, Quaternion.identity);
                tempObj.transform.SetParent(transform);
                ParticleEffectController tempParticleEffectController = tempObj.GetComponent<ParticleEffectController>();
                tempParticleEffectController.Initialize(this, (ParticleEffectEnum)i);

                m_availableParticleEffectControllers[i].Push(tempParticleEffectController);
            }
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
    }

    /// <summary>
    /// Accepts a ParticleEffectEnum and uses an available
    /// ParticleEffectController to play it at the passed location in the,
    /// passed direction
    /// </summary>
    /// <param name="pParticleEffectEnum"></param>
    /// <param name="pPosition"></param>
    /// <param name="pDirection"></param>
    public static void PlayParticleEffect(ParticleEffectEnum pParticleEffectEnum, Vector3 pPosition, Vector3 pDirection = default(Vector3))
    {
        if (pParticleEffectEnum == ParticleEffectEnum.None)
        {
            return;
        }

        if (m_instance.m_availableParticleEffectControllers[(int)pParticleEffectEnum].Count > 0)
        {
            m_instance.m_availableParticleEffectControllers[(int)pParticleEffectEnum].Pop().Play(pPosition, pDirection);
        }
    }

    /// <summary>
    /// Accepts and ParticleEffectEnum and uses an available
    /// ParticleEffectController to play it while it tracks the passed target
    /// with the passed offset, in the passed direction
    /// </summary>
    /// <param name="pParticleEffectEnum"></param>
    /// <param name="pTarget"></param>
    /// <param name="pDirection"></param>
    /// <param name="pOffset"></param>
    public static void PlayParticleEffect(ParticleEffectEnum pParticleEffectEnum, Transform pTarget, Vector3 pDirection = default(Vector3), Vector3 pOffset = default(Vector3))
    {
        if (pParticleEffectEnum == ParticleEffectEnum.None)
        {
            return;
        }

        if (m_instance.m_availableParticleEffectControllers[(int)pParticleEffectEnum].Count > 0)
        {
            m_instance.m_availableParticleEffectControllers[(int)pParticleEffectEnum].Pop().Play(pTarget, pDirection, pOffset);
        }
    }

    /// <summary>
    /// Puts an ParticleEffectControllers back into the stack of
    /// available ParticleEffectControllers
    /// </summary>
    /// <param name="pParticleEffectController"></param>
    public void FreeParticleEffectController(ParticleEffectController pParticleEffectController)
    {
        m_instance.m_availableParticleEffectControllers[(int)pParticleEffectController.ParticleEffectType].Push(pParticleEffectController);
    }

    #region Inspector Tools
    /// <summary>
    /// Prints out all the enumrations with their names and id's,
    /// so that you can easily view them from the inspector
    /// </summary>
    [Inspect]
    public void PrintEnumeration()
    {
        for (int i = 0; i < PARTICLE_EFFECT_TYPE_COUNT; i++)
        {
            Debug.Log(i + ": " + ((ParticleEffectEnum)i).ToString());
        }
    }
    #endregion

    #region Properties
    #endregion
}
