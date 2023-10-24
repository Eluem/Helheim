//**************************************************************************************
// File: ObjectFactory.cs
//
// Purpose: This class will be used to instantiate any game objects in order to
// decouple any instantiation/initialization logic from the rest of the game
//
// Note: For this implementation, I'm going to be using a MonoBehavior and exposing
// the factory to the Unity editor. In addition, all types of objects that can
// be instantiated through the object factory will be exposed so that I can actually
// attach the objects directly to the factory. This implementation (if I'm thinking
// this through correctly) should make it so I don't need to use the resources
// folder for anything (or at least instantiating prefabs). I'm not entirely sure
// if this will cause any issues with organization or having a lot of different types
// of objects yet, we'll see.
// 
// Every potential object type that this factory can instantiate will have it's own
// unique function. There won't be a generic function that you can call and pass
// a type as a string to instantiate it. If one is added, it'll be made as a
// wrapper and placed (using a case statement) around the functions that could also
// be called directly.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TO DO: Rename system for spawning projectiles to spawning.. idk just objects.. I'm using it for other stuff too (like spawning a lava splash).. maybe I shouldn't? Idk...
public enum SpawnableObjectType
{
    FireBall = 0,
    ForceBlast = 1,
    Shuriken = 2,
    WaterRipple = 3,
    UltraGreatswordEarthquake = 4,
    FireBallCharged = 5,
    CheatDeathForcePulse = 6
}

public class ObjectFactory : MonoBehaviour
{
    #region Declarations
    protected static ObjectFactory m_instance; //Used to allow static referencing of non-static values

    public GameObject m_playerPrefab;

    #region Hazards
    public GameObject m_fireBallPrefab;
    public GameObject m_fireBallExplosionPrefab;

    public GameObject m_fireBallChargedPrefab;
    public GameObject m_fireBallExplosionChargedPrefab;
    public GameObject m_fireBallFlamePoolChargedPrefab;

    public GameObject m_forceBlastPrefab;

    public GameObject m_cheatDeathForcePulsePrefab;

    public GameObject m_shurikenPrefab;

    public GameObject m_ultraGreatswordEarthquakePrefab;
    #endregion

    #region Effects
    public GameObject m_waterRipple;
    public GameObject m_playerTrack;
    #endregion

    protected Dictionary<string, Stack<ObjInfo>> m_objPool = new Dictionary<string, Stack<ObjInfo>>(); //This holds all pooled objects
    #endregion

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
        m_instance = this;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
    }

    /// <summary>
    /// Creates a player and returns the PlayerObjInfo component
    /// </summary>
    /// <param name="pPlayerInfo"></param>
    /// <param name="pPlayerName"></param>
    /// <param name="pInputManager"></param>
    /// <param name="pSkin"></param>
    /// <param name="pLoadout"></param>
    /// <param name="pSpawnPoint"></param>
    /// <returns></returns>
    public static PlayerObjInfo CreatePlayer(PlayerInfo pPlayerInfo, InputManager pInputManager, Dictionary<string, Sprite> pSkin, string pLoadout, Vector3 pSpawnPoint)
    {
        GameObject tempObj = (GameObject)Instantiate(m_instance.m_playerPrefab, pSpawnPoint, Quaternion.identity);

        PlayerController tempPlayerController = tempObj.GetComponent<PlayerController>();
        tempPlayerController.Initialize(pInputManager);

        PlayerObjInfo tempPlayerObjInfo = tempObj.GetComponent<PlayerObjInfo>();
        tempPlayerObjInfo.Initialize(pPlayerInfo, pSkin, pLoadout);

        tempPlayerObjInfo.Spawn(pPlayerInfo.Team);

        return tempPlayerObjInfo;
    }

    /// <summary>
    /// Spawns an object based on the SpawnableObjectType passed
    ///
    /// Note: Mostly meant to be used in animator?
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    /// <param name="pSpawnPoint"></param>
    /// <param name="pDirection"></param>
    /// <param name="pSpawnableObjectType"></param>
    public static void SpawnObject(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo, Vector3 pSpawnPoint, Vector3 pDirection, SpawnableObjectType pSpawnableObjectType)
    {
        switch (pSpawnableObjectType)
        {
            case SpawnableObjectType.FireBall:
                CreateFireBall(pOriginObjInfo, pSourceObjInfo, pSpawnPoint, pDirection);
                break;
            case SpawnableObjectType.FireBallCharged:
                CreateFireBallCharged(pOriginObjInfo, pSourceObjInfo, pSpawnPoint, pDirection);
                break;
            case SpawnableObjectType.ForceBlast:
                CreateForceBlast(pOriginObjInfo, pSourceObjInfo, pSpawnPoint);
                break;
            case SpawnableObjectType.Shuriken:
                CreateShuriken(pOriginObjInfo, pSourceObjInfo, pSpawnPoint, pDirection);
                break;
            case SpawnableObjectType.WaterRipple:
                CreateWaterRipple(pSpawnPoint);
                break;
            case SpawnableObjectType.UltraGreatswordEarthquake:
                CreateUltraGreatswordEarthquakeObjInfo(pOriginObjInfo, pSourceObjInfo, pSpawnPoint, pDirection);
                break;
            case SpawnableObjectType.CheatDeathForcePulse:
                CreateCheatDeathForcePulse(pOriginObjInfo, pSourceObjInfo, pSpawnPoint);
                break;
        }
    }

    /// <summary>
    /// Creates a fireBall and returns the FireBallObjInfo component
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    /// <param name="pSpawnPoint"></param>
    /// <param name="pDirection"></param>
    /// <returns></returns>
    public static FireBallObjInfo CreateFireBall(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo, Vector3 pSpawnPoint, Vector3 pDirection)
    {
        //TO DO: remove all of these comments below this one where object pooling isn't actually implemented....
        //If none were found in the object pool, create one and initialize it
        GameObject tempObj = (GameObject)Instantiate(m_instance.m_fireBallPrefab, pSpawnPoint, Quaternion.identity);

        FireBallObjInfo tempObjInfo = (FireBallObjInfo)tempObj.GetComponent(typeof(FireBallObjInfo));

        tempObjInfo.Initialize();
        tempObjInfo.Spawn(pOriginObjInfo, pSourceObjInfo, pDirection);

        return tempObjInfo;
    }

    /// <summary>
    /// Creates a fireBallExplosion and returns the FireBallExoplosionObjInfo component
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    /// <param name="pSpawnPoint"></param>
    /// <returns></returns>
    public static FireBallExplosionObjInfo CreateFireBallExplosion(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo, Vector3 pSpawnPoint)
    {
        //If none were found in the object pool, create one and initialize it
        GameObject tempObj = (GameObject)Instantiate(m_instance.m_fireBallExplosionPrefab, pSpawnPoint, Quaternion.identity);

        FireBallExplosionObjInfo tempObjInfo = (FireBallExplosionObjInfo)tempObj.GetComponent(typeof(FireBallExplosionObjInfo));

        tempObjInfo.Initialize();
        tempObjInfo.Spawn(pOriginObjInfo, pSourceObjInfo);

        return tempObjInfo;
    }

    /// <summary>
    /// Creates a fireBallCharged and returns the FireBallChargedObjInfo component
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    /// <param name="pSpawnPoint"></param>
    /// <param name="pDirection"></param>
    /// <returns></returns>
    public static FireBallChargedObjInfo CreateFireBallCharged(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo, Vector3 pSpawnPoint, Vector3 pDirection)
    {
        //TO DO: remove all of these comments below this one where object pooling isn't actually implemented....
        //If none were found in the object pool, create one and initialize it
        GameObject tempObj = (GameObject)Instantiate(m_instance.m_fireBallChargedPrefab, pSpawnPoint, Quaternion.identity);

        FireBallChargedObjInfo tempObjInfo = (FireBallChargedObjInfo)tempObj.GetComponent(typeof(FireBallChargedObjInfo));

        tempObjInfo.Initialize();
        tempObjInfo.Spawn(pOriginObjInfo, pSourceObjInfo, pDirection);

        return tempObjInfo;
    }

    /// <summary>
    /// Creates a fireBallExplosionCharged and returns the FireBallExplosionChargedObjInfo component
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    /// <param name="pSpawnPoint"></param>
    /// <returns></returns>
    public static FireBallExplosionChargedObjInfo CreateFireBallExplosionCharged(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo, Vector3 pSpawnPoint)
    {
        //If none were found in the object pool, create one and initialize it
        GameObject tempObj = (GameObject)Instantiate(m_instance.m_fireBallExplosionChargedPrefab, pSpawnPoint, Quaternion.identity);

        FireBallExplosionChargedObjInfo tempObjInfo = (FireBallExplosionChargedObjInfo)tempObj.GetComponent(typeof(FireBallExplosionChargedObjInfo));

        tempObjInfo.Initialize();
        tempObjInfo.Spawn(pOriginObjInfo, pSourceObjInfo);

        return tempObjInfo;
    }

    /// <summary>
    /// Creates a fireBallFlamePoolCharged and returns the FireBallFlamePoolChargedObjInfo component
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    /// <param name="pSpawnPoint"></param>
    /// <returns></returns>
    public static FireBallFlamePoolChargedObjInfo CreateFireBallFlamePoolCharged(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo, Vector3 pSpawnPoint)
    {
        //If none were found in the object pool, create one and initialize it
        GameObject tempObj = (GameObject)Instantiate(m_instance.m_fireBallFlamePoolChargedPrefab, pSpawnPoint, Quaternion.identity);

        FireBallFlamePoolChargedObjInfo tempObjInfo = (FireBallFlamePoolChargedObjInfo)tempObj.GetComponent(typeof(FireBallFlamePoolChargedObjInfo));

        tempObjInfo.Initialize();
        tempObjInfo.Spawn(pOriginObjInfo, pSourceObjInfo);

        return tempObjInfo;
    }

    /// <summary>
    /// Creates a forceBlast and returns the ForceBlastObjInfo component
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    /// <param name="pSpawnPoint"></param>
    /// <returns></returns>
    public static ForceBlastObjInfo CreateForceBlast(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo, Vector3 pSpawnPoint)
    {
        //If none were found in the object pool, create one and initialize it
        GameObject tempObj = (GameObject)Instantiate(m_instance.m_forceBlastPrefab, pSpawnPoint, Quaternion.identity);

        ForceBlastObjInfo tempObjInfo = (ForceBlastObjInfo)tempObj.GetComponent(typeof(ForceBlastObjInfo));

        tempObjInfo.Initialize();
        tempObjInfo.Spawn(pOriginObjInfo, pSourceObjInfo);

        return tempObjInfo;
    }

    /// <summary>
    /// Creates a cheatDeathForcePulse and returns the ForceBlastObjInfo component
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    /// <param name="pSpawnPoint"></param>
    /// <returns></returns>
    public static ForceBlastObjInfo CreateCheatDeathForcePulse(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo, Vector3 pSpawnPoint)
    {
        //If none were found in the object pool, create one and initialize it
        GameObject tempObj = (GameObject)Instantiate(m_instance.m_cheatDeathForcePulsePrefab, pSpawnPoint, Quaternion.identity);

        ForceBlastObjInfo tempObjInfo = (ForceBlastObjInfo)tempObj.GetComponent(typeof(ForceBlastObjInfo));

        tempObjInfo.Initialize();
        tempObjInfo.Spawn(pOriginObjInfo, pSourceObjInfo);

        return tempObjInfo;
    }

    /// <summary>
    /// Creates a shuriken and returns the ShurikenObjInfo component
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    /// <param name="pSpawnPoint"></param>
    /// <param name="pDirection"></param>
    /// <returns></returns>
    public static ShurikenObjInfo CreateShuriken(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo, Vector3 pSpawnPoint, Vector3 pDirection)
    {
        //If none were found in the object pool, create one and initialize it
        GameObject tempObj = (GameObject)Instantiate(m_instance.m_shurikenPrefab, pSpawnPoint, Quaternion.identity);

        ShurikenObjInfo tempObjInfo = (ShurikenObjInfo)tempObj.GetComponent(typeof(ShurikenObjInfo));

        tempObjInfo.Initialize();
        tempObjInfo.Spawn(pOriginObjInfo, pSourceObjInfo, ((Vector2)pDirection).Rotate(Random.Range(-1.0f, 1.0f)));

        return tempObjInfo;
    }

    /// <summary>
    /// Creates a fireBall and returns the FireBallObjInfo component
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    /// <param name="pSpawnPoint"></param>
    /// <param name="pDirection"></param>
    /// <returns></returns>
    public static UltraGreatswordEarthquakeObjInfo CreateUltraGreatswordEarthquakeObjInfo(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo, Vector3 pSpawnPoint, Vector3 pDirection)
    {
        //If none were found in the object pool, create one and initialize it
        GameObject tempObj = (GameObject)Instantiate(m_instance.m_ultraGreatswordEarthquakePrefab, pSpawnPoint, Quaternion.identity);

        UltraGreatswordEarthquakeObjInfo tempObjInfo = (UltraGreatswordEarthquakeObjInfo)tempObj.GetComponent(typeof(UltraGreatswordEarthquakeObjInfo));

        tempObjInfo.Initialize();
        tempObjInfo.Spawn(pOriginObjInfo, pSourceObjInfo, pDirection);

        return tempObjInfo;
    }

    //Object pooled versions of Fireball, FireBallExplosion, ForceBlast, and Shuriken
    /*
    /// <summary>
    /// Creates a fireBall and returns the FireBallObjInfo component
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    /// <param name="pSpawnPoint"></param>
    /// <param name="pDirection"></param>
    /// <returns></returns>
    public static FireBallObjInfo CreateFireBall(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo, Vector3 pSpawnPoint, Vector3 pDirection)
    {
        //If an object exists in the pool for this type, get it
        FireBallObjInfo tempObjInfo = (FireBallObjInfo)GetObjInfo("FireBallObjInfo");

        if (tempObjInfo == null)
        {
            //If none were found in the object pool, create one and initialize it
            GameObject tempObj = (GameObject)Instantiate(m_instance.m_fireBallPrefab, pSpawnPoint, Quaternion.identity);

            tempObjInfo = (FireBallObjInfo)tempObj.GetComponent(typeof(FireBallObjInfo));
            tempObjInfo.Initialize();
        }
        else
        {
            tempObjInfo.Transform.position = pSpawnPoint;
        }

        tempObjInfo.Spawn(pOriginObjInfo, pSourceObjInfo, pDirection);
        return tempObjInfo;
    }

    /// <summary>
    /// Creates a fireBallExplosion and returns the FireBallExoplosionObjInfo component
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    /// <param name="pSpawnPoint"></param>
    /// <returns></returns>
    public static FireBallExplosionObjInfo CreateFireBallExplosion(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo, Vector3 pSpawnPoint)
    {
        //If an object exists in the pool for this type, get it
        FireBallExplosionObjInfo tempObjInfo = (FireBallExplosionObjInfo)GetObjInfo("FireBallExplosionObjInfo");

        if (tempObjInfo == null)
        {
            //If none were found in the object pool, create one and initialize it
            GameObject tempObj = (GameObject)Instantiate(m_instance.m_fireBallExplosionPrefab, pSpawnPoint, Quaternion.identity);

            tempObjInfo = (FireBallExplosionObjInfo)tempObj.GetComponent(typeof(FireBallExplosionObjInfo));
            tempObjInfo.Initialize();
        }
        else
        {
            tempObjInfo.Transform.position = pSpawnPoint;
        }

        tempObjInfo.Spawn(pOriginObjInfo, pSourceObjInfo);
        return tempObjInfo;
    }

    /// <summary>
    /// Creates a forceBlast and returns the ForceBlastObjInfo component
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    /// <param name="pSpawnPoint"></param>
    /// <returns></returns>
    public static ForceBlastObjInfo CreateForceBlast(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo, Vector3 pSpawnPoint)
    {
        //If an object exists in the pool for this type, get it
        ForceBlastObjInfo tempObjInfo = (ForceBlastObjInfo)GetObjInfo("ForceBlastObjInfo");

        if (tempObjInfo == null)
        {
            //If none were found in the object pool, create one and initialize it
            GameObject tempObj = (GameObject)Instantiate(m_instance.m_forceBlastPrefab, pSpawnPoint, Quaternion.identity);

            tempObjInfo = (ForceBlastObjInfo)tempObj.GetComponent(typeof(ForceBlastObjInfo));
            tempObjInfo.Initialize();
        }
        else
        {
            tempObjInfo.Transform.position = pSpawnPoint;
        }

        tempObjInfo.Spawn(pOriginObjInfo, pSourceObjInfo);
        return tempObjInfo;
    }

    /// <summary>
    /// Creates a shuriken and returns the ShurikenObjInfo component
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    /// <param name="pSpawnPoint"></param>
    /// <param name="pDirection"></param>
    /// <returns></returns>
    public static ProjectileObjInfo CreateShuriken(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo, Vector3 pSpawnPoint, Vector3 pDirection)
    {
        //If an object exists in the pool for this type, get it
        ShurikenObjInfo tempObjInfo = (ShurikenObjInfo)GetObjInfo("ShurikenObjInfo");

        if (tempObjInfo == null)
        {
            //If none were found in the object pool, create one and initialize it
            GameObject tempObj = (GameObject)Instantiate(m_instance.m_shurikenPrefab, pSpawnPoint, Quaternion.identity);

            tempObjInfo = (ShurikenObjInfo)tempObj.GetComponent(typeof(ShurikenObjInfo));
            tempObjInfo.Initialize();
        }
        else
        {
            tempObjInfo.Transform.position = pSpawnPoint;
        }

        tempObjInfo.Spawn(pOriginObjInfo, pSourceObjInfo, ((Vector2)pDirection).Rotate(Random.Range(-1.0f, 1.0f)));
        return tempObjInfo;
    }
    */

    /// <summary>
    /// Creates a waterRipple and returns it's ObjInfo
    /// </summary>
    /// <param name="pSpawnPoint"></param>
    /// <returns></returns>
    public static ObjInfo CreateWaterRipple(Vector3 pSpawnPoint)
    {
        //If an object exists in the pool for this type, get it
        EffectObjInfo tempObjInfo = (EffectObjInfo)GetObjInfo("WaterRippleObjInfo");

        if (tempObjInfo == null)
        {
            //If none were found in the object pool, create one and initialize it
            GameObject tempObj = (GameObject)Instantiate(m_instance.m_waterRipple, pSpawnPoint, Quaternion.identity);

            tempObjInfo = (EffectObjInfo)tempObj.GetComponent(typeof(EffectObjInfo));

            tempObjInfo.Initialize("WaterRippleObjInfo");
        }
        else
        {
            tempObjInfo.Transform.position = new Vector3(pSpawnPoint.x, pSpawnPoint.y, tempObjInfo.transform.position.z);
        }

        tempObjInfo.Spawn();
        return tempObjInfo;
    }

    /// <summary>
    /// Creates a playerTrack and returns it's ObjInfo
    /// </summary>
    /// <param name="pSpawnPoint"></param>
    /// <returns></returns>
    public static ObjInfo CreatePlayerTrack(Sprite pTrackSprite, Transform pFootTransform)
    {
        //If there's no solid ground, the create fails and it returns null
        if(!CheckCircleOverlapSolidGround(pFootTransform.position, 0.2f))
        {
            return null;
        }


        //If an object exists in the pool for this type, get it
        EffectObjInfo tempObjInfo = (EffectObjInfo)GetObjInfo("PlayerTrackObjInfo");

        Vector3 tempSpawnPoint = pFootTransform.position;
        tempSpawnPoint.z++; //Put the track down one layer from the foot's current position
        Quaternion tempRotation = pFootTransform.rotation;

        if (tempObjInfo == null)
        {
            //If none were found in the object pool, create one and initialize it
            GameObject tempObj = (GameObject)Instantiate(m_instance.m_playerTrack, tempSpawnPoint, tempRotation);
            tempObjInfo = (EffectObjInfo)tempObj.GetComponent(typeof(EffectObjInfo));

            tempObjInfo.Initialize("PlayerTrackObjInfo");
        }
        else
        {
            tempObjInfo.Transform.position = tempSpawnPoint;
            tempObjInfo.transform.rotation = tempRotation;
        }

        tempObjInfo.SpriteRenderer.sprite = pTrackSprite;
        tempObjInfo.Spawn();
        return tempObjInfo;
    }

    /// <summary>
    /// Pops an object from the passed pool key from the stack. If none existed, returns null
    /// </summary>
    /// <param name="pKey">The key for the objInfo type. (ObjInfo.ObjType)</param>
    /// <returns></returns>
    protected static ObjInfo GetObjInfo(string pKey)
    {
        if (m_instance.m_objPool.ContainsKey(pKey) && m_instance.m_objPool[pKey].Count > 0)
        {
            return m_instance.m_objPool[pKey].Pop();
        }

        return null;
    }

    /// <summary>
    /// Pushes the passed ObjInfo back into the correct ObjInfo pool
    /// </summary>
    /// <param name="pObjInfo">ObjInfo to put into its pool</param>
    public static void PushObjInfoIntoPool(ObjInfo pObjInfo)
    {
        if (!m_instance.m_objPool.ContainsKey(pObjInfo.ObjType))
        {
            //If the pool for the specific object hasn't been created yet, create it
            m_instance.m_objPool.Add(pObjInfo.ObjType, new Stack<ObjInfo>());
        }

        m_instance.m_objPool[pObjInfo.ObjType].Push(pObjInfo);
    }

    public static bool CheckCircleOverlapSolidGround(Vector2 pPoint, float pRadius)
    {
        //Before creating the track, make sure the surface is valid.
        Collider2D[] tempColliders = Physics2D.OverlapCircleAll(pPoint, pRadius, ObjInfo.PITFLOOR_LAYERMASK);

        ObjInfo tempObjInfo;

        bool solidGround = true;
        for (int i = 0; i < tempColliders.Length; i++)
        {
            tempObjInfo = ObjInfo.GetObjInfoFromCollider(tempColliders[i]);
            if (tempObjInfo.IsType("TerrainObjInfo"))
            {
                if (tempObjInfo.IsType("WaterObjInfo"))
                {
                    solidGround = false;
                }
                else
                {
                    TerrainType tempTerrainType = ((TerrainObjInfo)tempObjInfo).m_terrainType;
                    if (tempTerrainType == TerrainType.Floor)
                    {
                        solidGround = true;
                        break;
                    }
                    else if (tempTerrainType == TerrainType.Pit)
                    {
                        solidGround = false;
                    }
                }
            }
        }

        return solidGround;
    }

    /// <summary>
    /// Test function that prints out all the object pool data
    /// </summary>
    public void PrintPoolData()
    {
        string data = "";

        foreach (string key in m_objPool.Keys)
        {
            data += key + ": " + m_objPool[key].Count + "\n";
        }

        Debug.Log(data);
    }

    #region Properties
    #endregion
}