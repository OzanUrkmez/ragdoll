using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TypeConstraintAttribute))]
public class TypeConstraintDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.ObjectReference)
        {
            // Also check that the user only uses the attribute on a GameObject or Component
            // because we need to call GetComponent
            Debug.LogError(string.Format("{0} - {1}: This drawer must be used only on Object types",
                property.serializedObject.targetObject.GetType().ToString(), property.displayName));
            return;
        }

        var constraint = attribute as TypeConstraintAttribute;

        Event evt = Event.current;
        if (DragAndDrop.objectReferences.Length > 0 && position.Contains(evt.mousePosition))
        {
            UnityEngine.Object draggedObject = DragAndDrop.objectReferences[0];
            try
            {
                var interf = Convert.ChangeType(draggedObject, constraint.Type);
            }
            catch
            {
                // Prevent dragging of an object that doesn't contain the interface type.
                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;

                if (evt.type == EventType.DragExited)
                    Debug.LogError(string.Format("Object assigned to '{0}' must implement interface '{1}'", property.name, constraint.Type));
            }

        }

        property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(GameObject), true);

        //If a value was set through other means(e.g.ObjectPicker)
        if (property.objectReferenceValue != null)
        {
            // Check if the interface is present.
            Component go = property.objectReferenceValue as Component;
            if (go != null && go.GetComponent(constraint.Type) == null)
            {
                // Clean out invalid references.
                property.objectReferenceValue = null;
                Debug.LogError(string.Format("Object assigned to '{0}' must implement interface '{1}'", property.name, constraint.Type));
            }
        }
    }
}