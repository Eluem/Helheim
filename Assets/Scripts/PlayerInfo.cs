//**************************************************************************************
// File: PlayerInfo.cs
//
// Purpose: This class tracks a.. sort of active player "profile". It's used to let
// the player manager know what's going on with a player, not it's PlayerController or
// the actual player prefab
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInfo
{
    #region Declarations
    private PlayerProfile m_playerProfile;

    private string m_playerName;
    private int m_playerIndex;

    private Color m_playerColor;

    private PlayerObjInfo m_playerObjInfo;

    private InputManager m_inputManager;

    private Dictionary<string, Sprite> m_skin;
    private string m_loadout;

    private int m_team = 0;

    private float m_timeToRespawn = 3f;
    private float m_respawnTimer = 0;

    //private PlayerStatusGUIController m_playerStatusGUIController; //Pointer to this player's status GUI controller TO DO: REMOVE ME
    #endregion

    /// <summary>
    /// Constructor for class
    /// </summary>
    /// <param name="pPlayerProfile"></param>
    /// <param name="pPlayerIndex"></param>
    /// <param name="pInputManager"></param>
    public PlayerInfo(PlayerProfile pPlayerProfile, int pPlayerIndex, InputManager pInputManager)
	{
        m_team = (pPlayerIndex % 2) + 1; //TO DO: CHANGE HOW I GET THE TEAMS!

        m_playerProfile = pPlayerProfile;

        m_inputManager = pInputManager;
        m_playerIndex = pPlayerIndex;
        m_skin = new Dictionary<string, Sprite>();

        m_playerName = m_playerProfile.PlayerName;
        LoadSkin(m_playerProfile.PlayerSkin);
        m_loadout = m_playerProfile.PlayerLoadout; // "2,1,0,2,0"; //Weapon,AltWeapon,Passive,Special1,Special2

        //CreatePlayerStatusGUI(); //TO DO: REMOVE ME
    }

    /// <summary>
    /// Update is called once per frame, returns true if the player respawned
    /// </summary>
    /// <param name="pDeltaTime"></param>
    public bool Update(float pDeltaTime)
    {
        if(m_respawnTimer > 0)
        {
            m_respawnTimer -= pDeltaTime;

            if(m_respawnTimer <= 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Spawns the player at the passed coordinates
    /// </summary>
    /// <param name="pSpawnPoint"></param>
    public PlayerObjInfo Spawn(Vector3 pSpawnPoint)
    {
        m_playerObjInfo = ObjectFactory.CreatePlayer(this, m_inputManager, m_skin, m_loadout, pSpawnPoint);

        //m_playerStatusGUIController.SetTarget(m_playerObjInfo); //TO DO: REMOVE ME

        return m_playerObjInfo;
    }

    //***********************************************************************
    // Method: LoadSkin
    //
    // Purpose: Loads all of the sprites for the skin with the passed name
    //***********************************************************************
    public void LoadSkin(string pSkinName)
    {
        m_skin.Clear();

        Sprite[] tempSprites = Resources.LoadAll<Sprite>("Art/Characters/CharacterSheet_" + pSkinName);

        foreach(Sprite s in tempSprites)
        {
            m_skin.Add(s.name, s);
        }
    }

    /// <summary>
    /// Handles what happens when the current player object dies
    /// </summary>
    public void OnDeath()
    {
        m_respawnTimer = m_timeToRespawn;
    }

    /* 
    //***********************************************************************
    // Method: CreatePlayerStatusGUI
    //
    // Purpose: Creates an instance of the PlayerStatusGUI prefab and
    // hooks it up
    //*********************************************************************** 
    protected void CreatePlayerStatusGUI() //TO DO: REMOVE ME
    {
        m_playerStatusGUIController = GUIFactory.EnablePlayerStatusGUI(this);
    }
    */

    #region Properties
    public bool IsAlive
    {
        get
        {
            return (m_playerObjInfo != null);
        }
    }

    public string PlayerName
    {
        get
        {
            return m_playerName;
        }
        set
        {
            m_playerName = value;
        }
    }

    public InputManager InputManager
    {
        get
        {
            return m_inputManager;
        }
    }

    public int PlayerIndex
    {
        get
        {
            return m_playerIndex;
        }
    }

    public int Team
    {
        get
        {
            return m_team;
        }
    }

    public float RespawnTimer
    {
        get
        {
            return m_respawnTimer;
        }
    }
    #endregion
}
