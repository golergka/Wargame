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

			if (parameterManager.intParameterBases.Count != parameterManager.intParameterNames.Count) {
				Debug.LogError("Inconsistent int parameter count!");
				return;
			}

			List<int> parametersToRemove = new List<int>();

			for (int i=0; i<parameterManager.intParameterNames.Count; i++) {
				EditorGUILayout.BeginVertical();

				EditorGUILayout.BeginHorizontal();
				parameterManager.intParameterNames[i] = EditorGUILayout.TextField(parameterManager.intParameterNames[i]);
				parameterManager.intParameterBases[i] = EditorGUILayout.IntField(parameterManager.intParameterBases[i]);
				EditorGUILayout.EndHorizontal();

				if (GUILayout.Button("Delete"))
					parametersToRemove.Add(i);

				EditorGUILayout.EndVertical();
			}

			foreach(int i in parametersToRemove) {
				parameterManager.intParameterBases.RemoveAt(i);
				parameterManager.intParameterNames.RemoveAt(i);
			}

			if (GUILayout.Button("Add")) {
				parameterManager.intParameterNames.Add("New parameter");
				parameterManager.intParameterBases.Add(0);
			}

			// if (parameterManager.floatParameterBases.Count != parameterManager.floatParameterNames.Count) {
			// 	Debug.LogError("Inconsistent int parameter count!");
			// 	return;
			// }



		} else {

			EditorGUILayout.LabelField("Play mode: parameters are immutable");

			foreach(KeyValuePair<string, Parameter> p in parameterManager.parameters)
				EditorGUILayout.LabelField(p.Key, p.Value.ToString() );

		}

	}
	
}
