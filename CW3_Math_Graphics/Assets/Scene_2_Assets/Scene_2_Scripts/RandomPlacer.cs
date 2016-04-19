using UnityEngine;

//Script to place obstacles inside the pipes in random position
public class RandomPlacer : PipeItemGenerator
{
    //Instanc eof the obstacles prefabs
    public PipeItem[] itemPrefabs;

    public override void GenerateItems(Pipe pipe)
    {
        float angleStep = pipe.CurveAngle / pipe.CurveSegmentCount;
        for (int i = 0; i < pipe.CurveSegmentCount; i++)
        {
            PipeItem item = Instantiate<PipeItem>(
                itemPrefabs[Random.Range(0, itemPrefabs.Length)]); //position them in a way that leave the player the possibility to avoid them
            float pipeRotation =
                (Random.Range(0, pipe.pipeSegmentCount) + 0.5f) * //And don't overlap across different pipe sections
                360f / pipe.pipeSegmentCount;
            item.Position(pipe, i * angleStep, pipeRotation);
        }
    }
}