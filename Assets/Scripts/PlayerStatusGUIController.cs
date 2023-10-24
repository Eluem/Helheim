//**************************************************************************************
// File: PlayerStatusGUIController.cs
//
// Purpose: This 
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public class PlayerStatusGUIController : MonoBehaviour
{
    #region Declarations
    #region Pointers
    [Inspect, Group("Pointers", 1)]
    public RectTransform m_rectTransform; //Pointer to the player status GUI object that this controls
    [Inspect, Group("Pointers")]
    protected PlayerInfo m_playerInfo; //Pointer to the player info that spawned this object
    [Inspect, Group("Pointers")]
    protected PlayerObjInfo m_target; //Pointer to the player object that this is tracking

    [Inspect, Group("Pointers")]
    public RectTransform m_healthCurrent; //Pointer to the health bar rect transform

    [Inspect, Group("Pointers")]
    public RectTransform m_staminaCurrent; //Pointer to the stamina bar rect transform

    [Inspect, Group("Pointers")]
    public RectTransform m_manaCurrent; //Pointer to the mana bar rect transform

    [Inspect, Group("Pointers")]
    public Animator[] m_manaPulseAnimators = new Animator[4];
    #endregion 

    private Vector2 m_healthStartingSize; //Size of the health bar when the object is created (used to create a base line)
    private Vector3 m_healthStartingPosition; //Local Position of the health bar when the object is created (used to create a base line)
    private int m_prevMaxHealth; //Stores the max health from the last update

    private Vector2 m_staminaStartingSize; //Size of the stamina bar when the object is created (used to create a base line)
    private Vector3 m_staminaStartingPosition; //Local Position of the stamina bar when the object is created (used to create a base line)
    private int m_prevMaxStamina; //Stores the max stamina from the last update

    private Vector2 m_manaStartingSize; //Size of the mana bar when the object is created (used to create a base line)
    private Vector3 m_manaStartingPosition; //Local Position of the mana bar when the object is created (used to create a base line)
    private int m_prevMaxMana; //Stores the max mana from the last update

    private float m_prevMana; //Stores the value of the player's mana from the last call to UpdateManaBar
    #endregion

    //***********************************************************************
    // Method: Initialize
    //
    // Purpose: This basically takes the place of a constructor
    //***********************************************************************
    public void Initialize(PlayerInfo pPlayerInfo)
    {
        m_playerInfo = pPlayerInfo;

        m_prevMaxHealth = 100;
        m_prevMaxStamina = 100;
        m_prevMaxMana = 100;
    }

    //***********************************************************************
    // Method: SetTarget
    //
    // Purpose: This sets the currently targeted player obj info
    //***********************************************************************
    public void SetTarget(PlayerObjInfo pTarget)
    {
        m_target = pTarget;

        m_prevMaxHealth = m_target.MaxHealth;
        m_prevMaxStamina = m_target.MaxStamina;
        m_prevMaxMana = m_target.MaxMana;
    }

    //***********************************************************************
    // Method: Start
    //
    // Purpose: Use this for initialization
    //***********************************************************************
    void Start()
	{
        m_healthStartingSize = m_healthCurrent.sizeDelta;
        m_healthStartingPosition = m_healthCurrent.localPosition;

        m_staminaStartingSize = m_staminaCurrent.sizeDelta;
        m_staminaStartingPosition = m_staminaCurrent.localPosition;

        m_manaStartingSize = m_manaCurrent.sizeDelta;
        m_manaStartingPosition = m_manaCurrent.localPosition;
    }
	
	//***********************************************************************
    // Method: Update
    //
    // Purpose: Update is called once per frame
    //***********************************************************************
	void Update()
	{
		if(m_target == null)
        {
            //Represents zeroed out stats if it didn't  get updated before the player was deleted/deactivated
            UpdateHealthBar(0, m_prevMaxHealth);
            UpdateStaminaBar(0, m_prevMaxStamina);
            UpdateManaBar(0, m_prevMaxMana);


            //NOTE: This used to actually reference m_target even though it was null.. this was a mistake on my part.. but it worked for some reason.... so this is here to remember that strangeness...
            //I think it worked because a null unity object is actually not null.. and doesn't act entirely null... not 100% sure why...
            /*
            //TO DO: This shouldn't be referencing m_target... m_target is null.. why the hell does this even work?
            UpdateHealthBar(0, m_target.MaxHealth);
            UpdateStaminaBar(0, m_target.MaxStamina);
            UpdateManaBar(0, m_target.MaxMana);
            */
        }
        else
        {
            UpdateHealthBar(m_target.HealthFloat, m_target.MaxHealth);
            UpdateStaminaBar(m_target.StaminaFloat, m_target.MaxStamina);
            UpdateManaBar(m_target.ManaFloat, m_target.MaxMana);

            m_prevMaxHealth = m_target.MaxHealth;
            m_prevMaxStamina = m_target.MaxStamina;
            m_prevMaxMana = m_target.MaxMana;
        }
	}

    //***********************************************************************
    // Method: UpdateHealthbar
    //
    // Purpose: Updates the healthBar to reflect the passed values
    //***********************************************************************
    protected void UpdateHealthBar(float pHealth, int pMaxHealth)
    {
        //Update the size of the health bar to match the target's remaining health
        m_healthCurrent.sizeDelta = new Vector2(m_healthStartingSize.x * (pHealth / pMaxHealth), m_healthStartingSize.y);
        //Update the position so that it doesn't shrink around the center
        m_healthCurrent.localPosition = new Vector3(m_healthStartingPosition.x - ((m_healthStartingSize.x - m_healthCurrent.sizeDelta.x) / 2), m_healthStartingPosition.y, m_healthStartingPosition.z);
    }

    //***********************************************************************
    // Method: UpdateStaminabar
    //
    // Purpose: Updates the staminaBar to reflect the passed values
    //***********************************************************************
    protected void UpdateStaminaBar(float pStamina, int pMaxStamina)
    {
        //Update the size of the stamina bar to match the target's remaining stamina
        m_staminaCurrent.sizeDelta = new Vector2(m_staminaStartingSize.x * (pStamina / pMaxStamina), m_staminaStartingSize.y);
        //Update the position so that it doesn't shrink around the center
        m_staminaCurrent.localPosition = new Vector3(m_staminaStartingPosition.x - ((m_staminaStartingSize.x - m_staminaCurrent.sizeDelta.x) / 2), m_staminaStartingPosition.y, m_staminaStartingPosition.z);
    }

    //***********************************************************************
    // Method: UpdateManaBar
    //
    // Purpose: Updates the manaBar to reflect the passed values
    //***********************************************************************
    protected void UpdateManaBar(float pMana, int pMaxMana)
    {
        //Update the size of the mana bar to match the target's remaining mana
        m_manaCurrent.sizeDelta = new Vector2(m_manaStartingSize.x * (pMana / pMaxMana), m_manaStartingSize.y);
        //Update the position so that it doesn't shrink around the center
        m_manaCurrent.localPosition = new Vector3(m_manaStartingPosition.x - ((m_manaStartingSize.x - m_manaCurrent.sizeDelta.x) / 2), m_manaStartingPosition.y, m_manaStartingPosition.z);

        //Handle representing filled mana "pills"
        int tempMaxManaPillFilledIndex = (int)(pMana / 25);
        for (int i = 0; i < 4; i++)
        {
            if (tempMaxManaPillFilledIndex > i && !m_manaPulseAnimators[i].GetBool("Filled"))
            {
                for (int j = 0; j <= i; j++)
                {
                    m_manaPulseAnimators[j].CrossFade("Flash", 0f);
                }
            }

            m_manaPulseAnimators[i].SetBool("Filled", tempMaxManaPillFilledIndex > i);
        }

        /*
        for(int i = 0; i < 4; i++)
        {
            m_manaPulseAnimators[i].SetBool("Filled", tempMaxManaPillFilledIndex > i);
        }
        */
    }

    #region Properties
    #endregion
}