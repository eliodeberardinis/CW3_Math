using UnityEngine;

//This script concatenates the pipes together and keeps track of their position and rotation and moves them around the origin where the player is set
public class PipeSystem : MonoBehaviour
{
    //References to the prefabs, the number of pipes in the scene and other variables
    public Pipe pipePrefab;
    public int pipeCount;
    private Pipe[] pipes;
    public int emptyPipeCount;

    //Initializes the pipe system by instantiating the pipes prefab
    private void Awake()
    {
        pipes = new Pipe[pipeCount];
        for (int i = 0; i < pipes.Length; i++)
        {
            Pipe pipe = pipes[i] = Instantiate<Pipe>(pipePrefab);
            pipe.transform.SetParent(transform, false);
        }
    }

    //Sets up the first pipe
    public Pipe SetupFirstPipe()
    {
        for (int i = 0; i < pipes.Length; i++)
        {
            Pipe pipe = pipes[i];
            pipe.Generate(i > emptyPipeCount); //Generates them free of obstacles
            if (i > 0)
            {
                pipe.AlignWith(pipes[i - 1]);
            }
        }
        AlignNextPipeWithOrigin(); //Aligns it with the origin where the player is
        transform.localPosition = new Vector3(0f, -pipes[1].CurveRadius);
        return pipes[1];
    }

    //Sets up the subsequnt pipes in a similar way of the first pipe
    public Pipe SetupNextPipe()
    {
        ShiftPipes();// shift the pipes in its array 
        AlignNextPipeWithOrigin(); // align the next pipe with the origin
        pipes[pipes.Length - 1].Generate(); 
        pipes[pipes.Length - 1].AlignWith(pipes[pipes.Length - 2]);
        transform.localPosition = new Vector3(0f, -pipes[1].CurveRadius); // resets its position.
        return pipes[1];
    }

    //Takes into account the pipe's length to attach them appropriately by shifting the old one around the origin by their length
    private void ShiftPipes()
    {
        Pipe temp = pipes[0];
        for (int i = 1; i < pipes.Length; i++)
        {
            pipes[i - 1] = pipes[i];
        }
        pipes[pipes.Length - 1] = temp; //The current first pipe becomes the new last pipe.
    }

    //Makes sure every new pipe aligns with the origin
    private void AlignNextPipeWithOrigin() 
    {
        Transform transformToAlign = pipes[1].transform; //resetting its position and rotation.
        for (int i = 0; i < pipes.Length; i++)// To make sure that all other pipes move along with it, just temporarily make them children of that pipe.
        {
            if (i != 1)
            {
                pipes[i].transform.SetParent(transformToAlign); //This is achieved by temporarely switching the objects parent transform
            }
        }

        transformToAlign.localPosition = Vector3.zero;
        transformToAlign.localRotation = Quaternion.identity;

        for (int i = 0; i < pipes.Length; i++) //Then switching it back 
        {
            if (i != 1)
            {
                pipes[i].transform.SetParent(transform);
            }
        }
    }
}