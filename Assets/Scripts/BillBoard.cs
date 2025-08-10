using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform playerCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCamera = GameObject.Find("playerCamera").transform;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(transform.position + playerCamera.forward);
    }
}
