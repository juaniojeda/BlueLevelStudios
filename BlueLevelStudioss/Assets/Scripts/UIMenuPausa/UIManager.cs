using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Pausa")]
    public Canvas pauseCanvas;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Temporizador")]
    public TextMeshProUGUI timerText;

    [Header("Ladrillos")]
    public TextMeshProUGUI brickCounterText;

    [Header("Rebotes en Paleta")]
    public TextMeshProUGUI paddleBounceText;

    [Header("Vidas")]
    public TextMeshProUGUI lifeText;

    private PauseMenuSystem _pauseMenu;
    private UITimerDisplay _timerDisplay;
    private UIBrickCounterDisplay _brickCounterDisplay;
    private UIPaddleBounceDisplay _paddleBounceDisplay;
    private LifeSystem _lifeSystem;

    public void Initialize(ITimeProvider timeProvider, BallController mainBall)
    {
        // Men? de pausa
        _pauseMenu = new PauseMenuSystem(pauseCanvas, musicSlider, sfxSlider);

        // Temporizador
        _timerDisplay = new UITimerDisplay(timerText, timeProvider);

        // Ladrillos
        _brickCounterDisplay = new UIBrickCounterDisplay(brickCounterText);

        // Rebotes en paleta (aqu? estaba el error)
        _paddleBounceDisplay = new UIPaddleBounceDisplay(paddleBounceText, mainBall);

        // Sistema de vidas
        _lifeSystem = new LifeSystem(3, lifeText);
        _lifeSystem.SetMainBall(mainBall);
        LifeSystemAccess.Register(_lifeSystem);
    }

    private void OnDestroy()
    {
        _pauseMenu?.Dispose();
        _timerDisplay?.Dispose();
        _brickCounterDisplay?.Dispose();
        _paddleBounceDisplay?.Dispose();
        _lifeSystem?.Dispose();
        LifeSystemAccess.Clear();
    }
}
