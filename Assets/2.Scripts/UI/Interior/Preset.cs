using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Preset : MonoBehaviour
{
    public int presetIndex;
    [SerializeField]
    public ScriptableInterior interiorData;
    public GameObject layout;
    public Dictionary<int, bool> isUnlock = new Dictionary<int, bool>();

    public void BuyInteriorByJelly(int index)
    {
        int j = 0;
        InteriorData interior = new InteriorData();
        foreach( var item in interiorData.positionDataList)
        {
            foreach(var i in item.InteriorDataList)
            {
                j++;
                if(i.index == index)
                {
                    interior = i;
                    break;
                }
            }
        }
        if(InteriorSceneManager.Instance.jelly >= interior.interiorUnlockCost)
        {
            InteriorSceneManager.Instance.DecreaseDia(interior.interiorUnlockCost);
            isUnlock[interior.index] = true;
        }
    }

    public void SetData(PresetData presetData)
    {

        foreach (var item in interiorData.positionDataList)
        {
            foreach (var i in item.InteriorDataList)
            {
                int key = i.index;
                isUnlock[key] = false;
            }
        }
        if(presetData.unlocks.Count<=0){
            return;
        }
        // 로그를 출력하여 전달받은 PresetData 객체의 상태를 확인
        Debug.Log($"Updating isUnlock with PresetData: {presetData.name}");
        foreach (var unlock in presetData.unlocks)
        {
            Debug.Log($"Unlock Item: {unlock.key}, State: {unlock.value}");
        }

        // PresetData로부터 isUnlock 정보를 업데이트
        isUnlock = presetData.ToDictionary();

        // 업데이트 후 isUnlock 상태를 로그로 출력
        foreach (var pair in isUnlock)
        {
            Debug.Log($"Updated isUnlock: Key {pair.Key}, Value {pair.Value}");
        }

    }

    public PresetData GetData()
    {
        PresetData preset = new PresetData(interiorData.name, isUnlock);
        return preset;
    }
}
