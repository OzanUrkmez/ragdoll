using UnityEditor;
using UnityEngine;


public class TypeConstraintAttribute : PropertyAttribute
{
    private System.Type type;

    public TypeConstraintAttribute(System.Type type)
    {
        this.type = type;
    }

    public System.Type Type
    {
        get { return type; }
    }
}