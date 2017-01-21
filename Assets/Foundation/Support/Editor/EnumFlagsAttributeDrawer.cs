using UnityEngine;
using UnityEditor;

// from: answers.unity3d.com/questions/486694/default-editor-enum-as-flags.html
[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsAttributeDrawer : PropertyDrawer
{
	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
	{
		property.intValue = EditorGUI.MaskField( position, label, property.intValue, property.enumNames );
	}
}
