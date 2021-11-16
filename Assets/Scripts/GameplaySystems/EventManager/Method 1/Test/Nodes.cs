
using System;
using UnityEngine;
public interface INode
{
    string DisplayText { get; set; }
    void PrintStrings();
}

[Serializable]
public class Node : INode
{
    public virtual void PrintStrings()
    {
        Debug.Log(_displayText);
	}

    [SerializeField] private string _displayText = "display text";
    public string DisplayText
    {
        get => _displayText;
        set => _displayText = value;
    }

}
[Serializable]
public class DerivedNode : Node
{

	public override void PrintStrings()
	{
        base.PrintStrings();
        Debug.Log(_displayTextDerived);
        _object.SetActive(true);
	}
	[SerializeField] private string _displayTextDerived = "derived";

    [SerializeField] private GameObject _object;
}