using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "Object", menuName = "PlacableObjects/Object")]
public class SO_Object : SO_NewUnlock
{
    //This object does not need all of the stuff from the newunlock object.

    public bool multiPlace;
    public bool placeUnderground;
    public int cost;
    public bool baseObject;

}
