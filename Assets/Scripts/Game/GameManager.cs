using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum EGameState
{
	Loading,
	Wait,
	Play
}

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public float GameSpeed = 10;
	public float GameSpeedScale = 1;
	public Color[] Colors;
	public Pattern RefPattern;
	public Camera MainCamera;
	public Material RefMaterial;
	public GameObject PlayerRef;
	public Vector3 PlayerOffset = Vector3.zero;
	public MenuManager Menu;
	public Canvas PlayerGUI;

	[SerializeField]
	private List<Pattern> _patterns;
	[SerializeField]
	private Color _currentColor;
	[SerializeField]
	private Color _nextColor;
	private int _colorIndex;
	private GameObject _player;
	private PlayerHealth _playerHealth;

	[SerializeField]
	private EGameState _gameState;
	private bool _isOnMenu;

	// Use this for initialization
	void OnEnable ()
	{
		Instance = this;
	}

	void Start ()
	{
		_patterns = new List<Pattern>();
		_gameState = EGameState.Loading;
		_isOnMenu = true;

		PlayerGUI.gameObject.SetActive(false);

		GameObjectPool.Instance.LoadProgress.AddListener(OnLoadProgress);
		GameObjectPool.Instance.LoadEnd.AddListener(OnLoadEnd);

		StartCoroutine(InitGame());
	}

	IEnumerator InitGame ()
	{
		yield return StartCoroutine(Menu.AnimateSplashScreen());
		yield return StartCoroutine(GameObjectPool.Instance.Init());
	}

	void OnLoadProgress (float progress)
	{
		Menu.SetProgress(progress);
	}

	void OnLoadEnd (float progress)
	{
		for(var c = 0; c < _patterns.Count; ++c)
		{
			Pattern pattern = _patterns[c];
			_patterns.Remove(pattern);
			GameObjectPool.AddObjectIntoPool(pattern.gameObject);
		}
		_patterns.Clear();

		for (var c = 0; c < 6; ++c)
		{
			AddNewPattern();
		}

		_colorIndex = Random.Range(0, Colors.Length);
		_currentColor = Colors[_colorIndex];
		// MainCamera.backgroundColor = _currentColor;
		RefMaterial.SetColor("_MKGlowTexColor", _currentColor);

		StartCoroutine(SwitchColor());
		Menu.SetWaitForInput();
		_gameState = EGameState.Play;
	}

	IEnumerator SwitchColor ()
	{
		float timer = 0;

		_colorIndex = ++_colorIndex % Colors.Length;

		Color baseColor = _currentColor;
		_nextColor = Colors[_colorIndex];

		while (timer < 1)
		{
			timer += Time.deltaTime * 0.2f;
			_currentColor = Color.Lerp(baseColor, _nextColor, timer);
			// MainCamera.backgroundColor = _currentColor;
			for(var i = 0; i < _patterns.Count; ++i)
			{
				_patterns[i].UpdateColor(_currentColor);
			}
			yield return null;
		}

		StartCoroutine(SwitchColor());
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(_gameState == EGameState.Play)
		{
			// GameSpeed += Time.deltaTime;
			for (var p = 0; p < _patterns.Count; ++p)
			{
				Pattern pattern = _patterns[p];
				pattern.transform.localPosition += Vector3.back * Time.deltaTime * GameSpeed * GameSpeedScale;

				if (pattern.transform.localPosition.z <= -24.0f)
				{
					_patterns.Remove(pattern);
					pattern.ToggleColliders(false);
					GameObjectPool.AddObjectIntoPool(pattern.gameObject);

					AddNewPattern();

					for (var i = 0; i < _patterns.Count; ++i)
					{
						_patterns[i].ToggleColliders(_patterns[i].transform.position.z < 24);
					}
				}
			}
			if (Input.GetMouseButtonUp(0) && _isOnMenu)
			{
				Menu.ShowMenu(false);
				_isOnMenu = false;

				_player = (GameObject)Instantiate(PlayerRef, PlayerOffset, Quaternion.identity);
				_player.transform.parent = transform.parent;
				_playerHealth = _player.GetComponent<PlayerHealth>();

				for (var c = 0; c < 3; ++c)
				{
					Pattern pattern = _patterns[_patterns.Count - 1];
					_patterns.Remove(pattern);
					GameObjectPool.AddObjectIntoPool(pattern.gameObject);
				}

				for (var c = 0; c < 3; ++c)
				{
					AddNewPattern();
				}

				PlayerGUI.gameObject.SetActive(true);
			}
		}
	}

	void AddNewPattern()
	{
		GameObject nextPattern = GameObjectPool.GetAvailableObject(_isOnMenu
			? "Pattern00" 
			: _patterns[_patterns.Count - 1].GetNextPattern()
		);

		float z	= _patterns.Count == 0
			? 0
			: _patterns[_patterns.Count - 1].transform.localPosition.z + _patterns[_patterns.Count - 1].transform.childCount * 4;

		nextPattern.transform.position = new Vector3(0, 0, z);
		nextPattern.transform.parent = transform;

		Pattern patternComp = nextPattern.GetComponent<Pattern>();
		patternComp.UpdateColor(_currentColor);

		_patterns.Add(patternComp);
	}

	public void Upgrade ()
	{
		GameSpeedScale += 0.1f;
		_playerHealth.Upgrade();
	}

	public void Downgrade ()
	{
		GameSpeedScale -= 0.05f;
		GameSpeedScale = Mathf.Max(0.5f, GameSpeedScale);
		_playerHealth.Downgrade();
	}
}
