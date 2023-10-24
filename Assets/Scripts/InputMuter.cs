//**************************************************************************************
// File: InputMuter.cs
//
// Purpose: This class will manage a list of animator parameters that are muted.
// You can add/remove paramaters from this list and you can check if paramters are in
// the list, if they are, you shouldn't edit them.
// (Should I make a function that actually handles editing the paramters and will
// simply ignore muted paramters.. or should I just use this for information and
// do the actual logic elsewhere?)
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputMuter
{
    #region Declarations
    private Dictionary<string, int> m_mutedParams;
    #endregion

    //***********************************************************************
    // Method: InputMuter
    //
    // Purpose: Constructor for class
    //***********************************************************************
    public InputMuter()
	{
        m_mutedParams = new Dictionary<string, int>();
	}

    //***********************************************************************
    // Method: MuteParam
    //
    // Purpose: Accepts a paramter name and adds it to the muted paramter
    // dictionary
    //***********************************************************************
    public void MuteParam(string pName)
    {
        if(m_mutedParams.ContainsKey(pName))
        {
            m_mutedParams[pName]++;
        }
        else
        {
            m_mutedParams.Add(pName, 1);
        }
    }

    //***********************************************************************
    // Method: UnmuteParam
    //
    // Purpose: Accepts a paramter name and removes an instance of muting
    //***********************************************************************
    public void UnmuteParam(string pName)
    {
        if (m_mutedParams.ContainsKey(pName) && m_mutedParams[pName] > 0)
        {
            m_mutedParams[pName]--;
        }
    }

    //***********************************************************************
    // Method: MuteParams
    //
    // Purpose: Accepts a comma separated string of parameter names and
    // adds each to the muted paramter dictionary
    //***********************************************************************
    public void MuteParams(string pNames)
    {
        string[] splitNames = pNames.Split(',');

        foreach(string name in splitNames)
        {
            MuteParam(name);
        }
    }

    //***********************************************************************
    // Method: UnmuteParams
    //
    // Purpose: Accepts a comma separated string of parameter names and
    // removes an instance of each from the muted paramter dictionary
    //***********************************************************************
    public void UnmuteParams(string pNames)
    {
        string[] splitNames = pNames.Split(',');

        foreach (string name in splitNames)
        {
            UnmuteParam(name);
        }
    }

    //***********************************************************************
    // Method: FullUnmuteParam
    //
    // Purpose: Accepts a paramter name and removes all instances of muting
    //***********************************************************************
    public void FullUnmuteParam(string pName)
    {
        if (m_mutedParams.ContainsKey(pName))
        {
            m_mutedParams[pName] = 0;
        }
    }

    //***********************************************************************
    // Method: IsParamMuted
    //
    // Purpose: Accepts a paramter name, returns whether or not it's muted
    //***********************************************************************
    public bool IsParamMuted(string pName)
    {
        if (m_mutedParams.ContainsKey(pName) && m_mutedParams[pName] > 0)
        {
            return true;
        }

        return false;
    }

    #region Properties
    #endregion
}
