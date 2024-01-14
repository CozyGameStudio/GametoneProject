using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    public bool IsAvailable { get { return !isTakenPlace; } }
    public string foodToMake = "potatoSoup";
    private bool isTakenPlace = false;

    // Start is called before the first frame update
    void Start()
    {
        isTakenPlace = false;
    }

    public void SwitchTakenPlace()
    {
        isTakenPlace = !isTakenPlace;
        if(isTakenPlace){
            SetAvailable();
        }
    }
    public void SetAvailable()
    {
        OrderManager.Instance.MachineAvailable();
    }
}
