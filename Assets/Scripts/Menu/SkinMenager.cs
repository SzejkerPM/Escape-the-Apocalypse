using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkinMenager : MonoBehaviour
{
    private int currentIndex; //index + 1 (no default skins)
    private int currentDefaultPlayerSkinIndex;
    private int currentDefaultGunSkinIndex;

    private int selectedPlayerSkin;
    private int selectedGunSkin;

    private int maxIndexPlayerSkins;
    private int maxIndexGunSkins;

    [SerializeField]
    private GameObject player;

    private List<GameObject> playerSkinToShow;
    private List<GameObject> gunSkinToShow;

    [SerializeField]
    private PlayerSkinSO[] playerSkins;

    [SerializeField]
    private GunSkinSO[] gunSkins;

    [SerializeField]
    private GameObject shopPlayerSkinPanel;

    [SerializeField]
    private GameObject shopGunSkinPanel;

    [SerializeField]
    private GameObject profilePlayerSkinsPanel;

    [SerializeField]
    private GameObject profileGunSkinsPanel;

    [SerializeField]
    private TextMeshProUGUI buyPlayerSkinGoldAmountText;

    [SerializeField]
    private TextMeshProUGUI buyGunSkinGoldAmountText;

    private PlayerData playerData;

    private InfoManager infoManager;

    [SerializeField]
    private GameStatesSO gameStates;

    private Animator animator;

    private bool isAnimatorSet = false;

    private void Start()
    {
        playerData = SaveSystem.LoadPlayerData();
        infoManager = FindObjectOfType<InfoManager>();
        playerSkinToShow = new();
        gunSkinToShow = new();
        currentIndex = 1;
        maxIndexPlayerSkins = playerSkins.Length - 1;
        maxIndexGunSkins = gunSkins.Length - 1;
    }

    private void Update()
    {
        if (gameStates.isGameStarded && !isAnimatorSet)
        {
            foreach (Transform child in player.transform)
            {
                if (child.CompareTag("PlayerSkin"))
                {
                    child.transform.rotation = Quaternion.identity;
                }
            }

            animator.applyRootMotion = false;
            animator.Play("run");

            isAnimatorSet = true;
        }
    }

    internal void ResetPanel()
    {
        playerData = SaveSystem.LoadPlayerData();
        infoManager = FindObjectOfType<InfoManager>();
        playerSkinToShow = new();
        gunSkinToShow = new();
        currentIndex = 1;
        maxIndexPlayerSkins = playerSkins.Length - 1;
        maxIndexGunSkins = gunSkins.Length - 1;
        SetSelectedSkinsIndexes();
    }

    internal void UpdateSkinsData()
    {
        playerData = SaveSystem.LoadPlayerData();

        selectedPlayerSkin = playerData.selectedPlayerSkinIndex;
        selectedGunSkin = playerData.selectedGunSkinIndex;

        for (int i = 0; i < playerSkins.Length; i++)
        {
            playerSkins[i].isOwned = playerData.ownedPlayerSkins[i];
            gunSkins[i].isOwned = playerData.ownedGunSkins[i];
        }
    }

    private void SetSelectedSkinsIndexes()
    {
        playerData = SaveSystem.LoadPlayerData();

        currentDefaultPlayerSkinIndex = playerData.selectedPlayerSkinIndex;
        currentDefaultGunSkinIndex = playerData.selectedGunSkinIndex;
    }

    internal void ShopShowPlayerSkin()
    {
        playerSkinToShow.Add(Instantiate(playerSkins[currentIndex].skinModel));
        playerSkinToShow[0].transform.SetParent(shopPlayerSkinPanel.transform, false);
        playerSkinToShow[0].transform.position = shopPlayerSkinPanel.transform.position;
        playerSkinToShow[0].layer = 5;
        animator = playerSkinToShow[0].GetComponent<Animator>();
        animator.Play("idle");

        foreach (Transform child in playerSkinToShow[0].transform)
        {
            child.gameObject.layer = 5;
        }
    }

    internal void ShopShowGunSkin()
    {
        gunSkinToShow.Add(Instantiate(gunSkins[currentIndex].skinModel));
        gunSkinToShow[0].transform.SetParent(shopGunSkinPanel.transform, false);
        gunSkinToShow[0].transform.position = shopGunSkinPanel.transform.position;
        gunSkinToShow[0].layer = 5;

        ChangeGunSkinScale(currentIndex, gunSkinToShow[0]);

        foreach (Transform child in gunSkinToShow[0].transform)
        {
            child.gameObject.layer = 5;
        }
    }

    internal void ProfileShowSkins()
    {
        foreach (var skin in playerSkins)
        {
            if (skin.skinIndex.Equals(currentDefaultPlayerSkinIndex))
            {
                playerSkinToShow.Add(Instantiate(skin.skinModel));
                playerSkinToShow[0].transform.SetParent(profilePlayerSkinsPanel.transform, false);
                playerSkinToShow[0].transform.position = profilePlayerSkinsPanel.transform.position;
                playerSkinToShow[0].layer = 5;
                animator = playerSkinToShow[0].GetComponent<Animator>();
                animator.Play("idle");

                foreach (Transform child in playerSkinToShow[0].transform)
                {
                    child.gameObject.layer = 5;
                }
            }
        }

        foreach (var skin in gunSkins)
        {
            if (skin.skinIndex.Equals(currentDefaultGunSkinIndex))
            {
                gunSkinToShow.Add(Instantiate(skin.skinModel));
                gunSkinToShow[0].transform.rotation = Quaternion.identity;
                gunSkinToShow[0].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                gunSkinToShow[0].transform.SetParent(profileGunSkinsPanel.transform, false);
                gunSkinToShow[0].transform.position = profileGunSkinsPanel.transform.position;
                gunSkinToShow[0].layer = 5;

                ChangeGunSkinScale(currentDefaultGunSkinIndex, gunSkinToShow[0]);

                foreach (Transform child in gunSkinToShow[0].transform)
                {
                    child.gameObject.layer = 5;
                }
            }
        }
    }

    private void ChangeGunSkinScale(int currentIndex, GameObject gunSkin)
    {
        switch (currentIndex)
        {
            case 0:
                gunSkin.transform.rotation = Quaternion.identity;
                gunSkin.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                break;
            case 1:
                gunSkin.transform.eulerAngles = new Vector3(-90, 0, 90);
                gunSkin.transform.localScale = new Vector3(32f, 32f, 32f);
                break;
            case 2:
                gunSkin.transform.eulerAngles = new Vector3(-90, 0, 90);
                gunSkin.transform.localScale = new Vector3(50f, 50f, 50f);
                break;
            case 3:
                gunSkin.transform.eulerAngles = new Vector3(-90, 0, 90);
                gunSkin.transform.localScale = new Vector3(50f, 50f, 50f);
                break;
            case 4:
                gunSkin.transform.rotation = Quaternion.identity;
                gunSkin.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                break;
            default:
                Debug.Log("Gun skin scale error: incorrect index");
                break;
        }
    }

    internal void InGameShowSkins()
    {
        UpdateSkinsData();
        UpdateSkins();
    }

    internal void ConfirmSelectedPlayerSkin()
    {
        var playerData = SaveSystem.LoadPlayerData();

        playerData.selectedPlayerSkinIndex = currentDefaultPlayerSkinIndex;
        selectedPlayerSkin = currentDefaultPlayerSkinIndex;

        SaveSystem.SavePlayerData(playerData);
    }

    internal void ConfirmSelectedGunSkin()
    {
        var playerData = SaveSystem.LoadPlayerData();

        playerData.selectedGunSkinIndex = currentDefaultGunSkinIndex;
        selectedGunSkin = currentDefaultGunSkinIndex;

        SaveSystem.SavePlayerData(playerData);
    }

    internal bool IsCurrentPlayerSkinSelected()
    {
        if (selectedPlayerSkin == currentDefaultPlayerSkinIndex)
        {
            return true;
        }
        else { return false; }
    }

    internal bool IsCurrentGunSkinSelected()
    {
        if (selectedGunSkin == currentDefaultGunSkinIndex)
        {
            return true;
        }
        else { return false; }
    }

    internal void ShopNextPlayerSkin()
    {
        ResetSkins();

        if (currentIndex < maxIndexPlayerSkins)
        {
            currentIndex++;
            ShopShowPlayerSkin();
        }
        else
        {
            currentIndex = 1;
            ShopShowPlayerSkin();
        }
    }

    internal void ShopPrevPlayerSkin()
    {
        ResetSkins();

        if (currentIndex > 1)
        {
            currentIndex--;
            ShopShowPlayerSkin();
        }
        else
        {
            currentIndex = maxIndexPlayerSkins;
            ShopShowPlayerSkin();
        }
    }

    internal void ShopNextGunSkin()
    {
        ResetSkins();

        if (currentIndex < maxIndexGunSkins)
        {
            currentIndex++;
            ShopShowGunSkin();
        }
        else
        {
            currentIndex = 1;
            ShopShowGunSkin();
        }
    }

    internal void ShopPrevGunSkin()
    {
        ResetSkins();

        if (currentIndex > 1)
        {
            currentIndex--;
            ShopShowGunSkin();
        }
        else
        {
            currentIndex = maxIndexGunSkins;
            ShopShowGunSkin();
        }
    }

    internal void ProfileNextPlayerSkin()
    {
        ResetSkins();

        if (currentDefaultPlayerSkinIndex < maxIndexPlayerSkins)
        {
            currentDefaultPlayerSkinIndex++;

            if (playerSkins[currentDefaultPlayerSkinIndex].isOwned)
            {
                ProfileShowSkins();
                return;
            }
            else
            {
                ProfileNextPlayerSkin();
            }
        }
        else
        {
            currentDefaultPlayerSkinIndex = 0;

            if (playerSkins[currentDefaultPlayerSkinIndex].isOwned)
            {
                ProfileShowSkins();
                return;
            }
            else
            {
                ProfileNextPlayerSkin();
            }
        }
    }

    internal void ProfilePrevPlayerSkin()
    {
        ResetSkins();

        if (currentDefaultPlayerSkinIndex > 0)
        {
            currentDefaultPlayerSkinIndex--;

            if (playerSkins[currentDefaultPlayerSkinIndex].isOwned)
            {
                ProfileShowSkins();
                return;
            }
            else
            {
                ProfilePrevPlayerSkin();
            }
        }
        else
        {
            currentDefaultPlayerSkinIndex = maxIndexPlayerSkins;

            if (playerSkins[currentDefaultPlayerSkinIndex].isOwned)
            {
                ProfileShowSkins();
                return;
            }
            else
            {
                ProfilePrevPlayerSkin();
            }
        }

    }

    internal void ProfileNextGunSkin()
    {
        ResetSkins();

        if (currentDefaultGunSkinIndex < maxIndexGunSkins)
        {
            currentDefaultGunSkinIndex++;

            if (gunSkins[currentDefaultGunSkinIndex].isOwned)
            {
                ProfileShowSkins();
                return;
            }
            else
            {
                ProfileNextGunSkin();
            }
        }
        else
        {
            currentDefaultGunSkinIndex = 0;

            if (gunSkins[currentDefaultGunSkinIndex].isOwned)
            {
                ProfileShowSkins();
                return;
            }
            else
            {
                ProfileNextGunSkin();
            }
        }
    }

    internal void ProfilePrevGunSkin()
    {
        ResetSkins();

        if (currentDefaultGunSkinIndex > 0)
        {
            currentDefaultGunSkinIndex--;

            if (gunSkins[currentDefaultGunSkinIndex].isOwned)
            {
                ProfileShowSkins();
                return;
            }
            else
            {
                ProfilePrevGunSkin();
            }
        }
        else
        {
            currentDefaultGunSkinIndex = maxIndexGunSkins;

            if (gunSkins[currentDefaultGunSkinIndex].isOwned)
            {
                ProfileShowSkins();
                return;
            }
            else
            {
                ProfilePrevGunSkin();
            }
        }
    }

    internal void BuyPlayerSkinOnCurrentIndex()
    {
        playerData = SaveSystem.LoadPlayerData();

        if (playerData.gold >= playerSkins[currentIndex].costGold && !playerSkins[currentIndex].isOwned)
        {
            playerData.gold -= playerSkins[currentIndex].costGold;
            playerData.ownedPlayerSkins[currentIndex] = true;
            playerSkins[currentIndex].isOwned = true;
            CheckForSkinAchievement(playerData);
            SaveSystem.SavePlayerData(playerData);
            infoManager.UpdateSaveFile();

            AudioManager.Instance.PlaySFX("Buy");
        }
    }

    internal void BuyGunSkinOnCurrentIndex()
    {
        playerData = SaveSystem.LoadPlayerData();

        if (playerData.gold >= gunSkins[currentIndex].costGold && !gunSkins[currentIndex].isOwned) // na razie operacje na coinsach, nie goldzie
        {
            playerData.gold -= gunSkins[currentIndex].costGold;
            playerData.ownedGunSkins[currentIndex] = true;
            gunSkins[currentIndex].isOwned = true;
            CheckForSkinAchievement(playerData);
            SaveSystem.SavePlayerData(playerData);
            infoManager.UpdateSaveFile();

            AudioManager.Instance.PlaySFX("Buy");
        }
    }

    private void CheckForSkinAchievement(PlayerData playerData)
    {
        if (playerData.achievementValue[15] != 1)
        {
            playerData.achievementValue[15] = 1;
        }
    }

    internal void UpdateSkins()
    {
        UpdatePlayerSkin();
        UpdateGunSkin();
    }

    private void UpdatePlayerSkin()
    {
        foreach (Transform child in player.transform)
        {
            if (child.CompareTag("PlayerSkin"))
            {
                Destroy(child.gameObject);
            }
        }

        GameObject newPlayerSkin = Instantiate(playerSkins[selectedPlayerSkin].skinModel);
        newPlayerSkin.transform.SetParent(player.transform, false);
        newPlayerSkin.transform.position = player.transform.position;
        newPlayerSkin.tag = "PlayerSkin";
        newPlayerSkin.layer = 0;

        foreach (Transform child in player.transform)
        {
            if (child.CompareTag("PlayerSkin"))
            {
                animator = child.GetComponent<Animator>();
            }
        }

    }

    private void UpdateGunSkin()
    {
        foreach (Transform child in player.transform)
        {
            if (child.CompareTag("PlayerSkin"))
            {
                var gunSlot = child.GetComponent<GunSlotManager>();
                GameObject newGunSkin = Instantiate(gunSkins[selectedGunSkin].skinModel, CheckForGunIndex(selectedGunSkin, gunSlot).transform);
                gunSlot.Gun = newGunSkin;
                newGunSkin.tag = "GunSkin";
                newGunSkin.layer = 0;
            }
        }

    }

    private GameObject CheckForGunIndex(int selectedIndex, GunSlotManager gunSlot)
    {
        switch (selectedIndex)
        {
            case 0:
                return gunSlot.GunSlotColt;

            case 1:
                return gunSlot.GunSlotAK;

            case 2:
                return gunSlot.GunSlotLaser;

            case 3:
                return gunSlot.GunSlotWand;

            case 4:
                return gunSlot.GunSlotFoodGun;

            default:
                Debug.Log("Setting gun slot failed: incorrect index:");
                return null;
        }
    }

    private void ResetSkins()
    {
        if (playerSkinToShow.Capacity > 0)
        {
            playerSkinToShow.Clear();
        }

        if (gunSkinToShow.Capacity > 0)
        {
            gunSkinToShow.Clear();
        }

        if (shopPlayerSkinPanel.transform.childCount > 0)
        {
            Destroy(shopPlayerSkinPanel.transform.GetChild(0).gameObject);
        }

        if (shopGunSkinPanel.transform.childCount > 0)
        {
            Destroy(shopGunSkinPanel.transform.GetChild(0).gameObject);
        }

        if (profilePlayerSkinsPanel.transform.childCount > 0)
        {
            Destroy(profilePlayerSkinsPanel.transform.GetChild(0).gameObject);
        }

        if (profileGunSkinsPanel.transform.childCount > 0)
        {
            Destroy(profileGunSkinsPanel.transform.GetChild(0).gameObject);
        }
    }

    internal void ResetSkinsOnQuit()
    {
        ResetSkins();
        currentIndex = 1;
    }

    public bool GetPlayerSkinIsOwnedOnCurrentIndex()
    {
        return playerSkins[currentIndex].isOwned;
    }

    public int GetPlayerSkinCostOnCurrentIndex()
    {
        return playerSkins[currentIndex].costGold;
    }

    public bool GetGunSkinIsOwnedOnCurrentIndex()
    {
        return gunSkins[currentIndex].isOwned;
    }

    public int GetGunSkinCostOnCurrentIndex()
    {
        return gunSkins[currentIndex].costGold;
    }
}
