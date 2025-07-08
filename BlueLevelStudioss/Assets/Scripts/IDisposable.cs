using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Utilities/IDisposable.cs
namespace YourGame.Utilities
{
    /// <summary>
    /// Interfaz sencilla para liberar recursos (un Register/Unregister puro).
    /// </summary>
    public interface IDisposable
    {
        void Dispose();
    }
}
