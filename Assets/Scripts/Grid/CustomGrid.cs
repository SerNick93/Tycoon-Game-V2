using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomGrid :MonoBehaviour
{
    [SerializeField] private float size = 1f;

    public Vector3 GetNearestPoint(Vector3 position)
    {
        //position -= transform.position;
        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);
        
        Vector3 result = new Vector3(
            (float)xCount / size,
            (float)yCount / size,
            (float)zCount / size
            );

        //result += transform.position;
        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int x = 0; x < 26; x++)
        {
            for (int z = 0; z < 26; z++)
            {
                var point = GetNearestPoint(new Vector3(x, 0f, z));
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }
}
