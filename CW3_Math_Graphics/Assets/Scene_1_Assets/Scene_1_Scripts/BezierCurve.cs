using UnityEngine;

//Class describing a Cubic Bezier Curve
public class BezierCurve : MonoBehaviour
{
    //Control Points f the Bezier Curve
    public Vector3[] points;

    //Get the point on the curve (parametrized in t)
    public Vector3 GetPoint(float t)
    {
        return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
    }

    //Show Velocity lines along the curve
    public Vector3 GetVelocity(float t) 
    {
 
        return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) - transform.position; 
    }

    //Normalizes the velocity curves for better visual impact
    public Vector3 GetDirection(float t) 
    {
        return GetVelocity(t).normalized;
    }

    //Resets the control points values to default (in a straight line)
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