using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	
	void HeroList() {
		
		GUILayout.BeginHorizontal( GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) );
		
		foreach (HeroController hero in PartyController.instance.heroes)
			if ( GUILayout.Button (hero.name, GUILayout.ExpandHeight(true) ) )
				PartyController.instance.Select(hero);
				
		
		GUILayout.EndHorizontal();
		
	}

	float timeScale = 1f;
	bool paused = false;
	
	void GameSpeedSwitch() {

		bool dirty = false;

		GUILayout.BeginVertical( GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) );

		GUILayout.BeginHorizontal( GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) );

		if ( GUILayout.Button("x1/2", GUILayout.ExpandHeight(true)) ) {
			timeScale = 0.5f;
			dirty = true;
		}

		if ( GUILayout.Button("x1", GUILayout.ExpandHeight(true)) ) {
			dirty = true;
			timeScale = 1f;
		}

		if ( GUILayout.Button("x2", GUILayout.ExpandHeight(true)) ) {
			dirty = true;
			timeScale = 2f;
		}

		GUILayout.EndHorizontal();
		
		if (paused) {
			
			if ( GUILayout.Button ("Play", GUILayout.ExpandHeight(true)) ) {
				dirty = true;
				paused = false;
			}
			
		} else {
			
			if (GUILayout.Button ("Pause", GUILayout.ExpandHeight(true)) ) {
				dirty = true;
				paused = true;
			}
			
		}

		GUILayout.EndVertical();

		if (dirty) {
			if (paused)
				Time.timeScale = 0f;
			else
				Time.timeScale = timeScale;
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
				
				foreach(HeroController hero in PartyController.instance.heroes)
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
		
		GameSpeedSwitch();
		
		TacticalButtons();
		
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		// end of bottom control block
		
	}
}
