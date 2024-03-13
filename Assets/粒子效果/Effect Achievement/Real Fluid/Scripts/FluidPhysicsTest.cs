using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidPhysicsTest : MonoBehaviour
{
    public Vector2 generateRange = new Vector2(-10, 10);

    public GameObject testObjectPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Instantiate(testObjectPrefab, 
                new Vector3(transform.position.x + Random.Range(generateRange.x, generateRange.y), transform.position.y, 0),
                Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector3(transform.position.x + generateRange.x, transform.position.y, 0),
            new Vector3(transform.position.x + generateRange.y, transform.position.y, 0));
    }
}
