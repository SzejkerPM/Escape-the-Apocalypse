using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField]
    private InGameInfoSO inGameInfo;

    private int maxAmmo;

    private void Start()
    {
        maxAmmo = FindObjectOfType<PlayerMenager>().MainPlayerData.maxAmmo;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inGameInfo.currentAmmo = maxAmmo;
            AudioManager.Instance.PlaySFX("AmmoPickup");
            Destroy(gameObject);
        }
    }
}
