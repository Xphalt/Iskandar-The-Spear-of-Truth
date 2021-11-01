using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class EventManager : MonoBehaviour
{
	[System.Serializable]
	public struct Action
	{
        [SerializeReference]
        [SelectImplementation(typeof(Condition))]
        public List<Condition> conditions;
		public List<Event> events;
	}

    
	[SerializeReference] 
    [SelectImplementation(typeof(Condition))]
    public Condition Condition;

	//[SerializeReference] public List<Condition> testCondition 
	//    = new List<Condition>()
	//    {  
	//        new KillCondition(),
	//        new KillCondition()
	//    }
	//;

	[SerializeField] private List<Action> actions;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public class SelectImplementationAttribute : PropertyAttribute
{
    public Type FieldType;

    public SelectImplementationAttribute(Type fieldType)
    {
        FieldType = fieldType;
    }
}

[CustomPropertyDrawer(typeof(SelectImplementationAttribute))]
public class SelectImplementationDrawer : PropertyDrawer
{
    private Type[] _implementations;
    private int _implementationTypeIndex;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (_implementations == null || GUILayout.Button("Refresh implementations"))
        {
            _implementations = GetImplementations((attribute as SelectImplementationAttribute).FieldType)
                .Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
        }

        EditorGUILayout.LabelField($"Found {_implementations.Count()} implementations");

        _implementationTypeIndex = EditorGUILayout.Popup(new GUIContent("Implementation"),
            _implementationTypeIndex, _implementations.Select(impl => impl.FullName).ToArray());

        if (GUILayout.Button("Create instance"))
        {
            property.managedReferenceValue = Activator.CreateInstance(_implementations[_implementationTypeIndex]);
        }
        EditorGUILayout.PropertyField(property, true);
    }

    public static Type[] GetImplementations(Type interfaceType)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
        return types.Where(p => interfaceType.IsAssignableFrom(p) && !p.IsAbstract).ToArray();
    }
}
