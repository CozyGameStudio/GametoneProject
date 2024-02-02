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
        foreach (var machineInterface in DataManager.Instance.machines)
        {
            if (machineInterface is Machine machine)
            {
                GameObject imageObj = Instantiate(BoxPrefab);
                imageObj.transform.SetParent(transform, false);

                MachineBox machineBox = imageObj.GetComponent<MachineBox>();
                if (machineBox != null)
                {
                    machineBox.InitBox(machine);
                }
                else
                {
                    Debug.LogError("Cannot find MachineBox component on the prefab.");
                }
            }
        }
    }
}
