using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCreationScript : MonoBehaviour
{
    [SerializeField]
    GameObject plane, wall;
    [SerializeField]
    Transform floorTilesParent, wallParents, objectParent;
    // Start is called before the first frame update
    void Awake()
    {        
        //Generate the room
        for (int i = 0; i < 25; i++)
        {
            for (int j = 0; j < 25; j++)
            {
                if (i == 0 || i == 24 || j == 0 || j == 24)
                {
                    Instantiate(wall, new Vector3(i + .5f, 1, j + .5f), Quaternion.identity, wallParents);
                }

                Instantiate(plane, new Vector3(i, 0, j), Quaternion.identity, floorTilesParent);
            }
        }

    }
}
