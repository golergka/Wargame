using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

//[CustomEditor(typeof(ParameterManager))]
public class ParameterManagerEditor : Editor {

	ParameterManager parameterManager;

	void Awake() {

		parameterManager = (ParameterManager) target;

	}

	bool showParameters;
	bool showMissingParameters;

	public override void OnInspectorGUI() {

		if (parameterManager.parameters != null) {

			EditorGUILayout.LabelField("Play mode: parameters are immutable");

			showParameters = EditorGUILayout.Foldout(showParameters, "Parameters");

			if (showParameters)
				foreach(KeyValuePair<string, Parameter> p in parameterManager.parameters)
					EditorGUILayout.LabelField(p.Key, p.Value.ToString() );

			EditorGUILayout.Space();

			showMissingParameters = EditorGUILayout.Foldout(showMissingParameters,
				"Missing parameters ( " + parameterManager.missingParameters.Count.ToString() + " )");

			if (showMissingParameters)
				foreach(string missing in parameterManager.missingParameters)
					EditorGUILayout.LabelField(missing);

		}

	}
	
}
