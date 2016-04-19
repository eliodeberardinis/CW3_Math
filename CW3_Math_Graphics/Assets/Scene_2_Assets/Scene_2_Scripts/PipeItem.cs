using UnityEngine;

public class PipeItem : MonoBehaviour
{
    //Instance of the obstacle rotater
    private Transform rotater;

    //Initialize the obstacle's rotator
    private void Awake()
    {
        rotater = transform.GetChild(0);
    }

    //Set the position of the obstacle on the pipe's internal surface taking into account the correct position along the curve and rotation
    public void Position(Pipe pipe, float curveRotation, float ringRotation)
    {
        transform.SetParent(pipe.transform, false);
        transform.localRotation = Quaternion.Euler(0f, 0f, -curveRotation);
        rotater.localPosition = new Vector3(0f, pipe.CurveRadius);
        rotater.localRotation = Quaternion.Euler(ringRotation, 0f, 0f);
    }
}