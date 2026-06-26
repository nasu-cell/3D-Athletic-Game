using UnityEngine;

public class Log : MonoBehaviour
{
    //スポナーを参照する
    public LogSpawner logSpawner;
    private Vector3 startPosition;
    private float speed;
    private Vector3 direction;
    private float range;
    //RigidBody
    private Rigidbody logRb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (logSpawner != null)
        {
            startPosition = transform.position;
            speed = logSpawner.speed;
            direction = logSpawner.direction;
            range = logSpawner.range;
        }
        if(direction != Vector3.zero)
        {
            direction.Normalize();
        }
        logRb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        logRb.AddForce(direction * speed, ForceMode.Acceleration);
    }
    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(startPosition, transform.position);
        if (distance >= range)
        {
            Destroy(gameObject);
        }
    }
}
