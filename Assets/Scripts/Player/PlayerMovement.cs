using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour
{
	private enum EDirection
	{
		Left = -1,
		Right = 1
	}

	[Range(0,360)]
	public int RotationSpeed = 25;

	[SerializeField]
	private Transform _arrowLeft;
	private Material _arrowLeftMaterial;
	[SerializeField]
	private Transform _arrowRight;
	private Material _arrowRightMaterial;
	[SerializeField]
	private EDirection _direction = EDirection.Left;
	[SerializeField]
	private Transform _ship;

	private Material _currentMaterial;
	private Coroutine _glowCoroutine;

	// Use this for initialization
	void Awake ()
	{
		_arrowLeftMaterial = _arrowLeft.GetComponent<MeshRenderer>().material;
		_arrowRightMaterial = _arrowRight.GetComponent<MeshRenderer>().material;
	}

	void Start ()
	{
		ShowDirection();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// If left click
		if (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
		{
			transform.Rotate(transform.forward, (int)_direction * RotationSpeed * Time.deltaTime * GameManager.Instance.GameSpeedScale);
		}
		else if (Input.GetMouseButtonUp(0))
		{
			SwitchDirection();
		}

		if(!Physics.Raycast(_ship.transform.position + _ship.transform.up * -0.35f, _ship.transform.up * -1.0f, 500.0f))
		{

		}
	}

	IEnumerator GlowArrow ()
	{
		float start = 0;
		float end = 1;
		float timer = 0;
		while (true)
		{
			timer += Time.deltaTime * 2;
			float value = Mathf.Lerp(start, end, timer);
			_currentMaterial.SetFloat("_MKGlowPower", value);

			if (timer > 1)
			{
				timer = 0;
				float tmp = start;
				start = end;
				end = tmp;
			}

			yield return null;
		}
	}

	void SwitchDirection ()
	{
		_direction = (EDirection)((int)_direction * -1);
		ShowDirection();
	}

	void ShowDirection ()
	{
		switch(_direction)
		{
			case EDirection.Left:
				_arrowRight.gameObject.SetActive(false);
				_arrowLeft.gameObject.SetActive(true);
				_currentMaterial = _arrowLeftMaterial;
				break;
			case EDirection.Right:
				_arrowRight.gameObject.SetActive(true);
				_arrowLeft.gameObject.SetActive(false);
				_currentMaterial = _arrowRightMaterial;
				break;
		}

		if(_glowCoroutine != null)
		{
			StopCoroutine(_glowCoroutine);
		}
		_glowCoroutine = StartCoroutine(GlowArrow());
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.GetComponent<Obstacle>())
		{
			GameManager.Instance.Downgrade();
		}
		else if(col.transform.parent && col.transform.parent.GetComponent<Indicator>())
		{
			GameManager.Instance.Upgrade();
		}
	}
}
