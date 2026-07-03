using UnityEngine;

public class SpeedChange : MonoBehaviour
{
    private PlayerController playerController;
    private float speed;
    public float changeSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerController.speedMultiplier = changeSpeed;
        }
    }
     void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerController.speedMultiplier = 1f;
        }
    }
}
