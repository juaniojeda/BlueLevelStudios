using UnityEngine;
public class BrickGenerator
{
    private BrickPool pool;
    private BrickGeneratorConfig config;
    private Transform origin;   // de dónde sale la primera posición
    private Transform parent;   // a dónde van tus bricks
    

    public BrickGenerator(BrickPool pool, BrickGeneratorConfig config,Transform startTransform, Transform parentTransform)
    {
        this.pool = pool;
        this.config = config;
        origin = startTransform;
        parent = parentTransform;
    }

    public void GenerateBricks()
    {
        Vector3 startPos = origin.position;

        for (int y = 0; y < config.rows; y++)
        {
            for (int x = 0; x < config.columns; x++)
            {
                Vector3 pos = startPos + new Vector3(x * (1 + config.spacing), y * (0.5f + config.spacing), 0f);

                var brick = pool.GetBrick(pos);

                brick.transform.SetParent(parent, worldPositionStays: true);
            }
        }
    }
}
