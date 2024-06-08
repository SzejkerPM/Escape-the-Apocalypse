using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField]
    private InGameInfoSO inGameInfo;

    private TutorialManager tutorialManager;

    private void Start()
    {
        tutorialManager = FindObjectOfType<TutorialManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inGameInfo.tutorialCounter++;
        }
    }


}
