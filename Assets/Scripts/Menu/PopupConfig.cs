using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupConfig : MonoBehaviour
{

    [SerializeField]
    public TextMeshProUGUI titleText;

    [SerializeField]
    public TextMeshProUGUI descriptionText;

    [SerializeField]
    public Image popupImage;

    [SerializeField]
    private float waitTime = 5;

    private PopupManager popupManager;


    private void OnEnable()
    {
        popupManager = FindObjectOfType<PopupManager>();
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(waitTime);
        popupManager.ShowNextAchievementPopup();
        Destroy(gameObject);
    }

}
