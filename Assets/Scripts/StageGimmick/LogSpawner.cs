using UnityEngine;

public class LogSpawner : MonoBehaviour
{
    public GameObject log;
    public float speed;
    public Vector3 direction;
    public float range;
    public float interval;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnLog", 2f, interval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnLog()
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, direction) * log.transform.rotation;
        GameObject SpawnLog = Instantiate(log, transform.position, rotation);
        Log logScript = SpawnLog.GetComponent<Log>();
        if(logScript != null)
        {
            logScript.logSpawner = this;
        }

    }
}
