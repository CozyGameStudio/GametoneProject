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
                foreach(var unlock in presetData.unlocks){
                    Debug.Log(unlock.key+" "+unlock.value);
                }
                if (presetData != null)
                {
                    preset.SetData(presetData);
                }
            }
            //설정된 프리셋에 맞게 레이아웃을 설정해주는 부분
            SetPreset();
            presetPanel.SetLayoutByIndex(activatedPresetNum);
            interiorPositionObjects[activatedPresetNum].isPreset=true;
            //포지션 오브젝트를 초기화 시켜준후, 활성화 된것만 세팅
            presetPanel.SetpositionByindex(activatedPresetNum);
            
            //저장된 인덱스에 맞게 가구 데이터를 설정해주는 부분
            for (int i = 0; i < stageData.presetData.Count; i++)//스테이지 별 프리셋 데이터 탐색
            {
                for(int j=0;j<stageData.presetData[i].furnitureByIndex.Count;j++)//스테이지 프리셋의 가구 인덱스 탐색
                {
                    interiorPositionObjects[i].list[j].fIndex = stageData.presetData[i].furnitureByIndex[j];//저장된 데이터의 인덱스 세팅
                    foreach (var positionData in interiorDatas[i].interiorData.positionDataList)//프리셋에 위치한 포지션 데이터중
                    {
                        foreach (var interiorData in positionData.InteriorDataList)//가구 리스트에 위치한 가구들을 탐색하여
                        {
                            if (interiorPositionObjects[i].list[j].fIndex.Equals(interiorData.index))//인테리어 포지션 인덱스를 키값으로 일치시켜 가구를 세팅한다.
                            {
                                interiorPositionObjects[i].list[j].Comfort = interiorData.Comfort;
                                interiorPositionObjects[i].list[j].positionSprites.sprite = interiorData.interiorImage;
                                interiorPositionObjects[i].list[j].isPosition = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError($"No data found for interior stage number: {currentStageName}");
            stageData = new InteriorStageData()
            {
                stageName = currentStageName,
                presetNum = 0,
                presetData = new List<PresetData>()
            };

            foreach (var preset in interiorDatas)
            {
                var presetData = new PresetData(preset.interiorData.name, new Dictionary<int, bool>());
                stageData.presetData.Add(presetData);
                preset.SetData(presetData);
            }

            systemData.interiorDatas.Add(stageData);
            activatedPresetNum=0;
            SetPreset();
            presetPanel.SetLayoutByIndex(activatedPresetNum);
            interiorPositionObjects[activatedPresetNum].isPreset = true;
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
        int tmp=0;
        foreach (var preset in interiorDatas)
        {
            PresetData presetData=preset.GetData();
            List<int> furnitureIndexes = new List<int>();
            foreach (var position in interiorPositionObjects[tmp].list)
            {
                furnitureIndexes.Add(position.fIndex); // 가구의 인덱스를 리스트에 추가
            }
            presetData.furnitureByIndex=furnitureIndexes;
            presetDataList.Add(presetData);
            tmp++;
        }
        data.presetData = presetDataList;

        return data;
    }


}
