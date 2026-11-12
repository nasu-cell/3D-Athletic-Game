using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using Cysharp.Threading.Tasks;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private int maxStock;
    [SerializeField] private int stock;
    [SerializeField] private SpawnPoint[] spawnPoints;

    // Cinemachine Camera
    [SerializeField] private CinemachineCamera cinemachineCamera;
    private bool isRespawning = false;

    public int savedSpawnPointID;
    public bool isGamePlaying = false;

    void Start()
    {
        GameStart();
    }

    public async UniTask StockDecrease()
    {
        // ゲーム停止中、またはリスポーン処理中なら無視
        if (!isGamePlaying || isRespawning)
        {
            return;
        }

        isRespawning = true;
        isGamePlaying = false;
        stock--;

        if (stock <= 0)
        {
            await GameOver();
        }
        else
        {
            await ReStart();
        }
    }

    public void GameStart()
    {
        stock = maxStock;
        savedSpawnPointID = 0;
        Cylinder();
        SpawnPlayer();
        isGamePlaying = true;
    }

    async UniTask GameOver()
    {
        await uiManager.StockChange(stock);
        await uiManager.GameOver(maxStock);
    }

    public async UniTask ReStart()
    {
        await uiManager.StockChange(stock);

        SpawnPlayer();

        isGamePlaying = true;
        isRespawning = false;
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
    private void Cylinder()
    {
        foreach(SpawnPoint spawnPoint in spawnPoints)
        {
            spawnPoint.CylinderAppear();
        }
    }
}