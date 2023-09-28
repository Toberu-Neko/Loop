using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour, IMapDamageableItem
{
	[SerializeField] private float life = 3;


    [Tooltip("持續時間")]
    [SerializeField] private float maxShakeDuration = 0f;

    [Tooltip("晃動程度")]
    [SerializeField] private float shakeMagnitude = 0.25f;

    Vector3 initialPosition;
    private float shakeDuration = 0f;

    void Awake()
	{
		initialPosition = transform.position;
	}

    void Update()
    {
		if (shakeDuration > 0)
		{
			transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

			shakeDuration -= Time.deltaTime;
		}
		else
		{
			shakeDuration = 0f;
			transform.localPosition = initialPosition;
		}
	}


    public void TakeDamage(float damage)
    {
        life -= 1;
        shakeDuration = maxShakeDuration;

        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
