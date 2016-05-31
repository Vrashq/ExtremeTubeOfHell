using UnityEngine;
using System.Collections;

public class Pattern : MonoBehaviour
{
	public string[] NextAvailablesPatterns;

	[SerializeField]
	private Part[] _parts;

	void Start()
	{
		_parts = GetComponentsInChildren<Part>();
	}

	public void UpdateColor (Color color)
	{
		for(var i = 0; i < _parts.Length; ++i)
		{
			_parts[i].UpdateColor(color);
		}
	}

	public void ToggleColliders (bool enable)
	{
		for(var i = 0; i < _parts.Length; ++i)
		{
			_parts[i].ToggleColliders(enable);
		}
	}

	public string GetNextPattern()
	{
		return NextAvailablesPatterns[Random.Range(0, NextAvailablesPatterns.Length - 1)];
	}
}
