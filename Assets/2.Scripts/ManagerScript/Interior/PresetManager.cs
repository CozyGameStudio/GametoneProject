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
        // Dictionary�� ����Ͽ� �����º��� ���׸�� �׷�ȭ
        var presetMap = new Dictionary<int, List<ScriptableInterior>>();

        foreach (var interior in interiorDatas)
        {
            // ���⼭ 'preset'�� Interior Ŭ���� ������ �������� ��Ÿ���� �����Դϴ�.
            int preset = interior.presetNumber;

            // �����¿� �ش��ϴ� ����Ʈ�� ������ ���� ����
            if (!presetMap.ContainsKey(preset))
            {
                presetMap[preset] = new List<ScriptableInterior>();
            }

            // �ش� ������ ����Ʈ�� ���׸��� �߰�
            presetMap[preset].Add(interior);
        }

        // Dictionary�� ������ groupPresets�� �߰�
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
