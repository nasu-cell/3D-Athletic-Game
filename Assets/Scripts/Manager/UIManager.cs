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
    public void StockChange(int stock)
    {
        StartCoroutine(StockDisplay(stock));
    }
    public void GameOver(int maxStock)
    {
        StartCoroutine(GameOverDisplay(maxStock));
    }
    public void ClickRestart()
    {
        gameOverPanel.SetActive(false);
    }
    IEnumerator GameOverDisplay(int maxStock)
    {
        gameOverPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        stockText.text = "X " + maxStock;
        stockPanelText.text = "X " + maxStock;
    }
    IEnumerator StockDisplay(int stock)
    {
        stockChangePanel.SetActive(true);
        stockText.text = "X " + stock;
        yield return new WaitForSeconds(1);
        stockPanelText.text = "X " + stock;
        yield return new WaitForSeconds(0.5f);
        stockChangePanel.SetActive(false);
    }
}
