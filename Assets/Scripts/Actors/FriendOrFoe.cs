using UnityEngine;
using System.Collections;

public class FriendOrFoe : MonoBehaviour {

	public enum FoFGroup {

		Player,
		Enemy,
		Wild, // that's default if an actor doesn't have a FriendOrFoe component

	}

	public FoFGroup group;

	static FoFGroup GetFoFGroup(MonoBehaviour someone) {

		FriendOrFoe fofComponent = someone.GetComponent<FriendOrFoe>();

		if (fofComponent == null)
			return FoFGroup.Wild;
		else
			return fofComponent.group;

	}

	public static bool IsEnemy(MonoBehaviour alice, MonoBehaviour bob) {  // it's a commutative operation

		FoFGroup aliceGroup = GetFoFGroup(alice);
		FoFGroup bobGroup = GetFoFGroup(bob);

		if (aliceGroup == FoFGroup.Wild ||
			bobGroup == FoFGroup.Wild)
			return true;

		if (aliceGroup == FoFGroup.Enemy && bobGroup != FoFGroup.Enemy)
			return true;

		if (bobGroup == FoFGroup.Enemy && aliceGroup != FoFGroup.Enemy)
			return true;

		return false;

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
