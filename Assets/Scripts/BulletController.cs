using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Vector3 startPosition;
    [SerializeField]
    private float bulletMaxRange;


    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (transform.position.z > startPosition.z + bulletMaxRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }

}
