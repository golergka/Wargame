using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	
	void HeroList() {
		
		GUILayout.BeginHorizontal( GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) );
		
		foreach (PlayerController hero in PartyController.instance.heroes)
			if ( GUILayout.Button (hero.name, GUILayout.ExpandHeight(true) ) )
				PartyController.instance.Select(hero);
				
		
		GUILayout.EndHorizontal();
		
	}
	
	void PauseSwitch() {
		
		if (Time.timeScale == 0f) {
			
			if ( GUILayout.Button ("Play", GUILayout.ExpandHeight(true)) )
				Time.timeScale = 1f;
			
		} else {
			
			if (GUILayout.Button ("Pause", GUILayout.ExpandHeight(true)) )
				Time.timeScale = 0f;
			
		}
		
	}
	
	void TacticalButtons() {
		
		GUILayout.BeginVertical( GUILayout.ExpandHeight(true) );
		
		if (PartyController.instance.followLeader) {
			
			if ( GUILayout.Button ("Stop following", GUILayout.ExpandHeight(true)) ) {
				
				PartyController.instance.followLeader = false;
				
			}
			
		} else {
			
			if ( GUILayout.Button ("Follow", GUILayout.ExpandHeight(true)) ) {
				
				PartyController.instance.followLeader = true;
				
			}
			
			if ( GUILayout.Button ("Go to leader", GUILayout.ExpandHeight(true)) ) {
				
				foreach(PlayerController hero in PartyController.instance.heroes)
					if (hero != PartyController.instance.leader)
						hero.Pursue(PartyController.instance.leader.transform);
				
			}
			
		}
		
		
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
