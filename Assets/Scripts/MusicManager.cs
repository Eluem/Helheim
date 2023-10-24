//**************************************************************************************
// File: MusicManager.cs
//
// Purpose: Manages the music for the game
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    #region Declarations
    public AudioClip[] m_musicClips;

    public AudioSource m_audioSource;
    #endregion
	
	/// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
		
	}

	/// <summary>
    /// Use this for initialization
    /// </summary>
	void Start()
	{
        m_audioSource.clip = m_musicClips[0];
        m_audioSource.Play();
	}
	
	/// <summary>
    /// Update is called once per frame
    /// </summary>
	void Update()
	{
		
	}
	
	#region Properties
    #endregion
}