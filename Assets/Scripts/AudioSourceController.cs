//**************************************************************************************
// File: AudioSourceController.cs
//
// Purpose: This class will be attached to a simple game object that will contain
// an AudioSource component. It will be used to be positioned and play sound.
// It can also be configured to track an object for the duration of a clip being
// played.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public class AudioSourceController : MonoBehaviour
{
    #region Declarations
    #region Pointers
    [Inspect, Group("Pointers", 1)]
    public AudioClipManager m_audioClipManager; //Pointer to the AudioClipManager that contains this
    [Inspect, Group("Pointers")]
    public AudioSource m_audioSource;
    #endregion

    private bool m_tracking; //Indicates whether or not this controller is tracking a target (as opposed to being parented directly to it)

    [Inspect]
    private Transform m_target;

    private Vector3 m_offset; //This is the offset with which the target will be tracked

    [Inspect]
    private bool m_inUse;
    #endregion

    //***********************************************************************
    // Method: Start
    //
    // Purpose: Use this for initialization
    //***********************************************************************
    void Start()
    {
    }

    //***********************************************************************
    // Method: Update
    //
    // Purpose: Update is called once per frame
    //***********************************************************************
    void Update()
    {
        if(m_tracking && m_target != null)
        {
            transform.position = m_target.position + m_offset; //TO DO: Consider making offset act relative to rotation as well?
        }

        m_inUse = m_audioSource.isPlaying;
        if(!m_inUse)
        {
            Deactivate();
        }
    }

    //***********************************************************************
    // Method: PlayAudioClip
    //
    // Purpose: Accepts an AudioClipInfo and simply plays the clip.
    //***********************************************************************
    public void PlayAudioClip(AudioClipInfo pAudioClipInfo)
    {
        //enabled = true;
        gameObject.SetActive(true);
        m_audioSource.PlayOneShot(pAudioClipInfo.AudioClip);
    }

    //***********************************************************************
    // Method: PlayAudioClip
    //
    // Purpose: Accepts an AudioClipInfo and a transform to follow
    //***********************************************************************
    public void PlayAudioClip(AudioClipInfo pAudioClipInfo, Transform pTarget, Vector3 pOffset, bool pSetTargetAsParent = false)
    {
        m_tracking = true;
        m_target = pTarget;
        m_offset = pOffset;
        transform.position = m_target.position + m_offset;

        if (pSetTargetAsParent)
        {
            transform.parent = m_target;
        }

        PlayAudioClip(pAudioClipInfo);
    }

    //***********************************************************************
    // Method: PlayAudioClip
    //
    // Purpose: Accepts an AudioClipInfo and a target position and plays
    // the clip there
    //***********************************************************************
    public void PlayAudioClip(AudioClipInfo pAudioClipInfo, Vector3 pPosition)
    {
        transform.position = pPosition;
        PlayAudioClip(pAudioClipInfo);
    }

    //***********************************************************************
    // Method: Deactivate
    //
    // Purpose: Deactivates this AudioSourceController and releases it
    // back to the pool of available AudioSourceControllers
    //***********************************************************************
    private void Deactivate()
    {
        m_tracking = false;
        m_target = null;
        transform.parent = m_audioClipManager.transform;
        transform.position = Vector3.zero;
        transform.localScale = Vector3.one;
        m_audioClipManager.FreeAudioSourceController(this);
        //enabled = false;
        gameObject.SetActive(false);
    }

    #region Properties
    public bool InUse
    {
        get
        {
            return m_inUse;
        }
    }
    #endregion
}
