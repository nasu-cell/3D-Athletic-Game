using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private int maxStock;
    [SerializeField] private int stock;
    [SerializeField] private SpawnPoint[] spawnPoints;

    // Cinemachine Camera
    [SerializeField] private CinemachineCamera cinemachineCamera;

    public int savedSpawnPointID;
    public bool isGamePlaying = false;

    void Start()
    {
        GameStart();
    }

    public void StockDecrease()
    {
        isGamePlaying = false;
        stock--;

        if (stock <= 0)
        {
            StartCoroutine(GameOver());
        }
        else
        {
            StartCoroutine(ReStart());
        }
    }

    public void GameStart()
    {
        stock = maxStock;
        savedSpawnPointID = 0;
        SpawnPlayer();
        isGamePlaying = true;
    }

    IEnumerator GameOver()
    {
        uiManager.StockChange(stock);
        yield return new WaitForSeconds(1.5f);
        uiManager.GameOver(maxStock);
    }

    IEnumerator ReStart()
    {
        uiManager.StockChange(stock);

        yield return new WaitForSeconds(1.5f);

        SpawnPlayer();

        isGamePlaying = true;
    }

    private void SpawnPlayer()
    {
        GameObject player = Instantiate(
            playerPrefab,
            spawnPoints[savedSpawnPointID].transform.position,
            Quaternion.identity
        );

        // 新しく生成したPlayerをカメラのTracking Targetに設定
        cinemachineCamera.Target.TrackingTarget = player.transform;
    }
}