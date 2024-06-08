using UnityEngine;

public class Rotate : MonoBehaviour
{

    [SerializeField]
    private float rotationX;

    [SerializeField]
    private float rotationY;

    [SerializeField]
    private float rotationZ;

    void FixedUpdate()
    {
        transform.Rotate(rotationX * Time.fixedDeltaTime, rotationY * Time.fixedDeltaTime, rotationZ * Time.fixedDeltaTime);
    }
}
