//**************************************************************************************
// File: GUIFactory.cs
//
// Purpose: This object will be used to spawn GUI elements programatically
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIFactory : MonoBehaviour
{
    #region Declarations
    protected static GUIFactory m_instance; //I'm not sure exactly why but I need this to make it static or something

    public Transform m_canvasTransform;

    public GameObject[] m_playerStatusGUIList;

    public GameObject m_charFloatingHUDPrefab;
    #endregion

    //***********************************************************************
    // Method: Awake
    //
    // Purpose: Use this for early initialization (even when disabled)
    //***********************************************************************
    void Awake()
    {
        m_instance = this;
    }

    //***********************************************************************
    // Method: Start
    //
    // Purpose: Use this for initialization
    //***********************************************************************
    void Start()
    {
        //m_instance = this;
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
    // Method: EnablePlayerStatusGUI
    //
    // Purpose: Enables a player status gui and returns the
    // PlayerStatusGUIController for it
    //***********************************************************************
    public static PlayerStatusGUIController EnablePlayerStatusGUI(PlayerInfo pPlayerInfo)
    {
        m_instance.m_playerStatusGUIList[pPlayerInfo.PlayerIndex].SetActive(true);
        return m_instance.m_playerStatusGUIList[pPlayerInfo.PlayerIndex].GetComponent<PlayerStatusGUIController>();


        /*
        GameObject tempObj = (GameObject)Object.Instantiate(m_instance.m_playerStatusGUIPrefab, Vector3.zero, Quaternion.identity);
        tempObj.transform.SetParent(m_instance.m_canvasTransform);
        tempObj.transform.localPosition = pSpawnPoint;

        PlayerStatusGUIController tempPlayerStatusGUIController = tempObj.GetComponent<PlayerStatusGUIController>();
        tempPlayerStatusGUIController.Initialize(pPlayerInfo);
        return tempPlayerStatusGUIController;
        */

    }

    //***********************************************************************
    // Method: SpawnCharFloatingHUD
    //
    // Purpose: Enables a player status gui and returns the
    // PlayerStatusGUIController for it
    //***********************************************************************
    public static CharFloatingHUDController CreateCharFloatingHUD(CharObjInfo pCharObjInfo, float pZAxisOrder, Vector2 pOffset = default(Vector2))
    {
        GameObject tempObj = (GameObject)Object.Instantiate(m_instance.m_charFloatingHUDPrefab, new Vector3(pCharObjInfo.Transform.position.x + pOffset.x, pCharObjInfo.Transform.position.y + pOffset.y, pZAxisOrder), Quaternion.identity);

        CharFloatingHUDController tempObjInfo = (CharFloatingHUDController)tempObj.GetComponent(typeof(CharFloatingHUDController));
        tempObjInfo.Initialize(pCharObjInfo, pZAxisOrder, pOffset);

        return tempObjInfo;
    }

    #region Properties
    #endregion
}
