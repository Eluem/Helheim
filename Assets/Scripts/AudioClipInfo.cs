//**************************************************************************************
// File: AudioClipInfo.cs
//
// Purpose: This class is used to allow me to create a name that can be displayed for
// each audio clip so that I can identify them in the AudioClipManager's array of clips
// (I would like to find a better solution for this... I'd like there to be a way to
// get the inspector to display the enumeration names instead of integers
// for the index)
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AudioClipInfo
{
    #region Declarations
    public string m_clipName;
    public AudioClip[] m_audioClips;
    #endregion

    #region Properties
    public string ClipName
    {
        get
        {
            return m_clipName;
        }
    }

    public AudioClip AudioClip
    {
        get
        {
            return m_audioClips[Random.Range(0, m_audioClips.Length)];
        }
    }
    #endregion
}
