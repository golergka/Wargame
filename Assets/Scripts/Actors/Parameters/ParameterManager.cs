using UnityEngine;
using System;
using System.Collections.Generic;

// it's a stub for a full-on parameter system!

public abstract class Parameter {

}

[Serializable]
public class IntParameter : Parameter {

	public int value { get; set; }

	public override string ToString() {
		return value.ToString();
	}

}

[Serializable]
public class FloatParameter : Parameter {

	public float value { get; set; }

	public override string ToString() {
		return value.ToString();
	}

}

public class ParameterManager : MonoBehaviour {

	public List<string> intParameterNames = new List<string>();
	public List<IntParameter> intParameterBases = new List<IntParameter>();

	public List<string> floatParameterNames = new List<string>();
	public List<FloatParameter> floatParameterBases = new List<FloatParameter>();

	public Dictionary<string, Parameter> parameters;

	void Awake() {

		parameters = new Dictionary<string, Parameter>();

		if (intParameterBases.Count != intParameterNames.Count) {
			Debug.LogError("Inconsistent int parameter count!");
			return;
		}

		if (floatParameterBases.Count != floatParameterNames.Count) {
			Debug.LogError("Inconsistent int parameter count!");
			return;
		}

		for (int i=0; i<intParameterNames.Count; i++)
			parameters.Add(intParameterNames[i], intParameterBases[i]);

		for (int i=0; i<floatParameterNames.Count; i++)
			parameters.Add(floatParameterNames[i], floatParameterBases[i]);

	}

}
