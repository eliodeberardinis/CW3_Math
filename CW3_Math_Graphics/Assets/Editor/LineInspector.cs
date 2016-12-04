using UnityEditor;
using UnityEngine;

//Custom Editor/Inspector for the Line component. This will override Unity's default inspector when using Lines.
[CustomEditor(typeof(Line))]
public class LineInspector : Editor {

    //Function used to actually draw the line
	private void OnSceneGUI () {
		Line line = target as Line;                   //Assign the Line component as a target of this inspector
		Transform handleTransform = line.transform;  //Assign the handles to the line component
		Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity; //Assign rotation in Local space
		Vector3 p0 = handleTransform.TransformPoint(line.p0);  //Assign the start and end point of the line to their respective transforms
		Vector3 p1 = handleTransform.TransformPoint(line.p1);

		Handles.color = Color.white;  //Color white
		Handles.DrawLine(p0, p1);     // Draw the line between the points

        EditorGUI.BeginChangeCheck(); //Check the change in local space for the first point

		p0 = Handles.DoPositionHandle(p0, handleRotation);
		if (EditorGUI.EndChangeCheck()) {   //And revert back to global
			Undo.RecordObject(line, "Move Point");
			EditorUtility.SetDirty(line);
			line.p0 = handleTransform.InverseTransformPoint(p0);
		}
		EditorGUI.BeginChangeCheck();
		p1 = Handles.DoPositionHandle(p1, handleRotation); //Do the same for the second point of the line
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(line, "Move Point");
			EditorUtility.SetDirty(line);
			line.p1 = handleTransform.InverseTransformPoint(p1);
		}
	}
}