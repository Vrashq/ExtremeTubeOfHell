using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
	[SerializeField]
	private Image _splashScreen;
	[SerializeField]
	private Text _progressText;
	[SerializeField]
	private Canvas _menu;

	public IEnumerator AnimateSplashScreen ()
	{
		yield return new WaitForSeconds(2f);
		_splashScreen.CrossFadeAlpha(0, 0.5f, true);
		SetProgress(0);
		ShowMenu(true);
		yield return new WaitForSeconds(0.5f);
	}

	public void SetProgress (float progress)
	{
		_progressText.text = "Loading: " + (Mathf.Floor(progress * 100.0f)).ToString() + "%";
	}

	public void SetWaitForInput ()
	{
		_progressText.text = "Tap to Play !";
	}

	public void ShowMenu (bool value)
	{
		_menu.gameObject.SetActive(value);
	}
}
