using UnityEngine;
using System;
using System.Collections.Generic;

// it's a stub for a full-on parameter system!

public abstract class Parameter {

}

[Serializable]
public class IntParameter : Parameter {

	public IntParameter(int defaultValue) {
		value = defaultValue;
	}

	public int value { get; set; }

	public override string ToString() {
		return value.ToString();
	}

}

[Serializable]
public class FloatParameter : Parameter {

	public FloatParameter(float defaultValue) {
		value = defaultValue;
	}

	public float value { get; set; }

	public override string ToString() {
		return value.ToString();
	}

}

public class ParameterManager : MonoBehaviour {

	public Dictionary<string, Parameter> parameters;

// These are used to save objects, and are transformed to real Parameters at Awake
#region Parameter factories

	public List<string> intParameterNames = new List<string>();
	public List<int> intParameterBases = new List<int>();

	public List<string> floatParameterNames = new List<string>();
	public List<float> floatParameterBases = new List<float>();

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
			parameters.Add(intParameterNames[i], new IntParameter(intParameterBases[i]));

		for (int i=0; i<floatParameterNames.Count; i++)
			parameters.Add(floatParameterNames[i], new FloatParameter(floatParameterBases[i]));

	}

#endregion

}
