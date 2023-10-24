//**************************************************************************************
// File: LavaEffectShapePulse.cs
//
// Purpose: Make a layered splotch of "lava" pulse it's alpha to make a nice effect
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class LavaEffectShapePulse : MonoBehaviour
{
    #region Declarations
    public Animator m_animator;

    public float PulseSpeed = 1;
    #endregion
	
	//***********************************************************************
    // Method: Initialize
    //
    // Purpose: This basically takes the place of a constructor
    //***********************************************************************
    public void Initialize()
    {
		
	}

	//***********************************************************************
    // Method: Start
    //
    // Purpose: Use this for initialization
    //***********************************************************************
	void Start()
	{
        m_animator.SetFloat("PulseSpeed", PulseSpeed);
    }
	
	//***********************************************************************
    // Method: Update
    //
    // Purpose: Update is called once per frame
    //***********************************************************************
	void Update()
	{
        m_animator.SetFloat("PulseSpeed", PulseSpeed); //TO DO: remove me for release, no reason to allow this to be changed on the fly
    }
	
	#region Properties
    #endregion
}