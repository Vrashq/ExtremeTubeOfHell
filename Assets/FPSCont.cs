using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSCont : MonoBehaviour {
	
	void Start ()
	{
		StartCoroutine(FPS_Enumerator());
	}

	IEnumerator FPS_Enumerator ()
	{
		while(true)
		{
			float msec = Time.deltaTime * 1000.0f;
			float fps = 1.0f / Time.deltaTime;
			GetComponent<Text>().text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
			yield return new WaitForSeconds(0.5f);
		}
	}
}