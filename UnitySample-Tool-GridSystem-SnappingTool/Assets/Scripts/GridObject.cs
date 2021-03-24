using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    ////// Gameobject prefab to display the selected building
    [SerializeField] private GameObject prefab;
    ////// Transform to assign a position for the anchor of the building when MouseButtonDown
    [SerializeField] private Transform anchor;
}
