using UnityEngine;

public class GunSlotManager : MonoBehaviour
{

    [SerializeField]
    private GameObject playerHand;

    [SerializeField]
    private GameObject gunSlotColt;

    [SerializeField]
    private GameObject gunSlotAK;

    [SerializeField]
    private GameObject gunSlotLaser;

    [SerializeField]
    private GameObject gunSlotWand;

    [SerializeField]
    private GameObject gunSlotFoodGun;

    private GameObject gun;


    void Update()
    {
        if (gun != null)
        {
            if (gunSlotColt.transform.childCount != 0)
            {
                gunSlotColt.transform.position = playerHand.transform.position;
                gunSlotColt.transform.rotation = playerHand.transform.rotation;
            }
            else if (gunSlotAK.transform.childCount != 0)
            {
                gunSlotAK.transform.position = playerHand.transform.position;
                gunSlotAK.transform.rotation = playerHand.transform.rotation;
            }
            else if (gunSlotLaser.transform.childCount != 0)
            {
                gunSlotLaser.transform.position = playerHand.transform.position;
                gunSlotLaser.transform.rotation = playerHand.transform.rotation;
            }
            else if (gunSlotWand.transform.childCount != 0)
            {
                gunSlotWand.transform.position = playerHand.transform.position;
                gunSlotWand.transform.rotation = playerHand.transform.rotation;
            }
            else if (gunSlotFoodGun.transform.childCount != 0)
            {
                gunSlotFoodGun.transform.position = playerHand.transform.position;
                gunSlotFoodGun.transform.rotation = playerHand.transform.rotation;
            }
        }

    }

    public GameObject Gun { get { return gun; } set { gun = value; } }

    public GameObject GunSlotColt { get { return gunSlotColt; } }

    public GameObject GunSlotAK { get { return gunSlotAK; } }

    public GameObject GunSlotLaser { get { return gunSlotLaser; } }

    public GameObject GunSlotWand { get { return gunSlotWand; } }

    public GameObject GunSlotFoodGun { get { return gunSlotFoodGun; } }
}
