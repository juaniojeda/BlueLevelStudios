using UnityEngine;

[CreateAssetMenu(fileName = "BrickGeneratorConfig", menuName = "Config/Brick Generator")]
public class BrickGeneratorConfig : ScriptableObject
{
    public int rows = 3;
    public int columns = 5;
    public float spacing = 0.1f;
}