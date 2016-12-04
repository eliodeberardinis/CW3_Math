using UnityEngine;

//Static Class implementing the cubic Bezier Curve and its derivatives
public static class Bezier
{
    //Equation of the Cubic Bezier
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) //Cubic Bezier
    {
        t = Mathf.Clamp01(t); //Nornalizes t between 0 and 1
        float oneMinusT = 1f - t;
        return                                         //Cubic Bezier formula
            oneMinusT * oneMinusT * oneMinusT * p0 +
            3f * oneMinusT * oneMinusT * t * p1 +
            3f * oneMinusT * t * t * p2 +
            t * t * t * p3;
    }

    //Derivative for Cubic Bezier (Velocity)
    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) 
    {
        t = Mathf.Clamp01(t); //Nornalizes t between 0 and 1
        float oneMinusT = 1f - t;
        return
            3f * oneMinusT * oneMinusT * (p1 - p0) +  //Cubic Bezier derivative formula
            6f * oneMinusT * t * (p2 - p1) +
            3f * t * t * (p3 - p2);
    }
}
