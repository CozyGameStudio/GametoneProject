using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteriorManager : MonoBehaviour
{
    private static InteriorManager instance;
    private List<ScriptableInterior> interiorDatas;

    public List<List<ScriptableInterior>> groupInteriors;
    public List<ScriptableInterior> choicePositionInteriors;

    public List<Position> positions;

    public static InteriorManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<InteriorManager>();
            }
            return instance;
        }
    }

    public void Awake()
    {
        if(instance != this)
        {
            Destroy(instance);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GetPositionDatas()
    {
        positions = PresetManager.Instance.choicePresetPositions;
        Debug.Log(positions.Count);
    }

    public void ClassifyInteriorsByPosition()
    {
        interiorDatas = PresetManager.Instance.choicePresetInteriors;
        if (!interiorDatas.Any())
        {
            Debug.LogError("choicePresetInteriorDatas is Empty");
            return;
        }
        groupInteriors = new List<List<ScriptableInterior>>();
        var map = new Dictionary<int, List<ScriptableInterior>>();

        foreach(var interior in interiorDatas)
        {
            int position  = interior.position;

            if (!map.ContainsKey(position))
            {
                map[position] = new List<ScriptableInterior>();
            }

            map[position].Add(interior);
        }

        groupInteriors = map.Values.ToList();
        Debug.Log(groupInteriors.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
