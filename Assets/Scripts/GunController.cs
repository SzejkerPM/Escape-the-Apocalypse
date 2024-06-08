using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private SwipeMenager swipeMenager;
    [SerializeField]
    private GameStatesSO gameStates;
    private bool isAbleToShot = false;
    [SerializeField]
    private Button ButtonFire;
    private bool hasAmmo = false;
    [SerializeField]
    private InGameInfoSO inGameInfo;
    private PlayerData playerData;

    [SerializeField]
    private GameObject player;

    private Animator animator;

    private void Start()
    {
        playerData = SaveSystem.LoadPlayerData();
    }
    private void Update()
    {
        CheckAmmo();

        if (animator == null)
        {
            foreach (Transform child in player.transform)
            {
                if (child.CompareTag("PlayerSkin"))
                {
                    animator = child.GetComponent<Animator>();
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (isAbleToShot)
        {
            Shot();
        }
    }

    public void FireButtonOnClick()
    {
        if (hasAmmo)
        {
            inGameInfo.currentAmmo--;
            isAbleToShot = true;
        }
    }

    private void Shot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletPrefab.transform.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        rb.AddForce(bulletSpawnPoint.forward * bulletSpeed, ForceMode.Impulse);

        PlayShootSound();
        PlayShootAnimation();

        isAbleToShot = false;
    }

    private void PlayShootSound()
    {
        if (playerData.selectedGunSkinIndex == 0)
        {
            AudioManager.Instance.PlaySFX("Colt");
        }
        else if (playerData.selectedGunSkinIndex == 1)
        {
            AudioManager.Instance.PlaySFX("M4");
        }
        else if (playerData.selectedGunSkinIndex == 2)
        {
            AudioManager.Instance.PlaySFX("Laser");
        }
        else if (playerData.selectedGunSkinIndex == 3)
        {
            AudioManager.Instance.PlaySFX("MagicWand");
        }
        else if (playerData.selectedGunSkinIndex == 4)
        {
            AudioManager.Instance.PlaySFX("FoodGun");
        }
    }

    private void PlayShootAnimation()
    {
        StartCoroutine(ShootAnimCourtine(1f));
    }

    private IEnumerator ShootAnimCourtine(float seconds)
    {
        animator.SetLayerWeight(animator.GetLayerIndex("FullBody"), 0f);
        animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 1f);
        animator.SetLayerWeight(animator.GetLayerIndex("LowerBody"), 1f);

        if (playerData.selectedGunSkinIndex == 0) // colt
        {
            animator.Play("shoot_gun");
        }
        else if (playerData.selectedGunSkinIndex == 1) // AK
        {
            animator.Play("shoot_ak");
        }
        else if (playerData.selectedGunSkinIndex == 2) // laser
        {
            animator.Play("shoot_gun");
        }
        else if (playerData.selectedGunSkinIndex == 3) // wand
        {
            animator.Play("shoot_wand");
        }
        else if (playerData.selectedGunSkinIndex == 4) // food gun
        {
            animator.Play("shoot_gun");
        }

        yield return new WaitForSeconds(seconds);

        animator.SetLayerWeight(animator.GetLayerIndex("FullBody"), 1f);
        animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 0f);
        animator.SetLayerWeight(animator.GetLayerIndex("LowerBody"), 0f);
    }


    private void CheckAmmo()
    {
        if (inGameInfo.currentAmmo > 0)
        {
            hasAmmo = true;
        }
        else
        {
            hasAmmo = false;
        }
    }
}
