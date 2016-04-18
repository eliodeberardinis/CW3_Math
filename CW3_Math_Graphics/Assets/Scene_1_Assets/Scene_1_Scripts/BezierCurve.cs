using UnityEngine;

public class BezierCurve : MonoBehaviour
{

    public Vector3[] points;

 

    public Vector3 GetPoint(float t)
    {
       // return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], t)); //For use with quadratic
        return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t)); //For use with Cubic
    }

    public Vector3 GetVelocity(float t) //to Show velocity lines along the curve
    {
       // return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], t)) - transform.position; //for use with quadratic


        return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) - transform.position; //for use with cubic

    }

    public Vector3 GetDirection(float t) //It just normalizes the velocity
    {
        return GetVelocity(t).normalized;
    }

    public void Reset()
    {
        points = new Vector3[] {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f)
        };
    }

}