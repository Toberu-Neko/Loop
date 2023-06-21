using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float FollowSpeed = 2f;
	[SerializeField] private Transform Target;

	// How long the object should shake for.
	[SerializeField] private float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	[SerializeField] private float shakeAmount = 0.1f;
	[SerializeField] private float decreaseFactor = 1.0f;

	Vector3 originalPos;

	void Awake()
	{
		Cursor.visible = false;
	}

	void OnEnable()
	{
		originalPos = transform.localPosition;
	}

	private void Update()
	{
		Vector3 newPosition = Target.position;
		newPosition.z = -10;
		transform.position = Vector3.Slerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);

		if (shakeDuration > 0)
		{
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
	}

	public void ShakeCamera()
	{
		originalPos = transform.localPosition;
		shakeDuration = 0.2f;
	}
}
