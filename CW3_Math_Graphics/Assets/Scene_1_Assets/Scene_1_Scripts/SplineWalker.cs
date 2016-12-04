using UnityEngine;

public class SplineWalker : MonoBehaviour
{

    public BezierSpline spline;  //Reference to the spline to which the object is attached to
    public float duration;      //The time the object takes to go around the spline
    private float progress;    //The amount of time that has passed since the object started moving along the spline
    public bool lookForward;  //Variable to check if the object is facing the direction it is moving towards

    //Enum for the walker mode of tracing the path. Once, in a loop or back and forth
    public enum SplineWalkerMode
    {
        Once,
        Loop,
        PingPong
    }

    public SplineWalkerMode mode;

    //Bool variable to check if the walker is facing the direction it is moving towards
    private bool goingForward = true;

    private void Update()
    {
        //The update function refreshes at every frame taking into account the movement of the object along the spline according to its speed and duration and updates the progress variable
        if (goingForward) //This is done either when the object is facing forward
        {
            progress += Time.deltaTime / duration;
            if (progress > 1f)
            {
                if (mode == SplineWalkerMode.Once)
                {
                    progress = 1f;
                }
                else if (mode == SplineWalkerMode.Loop)
                {
                    progress -= 1f;
                }
                else
                {
                    progress = 2f - progress;
                    goingForward = false;
                }
            }
        }
        else //or not by inverting the sign
        {
            progress -= Time.deltaTime / duration;
            if (progress < 0f)
            {
                progress = -progress;
                goingForward = true;
            }
        }

        //updates the position of the object along the spline
        Vector3 position = spline.GetPoint(progress);
        transform.localPosition = position;
        if (lookForward)
        {
            //Adjust the rotation of the object and forces to look forward if the relevant bool is set to true
            transform.LookAt(position + spline.GetDirection(progress));
        }
    }
}