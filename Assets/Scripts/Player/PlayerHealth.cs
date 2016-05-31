using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
	public int Health = 100;
	public Text GameSpeedText
	{
		get
		{
			if(!_healthText)
			{
				_healthText = GameManager.Instance.PlayerGUI.transform.GetChild(0).GetComponent<Text>();
			}
			return _healthText;
		}
		set
		{
			_healthText = value;
		}
	}
	private Text _healthText;

	public void EnableGUI()
	{
		GameSpeedText.gameObject.SetActive(true);
	}

	public void DisableGUI()
	{
		GameSpeedText.gameObject.SetActive(false);
	}

	public void OnHealthChanged ()
	{
		GameSpeedText.text = "GameSpeed: " + (Mathf.Floor(GameManager.Instance.GameSpeedScale * 10.0f) / 10.0f).ToString();
	}

	public void Upgrade ()
	{
		Health += 5;
		OnHealthChanged();
	}

	public void Downgrade ()
	{
		Health -= 5;
		OnHealthChanged();
	}
}
