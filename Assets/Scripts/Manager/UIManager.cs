using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text stockText;
    [SerializeField] private TMP_Text stockPanelText;
    [SerializeField] private GameObject stockChangePanel;
    [SerializeField] private GameObject gameOverPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public async UniTask StockChange(int stock)
    {
        stockChangePanel.SetActive(true);
        stockText.text = "X " + stock;
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        stockPanelText.text = "X " + stock;
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        stockChangePanel.SetActive(false);
    }
    public async UniTask GameOver(int maxStock)
    {
        gameOverPanel.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        stockText.text = "X " + maxStock;
        stockPanelText.text = "X " + maxStock;
    }
    public void ClickRestart()
    {
        gameOverPanel.SetActive(false);
    }
}
