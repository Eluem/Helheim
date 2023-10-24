//**************************************************************************************************************
// File: PlayerController.cs
//
// Purpose: This class handles all the logic for how the player should respond to
// any inputs and manages the statemachine.
//
// Should I implement the methodology from this thread?:
// http://forum.unity3d.com/threads/best-practices-handling-input-movement-animation-with-scripts.256637/
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************************************

//For this game, I'm going to communicate with the state machine in the Update function, based on player inputs and just set parameters.
//Anything that affects the player's physics will go into FixedUpdate, but it won't be based directly on player inputs.. it'll read the state machine's (animator) corrent state
//to handle the logic... all gameplay mechanics logic will reference the state machine to make decisions, not the controls.
//(Perhaps excluding... most likely excluding things that aren't strictly based on character states.. like opening menus or maybe moving the screen around to look around...
//also turning around and looking around isn't necessarily a special state... so maybe that does just look at the game pad controls.. also determining specifics.. like speed has to analyze the controls..)


using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public class PlayerController : MonoBehaviour
{
    #region Declarations
    //TO DO: Set up a "pointers" group and configure it correctly
    #region Pointers
    [Inspect, Group("Pointers", 1)]
    public Rigidbody2D m_rigidbody;
    [Inspect, Group("Pointers")]
    public Transform m_transform;
    [Inspect, Group("Pointers")]
    public Animator m_animator;
    [Inspect, Group("Pointers")]
    public PlayerObjInfo m_playerObjInfo;
    #endregion

    private InputManager m_inputManager;

    #region Parameter Muters
    [Inspect, Group("Input Muters", 2)]
    public bool m_allActionIsMutedDirect = false;
    [Inspect, Group("Input Muters")]
    public bool m_evadeIsMutedDirect = false;
    [Inspect, Group("Input Muters")]
    public bool m_interactIsMutedDirect = false;
    [Inspect, Group("Input Muters")]
    public bool m_specialAction1IsMutedDirect = false;
    [Inspect, Group("Input Muters")]
    public bool m_specialAction2IsMutedDirect = false;
    [Inspect, Group("Input Muters")]
    public bool m_attackBasicLightIsMutedDirect = false;
    [Inspect, Group("Input Muters")]
    public bool m_attackSpecialLightIsMutedDirect = false;
    [Inspect, Group("Input Muters")]
    public bool m_attackBasicHeavyIsMutedDirect = false;
    [Inspect, Group("Input Muters")]
    public bool m_attackSpecialHeavyIsMutedDirect = false;
    [Inspect, Group("Input Muters")]
    public bool m_movementIsMutedDirect = false;
    #endregion
    #endregion


    //***********************************************************************
    // Method: Initialize
    //
    // Purpose: This basically takes the place of a constructor
    //***********************************************************************
    public void Initialize(InputManager pInputManager)
    {
        m_inputManager = pInputManager;
    }

    //***********************************************************************
    // Method: Start
    //
    // Purpose: Use this for initialization
    //***********************************************************************
    void Start()
    {
        /*
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_transform = GetComponent<Transform>();
        m_animator = GetComponent<Animator>();
        m_playerObjInfo = GetComponent<PlayerObjInfo>();
        */

        if (m_inputManager == null)
        {
            m_inputManager = new GamepadInputManager("a"); //TO DO: Replace with PlayerAIInputManager
        }
    }

    //***********************************************************************
    // Method: Update
    //
    // Purpose: Once per frame updates
    //***********************************************************************
    void Update()
    {
        m_inputManager.Update(Time.deltaTime, m_allActionIsMutedDirect, m_evadeIsMutedDirect, m_interactIsMutedDirect, m_specialAction1IsMutedDirect, m_specialAction2IsMutedDirect, m_attackBasicLightIsMutedDirect, m_attackSpecialLightIsMutedDirect, m_attackBasicHeavyIsMutedDirect, m_attackSpecialHeavyIsMutedDirect, m_movementIsMutedDirect);

        Vector2 dir = m_inputManager.Movement.Direction;
        if (dir.magnitude > 1)
        {
            dir.Normalize();
        }

        #region Handle Controls for Statemachine
        //Handle special action checking
        #region Special Action
        //Special Action 1
        if (m_inputManager.SpecialAction1.IsDown && !m_inputManager.SpecialAction1.IsMuted)
        {
            if (m_playerObjInfo.CanDoAction("SpecialAction1"))
            {
                //TO DO: Rename this ability...
                //Handle a special case for the SwitchWeapon ability
                if (m_playerObjInfo.SpecialAbilityInfo1.Name == "SwitchWeapon")
                {
                    m_playerObjInfo.ToggleWeapons();
                }
                else
                {
                    //Determine if this action is going to be the major or minor special action
                    if (m_animator.GetInteger("SpecialActionMajorSlot") != 2)
                    {
                        //The major slot is currently not in use, use it
                        if (!MutuallyExclusiveParamSet("SpecialActionMajorPressed"))
                        {
                            m_animator.SetTrigger("SpecialActionMajorPressed");
                            m_animator.SetBool("SpecialActionMajorState", true);
                            m_animator.SetInteger("SpecialActionMajorChoice", m_playerObjInfo.SpecialAbilityInfo1.ID);
                            m_animator.SetInteger("SpecialActionMajorSlot", 1);
                            m_animator.SetInteger("SpecialActionMajorAmmo", m_playerObjInfo.SpecialAbilityInfo1.Ammo);
                        }
                    }
                    else
                    {
                        //The major slot is currently being used by the other special action button, use minor
                        if (!MutuallyExclusiveParamSet("SpecialActionMinorPressed"))
                        {
                            m_animator.SetTrigger("SpecialActionMinorPressed");
                            m_animator.SetBool("SpecialActionMinorState", true);
                            m_animator.SetInteger("SpecialActionMinorChoice", m_playerObjInfo.SpecialAbilityInfo1.ID);
                            m_animator.SetInteger("SpecialActionMinorSlot", 1);
                            m_animator.SetInteger("SpecialActionMinorAmmo", m_playerObjInfo.SpecialAbilityInfo1.Ammo);
                        }
                    }
                }
            }
        }
        else if (!m_inputManager.SpecialAction1.ButtonState)
        {
            if (m_animator.GetInteger("SpecialActionMajorSlot") == 1)
            {
                m_animator.SetBool("SpecialActionMajorState", false);
            }

            if (m_animator.GetInteger("SpecialActionMinorSlot") == 1)
            {
                m_animator.SetBool("SpecialActionMinorState", false);
            }
        }

        //Special Action 2
        if (m_inputManager.SpecialAction2.IsDown && !m_inputManager.SpecialAction2.IsMuted)
        {
            if (m_playerObjInfo.CanDoAction("SpecialAction2"))
            {
                //TO DO: Rename this ability...
                //Handle a special case for the SwitchWeapon ability
                if (m_playerObjInfo.SpecialAbilityInfo2.Name == "SwitchWeapon")
                {
                    m_playerObjInfo.ToggleWeapons();
                }
                else
                {
                    //Determine if this action is going to be the major or minor special action
                    if (m_animator.GetInteger("SpecialActionMajorSlot") != 1)
                    {
                        //The major slot is currently not in use, use it
                        if (!MutuallyExclusiveParamSet("SpecialActionMajorPressed"))
                        {
                            m_animator.SetTrigger("SpecialActionMajorPressed");
                            m_animator.SetBool("SpecialActionMajorState", true);
                            m_animator.SetInteger("SpecialActionMajorChoice", m_playerObjInfo.SpecialAbilityInfo2.ID);
                            m_animator.SetInteger("SpecialActionMajorSlot", 2);
                            m_animator.SetInteger("SpecialActionMajorAmmo", m_playerObjInfo.SpecialAbilityInfo2.Ammo);
                        }
                    }
                    else
                    {
                        //The major slot is currently being used by the other special action button, use minor
                        if (!MutuallyExclusiveParamSet("SpecialActionMinorPressed"))
                        {
                            m_animator.SetTrigger("SpecialActionMinorPressed");
                            m_animator.SetBool("SpecialActionMinorState", true);
                            m_animator.SetInteger("SpecialActionMinorChoice", m_playerObjInfo.SpecialAbilityInfo2.ID);
                            m_animator.SetInteger("SpecialActionMinorSlot", 2);
                            m_animator.SetInteger("SpecialActionMinorAmmo", m_playerObjInfo.SpecialAbilityInfo2.Ammo);
                        }
                    }
                }
            }
        }
        else if (!m_inputManager.SpecialAction2.ButtonState)
        {
            if (m_animator.GetInteger("SpecialActionMajorSlot") == 2)
            {
                m_animator.SetBool("SpecialActionMajorState", false);
            }

            if (m_animator.GetInteger("SpecialActionMinorSlot") == 2)
            {
                m_animator.SetBool("SpecialActionMinorState", false);
            }
        }
        #endregion

        //Handle interaction checking
        #region Interact
        //Interact
        if (m_inputManager.Interact.IsDown && !m_inputManager.Interact.IsMuted && !MutuallyExclusiveParamSet("InteractPressed"))
        {
            if (m_playerObjInfo.CanDoAction("Interact"))
            {
                m_animator.SetTrigger("InteractPressed");
                m_animator.SetBool("InteractState", true);
            }
        }
        else if (!m_inputManager.Interact.ButtonState)
        {
            m_animator.SetBool("InteractState", false);
        }
        #endregion

        //Handle combat checking
        #region Combat
        //Attack Basic Light
        if (m_inputManager.AttackBasicLight.IsDown && !m_inputManager.AttackBasicLight.IsMuted && !MutuallyExclusiveParamSet("AttackBasicLightPressed"))
        {
            if (m_playerObjInfo.CanDoAction("AttackBasicLight"))
            {
                m_animator.SetTrigger("AttackBasicLightPressed");
                m_animator.SetBool("AttackBasicLightState", true);
            }
        }
        else if (!m_inputManager.AttackBasicLight.ButtonState)
        {
            m_animator.SetBool("AttackBasicLightState", false);
        }

        //Attack Basic Heavy
        if (m_inputManager.AttackBasicHeavy.IsDown && !m_inputManager.AttackBasicHeavy.IsMuted && !MutuallyExclusiveParamSet("AttackBasicHeavyPressed"))
        {
            if (m_playerObjInfo.CanDoAction("AttackBasicHeavy"))
            {
                m_animator.SetTrigger("AttackBasicHeavyPressed");
                m_animator.SetBool("AttackBasicHeavyState", true);
            }
        }
        else if (!m_inputManager.AttackBasicHeavy.ButtonState)
        {
            m_animator.SetBool("AttackBasicHeavyState", false);
        }

        //Attack Special Light
        if (m_inputManager.AttackSpecialLight.IsDown && !m_inputManager.AttackSpecialLight.IsMuted && !MutuallyExclusiveParamSet("AttackSpecialLightPressed"))
        {
            if (m_playerObjInfo.CanDoAction("AttackSpecialLight"))
            {
                m_animator.SetTrigger("AttackSpecialLightPressed");
                m_animator.SetBool("AttackSpecialLightState", true);
            }
        }
        else if (!m_inputManager.AttackSpecialLight.ButtonState)
        {
            m_animator.SetBool("AttackSpecialLightState", false);
        }

        /*
        //TO DO: REMOVE ME, THIS IS CURRENTLY HERE TO DISABLE THE LEFT TRIGGER SO IT DOESN'T LEAVE THE PLAYER UNABLE TO DO OTHER "mutually exclusive" ACTIONS.. Note.. is that system even a good idea???
        //TO DO: UNCOMMENT ME (not remove me.. but I left the message above because I scheduled it into todoist so I can easily search it.
        //Attack Special Heavy
        if (m_inputManager.AttackSpecialHeavy.IsDown && !m_inputManager.AttackSpecialHeavy.IsMuted && !MutuallyExclusiveParamSet("AttackSpecialHeavyPressed"))
        {
            if (m_playerObjInfo.CanDoAction("AttackSpecialHeavy"))
            {
                m_animator.SetTrigger("AttackSpecialHeavyPressed");
                m_animator.SetBool("AttackSpecialHeavyState", true);
            }
        }
        else if (!m_inputManager.AttackSpecialHeavy.ButtonState)
        {
            m_animator.SetBool("AttackSpecialHeavyState", false);
        }
        */
        #endregion

        //Handle walk/run/sprint/evade checking
        #region Mobility
        //Evade
        if (!m_inputManager.Evade.IsMuted && !MutuallyExclusiveParamSet("EvadePressed"))
        {
            if (m_playerObjInfo.CanDoAction("Evade"))
            {
                //Set the EvadePressedDuringSprint trigger, this is the only time the actual "IsDown" button state should matter for evade, and is only queued while the sprint animation is running
                //This is a cludge to create a more sensitive response for the dive animation compared to a roll (roll requires a tap, dive allows you to press and hold and should respond immediately)
                if (m_inputManager.Evade.IsDown && m_animator.GetCurrentAnimatorStateInfo(0).IsName("Sprint"))
                {
                    m_animator.SetTrigger("EvadePressedDuringSprint");
                }

                //Can't allow the tapped trigger to go off during sprint, since then the trigger will have an unusual queue duration (since sprint can't consume it and can last a long time)
                //This is due to the fact that the sprinting "dive" action needs to be more sensitive, and should go off whenever the button is PRESSED during sprint
                //Additional cludge added to prevent issue with a dodge/backstep getting buffered until you stop walking or your dodge comes off cooldown
                if (m_inputManager.Evade.IsTapped && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Sprint") && !(m_animator.GetBool("MovementState") && m_animator.GetInteger("DodgeCount") < 1))
                {
                    m_animator.SetTrigger("EvadeTapped");
                }

                if (m_inputManager.Evade.IsHeld)
                {
                    m_animator.SetBool("EvadeHeld", true);
                }

                if (m_inputManager.Evade.ButtonState)
                {
                    m_animator.SetBool("EvadeState", true);
                }
            }
        }
        if (!m_inputManager.Evade.ButtonState)
        {
            m_animator.SetBool("EvadeState", false);

            m_animator.SetBool("EvadeHeld", false);
        }

        /*
        if (((m_inputManager.Evade.IsTapped && !MutuallyExclusiveParamSet("Evade")) || (m_inputManager.Evade.IsDown && m_animator.GetCurrentAnimatorStateInfo(0).IsName("Sprint"))) && !m_inputManager.Evade.IsMuted)
        {
            if (m_playerObjInfo.CanDoAction("Evade"))
            {
                m_animator.SetTrigger("EvadePressed");
                m_animator.SetBool("EvadeState", true);
            }
        }
        else if(!m_inputManager.Evade.ButtonState)
        {
            m_animator.SetBool("EvadeState", false);
        }
        */

        //Movement
        m_animator.SetFloat("MovementMagnitude", dir.magnitude);
        //m_animator.SetBool("MovementState", dir.magnitude > 0 && !m_inputManager.Movement.IsMuted);
        #endregion
        #endregion

        #region Handle Rotation //Should this be done in the physics area??
        //Consider using a force to make the rotation take time
        if (dir.magnitude > 0 /*&& (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || m_animator.GetCurrentAnimatorStateInfo(0).IsName("Sprint"))*/ && !m_inputManager.Movement.IsMuted)
        {
            //Set movement state
            m_animator.SetBool("MovementState", true);            

            //Find the target turn angle
            float targetAngle = dir.GetAngle();

            //Initialize turn speed
            float tempTurnSpeed = m_playerObjInfo.TurnSpeed;

            //Determine if turning left or right and apply the appropriate turn speed modifier
            //Use some maths to figure out if the direction is to the right or left of the current up direction (http://stackoverflow.com/questions/1560492/how-to-tell-whether-a-point-is-to-the-right-or-left-side-of-a-line)
            float tempLeftRight = ((dir.x - m_transform.up.x) * -m_transform.up.y) - ((dir.y - m_transform.up.y) * -m_transform.up.x);
            if (tempLeftRight < 0) //Right
            {
                tempTurnSpeed = m_playerObjInfo.TurnSpeed * m_playerObjInfo.TurnSpeedRightAnimMod;
            }
            else if (tempLeftRight > 0) //Left
            {
                tempTurnSpeed = m_playerObjInfo.TurnSpeed * m_playerObjInfo.TurnSpeedLeftAnimMod;
            }
            /*
            else //Center
            {
                Debug.Log("Center"); //TO DO: Should I handle this specially?
            }
            */

            m_transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), tempTurnSpeed * Time.deltaTime);

            
            //Set turning state
            Vector2 normalizedDir = dir.normalized;
            m_animator.SetBool("Turning", (m_transform.up.x > normalizedDir.x + .01 || m_transform.up.x < normalizedDir.x - .01 || m_transform.up.y > normalizedDir.y + .01 || m_transform.up.y < normalizedDir.y - .01));
        }
        else
        {
            //Set movement state
            m_animator.SetBool("MovementState", false);

            //Set turning state
            m_animator.SetBool("Turning", false);
        }
        #endregion
    }

    /// <summary>
    /// Physicsy Updates
    /// </summary>
    void FixedUpdate()
    {
        //TO DO: Use something other than currentanimator state info.. use some MovementState enumeration or boolean that animators can set instead so that I can have many different states
        //with similar movement properties
        if (m_inputManager.Movement.Direction.magnitude > 0)
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Sprint"))
            {
                m_rigidbody.AddForce(m_transform.up * m_playerObjInfo.SprintSpeed);
            }
            else if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                m_rigidbody.AddForce(m_inputManager.Movement.Direction.normalized * m_playerObjInfo.RunSpeed);
            }
        }
    }

    //***********************************************************************
    // Method: MutuallyExclusiveParamSet
    //
    // Purpose: Checks the passed animation parameter name to see if
    // any other action is already queued up which would block this action.
    // 
    // TO DO: Consider the possibility that instead of blocking actions,
    // it would be better to override sometimes?
    //***********************************************************************
    public bool MutuallyExclusiveParamSet(string pParamName)
    {
        switch (pParamName)
        {
            case "AttackBasicLightPressed":
                return m_animator.GetBool("AttackSpecialLightPressed") || m_animator.GetBool("AttackBasicHeavyPressed") || m_animator.GetBool("AttackSpecialHeavyPressed") || m_animator.GetBool("SpecialActionMajorPressed");
            case "AttackSpecialLightPressed":
                return m_animator.GetBool("AttackBasicLightPressed") || m_animator.GetBool("AttackBasicHeavyPressed") || m_animator.GetBool("AttackSpecialHeavyPressed") || m_animator.GetBool("SpecialActionMajorPressed");
            case "AttackBasicHeavyPressed":
                return m_animator.GetBool("AttackBasicLightPressed") || m_animator.GetBool("AttackSpecialLightPressed") || m_animator.GetBool("AttackSpecialHeavyPressed") || m_animator.GetBool("SpecialActionMajorPressed");
            case "AttackSpecialHeavyPressed":
                return m_animator.GetBool("AttackBasicLightPressed") || m_animator.GetBool("AttackSpecialLightPressed") || m_animator.GetBool("AttackBasicHeavyPressed") || m_animator.GetBool("SpecialActionMajorPressed");
            case "SpecialActionMajorPressed":
                return m_animator.GetBool("AttackBasicLightPressed") || m_animator.GetBool("AttackSpecialLightPressed") || m_animator.GetBool("AttackBasicHeavyPressed") || m_animator.GetBool("AttackSpecialHeavyPressed");
            case "SpecialActionMinorPressed":
                return m_animator.GetBool("AttackBasicLightPressed") || m_animator.GetBool("AttackSpecialLightPressed") || m_animator.GetBool("AttackBasicHeavyPressed") || m_animator.GetBool("AttackSpecialHeavyPressed");
        }

        return false;
    }

    //***********************************************************************
    // Method: MuteParams
    //
    // Purpose: Grants external access to the MuteParams function
    // (Mainly for the animation event list)
    //***********************************************************************
    public void MuteParams(string pNames)
    {
        Debug.Log("Mute: " + pNames);
        m_inputManager.InputMuter.MuteParams(pNames);
    }

    //***********************************************************************
    // Method: UnmuteParams
    //
    // Purpose: Grants external access to the UnmuteParams function
    // (Mainly for the animation event list)
    //***********************************************************************
    public void UnmuteParams(string pNames)
    {
        Debug.Log("Unmute: " + pNames);
        m_inputManager.InputMuter.UnmuteParams(pNames);
    }

    //***********************************************************************
    // Method: FullUnmuteParam
    //
    // Purpose: Grants external access to the FullUnmuteParam function
    // (Mainly for the animation event list)
    //***********************************************************************
    public void FullUnmuteParam(string pName)
    {
        m_inputManager.InputMuter.FullUnmuteParam(pName);
    }

    #region Properties
    public InputManager InputManager
    {
        get
        {
            return m_inputManager;
        }
    }
    #endregion
}