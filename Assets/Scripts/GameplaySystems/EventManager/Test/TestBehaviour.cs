using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//public class TestBehaviour : MonoBehaviour
//{
//    [SerializeReference] public INode Node;
//}

public class TestBehaviour : MonoBehaviour
{
    [SerializeReference]
    public List<INode> nodes = new List<INode>();


	private void Start()
	{
		foreach(INode node in nodes)
		{
			node.PrintStrings();
		}
	}
}