using UnityEngine;
using UnityEngine.UI;

public class PauseMenuSystem : IPauseMenu
{
    private Canvas _pauseCanvas;
    private Slider _musicSlider;
    private Slider _sfxSlider;

    private bool _isPaused = false;
    public bool IsPaused => _isPaused;

    public PauseMenuSystem(Canvas pauseCanvas, Slider musicSlider, Slider sfxSlider)
    {
        _pauseCanvas = pauseCanvas;
        _musicSlider = musicSlider;
        _sfxSlider = sfxSlider;

        _pauseCanvas.enabled = false;

        if (GlobalAudioController.Instance != null)
        {
            if (GlobalAudioController.Instance.TryGetMixerVolume("MUSICVolume", out float musicDb))
                _musicSlider.value = Mathf.InverseLerp(-80f, 0f, musicDb);

            if (GlobalAudioController.Instance.TryGetMixerVolume("SFXVolume", out float sfxDb))
                _sfxSlider.value = Mathf.InverseLerp(-80f, 0f, sfxDb);

            _musicSlider.onValueChanged.AddListener(GlobalAudioController.Instance.SetMusicVolumeFromSlider);
            _sfxSlider.onValueChanged.AddListener(GlobalAudioController.Instance.SetSFXVolumeFromSlider);
        }

        GameManager.Instance.Register(this);
    }

    public void CustomUpdate(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        _pauseCanvas.enabled = _isPaused;
        Time.timeScale = _isPaused ? 0f : 1f;
    }

    public void Dispose()
    {
        UpdateManager.Instance.Unregister(this);

        if (GlobalAudioController.Instance != null)
        {
            GlobalAudioController.Instance.UnregisterSliders(_musicSlider, _sfxSlider);
        }
    }
}
