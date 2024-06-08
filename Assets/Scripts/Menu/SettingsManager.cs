using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private Button musicButton;

    [SerializeField]
    private Button sfxButton;

    [SerializeField]
    private Sprite soundOnSprite;

    [SerializeField]
    private Sprite soundOffSprite;

    [SerializeField]
    private TMP_InputField playerInputName;

    [SerializeField]
    private string urlPrivacyPolicy;

    [SerializeField]
    private GameObject rateUsPanelAndroid;

    [SerializeField]
    private GameObject rateUsPanelIos;

    [SerializeField]
    private TextMeshProUGUI gameVersionAndroid;

    [SerializeField]
    private TextMeshProUGUI gameVersionIos;

    private PlayerData playerData;

    private void Start()
    {

        musicButton.onClick.AddListener(delegate { AudioManager.Instance.MuteMusic(); });
        sfxButton.onClick.AddListener(delegate { AudioManager.Instance.MuteSFX(); });

        AudioManager.OnMusicChanged += HandleOnMusicChanged;
        AudioManager.OnSFXChanged += HandleOnSFXChanged;

        playerData = SaveSystem.LoadPlayerData();

        SetSpritesForMuteButtons();

        if (!playerData.playerName.Equals(string.Empty))
        {
            playerInputName.text = playerData.playerName;
        }
        else
        {
            playerInputName.text = "Your Name";
        }

        Debug.Log("Player name: " + playerData.playerName);

        gameVersionAndroid.text = "Game Version: " + Application.version;
        gameVersionIos.text = "Game Version: " + Application.version;
    }

    private void SetSpritesForMuteButtons()
    {

        if (playerData.isMusicMuted)
        {
            musicButton.image.sprite = soundOffSprite;
        }
        else
        {
            musicButton.image.sprite = soundOnSprite;
        }

        if (playerData.isSFXMuted)
        {
            sfxButton.image.sprite = soundOffSprite;
        }
        else
        {
            sfxButton.image.sprite = soundOnSprite;
        }
    }

    private void HandleOnMusicChanged(bool isMusicOn)
    {
        if (isMusicOn)
        {
            musicButton.image.sprite = soundOnSprite;
        }
        else
        {
            musicButton.image.sprite = soundOffSprite;
        }
    }

    private void HandleOnSFXChanged(bool isSFXOn)
    {
        if (isSFXOn)
        {
            sfxButton.image.sprite = soundOnSprite;
        }
        else
        {
            sfxButton.image.sprite = soundOffSprite;
        }

    }

    private void OnDestroy()
    {
        AudioManager.OnMusicChanged -= HandleOnMusicChanged;
        AudioManager.OnSFXChanged -= HandleOnSFXChanged;
    }

    public void OnClickSavePlayerName()
    {
        if (!playerData.playerName.Equals(playerInputName.text))
        {
            playerData.playerName = playerInputName.text;
            SaveSystem.SavePlayerData(playerData);

            Debug.Log("New name saved: " + playerInputName.text);
        }
        else
        {
            Debug.Log("Name is already saved!");
        }
    }

    public void OnClickOpenLinkPrivacyPolicy()
    {
        if (urlPrivacyPolicy != null && !urlPrivacyPolicy.Equals(string.Empty))
        {
            Application.OpenURL(urlPrivacyPolicy);

        }

    }

    public void OnClickOpenLinkFacebook()
    {
        Debug.Log("Facebook");
    }

    public void OnClickOpenLinkInstagram()
    {
        Debug.Log("Instagram");
    }

    public void OnClickOpenLinkRateUsAndroid()
    {
        Debug.Log("Rate us (Android)!");
    }

    public void OnClickOpenLinkRateUsIos()
    {
        Debug.Log("Rate us (iOS)!");
    }

    public void OnClickRestorePurchases()
    {
        Debug.Log("Restore Purchases");
    }

    public void OnClickTermsOfUse()
    {
        Debug.Log("Terms of Use");
    }

    public void OnClickOpenRateUsPanel()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            rateUsPanelIos.SetActive(true);
        }
        else
        {
            rateUsPanelAndroid.SetActive(true);
        }
    }

    public void OnClickReturnToSettings()
    {
        if (rateUsPanelAndroid.activeSelf)
        {
            rateUsPanelAndroid.SetActive(false);
        }

        if (rateUsPanelIos.activeSelf)
        {
            rateUsPanelIos.SetActive(false);
        }
    }
}
