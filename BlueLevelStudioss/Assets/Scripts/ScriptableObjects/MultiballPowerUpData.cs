using UnityEngine;

[CreateAssetMenu(fileName = "MultiballPowerUpData", menuName = "Game/Data/PowerUp/Multiball")]
public class MultiballPowerUpData : ScriptableObject
{
    [Header("Prefab & Chance to Spawn")]
    [Tooltip("Prefab que el jugador debe atrapar para activar el multiball")]
    public GameObject powerUpPrefab;
    [Range(0f, 1f)] public float spawnChance = 0.05f;

    [Header("Falling Behavior")]
    [Tooltip("Velocidad a la que cae el icono del power‑up")]
    public float fallSpeed = 2f;

    [Header("Multiball Effect")]
    [Tooltip("Cuántas bolas extra generar al atrapar")]
    public int count = 2;
    [Tooltip("Ángulo total del abanico en grados")]
    public float spreadAngle = 45f;
}
