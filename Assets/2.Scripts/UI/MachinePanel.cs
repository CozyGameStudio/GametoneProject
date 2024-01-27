using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachinePanel : MonoBehaviour
{
    public GameObject BoxPrefab;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < DataManager.Instance.machines.Count; i++)
        {
            GameObject imageObj = Instantiate(BoxPrefab);
            imageObj.transform.SetParent(transform, false);
            
            MachineBox machineBox = imageObj.GetComponent<MachineBox>();
            if(machineBox != null)
            {
                machineBox.InitBox(DataManager.Instance.machines[i]);
            }
            else
            {
                Debug.LogError("Cannot find machineBox");
            }
        }
    }
}
