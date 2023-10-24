//**************************************************************************************
// File: MenuManager.cs
//
// Purpose: Controls what menu screen is curerntly showing, acts as a sort of
// GameState manager as well.
//
// TO DO: Rename to GameStateManager?
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;

public enum GameState
{
    StartMenu = 0,
    MainMenu = 1,
    InGameMenu = 2,
    InGame = 3
}


[AdvancedInspector]
public class MenuManager : MonoBehaviour
{
    #region Declarations
    protected static MenuManager m_instance; //Used to allow static referencing of non-static values

    [Inspect]
    public PlayerManager m_playerManager;

    [Inspect]
    protected GameState m_gameState = GameState.StartMenu;

    protected bool m_initialized = false;
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        DontDestroyOnLoad(transform.parent.gameObject);

        m_initialized = true;
	}

	/// <summary>
    /// Use this for initialization
    /// </summary>
	void Start()
	{
        if(!m_initialized)
        {
            Initialize();
        }
	}
	
	/// <summary>
    /// Update is called once per frame
    /// </summary>
	void Update()
	{
		//switch(m_gameState)
  //      {
  //          case GameState.StartMenu:
  //              break;
  //          case GameState.MainMenu:
  //              break;
  //          case GameState.InGame:
  //              break;
  //          case GameState.InGameMenu:
  //              break;
  //      }
	}

    /// <summary>
    /// Should be called after loading any new map, tells all the system objects that they need to reload
    /// </summary>
    public void NewMapLoaded()
    {

    }

    /// <summary>
    /// Loads the "EpicBalcony" Map
    /// </summary>
    public void StartEpicBalconyMap()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("EpicBalcony");
    }
	
	#region Properties
    public static GameState CurrentGameState
    {
        get
        {
            return m_instance.m_gameState;
        }
    }
    #endregion
}