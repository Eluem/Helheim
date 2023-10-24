//**************************************************************************************
// File: PlayerProfile.cs
//
// Purpose: This class is meant to store all of a player's preferences, loadout,
// unlocks, ect... and handles serializing and deserializing to the server
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerProfile
{
    #region Declarations
    private string m_playerName; //Stores the name of the player
    private string m_playerSkin; //Stores the skin the player uses
    private string m_playerLoadout; //Stores the player's loadout
    #endregion
	
	//***********************************************************************
	// Method: PlayerProfile
	//
	// Purpose: Constructor for class
	//***********************************************************************
	public PlayerProfile(string pPlayerName, string pPlayerSkin, string pPlayerLoadout)
	{
        m_playerName = pPlayerName;
        m_playerSkin = pPlayerSkin;
        m_playerLoadout = pPlayerLoadout;
	}
	
	#region Properties
    public string PlayerName
    {
        get
        {
            return m_playerName;
        }
    }

    public string PlayerSkin
    {
        get
        {
            return m_playerSkin;
        }
    }

    public string PlayerLoadout
    {
        get
        {
            return m_playerLoadout;
        }
    }
    #endregion
}