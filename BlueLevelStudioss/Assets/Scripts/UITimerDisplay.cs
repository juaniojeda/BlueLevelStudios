using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITimerDisplay : MonoBehaviour, ICustomUpdate
{
    public TextMeshProUGUI timerText;
    public GameStateManager gameStateManager;

    private void Awake()
    {
        UpdateManager.Instance.Register(this);
    }

    private void OnDestroy()
    {
        if (UpdateManager.Instance != null)
            UpdateManager.Instance.Unregister(this);
    }

    public void CustomUpdate(float deltaTime)
    {
        if (gameStateManager == null) return;

        float timeRemaining = gameStateManager.GetTimeRemaining();
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}


