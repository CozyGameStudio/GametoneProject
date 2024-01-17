using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachinePanel : MonoBehaviour
{
    public GameObject BoxPrefab;
    private ScriptableMachine[] machineData;
    // Start is called before the first frame update
    void Start()
    {
        machineData = BusinessManager.Instance.machineData;
        for(int i = 0; i < machineData.Length; i++)
        {
            GameObject imageObj = Instantiate(BoxPrefab);
            imageObj.transform.SetParent(transform, false);
            
            MachineBox machineBox = imageObj.GetComponent<MachineBox>();
            if(machineBox != null)
            {
                machineBox.InitBox(machineData[i]);
            }
            else
            {
                Debug.LogError("Cannot find machineBox");
            }
        }
    }
}
