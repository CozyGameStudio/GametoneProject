using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class PositionObject 
{
    public int fIndex;
    public SpriteRenderer positionSprites;
    public int Comfort;
    public bool isPosition;
}


[System.Serializable]
public class PositionList
{
    public List<PositionObject> list = new List<PositionObject>();
    public bool isPreset;
}

public class InteriorManager : MonoBehaviour
{
    private static InteriorManager instance;
    public PresetPanel presetPanel;
    public List<Preset> interiorDatas = new List<Preset>();

    public int ToTalComfort;
    private Scene currentInteriorScene;
    string currentStageName;
    public List<PositionList> interiorPositionObjects = new List<PositionList>();
    [HideInInspector]
    public int activatedPresetNum=0;

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

    private void Start()
    {
        
    }

    public void SetPreset()
    {
        presetPanel.SetData(interiorDatas);
    }

    public void ComfortUpdate()
    {
        //총 쾌적도를 구하는 시스템
        ToTalComfort = 0;
        PositionList list = new PositionList();

        foreach(var posObj in interiorPositionObjects)
        {
            if(posObj != null)
            {
                if(posObj.isPreset == true)
                {
                    list = posObj;
                }
            }
            else
            {
                Debug.LogError("Not found interiorPositionObjects");
            }
        }
        if(list == null)
        {
            Debug.Log("is not choice Preset");
        }
        else
        {
            foreach(var posObj in list.list)
            {
                if(posObj != null)
                {
                    if(posObj.isPosition == true)
                    {
                        ToTalComfort += posObj.Comfort;
                    }
                }
            }
        }
        InteriorSceneManager.Instance.ComfortUpdate(ToTalComfort);
    }

    public void SetData(SystemData systemData)
    {
        currentInteriorScene = SceneManager.GetActiveScene();
        currentStageName=currentInteriorScene.name;
        //프리셋의 언락 상태를 세팅 해주는 부분
        var stageData = systemData.interiorDatas.FirstOrDefault(stage => stage.stageName.Equals(currentStageName));
        if (stageData != null)
        {
            activatedPresetNum = stageData.presetNum;
            foreach (var preset in interiorDatas)
            {
                var presetData = stageData.presetData.Find(data => data.name.Equals(preset.interiorData.name));
                if (presetData != null)
                {
                    preset.SetData(presetData);
                }
            }
            //설정된 프리셋에 맞게 레이아웃을 설정해주는 부분
            SetPreset();    
            presetPanel.SetLayoutByIndex(activatedPresetNum);
            interiorPositionObjects[activatedPresetNum].isPreset=true;
            //저장된 인덱스에 맞게 가구 데이터를 설정해주는 부분
            for (int i = 0; i < stageData.furnitureByIndex.Count; i++)
            {
                interiorPositionObjects[activatedPresetNum].list[i].fIndex= stageData.furnitureByIndex[i];
                foreach(var positionData in interiorDatas[activatedPresetNum].interiorData.positionDataList){
                    foreach(var interiorData in positionData.InteriorDataList){
                        if (interiorPositionObjects[activatedPresetNum].list[i].fIndex.Equals(interiorData.index)){
                            interiorPositionObjects[activatedPresetNum].list[i].Comfort= interiorData.Comfort;
                            interiorPositionObjects[activatedPresetNum].list[i].positionSprites.sprite = interiorData.interiorImage;
                            interiorPositionObjects[activatedPresetNum].list[i].isPosition=true;
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError($"No data found for interior stage number: {currentStageName}");
        }
        ComfortUpdate();
    }

    public InteriorStageData GetData()
    {
        InteriorStageData data = new InteriorStageData();
        data.stageName = currentStageName;
        data.presetNum = activatedPresetNum; // 현재 활성화된 프리셋 번호 저장

        // 현재 프리셋의 모든 프리셋 데이터를 리스트에 추가
        List<PresetData> presetDataList = new List<PresetData>();
        foreach (var preset in interiorDatas)
        {
            presetDataList.Add(preset.GetData());
        }
        data.presetData = presetDataList;

        // 현재 활성화된 프리셋에 대한 가구 배치 정보 저장
        List<int> furnitureIndexes = new List<int>();
        if (interiorPositionObjects.Count > activatedPresetNum)
        {
            foreach (var position in interiorPositionObjects[activatedPresetNum].list)
            {
                furnitureIndexes.Add(position.fIndex); // 가구의 인덱스를 리스트에 추가
            }
        }
        data.furnitureByIndex = furnitureIndexes; // 가구 인덱스 정보를 데이터 객체에 저장

        return data;
    }


}
