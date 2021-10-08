using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;

	// How long the object should shake for.
	public float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	private float _shakeTimer;

	Vector3 originalPos;

	void Awake()
	{
		_shakeTimer = 0;
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	public void Update()
	{
		if (_shakeTimer > 0)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			_shakeTimer -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			_shakeTimer = 0f;
			camTransform.localPosition = originalPos;
		}
	}

	public void Shake()
	{
		Debug.Log("shakin");
		_shakeTimer = shakeDuration;
	}
}