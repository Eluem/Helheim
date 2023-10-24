//**************************************************************************************
// File: PlayerObjInfo.cs
//
// Purpose: This class handles all information and interactions for player objects
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;
using Com.LuisPedroFonseca.ProCamera2D;

[AdvancedInspector]
public class PlayerObjInfo : CharObjInfo
{
    #region Declarations
    [Inspect]
    public string m_inspectorSkinName = "CharacterSheet_Male_BrownHair";


    #region Pointers
    protected PlayerInfo m_playerInfo; //Pointer to the PlayerInfo object that spawned this player object
    #endregion
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    /// <param name="pPlayerInfo"></param>
    /// <param name="pSkin"></param>
    /// <param name="pLoadout"></param>
    /// <param name="pCharName"></param>
    public void Initialize(PlayerInfo pPlayerInfo, Dictionary<string, Sprite> pSkin, string pLoadout)
    {
        base.Initialize(pPlayerInfo.PlayerName, pLoadout, "PlayerObjInfo");

        m_playerInfo = pPlayerInfo;
        SetSkin(pSkin);
    }

    /// <summary>
    /// This is the code that runs each time this object is spawned
    /// </summary>
    public override void Spawn(int pTeam)
    {
        base.Spawn(pTeam);

        //TO DO: Only do this if the player is local! Otherwise, characters only gain focus under specific conditions!
        ProCamera2D.Instance.AddCameraTarget(m_transform);
    }

    /// <summary>
    /// Destroys this object (or pretends to, if it's pooled)
    /// </summary>
    public override void DestroyMe()
    {
        base.DestroyMe();

        ProCamera2D.Instance.RemoveCameraTarget(m_transform);
    }

    //***********************************************************************
    // Method: IsType
    //
    // Purpose: Accepts an object type and returns true if one of the types
    // that it is matches the type passed.
    // i.e. PlayerObjInfo is also a CharObjInfo and a DestructibleObjInfo
    //***********************************************************************
    public override bool IsType(string pObjType)
    {
        if (pObjType == "ActorObjInfo" || pObjType == "DestructibleObjInfo" || pObjType == "CharObjInfo" || pObjType == "PlayerObjInfo" || pObjType == ObjType)
        {
            return true;
        }

        return false;
    }

    //***********************************************************************
    // Method: Start
    //
    // Purpose: Use this for initialization
    //***********************************************************************
    protected override void Start()
    {
        if (!m_initialized)
        {
            base.Initialize("FathErr", "0,0,0,0,0", "PlayerObjInfo");
        }
    }

    //***********************************************************************
    // Method: Update
    //
    // Purpose: Update is called once per frame
    //***********************************************************************
    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// This is used so that OnDeath can trigger an animation which
    /// can trigger OnDeathFinalAnim which will call OnDeathFinal
    /// </summary>
    protected override void OnDeathFinal()
    {
        m_playerInfo.OnDeath();
        base.OnDeathFinal();
    }

    //***********************************************************************
    // Method: SetSkin
    //
    // Purpose: Accepts a skin as a dictionary of sprite names and sprites
    // and applies all the parts to the player
    //***********************************************************************
    public void SetSkin(Dictionary<string, Sprite> pSkin)
    {
        Component[] spriteRenderers;
        spriteRenderers = GetComponentsInChildren(typeof(SpriteRenderer), true);

        foreach(SpriteRenderer sr in spriteRenderers)
        {
            if (pSkin.ContainsKey(sr.gameObject.name))
            {
                sr.sprite = pSkin[sr.gameObject.name];
            }
        }

        m_footLeftTrack = pSkin["FootLeftPrint"];
        m_footRightTrack = pSkin["FootRightPrint"];
    }

#if UNITY_EDITOR
    /// <summary>
    /// Used to set the skin automatically through the inspector
    /// </summary>
    [Inspect]
    public void Inspector_SetSkin()
    {
        string tempSkinName = "Final";

        Dictionary<string, Sprite> tempSkin = new Dictionary<string, Sprite>();

        Sprite[] tempSprites = Resources.LoadAll<Sprite>("Art/Characters/CharacterSheet_" + tempSkinName);

        foreach (Sprite s in tempSprites)
        {
            tempSkin.Add(s.name, s);
        }

        SetSkin(tempSkin);

        //Component[] spriteRenderers;
        //spriteRenderers = GetComponentsInChildren(typeof(SpriteRenderer), true);

        //foreach (SpriteRenderer sr in spriteRenderers)
        //{
        //    if (tempSkin.ContainsKey(sr.gameObject.name))
        //    {
        //        sr.sprite = tempSkin[sr.gameObject.name];
        //    }
        //}
    }

    /// <summary>
    /// This is a temporary test function which should force the player to "roll" forward
    /// </summary>
    [Inspect]
    public void Inspector_TestDash()
    {
        if (EquippedWeaponInfo.WeaponStance == Stance.DualDagger)
        {
            m_animator.CrossFade(-1263748749, 0);
        }

        if (EquippedWeaponInfo.WeaponStance == Stance.UltraGreatsword)
        {
            m_animator.CrossFade(-1127813352, 0);
        }
    }

    [Inspect]
    public void Inspector_SetSkin_Dynamic()
    {
        string tempSkinName = m_inspectorSkinName;

        Dictionary<string, Sprite> tempSkin = new Dictionary<string, Sprite>();

        Sprite[] tempSprites = Resources.LoadAll<Sprite>("Art/Characters/CharacterSheet_" + tempSkinName);

        foreach (Sprite s in tempSprites)
        {
            tempSkin.Add(s.name, s);
        }

        SetSkin(tempSkin);

        //Component[] spriteRenderers;
        //spriteRenderers = GetComponentsInChildren(typeof(SpriteRenderer), true);

        //foreach (SpriteRenderer sr in spriteRenderers)
        //{
        //    if (tempSkin.ContainsKey(sr.gameObject.name))
        //    {
        //        sr.sprite = tempSkin[sr.gameObject.name];
        //    }
        //}
    }

    [Inspect]
    public void Inspector_Launch()
    {
        SufferDamagePacket(new DamagePacketStatusEffect(this, this, this, new DamageContainer(0, 0, 0, 0, 0, 0, 0, 20, 3), 1, 0, AudioClipEnum.None, ParticleEffectEnum.None, KnockbackType.ExternalDir, KnockbackType.ExternalDir, Transform.up));
    }

    [Inspect]
    public void Inspector_KnockDown()
    {
        SufferDamagePacket(new DamagePacketStatusEffect(this, this, this, new DamageContainer(0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 10), 1, 0, AudioClipEnum.None, ParticleEffectEnum.None, KnockbackType.ExternalDir, KnockbackType.ExternalDir, Transform.up));
    }

    [Inspect]
    public void Inspector_Stagger()
    {
        SufferDamagePacket(new DamagePacketStatusEffect(this, this, this, new DamageContainer(0, 1000, 0, 0, 0, 0, 0, 0, 0), 1, 0, AudioClipEnum.None, ParticleEffectEnum.None, KnockbackType.ExternalDir, KnockbackType.ExternalDir, Transform.up));
    }

    [Inspect]
    public void Inspector_LaunchAndStagger()
    {
        SufferDamagePacket(new DamagePacketStatusEffect(this, this, this, new DamageContainer(0, 0, 0, 0, 0, 0, 0, 20, 3), 1, 0, AudioClipEnum.None, ParticleEffectEnum.None, KnockbackType.ExternalDir, KnockbackType.ExternalDir, Transform.up));
        SufferDamagePacket(new DamagePacketStatusEffect(this, this, this, new DamageContainer(0, 1000, 0, 0, 0, 0, 0, 0, 0), 1, 0, AudioClipEnum.None, ParticleEffectEnum.None, KnockbackType.ExternalDir, KnockbackType.ExternalDir, Transform.up));
    }

    [Inspect]
    public void Inspector_Teleport()
    {
        Transform.position = Transform.position + (Transform.up * 5);
    }

    [Inspect]
    public void Inspector_ForceAttackBasicLight()
    {
        m_playerController.InputManager.AttackBasicLight.ForcePress(.2f);
    }

    [Inspect]
    public void Inspector_PumpStats()
    {
        HealthFloat = 10000;
        m_maxHealth = 10000;

        m_mana = 10000;
        m_maxMana = 10000;
    }
#endif

    #region Properties
    #endregion
}
