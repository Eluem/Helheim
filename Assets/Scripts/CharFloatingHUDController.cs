//**************************************************************************************
// File: CharFloatingHUDController.cs
//
// Purpose: This controls the HUD that will float over a character
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using AdvancedInspector;

[AdvancedInspector]
public class CharFloatingHUDController : FloatingHUDController
{
    #region Declarations
    #region Pointers
    [Inspect(1), Group("Pointers")]
    public Image m_staminaBarSlider;
    [Inspect(1), Group("Pointers")]
    public Image m_manaBarSlider;
    [Inspect(1), Group("Pointers")]
    public GameObject m_subBarBackground1; //Pointer to the first sub bar so that it can be toggled
    [Inspect(1), Group("Pointers")]
    public GameObject m_subBarBackground2; //Pointer to the second sub bar so that it can be toggled
    [Inspect(1), Group("Pointers")]
    public GameObject m_subBarBackgroundLarge; //Pointer to the large sub bar so that it can be toggled
    #endregion

    #region Sub Bar
    #region Sub Bar Config
    [Inspect, Group("Sub Bar Config", 1)]
    public Sprite m_subBarSocketSprite;
    [Inspect, Group("Sub Bar Config")]
    public Sprite m_subBarFillSprite;
    [Space]
    [Inspect, Group("Sub Bar Config")]
    public Vector2 m_subBar1Center;
    [Inspect, Group("Sub Bar Config")]
    public Vector2 m_subBar1Size;
    [Space]
    [Inspect, Group("Sub Bar Config")]
    public Vector2 m_subBar2Center;
    [Inspect, Group("Sub Bar Config")]
    public Vector2 m_subBar2Size;
    [Space]
    [Inspect, Group("Sub Bar Config")]
    public Vector2 m_subBarLargeCenter;
    [Inspect, Group("Sub Bar Config")]
    public Vector2 m_subBarLargeSize;
    #endregion

    protected SubBar m_subBar1;
    protected SubBar m_subBar2;
    protected SubBar m_subBarLarge;
    #endregion
    #endregion

    //***********************************************************************
    // Method: Initialize
    //
    // Purpose: This basically takes the place of a constructor
    //***********************************************************************
    public void Initialize(DestructibleObjInfo pOwnerObjInfo, float pZAxisOrder, Vector2 pOffset)
    {
        base.Initialize(pOwnerObjInfo, pZAxisOrder, pOffset, "CharFloatingHUDController");
        //InitializeSubBars(); This can't be done here... the main initialize function is called too early
    }

    //***********************************************************************
    // Method: Start
    //
    // Purpose: Use this for initialization
    //***********************************************************************
    protected override void Start()
    {
        base.Start();
    }

    //***********************************************************************
    // Method: Update
    //
    // Purpose: Update is called once per frame
    //***********************************************************************
    protected override void Update()
    {
        base.Update();
        UpdateStaminaBar();
        UpdateManaBar();
        UpdateSubBars();
    }

    //***********************************************************************
    // Method: UpdateStaminaBar
    //
    // Purpose: Updates the stamina bar
    //***********************************************************************
    protected void UpdateStaminaBar()
    {
        m_staminaBarSlider.fillAmount = ((CharObjInfo)m_ownerObjInfo).StaminaFloat / ((CharObjInfo)m_ownerObjInfo).MaxStamina;
    }

    //***********************************************************************
    // Method: UpdateManaBar
    //
    // Purpose: Updates the mana bar
    //***********************************************************************
    protected void UpdateManaBar()
    {
        m_manaBarSlider.fillAmount = ((CharObjInfo)m_ownerObjInfo).ManaFloat / ((CharObjInfo)m_ownerObjInfo).MaxMana;
    }

    /// <summary>
    /// Updates the sub bars that are enabled
    /// </summary>
    protected void UpdateSubBars()
    {
        if(m_subBar1 != null && m_subBar1.Enabled)
        {
            m_subBar1.Update();
        }

        if (m_subBar2 != null && m_subBar2.Enabled)
        {
            m_subBar2.Update();
        }

        if (m_subBarLarge != null && m_subBarLarge.Enabled)
        {
            m_subBarLarge.Update();
        }
    }

    //***********************************************************************
    // Method: InitializeSubBars
    //
    // Purpose: Configures the values for the sub bars based on the
    // character's loadout
    //***********************************************************************
    public void InitializeSubBars()
    {
        //Clear out the sub bars
        if (m_subBar1 != null)
        {
            m_subBar1.Clear();
            m_subBar1.Enabled = false;
        }
        m_subBar1 = null;

        if (m_subBar2 != null)
        {
            m_subBar2.Clear();
            m_subBar2.Enabled = false;
        }
        m_subBar2 = null;

        if (m_subBarLarge != null)
        {
            m_subBarLarge.Clear();
            m_subBarLarge.Enabled = false;
        }
        m_subBarLarge = null;

        CharObjInfo tempCharObjInfo = (CharObjInfo)m_ownerObjInfo;

        if (tempCharObjInfo.SpecialAbilityInfo1.GUIDisplayMode != SpecialAbilityInfo.DisplayMode.None)
        {
            m_subBar1 = new SubBar(m_canvas, m_subBarBackground1, m_subBarSocketSprite, m_subBarFillSprite, m_subBar1Center, m_subBar1Size, tempCharObjInfo, tempCharObjInfo.SpecialAbilityInfo1);
            m_subBar1.Enabled = true;
        }

        if (tempCharObjInfo.SpecialAbilityInfo2.GUIDisplayMode != SpecialAbilityInfo.DisplayMode.None)
        {
            if (m_subBar1 == null)
            {
                m_subBar1 = new SubBar(m_canvas, m_subBarBackground1, m_subBarSocketSprite, m_subBarFillSprite, m_subBar1Center, m_subBar1Size, tempCharObjInfo, tempCharObjInfo.SpecialAbilityInfo2);
                m_subBar1.Enabled = true;
            }
            else
            {
                m_subBar2 = new SubBar(m_canvas, m_subBarBackground2, m_subBarSocketSprite, m_subBarFillSprite, m_subBar2Center, m_subBar2Size, tempCharObjInfo, tempCharObjInfo.SpecialAbilityInfo2);
                m_subBar2.Enabled = true;
            }
        }

        //m_subBarLarge = new SubBar(m_canvas, m_subBarBackgroundLarge, m_subBarSocketSprite, m_subBarFillSprite, m_subBarLargeCenter, m_subBarLargeSize);
    }

    #region Properties
    private CharObjInfo OwnerObjInfo
    {
        get
        {
            return (CharObjInfo)m_ownerObjInfo;
        }
    }
    #endregion



    /// <summary>
    /// Defines all the properties and interactions of a sub bar
    /// </summary>
    protected class SubBar
    {
        #region Declarations
        protected bool m_enabled;

        protected Canvas m_canvas;

        protected GameObject m_background; //Used to toggle the background on and off if this sub bar is enabled
        protected Sprite m_socketSprite; //Sprite used to generate sockets
        protected Sprite m_fillSprite; //Sprite used to generate fills
        protected Vector2 m_center; //Center point for this bar
        protected Vector2 m_size; //Overall bounding size for this bar

        protected CharObjInfo m_charObjInfo; //Pointer to the character that this bar is tracking
        protected SpecialAbilityInfo m_specialAbilityInfo; //Pointer to the special ability this bar is tracking

        protected List<SubBarGUIElement> m_sockets; //Stores a list of socket GUI Elements
        protected List<SubBarGUIElement> m_fills; //Stores a list of fill GUI Elements

        protected int m_prevAmmo; //Stores the previous ammo value to allow only updating the GUI when the ammo changes
        #endregion

        /// <summary>
        /// Constructor for the SubBar class
        /// </summary>
        public SubBar(Canvas pCanvas, GameObject pBackground, Sprite pSocketSprite, Sprite pFillSprite, Vector2 pCenter, Vector2 pSize, CharObjInfo pCharObjInfo, SpecialAbilityInfo pSpecialAbilityinfo)
        {
            m_canvas = pCanvas;

            m_background = pBackground;
            m_socketSprite = pSocketSprite;
            m_fillSprite = pFillSprite;
            m_center = pCenter;
            m_size = pSize;

            m_charObjInfo = pCharObjInfo;
            m_specialAbilityInfo = pSpecialAbilityinfo;

            m_sockets = new List<SubBarGUIElement>();
            m_fills = new List<SubBarGUIElement>();

            InitializeGUIElements();

            m_prevAmmo = m_specialAbilityInfo.Ammo;
        }

        /// <summary>
        /// Update this SubBar
        /// </summary>
        public void Update()
        {
            if(m_specialAbilityInfo.GUIDisplayMode == SpecialAbilityInfo.DisplayMode.Ammo)
            {   
                //Fill in any ammo that needs to be refilled            
                for(int i = m_prevAmmo; i < m_specialAbilityInfo.Ammo; i++)
                {
                    m_fills[i].gameObject.SetActive(true);
                }

                //Remove any ammo that needs to be emptied
                for (int i = m_prevAmmo - 1; i > m_specialAbilityInfo.Ammo - 1; i--)
                {
                    m_fills[i].gameObject.SetActive(false);
                }

                m_prevAmmo = m_specialAbilityInfo.Ammo;
            }
        }

        /// <summary>
        /// Initializes all the GUI elements for this bar
        /// </summary>
        public void InitializeGUIElements()
        {
            Clear();

            if (m_specialAbilityInfo.GUIDisplayMode == SpecialAbilityInfo.DisplayMode.Ammo)
            {
                Vector2 size;
                Vector2 startPos;
                Vector2 currPos;

                SubBarGUIElement currElement;

                size = new Vector2(m_size.x / m_specialAbilityInfo.MaxAmmo, m_size.y);
                startPos = new Vector2((m_center.x - (m_size.x / 2)) + (size.x / 2), m_center.y);
                
                currPos = startPos;
                currElement = new SubBarGUIElement(m_canvas, m_socketSprite, currPos, size, "Socket 0");
                m_sockets.Add(currElement);
                for (int i = 1; i < m_specialAbilityInfo.MaxAmmo; i++)
                {
                    currPos.x += size.x;
                    currElement = new SubBarGUIElement(m_canvas, m_socketSprite, currPos, size, "Socket " + i);
                    m_sockets.Add(currElement);
                }


                currPos = startPos;
                currElement = new SubBarGUIElement(m_canvas, m_fillSprite, currPos, size, "Fill 0");
                m_fills.Add(currElement);
                for (int i = 1; i < m_specialAbilityInfo.MaxAmmo; i++)
                {
                    currPos.x += size.x;
                    currElement = new SubBarGUIElement(m_canvas, m_fillSprite, currPos, size, "Fill " + i);
                    m_fills.Add(currElement);
                }
            }
            else
            {
                //TO DO: Implement other modes
                Debug.Log(m_specialAbilityInfo.GUIDisplayMode + " not yet implemented!");
            }
        }

        /// <summary>
        /// Deletes all the game objects generated by this sub bar
        /// </summary>
        public void Clear()
        {
            foreach(SubBarGUIElement elem in m_sockets)
            {
                Destroy(elem.gameObject);
            }
            m_sockets.Clear();

            foreach(SubBarGUIElement elem in m_fills)
            {
                Destroy(elem.gameObject);
            }
            m_fills.Clear();
        }

        #region Properties
        public bool Enabled
        {
            get
            {
                return m_enabled;
            }
            set
            {
                m_enabled = value;
                m_background.SetActive(Enabled);
            }
        }
        #endregion
    }

    /// <summary>
    /// Stores a pointer to the GameObject, RectTransform, and Image of an element for the SubBar. This is done to prevent GetComponent calls
    /// </summary>
    protected class SubBarGUIElement
    {
        public GameObject gameObject;
        public RectTransform rectTransform;
        public Image image;

        /// <summary>
        /// Constructor for SubBarGUIElement
        /// </summary>
        public SubBarGUIElement(Canvas pCanvas, Sprite pSprite, Vector2 pPos, Vector2 pSize, string pName)
        {
            gameObject = new GameObject(pName);
            rectTransform = (RectTransform)gameObject.AddComponent(typeof(RectTransform));
            image = (Image)gameObject.AddComponent(typeof(Image));

            rectTransform.SetParent(pCanvas.transform);
            rectTransform.localScale = new Vector3(1, 1, 1);

            image.type = Image.Type.Sliced;
            image.sprite = pSprite;
            
            rectTransform.localPosition = pPos;
            rectTransform.sizeDelta = pSize;
        }
    }
}
