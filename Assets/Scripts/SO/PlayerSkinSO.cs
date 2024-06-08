using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkin", menuName = "Custom/PlayerSkin")]
public class PlayerSkinSO : ScriptableObject
{
    public int skinIndex;
    public string skinName;
    public int costGold;
    public bool isOwned;
    public GameObject skinModel;

    // TODO: doda� skiny specjalne, kt�re mo�na kupi� za zasoby i coiny
    // private int costCoins;
    // private int costFirstAid;
    // private int costShield;
    // private int costFood;

}
