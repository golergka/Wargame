using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(AgroList))]
public class AgroListEditor : Editor {

	AgroList agroList;

	void Awake() {

		agroList = (AgroList) target;

	}

	public override void OnInspectorGUI() {

		Dictionary<Health, int> sortedAgroList = agroList.sortedAgroList;

		if (sortedAgroList != null) {

			if (sortedAgroList.Count == 0)
				EditorGUILayout.LabelField("Agro list is empty");
			else
				EditorGUILayout.LabelField("Agro list:");

			foreach( KeyValuePair<Health, int> agroListMember in sortedAgroList ) {

				GUIStyle style = new GUIStyle();

				if ( agroListMember.Key == agroList.agroLeader)
					style.fontStyle = FontStyle.Bold;

				EditorGUILayout.LabelField(agroListMember.Key.gameObject.name, agroListMember.Value.ToString(), style );
			}

		} else {

			EditorGUILayout.LabelField("Agro list is not initialized");

		}

	}

}
