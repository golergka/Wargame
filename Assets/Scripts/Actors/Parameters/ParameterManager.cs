using UnityEngine;
using System;
using System.Collections.Generic;

// It's a stub for a full-on parameter system!
// THERE BE MONSTERS

[Serializable]
public abstract class BaseParameter {

	public string key;

	public abstract Parameter CreateParameter();

}

[Serializable]
public class IntBaseParameter : BaseParameter {

	public int baseValue;

	public IntBaseParameter(string key, int baseValue) {
		this.key = key;
		this.baseValue = baseValue;
	}

	public override Parameter CreateParameter() {

		return new Parameter<int>(baseValue);

	}

}

[Serializable]
public class FloatBaseParameter : BaseParameter {

	public float baseValue;

	public FloatBaseParameter(string key, float baseValue) {
		this.key = key;
		this.baseValue = baseValue;
	}

	public override Parameter CreateParameter() {

		return new Parameter<float>(baseValue);

	}

}

public abstract class Parameter {

}

public class Parameter<T> : Parameter {

	private T _currentValue;
	public T currentValue {
		get { return _currentValue; }
		set {

			_currentValue = value;

			if (ValueChanged != null)
				ValueChanged(this, _currentValue);

		}
	}

	public event Action<Parameter<T>, T> ValueChanged; // sender, new value

	public Parameter(T defaultValue) {
		currentValue = defaultValue;
	}

	public override string ToString() {
		return currentValue.ToString();
	}

}

public class ParameterManager : MonoBehaviour {

	public Dictionary<string, Parameter> parameters;

#region Edit time constructors

	public List<IntBaseParameter> intBaseParameters = new List<IntBaseParameter>();
	public List<FloatBaseParameter> floatBaseParameters = new List<FloatBaseParameter>();

	void Awake() {

		parameters = new Dictionary<string, Parameter>();

		foreach(IntBaseParameter baseParameter in intBaseParameters) {

			if (parameters.ContainsKey(baseParameter.key)) {
				Debug.LogWarning("Duplicate key: " + baseParameter.key);
			} else {

				parameters.Add(baseParameter.key, baseParameter.CreateParameter());

			}

		}

		foreach(FloatBaseParameter baseParameter in floatBaseParameters) {

			if (parameters.ContainsKey(baseParameter.key)) {
				Debug.LogWarning("Duplicate key: " + baseParameter.key);
			} else {

				parameters.Add(baseParameter.key, baseParameter.CreateParameter());

			}

		}

	}

#endregion

#region Runtime accessors

	public List<String> missingParameters = new List<String>();
	// Parameters that have been asked, but wasn't there
	// For easier tracking of setup failures

	public T GetParameterValue<T>(string key) {

		Parameter<T> parameter = GetParameter<T>(key);

		if (parameter == null)
			return default(T);
		else
			return parameter.currentValue;

	}

	public Parameter<T> GetParameter<T>(string key) {

		if (!parameters.ContainsKey(key)) {

			Debug.LogWarning("Parameter not found: " + key + " on object: " + gameObject.name);
			if (!missingParameters.Contains(key))
				missingParameters.Add(key);

			return null;

		}

		Parameter parameter = parameters[key];

		if (parameter is Parameter<T>) {

			Parameter<T> typedParameter = (Parameter<T>) parameter;
			return typedParameter;

		}

		Debug.LogError("Unexpected type. Expected " + typeof(Parameter<T>).ToString() + " got " + parameter.GetType() );
		return null;

	}

	public void SetParameterValue<T>(string key, T value) {

		if (!parameters.ContainsKey(key)) {

			parameters.Add(key, new Parameter<T>(value));

		} else {

			Parameter parameter = parameters[key];

			if (parameter is Parameter<T>) {

				Parameter<T> typedParameter = (Parameter<T>) parameter;
				typedParameter.currentValue = value;

			} else {

				Debug.LogError("Unexpected type. Expected " + typeof(Parameter<T>).ToString() + " got " + parameter.GetType() );

			}

		}

	}

#endregion

}
