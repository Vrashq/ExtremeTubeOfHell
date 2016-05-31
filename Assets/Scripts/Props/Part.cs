using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Part : MonoBehaviour {
	private Transform _root;
	private Material[] _materials;
	private int _childCount;
	private List<Transform> _childrens;

	void Start()
	{
		_childrens = new List<Transform>();
		_root = transform.GetChild(0);
		_childCount = _root.childCount;
		_materials = new Material[_root.transform.childCount];
		for (int i = 0; i < _childCount; ++i)
		{
			Transform child = _root.GetChild(i).GetChild(0);
			_materials[i] = child.GetComponent<MeshRenderer>().material;
			_childrens.Add(child);
		}
	}

	public void UpdateColor (Color color)
	{
		for (var i = 0; i < _materials.Length; ++i)
		{
			_materials[i].SetColor("_EmissionColor", color);
		}
	}

	public void ToggleColliders(bool enable)
	{
		for (var i = 0; i < _childrens.Count; ++i)
		{
			_childrens[i].GetComponent<Collider>().enabled = enable;
		}
	}

}
