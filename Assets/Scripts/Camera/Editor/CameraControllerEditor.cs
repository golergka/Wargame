using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor {

	CameraController cameraController;
	Transform cameraTarget;
	float cameraDistance {
		get {
		
			return Vector3.Distance(cameraController.transform.position, cameraTarget.position);

		}
		set {

			cameraController.cameraOffset.Normalize();
			cameraController.cameraOffset *= value;

		}
	}

	void OnEnable() {

		cameraController = (CameraController) target;

	}
	
	public override void OnInspectorGUI() {

		DrawDefaultInspector();
		EditorGUILayout.LabelField("Example target is used to help setup camera");
		cameraTarget = (Transform) EditorGUILayout.ObjectField(cameraTarget, typeof(Transform), true);

		if (cameraTarget != null) {

			cameraController.transform.position = cameraTarget.transform.position + cameraController.cameraOffset;
			cameraController.transform.LookAt(cameraTarget.position + cameraController.cameraTargetOffset);
			cameraDistance = EditorGUILayout.FloatField("Camera distnace:", cameraDistance);

		}

	}

}
