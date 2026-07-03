using System.Collections;
using UnityEngine;

public class FallBlock : MonoBehaviour
{
    public float fallSpeed;
    public float timeLimit;
    public float lowLimit;
    public float respawnTime;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private float timer;
    [SerializeField] private bool fall = false;
    private MeshRenderer meshRenderer;
    private Collider blockCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        meshRenderer = GetComponent<MeshRenderer>();
        blockCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= timeLimit)
        {
            fall = true;
            timer = 0f;
        }
        if (fall)
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
            if (transform.position.y < lowLimit)
            {
                fall = false;
                StartCoroutine(Respawn());
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            timer = 0f;
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if (!fall && collision.gameObject.CompareTag("Player"))
        {
            timer += Time.deltaTime;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (!fall && collision.gameObject.CompareTag("Player"))
        {
            timer = 0f;
        }
    }
    IEnumerator Respawn()
    {
        if (meshRenderer != null) meshRenderer.enabled = false;
        if (blockCollider != null) blockCollider.enabled = false;
        transform.position = startPosition;
        yield return new WaitForSeconds(respawnTime);
        if (meshRenderer != null) meshRenderer.enabled = true;
        if (blockCollider != null) blockCollider.enabled = true;
    }
}
