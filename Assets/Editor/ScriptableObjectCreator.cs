#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Unity.Mathematics;

public static class ScriptableObjectCreator
{
    [MenuItem("SrpObject/CreateFoodByDatabase")]
    public static void CreateScriptableFoodObjects()
    {
        Team5DataTable_Type.FoodTypeData.Load();
        Team5DataTable_Value.FoodValueData.Load();
        List<Team5DataTable_Type.FoodTypeData> foodTypes = Team5DataTable_Type.FoodTypeData.FoodTypeDataList;
        List<Team5DataTable_Value.FoodValueData> foodValues = Team5DataTable_Value.FoodValueData.FoodValueDataList;
        Debug.Log($"Food Types Count: {foodTypes.Count}");
        Debug.Log($"Food Values Count: {foodValues.Count}");
        foreach (var foodType in foodTypes)
        {
            string assetPath = $"Assets/2.Scripts/ScriptableObject/Foods/SO_{foodType.foodName}.asset";
    #if UNITY_EDITOR
            ScriptableFood dataObject = AssetDatabase.LoadAssetAtPath<ScriptableFood>(assetPath);
    #endif
            if (dataObject == null)
            {
                dataObject = ScriptableObject.CreateInstance<ScriptableFood>();
                AssetDatabase.CreateAsset(dataObject, assetPath);
            }

            dataObject.index = foodType.index;
            dataObject.foodName = foodType.foodName;
            dataObject.foodNameInKorean = foodType.foodNameInKorean; // 필요한 경우 추가
            dataObject.stageToUse = foodType.stageToUse;

            // foodValues를 필터링하여 dataObject에 저장
            var filteredValues = foodValues.Where(v => v.foodName.StartsWith(foodType.foodName)).ToList();
            dataObject.foodPrice = new int[filteredValues.Count];
            dataObject.upgradeMoney = new int[filteredValues.Count];

            foreach (var value in filteredValues)
            {
                string[] split = value.foodName.Split('_');
                int levelIndex = int.Parse(split[1]) - 1;  // 레벨이 1부터 시작한다고 가정
                dataObject.foodPrice[levelIndex] = value.saleValue;
                dataObject.upgradeMoney[levelIndex] = value.upgradeValue;
            }

            EditorUtility.SetDirty(dataObject);
        }
    #if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        Debug.Log("save Completed");
    #endif
    }

    [MenuItem("SrpObject/CreateMachineByDatabase")]
    public static void CreateScriptableMachineObjects()
    {
        Team5DataTable_Type.MachineTypeData.Load();
        Team5DataTable_Value.MachineValueData.Load();
        List<Team5DataTable_Type.MachineTypeData> machineTypes = Team5DataTable_Type.MachineTypeData.MachineTypeDataList;
        List<Team5DataTable_Value.MachineValueData> machineValues = Team5DataTable_Value.MachineValueData.MachineValueDataList;

        foreach (var machineType in machineTypes)
        {
            string assetPath = $"Assets/2.Scripts/ScriptableObject/Machines/SO_{machineType.machineName}.asset";
#if UNITY_EDITOR
            ScriptableMachine dataObject = AssetDatabase.LoadAssetAtPath<ScriptableMachine>(assetPath);
#endif
            if (dataObject == null)
            {
                dataObject = ScriptableObject.CreateInstance<ScriptableMachine>();
                AssetDatabase.CreateAsset(dataObject, assetPath);
            }

            dataObject.index = machineType.index;
            dataObject.machineName = machineType.machineName;
            dataObject.machineNameInKorean = machineType.machineNameInKorean; // 필요한 경우 추가
            dataObject.stageToUse = machineType.stageToUse;

            // foodValues를 필터링하여 dataObject에 저장
            var filteredValues = machineValues.Where(v => v.machineName.StartsWith(machineType.machineName)).ToList();
            dataObject.cookTime = new float[filteredValues.Count];
            dataObject.upgradeMoney = new int[filteredValues.Count];

            foreach (var value in filteredValues)
            {
                string[] split = value.machineName.Split('_');
                int levelIndex = int.Parse(split[1]) - 1;  // 레벨이 1부터 시작한다고 가정
                dataObject.cookTime[levelIndex] = value.cookTime;
                dataObject.upgradeMoney[levelIndex] = value.upgradeValue;
            }

            EditorUtility.SetDirty(dataObject);
        }
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
#endif
    }
    [MenuItem("SrpObject/CreateMissionByDatabase")]
    public static void CreateScriptablMissionObjects()
    {
        Team5DataTable_Mission.Data.Load();
        List<Team5DataTable_Mission.Data> missionDatas = Team5DataTable_Mission.Data.DataList;

        foreach (var missionData in missionDatas)
        {
            string assetPath = $"Assets/2.Scripts/ScriptableObject/Missions/SO_{missionData.missionContent}.asset";
#if UNITY_EDITOR
            ScriptableMission dataObject = AssetDatabase.LoadAssetAtPath<ScriptableMission>(assetPath);
#endif
            if (dataObject == null)
            {
                dataObject = ScriptableObject.CreateInstance<ScriptableMission>();
                AssetDatabase.CreateAsset(dataObject, assetPath);
            }

            if (Enum.TryParse(missionData.missionType, true, out MissionType missionContentEnum))
            {
                dataObject.missionType = missionContentEnum;
            }
            else
            {
                Debug.LogError($"Invalid mission content string: {missionData.missionContent}");
            }
            int activatedIndex = missionData.missionContent.IndexOf("Activated");
            int levelIndex = missionData.missionContent.IndexOf("Level");

            if (activatedIndex != -1)
            {
                dataObject.targetName = missionData.missionContent.Substring(0, activatedIndex).Trim();
                dataObject.missionContent = MissionContent.ActivatedCheck;
            }
            else if (levelIndex != -1)
            {
                dataObject.targetName = missionData.missionContent.Substring(0, levelIndex).Trim();
                dataObject.missionContent = MissionContent.LevelCheck; 
            }
            else if (missionData.missionContent.Contains("Customer"))
            {
                dataObject.missionContent = MissionContent.CustomerCheck;
            }
            else if (missionData.missionContent.Contains("Sales"))
            {
                dataObject.missionContent = MissionContent.SalesCheck;
            }
            else
            {
                // "Available" 또는 "Level"이 없는 경우의 처리
                Debug.LogError($"Invalid mission content format: {missionData.missionContent}");
                // 추가적인 처리가 필요할 수 있습니다.
            }
            dataObject.criteria = missionData.criteria; // 필요한 경우 추가
            dataObject.cost = missionData.cost; // 필요한 경우 추가
            dataObject.stageToAppear = missionData.stageToAppear;

            EditorUtility.SetDirty(dataObject);
        }
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
#endif
    }
   
}
