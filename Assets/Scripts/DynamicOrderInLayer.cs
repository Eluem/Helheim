//**************************************************************************************
// File: DynamicOrderInLayer.cs
//
// Purpose: This is used to dynamically set the Order In Layer for all sprites in the
// object based on their current x/y position. This is done to create the illusion that
// the object can move "behind" other objects in the scene. The order in layer value 
// will be based on a "camera angle" system to determine how much to change the order
// in layer based on the x/y values.
//
// i.e. The "camera angle" is tilted so that only the y axis should be considered...
// or it's tilted so the a specific diagonal axis should be considered... OR a more
// complex scenario could be like the one currently presented in the cliff map where
// the camera angle is sort of abstract and as you go away from the center of the x,
// you move up and down.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

//TO DO: Add struct/class to manage the sprites so that their default layer can be stored, if needed.. or other information can be managed per sprite renderer
//TO DO: Implement system to allow a good way to define an x/y:order in layer mapping system for more complex maps... possibly even supporting direct forced position ranges creating different orderings

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicOrderInLayer : MonoBehaviour
{
    #region Declarations
    //List<SpriteRenderer> m_spriteRenderers = new List<SpriteRenderer>(); //List of all sprite renderers for the object
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
		
	}

    /// <summary>
    /// Searches the object for all sprite renderers and adds them to the m_spriteRenderers list
    /// so that they can be managed
    /// </summary>
    void ObtainSpriteRenderers()
    {

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