using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(PartyController))]
public class PartyControllerEditor : Editor {
	
	PartyController partyController;
	
	void OnEnable() {
		
		partyController = (PartyController) target;
		
	}
	
	public override void OnInspectorGUI() {
		
		DrawDefaultInspector();
		if ( GUILayout.Button("Populate") ) {
			
			partyController.heroes = new List<HeroController> ( (HeroController[] ) FindObjectsOfType(typeof(HeroController)) );
			
		}
		
	}
	
}
