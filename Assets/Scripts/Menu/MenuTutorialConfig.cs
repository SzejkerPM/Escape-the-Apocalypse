using TMPro;
using UnityEngine;

public class MenuTutorialConfig : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public TextMeshProUGUI descriptionText;

    public void ExitOnClick()
    {
        Destroy(gameObject);
    }
}
