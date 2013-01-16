using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(ParameterManager))]
public class ParameterManagerEditor : Editor {

	ParameterManager parameterManager;

	void Awake() {

		parameterManager = (ParameterManager) target;

	}

	public override void OnInspectorGUI() {

		if (parameterManager.parameters == null) {

			EditorGUILayout.LabelField("Edit mode — unimplemented");


		} else {

			EditorGUILayout.LabelField("Play mode");

			foreach(KeyValuePair<string, Parameter> p in parameterManager.parameters)
				EditorGUILayout.LabelField(p.Key, p.Value.ToString() );

		}

	}
	
}
