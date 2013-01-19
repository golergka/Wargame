using UnityEngine;
using System;
using System.Collections.Generic;

// It's a stub for a full-on parameter system!
// THERE BE MONSTERS

// Really, there is A LOT OF CODE DUPLICATION HERE.
// It might look ugly. I'm sorry.
// Every sane C# programmer is going to wonder why didn't I use generics for this.
// But there's a simple explanation: generic parameter class wouldn't be serializable for Unity.
// Therefore, I COPIED CODE. For float and int parameters.
// If you'll have to add another parameter type, I pity you.

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

	const int DEFAULT_INT_VALUE = 0;
	const float DEFAULT_FLOAT_VALUE = 0f;

	public List<String> missingParameters = new List<String>();
	// Parameters that have been asked, but wasn't there
	// For easier tracking of setup failures

	public int GetIntParameter(string key) {

		if (!parameters.ContainsKey(key)) {

			Debug.LogWarning("Parameter not found: " + key);
			if (!missingParameters.Contains(key))
				missingParameters.Add(key);

			return DEFAULT_INT_VALUE;

		}

		Parameter parameter = parameters[key];

		if (! (parameter is IntParameter) ) {
			Debug.LogWarning("Parameter isn't an int: " + key);
			return DEFAULT_INT_VALUE;
		}

		IntParameter intParameter = (IntParameter) parameter;
		return intParameter.value;

	}

	public float GetFloatParameter(string key) {

		Parameter parameter = parameters[key];

		if (parameter == null) {
			Debug.LogWarning("Parameter not found: " + key);
			return DEFAULT_FLOAT_VALUE;
		}

		if (! (parameter is FloatParameter) ) {
			Debug.LogWarning("Parameter isn't a float: " + key);
			return DEFAULT_FLOAT_VALUE;
		}

		FloatParameter flatParameter = (FloatParameter) parameter;
		return flatParameter.value;

	}

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
