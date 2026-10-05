using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private int spawnPointID;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject cylinder;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CylinderAppear()
    {
        if (cylinder != null)
        {
            cylinder.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            gameManager.savedSpawnPointID = spawnPointID;
            if(cylinder != null)
            {
                cylinder.SetActive(false);
            }
        }
    }
}
