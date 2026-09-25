using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private int spawnPointID;
    [SerializeField] private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            gameManager.savedSpawnPointID = spawnPointID;
        }
    }
}
