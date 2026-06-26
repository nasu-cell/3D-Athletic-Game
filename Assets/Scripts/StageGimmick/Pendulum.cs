using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public float speed;
    public float maxAngle;
    private Quaternion startRotation;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Sin(Time.time * speed) * maxAngle;
        transform.localRotation = startRotation * Quaternion.Euler(0, 0, angle);
    }
}
