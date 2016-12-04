using UnityEngine;

//Abstract class to generate obstacles in the pipe
public abstract class PipeItemGenerator : MonoBehaviour
{

    public abstract void GenerateItems(Pipe pipe);
}