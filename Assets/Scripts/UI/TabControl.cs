using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabControl : MonoBehaviour
{
    public enum EView
    {
        GAME,
        SHOP,
        UPGRADES,
        STATS,
        EXTRA
    }

    // Tabs
    [Header("Tab images")]
    public Image shop;
    public Image upgrades;
    public Image stats;
    public Image extra;

    [Header("Tab sprites")]
    public Sprite shopSprite;
    public Sprite upgradesSprite;
    public Sprite statsSprite;
    public Sprite extraSprite;

    [Header("Selected Tab sprites")]
    public Sprite shopSpriteSelected;
    public Sprite upgradesSpriteSelected;
    public Sprite statsSpriteSelected;
    public Sprite extraSpriteSelected;

    [Header("Tab panels")]
    public RectTransform shopPanel;
    public RectTransform upgradesPanel;
    public RectTransform statsPanel;
    public RectTransform extraPanel;

    [Header("Current View")]
    public EView currentView;

    [Header("Animation")]
    public AnimationCurve tabAnimationCurve;
    public float transitionTime = 2.0f;

    [Header("Layout")]
    public float marginBotton = 100.0f;

    float animationCurveTime;

    RectTransform m_RectTransform;
    AudioSource m_AudioSource;

    Vector2 closedPosition;
    Vector2 openedPosition;

    Vector2 targetTabsPosition;
    Vector2 sourceTabsPosition;

    void Awake()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        float canvasHeight = canvas.GetComponent<RectTransform>().rect.height - marginBotton;

        closedPosition = new Vector2(0, canvasHeight);
        openedPosition = new Vector2(0, 0);

        m_AudioSource = GetComponent<AudioSource>();

        m_RectTransform = GetComponent<RectTransform>();
        m_RectTransform.sizeDelta = new Vector2(m_RectTransform.sizeDelta.x, canvasHeight);
        m_RectTransform.anchoredPosition = closedPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        animationCurveTime = 0.0f;
        currentView = EView.GAME;
        targetTabsPosition = m_RectTransform.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Update animation
        if (animationCurveTime >= 0.0f)
        {
            animationCurveTime -= Time.deltaTime;
            float pos = tabAnimationCurve.Evaluate(animationCurveTime / transitionTime);
            m_RectTransform.anchoredPosition = Vector2.Lerp(sourceTabsPosition, targetTabsPosition, 1.0f - pos);
        }
    }

    void ResetIcons()
    {
        shop.sprite = shopSprite;
        upgrades.sprite = upgradesSprite;
        stats.sprite = statsSprite;
        extra.sprite = extraSprite;
    }

    void ResetPanels()
    {
        shopPanel.gameObject.SetActive(false);
        upgradesPanel.gameObject.SetActive(false);
        statsPanel.gameObject.SetActive(false);
        extraPanel.gameObject.SetActive(false);
    }

    void SetViewIconAndPanel(EView view)
    {
        switch(view)
        {
            case EView.SHOP:
                shop.sprite = shopSpriteSelected;
                shopPanel.gameObject.SetActive(true);
                GUIManager.UpdateStoreItems();
                break;
            case EView.UPGRADES:
                upgrades.sprite = upgradesSpriteSelected;
                upgradesPanel.gameObject.SetActive(true);
                GUIManager.UpdateUpgrades();
                GUIManager.ClearUpgradeNotification();
                break;
            case EView.STATS:
                stats.sprite = statsSpriteSelected;
                statsPanel.gameObject.SetActive(true);
                break;
            case EView.EXTRA:
                extra.sprite = extraSpriteSelected;
                extraPanel.gameObject.SetActive(true);
                break;
        }
    }

    void GoToView(EView view)
    {
        if (currentView != view)
        {
            ResetIcons();

            if (view == EView.GAME)
            {
                sourceTabsPosition = m_RectTransform.anchoredPosition;
                targetTabsPosition = closedPosition;
                DataManager.InGame = true;
            }
            else
            {
                ResetPanels();
                SetViewIconAndPanel(view);

                sourceTabsPosition = m_RectTransform.anchoredPosition;
                targetTabsPosition = openedPosition;

                DataManager.InGame = false;
            }

            animationCurveTime = transitionTime;
            currentView = view;
            m_AudioSource.Play();
        }
        else
        {
            // Clicking back
            if (view != EView.GAME)
            {
                GoToGameView();
            }
        }
    }

    public void GoToShopView()
    {
        GoToView(EView.SHOP);
    }

    public void GoToUpgradesView()
    {
        GoToView(EView.UPGRADES);
    }

    public void GoToStatsView()
    {
        GoToView(EView.STATS);
    }

    public void GoToExtrasView()
    {
        GoToView(EView.EXTRA);
    }

    public void GoToGameView()
    {
        GoToView(EView.GAME);
    }
}
