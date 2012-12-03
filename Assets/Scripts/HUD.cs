using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	
	void HeroList() {
		
		GUILayout.BeginHorizontal( GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) );
		
		foreach (PlayerController hero in PlayerController.characters)
			if ( GUILayout.Button (hero.name, GUILayout.ExpandHeight(true) ) )
				hero.Select ();
				
		
		GUILayout.EndHorizontal();
		
	}
	
	void PauseSwitch() {
		
		if (Time.timeScale == 0f) {
			
			if ( GUILayout.Button ("Play") )
				Time.timeScale = 1f;
			
		} else {
			
			if (GUILayout.Button ("Pause") )
				Time.timeScale = 0f;
			
		}
		
	}
	
	void TacticalButtons() {
		
		GUILayout.BeginVertical( GUILayout.ExpandHeight(true) );
		
		GUILayout.Button ("Follow leader");
		GUILayout.Button ("Go to leader");
		
		GUILayout.EndVertical();
		
	}
	
	public float height = 0.2f;
	public float width = 0.2f;
	
	void OnGUI() {
		
		// bottom control block
		
		float x = Screen.width * width;
		float y = Screen.height * (1 - height);
		float xMax = Screen.width * (1 - width);
		float yMax = Screen.height * height;
		
		GUILayout.BeginArea( new Rect(x,y,xMax,yMax) );
			
		GUILayout.BeginHorizontal( GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) );
		
		HeroList ();
		
		PauseSwitch();
		
		TacticalButtons();
		
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		// end of bottom control block
		
	}
}
