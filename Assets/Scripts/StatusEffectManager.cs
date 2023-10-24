//**************************************************************************************
// File: StatusEffectManager.cs
//
// Purpose: This manages all the status effects for a DestructibleObjInfo
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatusEffectManager
{
    #region Declarations
    #region Pointers
    protected DestructibleObjInfo m_destructibleObjInfo;
    #endregion

    //TO DO: Look into object pooling the StatusEffects
    protected List<StatusEffect> m_statusEffects; //Stores a list of all the status effects currently being applied

    protected List<StatusEffect> m_statusEffectsToAdd; //Stores a list of status effects to add (declared here to prevent constantly remaking this list during update)
    protected List<StatusEffect> m_statusEffectsToRemove; //Stores a list of status effects to remove (declared here to prevent constantly remaking this list during update)
    #endregion

    //***********************************************************************
    // Method: StatusEffectManager
    //
    // Purpose: Constructor for class
    //***********************************************************************
    public StatusEffectManager(DestructibleObjInfo pDestructibleObjInfo)
    {
        m_destructibleObjInfo = pDestructibleObjInfo;

        m_statusEffects = new List<StatusEffect>();
        m_statusEffectsToAdd = new List<StatusEffect>();
        m_statusEffectsToRemove = new List<StatusEffect>();
    }

    //***********************************************************************
    // Method: Update
    //
    // Purpose: Called once per frame by the parent DestructibleObjInfo
    //***********************************************************************
    public void Update(float pDeltaTime)
    {
        //Add all delayed status effects that were added since the last update
        foreach(StatusEffect statusEffect in m_statusEffectsToAdd)
        {
            AddImmediate(statusEffect);
        }
        m_statusEffectsToAdd.Clear();


        //Update all status effects
        foreach (StatusEffect statusEffect in m_statusEffects)
        {
            statusEffect.Update(pDeltaTime);
        }

        //Add all delayed status effects that were added during this update
        foreach (StatusEffect statusEffect in m_statusEffectsToAdd)
        {
            AddImmediate(statusEffect);
        }
        m_statusEffectsToAdd.Clear();


        //Remove all completed status effects
        foreach (StatusEffect statusEffect in m_statusEffects)
        {
            if (statusEffect.Done)
            {
                m_statusEffectsToRemove.Add(statusEffect);
            }
        }
        foreach (StatusEffect statusEffect in m_statusEffectsToRemove)
        {
            Remove(statusEffect);
        }
        m_statusEffectsToRemove.Clear();
    }

    //***********************************************************************
    // Method: ForceTick
    //
    // Purpose: Forces the first instance of the status effect with the
    // passed StatusEffectType
    //***********************************************************************
    public void ForceTick(StatusEffectType pStatusEffectType)
    {
        StatusEffect doneEffect = null; //Used if this causes the effect to finish

        foreach (StatusEffect statusEffect in m_statusEffects)
        {
            if (statusEffect.EffectType == pStatusEffectType)
            {
                statusEffect.Tick();
                if (statusEffect.Done)
                {
                    doneEffect = statusEffect;
                }
                break;
            }
        }

        if (doneEffect != null)
        {
            Remove(doneEffect);
        }
    }

    //***********************************************************************
    // Method: AddImmediate
    //
    // Purpose: Adds a status effect to be managed, at the next update
    //***********************************************************************
    public virtual void Add(StatusEffect pStatusEffect)
    {
        if(pStatusEffect.CanStack)
        {
            m_statusEffectsToAdd.Add(pStatusEffect);
        }
        else
        {
            StatusEffect tempExistingStatusEffect = GetStatusEffect(pStatusEffect.EffectType);
            if (tempExistingStatusEffect == null)
            {
                m_statusEffectsToAdd.Add(pStatusEffect);
            }
            else
            {
                tempExistingStatusEffect.Merge(pStatusEffect);
            }
        }
    }

    //***********************************************************************
    // Method: AddImmediate
    //
    // Purpose: Adds a status effect to be managed, immediately
    //***********************************************************************
    public virtual void AddImmediate(StatusEffect pStatusEffect)
    {
        //If the status effect is designed to resolve immediately, do so
        if (pStatusEffect.TimePerTick < 0)
        {
            pStatusEffect.Tick();
        }

        //If the status effect is not considered done, add it to the list
        if (!pStatusEffect.Done)
        {
            //If it can stack, add it to the list
            if (pStatusEffect.CanStack)
            {
                m_statusEffects.Add(pStatusEffect);
            }
            //Otherwise, search for an existing instance of the effect
            else
            {
                bool found = false;
                foreach(StatusEffect statusEffect in m_statusEffects)
                {
                    //If an existing instance is found, merge them
                    if (statusEffect.EffectType == pStatusEffect.EffectType)
                    {
                        found = true;
                        statusEffect.Merge(pStatusEffect);
                        break;
                    }
                }
                //Otherwise, add it
                if (!found)
                {
                    m_statusEffects.Add(pStatusEffect);
                }
            }
        }
    }


    //***********************************************************************
    // Method: Remove
    //
    // Purpose: Removes a status effect (generally called by a StatusEffect
    // when it finishes its last tick)
    //***********************************************************************
    public void Remove(StatusEffect pStatusEffect)
    {
        m_statusEffects.Remove(pStatusEffect);
    }


    //TO DO: POSSIBLY CHANGE SYSTEM TO USE A DICTIONARY OF STATUSEFECTTYPES, MIGHT BE MORE EFFICIENT!
    /// <summary>
    /// Returns the first instance of the passed status effect type. If it's not found, returns null
    /// </summary>
    /// <param name="pStatusEffectType">Type of status effect to return</param>
    /// <returns></returns>
    public StatusEffect GetStatusEffect(StatusEffectType pStatusEffectType)
    {
        foreach (StatusEffect statusEffect in m_statusEffects)
        {
            if (statusEffect.EffectType == pStatusEffectType)
            {
                return statusEffect;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns all status effects that have the passed type
    /// </summary>
    /// <param name="pStatusEffectType">Type of status effect to return</param>
    /// <returns></returns>
    public List<StatusEffect> GetStatusEffectAll(StatusEffectType pStatusEffectType)
    {
        List<StatusEffect> tempStatusEffectList = new List<StatusEffect>();

        foreach(StatusEffect statusEffect in m_statusEffects)
        {
            if(statusEffect.EffectType == pStatusEffectType)
            {
                tempStatusEffectList.Add(statusEffect);
            }
        }

        return tempStatusEffectList;
    }

    #region Properties
    #endregion
}
