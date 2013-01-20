using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Vision))]
public class VisionEditor : Editor {

	FloatBaseParameter visionDistanceParameter;

	void OnEnable() {

		Vision vision = (Vision) target;
		ParameterManager parameterManager = vision.GetComponent<ParameterManager>();

		foreach(FloatBaseParameter floatBaseParameter in parameterManager.floatBaseParameters) {
			if (floatBaseParameter.key == Vision.VISION_DISTANCE_KEY) {
				visionDistanceParameter = floatBaseParameter;
				return;
			}
		}

		visionDistanceParameter = new FloatBaseParameter(Vision.VISION_DISTANCE_KEY, default(float) );
		parameterManager.floatBaseParameters.Add( visionDistanceParameter );

	}

	public override void OnInspectorGUI() {
		
		if ( VisibleGrid.instance == null ) {
			EditorGUILayout.LabelField("Please, create VisibleGrid object.");
			return;
		}

		visionDistanceParameter.baseValue = EditorGUILayout.Slider(visionDistanceParameter.baseValue, 0, VisibleGrid.instance.gridStep);
		
	}

}
