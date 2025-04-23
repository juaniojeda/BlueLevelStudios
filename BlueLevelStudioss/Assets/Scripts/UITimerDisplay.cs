using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITimerDisplay : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameStateManager gameStateManager;

    private void Update()
    {
        if (gameStateManager == null) return;

        float timeRemaining = gameStateManager.GetTimeRemaining();
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

