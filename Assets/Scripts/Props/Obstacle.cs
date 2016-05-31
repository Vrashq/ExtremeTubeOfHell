using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
	void OnEnable ()
	{
		GetComponent<BoxCollider>().isTrigger = true;
	}
}
