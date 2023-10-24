//**************************************************************************************
// File: AudioClipManager.cs
//
// Purpose: Contains and organizes all the audio clips for other objects to call
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

//TO DO: Should I refactor this to be more like the ParticleEffectManager... having a single pointer to an AudioClipController prefab
//and then spawning a bunch of them into the available AudioClipController stack?

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;

public enum AudioClipEnum
{
    None = -1,
    Unarmed_AttackBasicLight = 0,
    Zweihander_AttackBasicLight = 1,
    DamageTest = 2,
    FireTest = 3,
    LargeSharpSwing1 = 4,
    LargeSharpSwing2 = 5,
    LargeSharpOverhead1 = 6,
    VeryLargeSharpSwing1 = 7,
    VeryLargeSwingWindUp1 = 8,
    SmallSharpSwing1 = 9,
    SmallSharpSwing2 = 10,
    SmallSharpSwing3 = 11,
    SmallSharpSwingWindUp1 = 12,
    SmallSharpSwingWindUp2 = 13,
    CharacterHit1 = 14
}

[AdvancedInspector]
public class AudioClipManager : MonoBehaviour
{
    #region Declarations
    protected static AudioClipManager m_instance;

    [Inspect]
    public AudioClipInfo[] m_audioClips;

    public const int MAX_AUDIO_SOURCES = 35; //Unity can handle 36, leave one for music

    [Inspect]
    public AudioSourceController[] m_audioSourceControllers = new AudioSourceController[MAX_AUDIO_SOURCES];

    private Stack<AudioSourceController> m_availableAudioSourceControllers;

    #endregion

    //***********************************************************************
    // Method: Start
    //
    // Purpose: Use this for initialization
    //***********************************************************************
    void Start()
    {
        m_instance = this;

        //Builds the stack of available audio source controllers
        m_availableAudioSourceControllers = new Stack<AudioSourceController>();
        foreach (AudioSourceController audioSourceController in m_audioSourceControllers)
        {
            m_availableAudioSourceControllers.Push(audioSourceController);
        }
    }

    //***********************************************************************
    // Method: Update
    //
    // Purpose: Update is called once per frame
    //***********************************************************************
    void Update()
    {
    }


    //***********************************************************************
    // Method: GetAudioClip
    //
    // Purpose: Accepts an AudioClipEnum and returns the corresponding
    // audio clip
    //***********************************************************************
    public static AudioClip GetAudioClip(AudioClipEnum pAudioClipEnum)
    {
        return m_instance.m_audioClips[(int)pAudioClipEnum].AudioClip;
    }

    //***********************************************************************
    // Method: PlayAudioClip
    //
    // Purpose: Accepts an AudioClipEnum and uses an available
    // AudioSourceController to play it
    //***********************************************************************
    public static void PlayAudioClip(AudioClipEnum pAudioClipEnum)
    {
        if (pAudioClipEnum == AudioClipEnum.None)
        {
            return;
        }

        if (m_instance.m_availableAudioSourceControllers.Count > 0)
        {
            m_instance.m_availableAudioSourceControllers.Pop().PlayAudioClip(m_instance.m_audioClips[(int)pAudioClipEnum]);
        }
    }

    //***********************************************************************
    // Method: PlayAudioClip
    //
    // Purpose: Accepts and AudioClipEnum and uses an available
    // AudioSourceController to play it while it tracks the passed target
    //***********************************************************************
    public static void PlayAudioClip(AudioClipEnum pAudioClipEnum, Transform pTarget, Vector3 pOffset = default(Vector3))
    {
        if (pAudioClipEnum == AudioClipEnum.None)
        {
            return;
        }

        if (m_instance.m_availableAudioSourceControllers.Count > 0)
        {
            m_instance.m_availableAudioSourceControllers.Pop().PlayAudioClip(m_instance.m_audioClips[(int)pAudioClipEnum], pTarget, pOffset);
        }
    }

    //***********************************************************************
    // Method: PlayAudioClip
    //
    // Purpose: Accepts and AudioClipEnum and uses an available
    // AudioSourceController to play it at the specified position
    //***********************************************************************
    public static void PlayAudioClip(AudioClipEnum pAudioClipEnum, Vector3 pPosition)
    {
        if (pAudioClipEnum == AudioClipEnum.None)
        {
            return;
        }

        if (m_instance.m_availableAudioSourceControllers.Count > 0)
        {
            m_instance.m_availableAudioSourceControllers.Pop().PlayAudioClip(m_instance.m_audioClips[(int)pAudioClipEnum], pPosition);
        }
    }

    //***********************************************************************
    // Method: FreeAudioSourceController
    //
    // Purpose: Puts an AudioSourceController back into the stack of
    // available AudioSourceControllers
    //***********************************************************************
    public void FreeAudioSourceController(AudioSourceController pAudioSourceController)
    {
        m_instance.m_availableAudioSourceControllers.Push(pAudioSourceController);
    }

    #region Inspector Tools
    //***********************************************************************
    // Method: PrintEnumeration
    //
    // Purpose: Prints out all the enumrations with their names and id's,
    // so that you can easily view them from the inspector
    //***********************************************************************
    [Inspect]
    public void PrintEnumeration()
    {
        for (int i = 0; i < m_audioClips.Length; i++)
        {
            Debug.Log(i + ": " + ((AudioClipEnum)i).ToString());
        }
    }

    //***********************************************************************
    // Method: UpdateClipNames
    //
    // Purpose: Goes through all the existing audio clips and uses the
    // AUdioClipEnum to give them new names
    //***********************************************************************
    [Inspect]
    public void UpdateClipNames()
    {
        for (int i = 0; i < m_audioClips.Length; i++)
        {
            m_audioClips[i].m_clipName = ((AudioClipEnum)i).ToString();
        }
    }
    #endregion

    #region Properties
    #endregion
}
