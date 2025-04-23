using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour, ICustomUpdate
{
    private bool gameEnded = false;
    private float timer = 0f;
    private float timeLimit = 90f; // 1 minuto y medio

    public void CustomUpdate(float deltaTime)
    {
        if (gameEnded) return;

        timer += deltaTime;

        if (AllBricksDestroyed())
        {
            gameEnded = true;
            Debug.Log("¡GANASTE!");
            Invoke(nameof(LoadVictoryScreen), 2f);
            return;
        }

        if (timer >= timeLimit)
        {
            gameEnded = true;
            Debug.Log("¡PERDISTE POR TIEMPO!");
            Invoke(nameof(LoadGameOverScreen), 2f);
        }
    }

    bool AllBricksDestroyed()
    {
        var bricks = Object.FindObjectsByType<Brick>(FindObjectsSortMode.None);
        foreach (var brick in bricks)
        {
            if (brick != null && brick.gameObject.activeInHierarchy)
                return false;
        }
        return true;
    }
    public float GetTimeRemaining()
    {
        return Mathf.Max(0f, timeLimit - timer);
    }
    void LoadVictoryScreen()
    {
        SceneManager.LoadScene("VictoryScreen");
    }

    void LoadGameOverScreen()
    {
        SceneManager.LoadScene("GameOverScreen");
    }

    private void Awake()
    {
        if (UpdateManager.Instance != null)
            UpdateManager.Instance.Register(this);
    }

    private void OnDestroy()
    {
        if (UpdateManager.Instance != null)
            UpdateManager.Instance.Unregister(this);
    }
}
