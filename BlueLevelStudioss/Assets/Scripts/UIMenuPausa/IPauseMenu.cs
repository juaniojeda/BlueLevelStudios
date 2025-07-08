using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IPauseMenu : ICustomUpdate
{
    void TogglePause();
    bool IsPaused { get; }
}
