using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class GameUIView : MonoBehaviour
{
    [Header("Commander UI")]
    [SerializeField] private Slider commanderHpSlider;
    [SerializeField] private TextMeshProUGUI commanderHpText;

    [Header("Wave Info UI")]
    [SerializeField] private TextMeshProUGUI waveCountText;
    [SerializeField] private TextMeshProUGUI aliveEnemyCountText;
    [SerializeField] private TextMeshProUGUI waveDelayText;

    [Header("Result Screens")]
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject defeatScreen;

    private void Awake()
    {
        HideResultScreens();
        if (waveDelayText != null) waveDelayText.gameObject.SetActive(false);
    }
    
    public void UpdateCommanderHp(float current, float max)
    {
        if (commanderHpSlider != null)
        {
            commanderHpSlider.maxValue = max;
            commanderHpSlider.value = current;
        }
        if (commanderHpText != null)
        {
            commanderHpText.text = $"{current} / {max}";
        }
    }
    
    public void UpdateWaveInfo(int currentWaveIndex, int maxWaves)
    {
        if (waveCountText == null) return;
        waveCountText.text = $"Wave: {currentWaveIndex + 1} / {maxWaves}";
    }
    
    public void UpdateAliveEnemies(int current, int total)
    {
        if (aliveEnemyCountText == null) return;
        aliveEnemyCountText.text = $"Enemies: {current} / {total}";
    }

    public void UpdateDelayCountdown(float secondsLeft)
    {
        if (waveDelayText == null) return;
        
        if (secondsLeft > 0)
        {
            waveDelayText.gameObject.SetActive(true);
            waveDelayText.text = $"Next Wave in: {secondsLeft}s";
        }
        else
        {
            waveDelayText.gameObject.SetActive(false);
        }
    }

    public void ShowVictoryScreen()
    {
        if (victoryScreen == null) return;
        victoryScreen.SetActive(true);
    }

    public void ShowDefeatScreen()
    {
        if (defeatScreen == null) return;
        defeatScreen.SetActive(true);
    }

    private void HideResultScreens()
    {
        if (victoryScreen != null) victoryScreen.SetActive(false);
        if (defeatScreen != null) defeatScreen.SetActive(false);
    }
}
