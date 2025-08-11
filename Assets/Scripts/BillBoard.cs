using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform playerCamera;

    void Start()
    {
        playerCamera = GameObject.Find("playerCamera").transform;
    }

    void FixedUpdate()
    {
        transform.LookAt(transform.position + playerCamera.forward);
    }
}
