using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject player;
    public Vector3 offSet = new Vector3(0, 3, 3);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offSet;
    }
}
