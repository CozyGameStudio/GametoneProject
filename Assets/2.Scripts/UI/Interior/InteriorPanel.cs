using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteriorPanel : MonoBehaviour
{
    public GameObject interiorPrefab;

    private List<List<ScriptableInterior>> groupInteriors;
    private List<GameObject> InteriorButtons;
    // Start is called before the first frame update
    void Start()
    {
        InteriorButtons = new List<GameObject>();
        InitPanel(1);
    }

    public void InitPanel(int num)
    {
        InteriorManager.Instance.GetPositionDatas();
        InteriorManager.Instance.ClassifyInteriorsByPosition();
        groupInteriors = InteriorManager.Instance.groupInteriors;

        if (InteriorButtons.Any())
        {
            foreach(GameObject go in InteriorButtons)
            {
                Destroy(go);
            }
        }

        for(int i = 0; i < groupInteriors[num-1].Count; i++)
        {
            GameObject imageObj = Instantiate(interiorPrefab);
            imageObj.transform.SetParent(transform, false);
            InteriorButtons.Add(imageObj);
            InteriorBox interiorBox = imageObj.GetComponent<InteriorBox>();
            if(interiorBox != null)
            {
                interiorBox.InitBox(groupInteriors[num-1][i]);
            }
            else
            {
                Debug.LogError("Cannot find interiorBox");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
