using UnityEditor;
using UnityEngine;

//Custom Editor/Inspector for the BezierCurve component (line of code already described in the LineInspector script are not repeated)
[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor
{
    //References to the Bezier Curve, transofrm and rotation
    private BezierCurve curve;
    private Transform handleTransform;
    private Quaternion handleRotation;

    
    private const int lineSteps = 10; // Used to refine the pace so we don't see straight lines between points. (We know there are straight lines but we don't want to see them!)
    private const float directionScale = 0.5f; //Rescale factor for the velocity lines

    //Draws the curve and handles the appropriate transformation of control points between the local (curve) space and world space 
    private void OnSceneGUI()
    {
        curve = target as BezierCurve; 
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

        //Show the control points
        Vector3 p0 = ShowPoint(0);
        Vector3 p1 = ShowPoint(1);
        Vector3 p2 = ShowPoint(2);
        Vector3 p3 = ShowPoint(3);

        //Draw lines between control points
        Handles.color = Color.grey;
        Handles.DrawLine(p0, p1);
        Handles.DrawLine(p1, p2);
        Handles.DrawLine(p2, p3);

        //Draw the Bezier Curve
        ShowDirections();
        Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);

    }

    //Draw the "Velocity Lines along the curve"
    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = curve.GetPoint(0f);
        Handles.DrawLine(point, point + curve.GetDirection(0f) * directionScale);
        for (int i = 1; i <= lineSteps; i++) //Sets the velocity line so that they don't clutter
        {
            point = curve.GetPoint(i / (float)lineSteps);
            Handles.DrawLine(point, point + curve.GetDirection(i / (float)lineSteps) * directionScale);
        }
    }

    //Carry out the necessary transformations between world and local space for the control points (The same as for the line)
    private Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(curve.points[index]);
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            curve.points[index] = handleTransform.InverseTransformPoint(point);
        }
        return point;
    }
}