using UnityEngine;

public class MagnetToPlayer : MonoBehaviour
{
    private Transform player;
    [SerializeField]
    private float magnetSpeed;
    [SerializeField]
    private float magnetRange;

    private Vector3 startPosition;

    private PlayerMenager playerMenager;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        playerMenager = FindObjectOfType<PlayerMenager>();

        startPosition = transform.position;
    }
    void FixedUpdate()
    {
        if (playerMenager.MagnetTime > 0 && (transform.position.z - player.transform.position.z) <= magnetRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, magnetSpeed * Time.fixedDeltaTime);
        }
        else if (playerMenager.MagnetTime <= 0 && startPosition != transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, magnetSpeed * Time.fixedDeltaTime);
        }
    }
}
