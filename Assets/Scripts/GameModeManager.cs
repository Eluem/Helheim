//**************************************************************************************
// File: GameModeManager.cs
//
// Purpose: Manages all the details of the current game mode
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

//TO DO: Robustify this... right now I have a bunch of hardcoded CTF stuff... maybe this should be an abstract class or something... need to figure the details out...
//maybe it should be different for each game mode/map and it can be individually hardcoded? instead of being a gamemode.. it can just be a map manager? idk...

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public enum GameState_REMOVEME
{
    playing = 0,
    scoreScreen = 1,
    waitingForPlayers = 2
}

public class GameModeManager : MonoBehaviour
{
    #region Declarations
    protected static GameModeManager m_instance; //Used to allow static referencing of non-static values

    public GameObject m_playerManager; //Pointer to the player manager

    public GameObject m_waitingForPlayersGUI; //Pointer to the waiting for players GUI
    public GameObject m_gameScoreTrackerGUI; //Pointer to the top of the screen score tracker GUI
    public GameObject m_scoreScreenGUI; //Pointer to the score screen GUI

    public Text m_waitingForPlayers;
    public GameObject m_team1PressStart;
    public GameObject m_team2PressStart;

    public Text m_gameTimeText; //Pointer to the game time UI text
    public Text m_redScoreText; //Pointer to the red score UI text
    public Text m_blueScoreText; //Pointer to the blue score UI text

    public Text m_scoreScreenTimerText; //Pointer to the score screen timer text
    public Text m_winnerTextRed;
    public Text m_winnerTextBlue;

    public float m_gameTime = 300; //Stores the remaining gametime in seconds
    protected int m_redScore = 0; //Stores the red team's flag caps
    protected int m_blueScore = 0; //Stores the blue team's flag caps

    protected int m_numPlayersHitStart = 0;
    protected List<XInputDotNetPure.PlayerIndex> m_joinedPlayers = new List<PlayerIndex>();

    public int m_maxScore = 3;

    protected bool m_initialized = false;


    public GameState_REMOVEME m_gameState;

    XInputDotNetPure.PlayerIndex[] m_XInputPlayerIndexes; //Stores an array of playerIndexes to be looped through
    XInputDotNetPure.ButtonState[] m_XInputStartButtonStatesPrev; //Stores an array of all the start button states from last frame
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        m_instance = this;
        m_gameState = GameState_REMOVEME.waitingForPlayers;

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

        m_initialized = true;
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
        if (!m_initialized)
        {
            Initialize();
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        switch (m_gameState)
        {
            case GameState_REMOVEME.playing:
                m_gameTime -= Time.deltaTime;
                int minutes = (int)(m_gameTime / 60f);
                string seconds = ((int)(m_gameTime - (minutes * 60))).ToString();
                if (seconds.Length < 2)
                {
                    seconds = "0" + seconds;
                }
                m_gameTimeText.text = minutes + ":" + seconds;

                if (m_gameTime < 0)
                {
                    GameEnd();
                }
                break;
            case GameState_REMOVEME.scoreScreen:
                m_gameTime -= Time.deltaTime;
                m_scoreScreenTimerText.text = Mathf.Round(m_gameTime).ToString();
                if (m_gameTime < 0)
                {
                    RestartGame();
                }
                break;
            case GameState_REMOVEME.waitingForPlayers:
                PlayerJoined();

                if (m_numPlayersHitStart >= 2)
                {
                    StartGame();
                }
                else if(Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
                {
                    ForceJoinPlayers();
                }
                break;
        }
    }

    /// <summary>
    /// Increments the score of the team which corresonds to the passed team index
    /// </summary>
    /// <param name="pTeam">Index of the team whose score should be incremented</param>
    public static void IncrementTeamScore(int pTeam)
    {
        switch (pTeam)
        {
            case 1:
                m_instance.m_redScore++;
                m_instance.m_redScoreText.text = m_instance.m_redScore.ToString();
                if (m_instance.m_redScore >= m_instance.m_maxScore)
                {
                    m_instance.GameEnd();
                }
                break;
            case 2:
                m_instance.m_blueScore++;
                m_instance.m_blueScoreText.text = m_instance.m_blueScore.ToString();
                if (m_instance.m_blueScore >= m_instance.m_maxScore)
                {
                    m_instance.GameEnd();
                }
                break;
        }
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    protected void StartGame()
    {
        m_gameState = GameState_REMOVEME.playing;
        m_gameScoreTrackerGUI.SetActive(true);
        m_waitingForPlayersGUI.SetActive(false);

        m_playerManager.SetActive(true);
    }

    /// <summary>
    /// Handles what happens when the game has ended
    /// </summary>
    protected void GameEnd()
    {
        m_gameState = GameState_REMOVEME.scoreScreen;
        m_gameTime = 10;
        m_gameScoreTrackerGUI.SetActive(false);
        m_scoreScreenGUI.SetActive(true);

        string winMethod = "cap out!";

        if (m_redScore > m_blueScore)
        {
            if (m_redScore < m_maxScore)
            {
                winMethod = "time out!";
            }

            m_winnerTextRed.gameObject.SetActive(true);
            m_winnerTextRed.text = "Red Team won with " + m_redScore + " cap(s) via " + winMethod;
        }
        else if (m_blueScore > m_redScore)
        {
            if (m_blueScore < m_maxScore)
            {
                winMethod = "time out!";
            }

            m_winnerTextBlue.gameObject.SetActive(true);
            m_winnerTextBlue.text = "Blue Team won with " + m_blueScore + " cap(s) via " + winMethod;
            m_winnerTextBlue.gameObject.SetActive(true);
        }
        else
        {
            m_winnerTextBlue.gameObject.SetActive(true);
            m_winnerTextBlue.color = Color.black;
            m_winnerTextBlue.text = "DRAW! Both teams had " + m_blueScore + " cap(s)";
        }

        //UnityEngine.SceneManagement.SceneManager.LoadScene("EpicBalcony-CTF");
        //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Restarts the game
    /// </summary>
    protected void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("EpicBalcony-CTF");
    }

    /// <summary>
    /// Returns true if a new player hit start
    /// </summary>
    /// <returns></returns>
    protected bool PlayerJoined()
    {
        bool tempPlayerJoined = false;
        
        for (int i = 0; i < m_XInputPlayerIndexes.Length; i++)
        {
            XInputDotNetPure.ButtonState tempStartButtonStateCurr = XInputDotNetPure.GamePad.GetState(m_XInputPlayerIndexes[i]).Buttons.Start;
            bool playerTwoOverride = XInputDotNetPure.GamePad.GetState(m_XInputPlayerIndexes[0]).Buttons.Back == ButtonState.Pressed || XInputDotNetPure.GamePad.GetState(m_XInputPlayerIndexes[1]).Buttons.Back == ButtonState.Pressed;

            if (((tempStartButtonStateCurr == ButtonState.Pressed && m_XInputStartButtonStatesPrev[i] == ButtonState.Released) || (i == 1 && playerTwoOverride)) && !IsPlayerJoined(m_XInputPlayerIndexes[i]))
            {
                m_joinedPlayers.Add(m_XInputPlayerIndexes[i]);

                if (m_numPlayersHitStart >= 1)
                {
                    m_team2PressStart.SetActive(false);
                }
                else
                {
                    m_team1PressStart.SetActive(false);
                }

                m_numPlayersHitStart++;

                tempPlayerJoined = true;
            }
            m_XInputStartButtonStatesPrev[i] = tempStartButtonStateCurr;
        }

        return tempPlayerJoined;
    }

    //Debug function that forcefully joins two players
    protected void ForceJoinPlayers()
    {
        m_joinedPlayers.Add(m_XInputPlayerIndexes[1]);
        m_joinedPlayers.Add(m_XInputPlayerIndexes[0]);

        m_team2PressStart.SetActive(false);
        m_team1PressStart.SetActive(false);

        m_numPlayersHitStart = 2;
    }

    protected bool IsPlayerJoined(XInputDotNetPure.PlayerIndex pPlayerIndex)
    {
        for (int i = 0; i < m_joinedPlayers.Count; i++)
        {
            if (m_joinedPlayers[i] == pPlayerIndex)
            {
                return true;
            }
        }
        return false;
    }

    #region Properties
    public static GameState_REMOVEME GameState_REMOVEME
    {
        get
        {
            return m_instance.m_gameState;
        }
    }

    public static List<XInputDotNetPure.PlayerIndex> JoinedPlayers
    {
        get
        {
            return m_instance.m_joinedPlayers;
        }
    }
    #endregion
}