using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    public bool isTakenPlace{get;private set;}
    // Start is called before the first frame update
    void Start()
    {
        isTakenPlace = false;
    }
    public void switchTakenPlace()
    {
        isTakenPlace =!isTakenPlace;
    }
}
