using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    // index sprite: 0 = achievement, 1 = red alert, 2 = green alert

    [SerializeField]
    private GameObject canvas;

    [SerializeField]
    private GameObject achievementPopupPrefab;

    [SerializeField]
    private GameObject alertPopupPrefab;

    [SerializeField]
    private PopupImagesSO popupImages;

    private Queue<GameObject> achievementPopupQueue = new();
    private Queue<GameObject> alertPopupQueue = new();

    private bool firstPopupShowed = false;

    private void Update()
    {
        if (!firstPopupShowed)
        {
            ShowNextAchievementPopup();
            firstPopupShowed = true;
        }

        if (achievementPopupQueue.Count == 0)
        {
            firstPopupShowed = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            alertPopupQueue.Clear();
        }
    }

    internal void CreateAchievementPopup(int popupImageIndex, string title, string description)
    {
        Debug.Log("POPUP_MANAGER: Creating top pop-up!");

        var popup = Instantiate(achievementPopupPrefab, canvas.transform);
        popup.GetComponent<PopupConfig>().titleText.text = title;
        popup.GetComponent<PopupConfig>().descriptionText.text = description;
        foreach (Transform child in popup.transform)
        {
            if (child.name.Equals("Image"))
            {
                child.GetComponent<Image>().sprite = popupImages.sprites[popupImageIndex];
            }

        }
        popup.SetActive(false);

        achievementPopupQueue.Enqueue(popup);
    }

    internal void CreateAlertPopup(int popupImageIndex, string description)
    {
        Debug.Log("POPUP_MANAGER: Creating bottom pop-up!");

        var popup = Instantiate(alertPopupPrefab, canvas.transform);
        popup.GetComponent<PopupConfig>().descriptionText.text = description;
        foreach (Transform child in popup.transform)
        {
            if (child.name.Equals("Image"))
            {
                child.GetComponent<Image>().sprite = popupImages.sprites[popupImageIndex];
            }

        }

        if (alertPopupQueue.Count > 0)
        {
            alertPopupQueue.Clear();
        }

        alertPopupQueue.Enqueue(popup);
        AudioManager.Instance.PlaySFX("Popup");
    }

    internal void ShowNextAchievementPopup()
    {
        if (achievementPopupQueue.Count > 0)
        {
            var nextPopup = achievementPopupQueue.Dequeue();
            nextPopup.SetActive(true);
            AudioManager.Instance.PlaySFX("Achievement");
        }
    }
}
