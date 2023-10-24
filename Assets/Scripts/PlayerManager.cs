//**************************************************************************************
// File: PlayerManager.cs
//
// Purpose: Handles spawning and controlling all (local only?) players
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;
using XInputDotNetPure;

[AdvancedInspector]
public class PlayerManager : MonoBehaviour, ISystemObject
{
    #region Declarations
    [Inspect]
    public TeamSpawnPointInfo[] m_spawnPoints;

    List<PlayerInfo> m_players;

    string[] m_inputSuffixes;

    bool m_usingXInput = true; //Indicates if XInput is being used
    XInputDotNetPure.PlayerIndex[] m_XInputPlayerIndexes; //Stores an array of playerIndexes to be looped through
    XInputDotNetPure.ButtonState[] m_XInputStartButtonStatesPrev; //Stores an array of all the start button states from last frame
    #endregion

    /// <summary>
    /// This function will be called any time a new map is loaded
    /// </summary>
    /// <param name="pSceneName">Name of the scene being loaded for the map</param>
    public void Reload(string pSceneName)
    {
        //Replace all the spawn points with new spawn points from this map.
        //m_spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint-Player");
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Awake() //TO DO: This used to be start.. but I changed it to Awake to prevent it from firing after OnEnable...
    {
        m_players = new List<PlayerInfo>();

        if (m_usingXInput)
        {
            m_inputSuffixes = new string[0]; // 1];
            //m_inputSuffixes[0] = "kb"; TO DO: Implement keyboard buttons

            m_XInputPlayerIndexes = new XInputDotNetPure.PlayerIndex[4];
            m_XInputPlayerIndexes[0] = PlayerIndex.One;
            m_XInputPlayerIndexes[1] = PlayerIndex.Two;
            m_XInputPlayerIndexes[2] = PlayerIndex.Three;
            m_XInputPlayerIndexes[3] = PlayerIndex.Four;

            m_XInputStartButtonStatesPrev = new XInputDotNetPure.ButtonState[4];
            m_XInputStartButtonStatesPrev[0] = ButtonState.Released;
            m_XInputStartButtonStatesPrev[1] = ButtonState.Released;
            m_XInputStartButtonStatesPrev[2] = ButtonState.Released;
            m_XInputStartButtonStatesPrev[3] = ButtonState.Released;
        }
        else
        {
            m_XInputPlayerIndexes = new XInputDotNetPure.PlayerIndex[0];

            m_inputSuffixes = new string[2]; // 5];
            m_inputSuffixes[0] = "a";
            m_inputSuffixes[1] = "b";
            //m_inputSuffixes[2] = "c";
            //m_inputSuffixes[3] = "d";
            //m_inputSuffixes[4] = "kb";
        }

        gameObject.SetActive(false);
    }

    /// <summary>
    /// Fires when this component becomes enabled
    /// </summary>
    void OnEnable()
    {
        float xOffset = 2.5f;

        for (int i = 0; i < GameModeManager.JoinedPlayers.Count; i++)
        {
            PlayerInfo tempPlayerInfo = GetPlayerByInputDevice(GameModeManager.JoinedPlayers[i]);
            //If someone did, check if that input device is already claimed by a player
            if (tempPlayerInfo == null)
            {
                tempPlayerInfo = new PlayerInfo(TESTINGGetPlayerProfile(), m_players.Count, new XInputGamepadInputManager("nullInputSuffix", GameModeManager.JoinedPlayers[i])); //TO DO: Consider finding the Unity input suffix by looping.. probably can't make consistent?
                m_players.Add(tempPlayerInfo);
            }

            //Check if they're dead
            if (!tempPlayerInfo.IsAlive)
            {
                if(tempPlayerInfo.Team == 1)
                {
                    xOffset = Mathf.Abs(xOffset);
                }
                else
                {
                    xOffset = -1 * Mathf.Abs(xOffset);
                }

                //If they are, spawn them
                tempPlayerInfo.Spawn(m_spawnPoints[tempPlayerInfo.Team].GetRandomSpawnPoint().position + new Vector3(xOffset, 0, 0));
            }
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        foreach(PlayerInfo playerInfo in m_players)
        {
            if(playerInfo.Update(Time.deltaTime))
            {
                playerInfo.Spawn(m_spawnPoints[playerInfo.Team].GetRandomSpawnPoint().position);
            }
        }

        //Check if any gamepads hit "start" using XInputDotNetPure
        #region Handle XInputDotNetPure
        for (int i = 0; i < m_XInputPlayerIndexes.Length; i++)
        {
            /*
             //Code to force spawn players with back button
            //TO DO: REMOVE ME DELETE ME
            if(XInputDotNetPure.GamePad.GetState(m_XInputPlayerIndexes[i]).Buttons.Back == ButtonState.Pressed)
            {
                Debug.Log("delete me!");
                Inspector_ForceSpawnPlayer();
            }
            */

            XInputDotNetPure.ButtonState tempStartButtonStateCurr = XInputDotNetPure.GamePad.GetState(m_XInputPlayerIndexes[i]).Buttons.Start;

            if (tempStartButtonStateCurr == ButtonState.Pressed && m_XInputStartButtonStatesPrev[i] == ButtonState.Released)
            {
                PlayerInfo tempPlayerInfo = GetPlayerByInputDevice(m_XInputPlayerIndexes[i]);
                //If someone did, check if that input device is already claimed by a player
                if (tempPlayerInfo == null)
                {
                    tempPlayerInfo = new PlayerInfo(TESTINGGetPlayerProfile(), m_players.Count, new XInputGamepadInputManager("nullInputSuffix", m_XInputPlayerIndexes[i])); //TO DO: Consider finding the Unity input suffix by looping.. probably can't make consistent?
                    m_players.Add(tempPlayerInfo);

                    //Check if they're dead
                    if (!tempPlayerInfo.IsAlive)
                    {
                        //If they are, spawn them
                        tempPlayerInfo.Spawn(m_spawnPoints[tempPlayerInfo.Team].GetRandomSpawnPoint().position);
                    }
                }

                //Respawning is now done automatically with a respawn timer
                ////Check if they're dead
                //if (!tempPlayerInfo.IsAlive)
                //{
                //    //If they are, spawn them
                //    tempPlayerInfo.Spawn(m_spawnPoints[tempPlayerInfo.Team].GetRandomSpawnPoint().position);
                //}
            }
            m_XInputStartButtonStatesPrev[i] = tempStartButtonStateCurr;
        }
        #endregion

        //Check if any inputs hit "start" using the built in unity input system
        #region Handle unityInput
        for (int i = 0; i < m_inputSuffixes.Length; i++)
        {
            if (Input.GetButton("Start_" + m_inputSuffixes[i]))
            {
                PlayerInfo tempPlayerInfo = GetPlayerByInputDevice(m_inputSuffixes[i]);
                //If someone did, check if that input device is already claimed by a player
                if (tempPlayerInfo == null)
                {
                    InputManager tempInputManager = null;
                    //If they aren't, create a player for them
                    if (m_inputSuffixes[i] == "kb")
                    {
                        //TO DO: Add support for keyboard input
                        Debug.Log("ERROR: KEYBOARD INPUT IS NOT SUPPORTED CURRENTLY");
                    }
                    else
                    {
                        tempInputManager = new GamepadInputManager(m_inputSuffixes[i]);
                    }
                    tempPlayerInfo = new PlayerInfo(TESTINGGetPlayerProfile(), m_players.Count, tempInputManager);
                    m_players.Add(tempPlayerInfo);
                }

                //Check if they're dead
                if (!tempPlayerInfo.IsAlive)
                {
                    //If they are, spawn them
                    tempPlayerInfo.Spawn(m_spawnPoints[tempPlayerInfo.Team].GetRandomSpawnPoint().position);
                }
            }
        }
        #endregion  
    }

    /// <summary>
    /// Accepts an InputSuffix (which basically identifies an
    /// input device and returns the player info associated with it)
    /// </summary>
    /// <param name="pInputSuffix"></param>
    /// <returns></returns>
    PlayerInfo GetPlayerByInputDevice(string pInputSuffix)
    {
        PlayerInfo tempPlayerInfo = null;
        foreach (PlayerInfo playerInfo in m_players)
        {
            if (playerInfo.InputManager.Matches(pInputSuffix))
            {
                tempPlayerInfo = playerInfo;
                break;
            }
        }

        return tempPlayerInfo;
    }

    /// <summary>
    /// Accepts an XInputPlayerIndex (which basically identifies an
    /// input device and returns the player info associated with it)
    /// </summary>
    /// <param name="pXInputPlayerIndex"></param>
    /// <returns></returns>
    PlayerInfo GetPlayerByInputDevice(XInputDotNetPure.PlayerIndex pXInputPlayerIndex)
    {
        PlayerInfo tempPlayerInfo = null;
        foreach (PlayerInfo playerInfo in m_players)
        {
            if (playerInfo.InputManager.Matches(pXInputPlayerIndex))
            {
                tempPlayerInfo = playerInfo;
                break;
            }
        }

        return tempPlayerInfo;
    }

    /// <summary>
    /// Test function to create a player profile with hardcoded
    /// values based on the count of current players 'logged in' so that I
    /// can have two loadouts
    /// </summary>
    /// <returns></returns>
    private PlayerProfile TESTINGGetPlayerProfile()
    {
        switch (m_players.Count)
        {
            case 0:
                return new PlayerProfile("Eluem", "FoxMask", "3,1,0,3,1"); //return new PlayerProfile("Eluem", "Male_BrownHair", "3,1,0,3,1"); //return new PlayerProfile("Eluem", "Male_RedTeam1", "3,1,0,3,1");
            case 1:
                return new PlayerProfile("Cippielia", "YinMask", "2,1,0,1,2"); //return new PlayerProfile("Cippielia", "Male_BlondHair", "2,1,0,1,2"); //return new PlayerProfile("Cippielia", "Male_BlueTeam1", "2,1,0,1,2");
            case 2:
                return new PlayerProfile("Eluem", "Male_RedTeam2", "2,1,0,1,2");
            case 3:
                return new PlayerProfile("Cippielia", "Male_BlueTeam2", "3,1,0,3,1");
        }

        return new PlayerProfile("Fatherr", "Fatherr", "3,1,0,3,1");

        ////"2,1,0,2,0"
        //if(m_players.Count > 0)
        //{
        //    return new PlayerProfile("Cippielia", "Male_BlueTeam1", "2,1,0,1,2"); //Weapon,AltWeapon,Passive,Special1,Special2
        //}

        //return new PlayerProfile("Eluem", "Male_RedTeam1", "3,1,0,3,1");
        ////return new PlayerProfile("Eluem", "Male_BrownHair", "2,1,0,3,1");
    }

    [Inspect]
    public void Inspector_ForceSpawnPlayer()
    {
        PlayerProfile tempPlayerProfile = new PlayerProfile("Cippielia", "Male_BlondHair", "2,1,0,1,2"); //return new PlayerProfile("Cippielia", "Male_BlueTeam1", "2,1,0,1,2");


        PlayerInfo tempPlayerInfo = new PlayerInfo(tempPlayerProfile, m_players.Count, new XInputGamepadInputManager("nullInputSuffix", m_XInputPlayerIndexes[1])); //TO DO: Consider finding the Unity input suffix by looping.. probably can't make consistent?

        m_players.Add(tempPlayerInfo);

        //Check if they're dead
        if (!tempPlayerInfo.IsAlive)
        {
            //If they are, spawn them
            tempPlayerInfo.Spawn(m_spawnPoints[tempPlayerInfo.Team].GetRandomSpawnPoint().position).name = "Player(" + m_players.Count + ")";
        }
    }


    #region Properties
    #endregion

    [System.Serializable]
    public class TeamSpawnPointInfo
    {
        #region Declarations
        public Transform[] m_spawnPoints;
        #endregion

        /// <summary>
        /// Returns a random spawnpoint for this team
        /// </summary>
        /// <returns></returns>
        public Transform GetRandomSpawnPoint()
        {
            return m_spawnPoints[Random.Range(0, m_spawnPoints.Length)];
        }

        #region Properties
        public Transform[] SpawnPoints
        {
            get
            {
                return m_spawnPoints;
            }
        }
        #endregion
    }
}
