using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PresetManager : MonoBehaviour
{
    private static PresetManager instance;
    private List<ScriptableInterior> interiorDatas;
    //public GameObject PresetPanel;
    public List<List<ScriptableInterior>> groupInteriors;
    public List<ScriptableInterior> choicePresetInteriors;


    
    public List<ScriptablePreset> presetDatas;

    public List<Position> positions;
    public List<List<Position>> groupPositions;
    public List<Position> choicePresetPositions;

    public bool IsFirstPresetClick = false;

    public static PresetManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<PresetManager>();
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

    public void Start()
    {

    }

    public void GetPresetDatas()
    {
        if (!presetDatas.Any())
        {
            presetDatas = InteriorSceneManager.Instance.presetDatas;
        }
    }

    public void ClassifyPositionsByPreset()
    {
        positions = InteriorSceneManager.Instance.positionDatas;
        if(!positions.Any())
        {
            Debug.Log("Positions is Empty");
        }
        groupPositions = new List<List<Position>>();

        var presetMap = new Dictionary<int, List<Position>>();

        foreach(var position in positions)
        {
            int preset = position.position.presetNumber;

            if(!presetMap.ContainsKey(preset))
            {
                presetMap[preset] = new List<Position>();
            }

            presetMap[preset].Add(position);
        }

        groupPositions = presetMap.Values.ToList();
        Debug.Log(groupPositions.Count);

    }

    public void ClassifyInteriorsByPreset()
    {
        interiorDatas = InteriorSceneManager.Instance.interiorDatas;
        if (!interiorDatas.Any())
        {
            Debug.LogError("InteriorDatas is Empty");
            return;
        }
        groupInteriors = new List<List<ScriptableInterior>>();
        // Dictionary를 사용하여 프리셋별로 인테리어를 그룹화
        var presetMap = new Dictionary<int, List<ScriptableInterior>>();

        foreach (var interior in interiorDatas)
        {
            // 여기서 'preset'는 Interior 클래스 내부의 프리셋을 나타내는 변수입니다.
            int preset = interior.presetNumber;

            // 프리셋에 해당하는 리스트가 없으면 새로 생성
            if (!presetMap.ContainsKey(preset))
            {
                presetMap[preset] = new List<ScriptableInterior>();
            }

            // 해당 프리셋 리스트에 인테리어 추가
            presetMap[preset].Add(interior);
        }

        // Dictionary의 값들을 groupPresets에 추가
        groupInteriors = presetMap.Values.ToList();
        Debug.Log(groupInteriors.Count);
    }

    public void ChoicePreset(int presetNumber)
    {
        if (choicePresetInteriors.Any())
        {
            choicePresetInteriors.Clear();
        }
        foreach(var interior in interiorDatas)
        {
            if (interior.presetNumber == presetNumber)
                choicePresetInteriors.Add(interior);
            else
            {

            }
        }
        if (choicePresetPositions.Any())
        {
            choicePresetPositions.Clear();
        }
        foreach(var position in positions)
        {
            if(position.position.presetNumber == presetNumber)
            {
                position.gameObject.SetActive(true);
                choicePresetPositions.Add(position);
            }
            else
            {
                position.gameObject.SetActive(false);
            }
        }
    }
}
