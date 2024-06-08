using UnityEngine;

[CreateAssetMenu(fileName = "GunSkin", menuName = "Custom/GunSkin")]
public class GunSkinSO : ScriptableObject
{

    public int skinIndex;
    public string skinName;
    public int costGold;
    public bool isOwned;
    public GameObject skinModel;

}
