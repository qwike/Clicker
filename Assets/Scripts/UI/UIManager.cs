using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public AudioMixer mainMixer;

    public GameObject pauseBtn;

    public Slider musicVol;
    public Slider sfxVol;

    public GameObject arenaPanel;
    public GameObject shopPanel;
    public GameObject achievementsPanel;
    public GameObject pausePanel;
    public GameObject victoryPanel;
    public Button victoryBtn;
    public GameObject diedPanel;
    public Button diedBtn;

    public GameObject lastOpenedPanel;

    public TextMeshProUGUI smallPotionAmountText;
    public TextMeshProUGUI mediumPotionAmountText;
    public TextMeshProUGUI largePotionAmountText;

    public bool isArenaPanel = false;

    void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        float savedMusicVol = PlayerPrefs.GetFloat("musicVol", 0.25f);
        float savedSfxVol = PlayerPrefs.GetFloat("sfxVol", 1f);

        musicVol.value = savedMusicVol;
        sfxVol.value = savedSfxVol;

        SetMusicVolume(savedMusicVol);
        SetSfxVolume(savedSfxVol);
        ShowPause();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (pausePanel.activeSelf)
            {
                TurnOffPause();
            }
            else
            {
                ShowPause();
            }
        }
    }

    public void ShowArena()
    {
        isArenaPanel = true;
        lastOpenedPanel = arenaPanel;
        arenaPanel.SetActive(true);
        shopPanel.SetActive(false);
        achievementsPanel.SetActive(false);
        pausePanel.SetActive(false);
        pauseBtn.SetActive(true);
        victoryPanel.SetActive(false);
        diedPanel.SetActive(false);
    }

    public void ShowShop()
    {
        isArenaPanel = false;
        lastOpenedPanel = shopPanel;
        arenaPanel.SetActive(false);
        shopPanel.SetActive(true);
        achievementsPanel.SetActive(false);
        pausePanel.SetActive(false);
        ShopManager.Instance.GenerateShop();
        pauseBtn.SetActive(true);
    }

    public void ShowAchievements()
    {
        isArenaPanel = false;
        lastOpenedPanel = achievementsPanel;
        arenaPanel.SetActive(false);
        shopPanel.SetActive(false);
        achievementsPanel.SetActive(true);
        pausePanel.SetActive(false);
        AchievementsManager.Instance.RefreshList();
        pauseBtn.SetActive(true);
    }

    public void ShowPause()
    {
        isArenaPanel = false;
        arenaPanel.SetActive(false);
        shopPanel.SetActive(false);
        achievementsPanel.SetActive(false);
        pausePanel.SetActive(true);
        pauseBtn.SetActive(false);
    }

    public void ShowVictory()
    {
        isArenaPanel = false;
        arenaPanel.SetActive(false);
        shopPanel.SetActive(false);
        achievementsPanel.SetActive(false);
        pausePanel.SetActive(false);
        pauseBtn.SetActive(false);
        StartCoroutine(ActivateButtonAfterDelay(victoryBtn, 5f));
        victoryPanel.SetActive(true);

    }

    public void ShowDied()
    {
        isArenaPanel = false;
        arenaPanel.SetActive(false);
        shopPanel.SetActive(false);
        achievementsPanel.SetActive(false);
        pausePanel.SetActive(false);
        pauseBtn.SetActive(false);
        StartCoroutine(ActivateButtonAfterDelay(diedBtn, 5f));
        diedPanel.SetActive(true);
    }

    IEnumerator ActivateButtonAfterDelay(Button btn, float delay)
    {
        btn.interactable = false;

        yield return new WaitForSeconds(delay);

        btn.interactable = true;
    }

    public void SetMusicVolume(float sliderValue)
    {
        mainMixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("musicVol", sliderValue);
    }

    public void SetSfxVolume(float sliderValue)
    {
        mainMixer.SetFloat("SfxVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("sfxVol", sliderValue);
    }

    public void TurnOffPause()
    {
        isArenaPanel = lastOpenedPanel == arenaPanel;
        pausePanel.SetActive(false);
        pauseBtn.SetActive(true);
        lastOpenedPanel.SetActive(true);
    }

    public void NewGame()
    {
        float savedMusicVol = PlayerPrefs.GetFloat("musicVol", 0.25f);
        float savedSfxVol = PlayerPrefs.GetFloat("sfxVol", 1f);

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetFloat("musicVol", savedMusicVol);
        PlayerPrefs.SetFloat("sfxVol", savedSfxVol);

        PlayerPrefs.Save();
        GameManager.Instance.Restart();
        Player.Instance.Restart();
        EnemySpawner.Instance.Restart();

        ShowArena();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void UpdateAllPotionTexts()
    {
        UpdateText(300, smallPotionAmountText);
        UpdateText(301, mediumPotionAmountText);
        UpdateText(302, largePotionAmountText);
    }

    void UpdateText(int itemID, TextMeshProUGUI textElement)
    {
        int amount = PlayerPrefs.GetInt("Item_" + itemID, 0);
        textElement.text = amount.ToString();
    }
}