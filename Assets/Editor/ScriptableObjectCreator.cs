#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Unity.Mathematics;
using Unity.VisualScripting;

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
        string foodDataListPath = "Assets/Resources/FoodDataList.asset";
        FoodDataList foodDataList = AssetDatabase.LoadAssetAtPath<FoodDataList>(foodDataListPath);
        foodDataList.cleanList();
        foreach (var foodType in foodTypes)
        {
            string assetPath = $"Assets/2.Scripts/ScriptableObject/Foods/{foodType.stageToUse}Stage/SO_{foodType.foodName}.asset";
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
            dataObject.foodNameInKorean = foodType.foodNameInKorean;
            dataObject.stageToUse = foodType.stageToUse;

            // foodValues를 필터링하여 dataObject에 저장
            var filteredValues = foodValues.Where(v => v.foodName.StartsWith(foodType.foodName)).ToList();
            dataObject.foodPrice = new int[filteredValues.Count];
            dataObject.upgradeMoney = new int[filteredValues.Count];

            foreach (var value in filteredValues)
            {
                string[] split = value.foodName.Split('_');
                int levelIndex = int.Parse(split[1]) - 1; 
                dataObject.foodPrice[levelIndex] = value.saleValue;
                dataObject.upgradeMoney[levelIndex] = value.upgradeValue;
            }

            EditorUtility.SetDirty(dataObject);
            foodDataList.AddFood(dataObject);
        }
        EditorUtility.SetDirty(foodDataList);
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
        string machineDataListPath= "Assets/Resources/MachineDataList.asset";
        MachineDataList machineDataList = AssetDatabase.LoadAssetAtPath<MachineDataList>(machineDataListPath);
        machineDataList.cleanList();
        foreach (var machineType in machineTypes)
        {
            string assetPath = $"Assets/2.Scripts/ScriptableObject/Machines/{machineType.stageToUse}Stage/SO_{machineType.machineName}.asset";
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
            dataObject.machineNameInKorean = machineType.machineNameInKorean;
            dataObject.stageToUse = machineType.stageToUse;
            dataObject.machineUnlockCost=machineType.costToUnlock;

            // foodValues를 필터링하여 dataObject에 저장
            var filteredValues = machineValues.Where(v => v.machineName.StartsWith(machineType.machineName)).ToList();
            dataObject.cookTime = new float[filteredValues.Count];
            dataObject.upgradeMoney = new int[filteredValues.Count];

            foreach (var value in filteredValues)
            {
                string[] split = value.machineName.Split('_');
                int levelIndex = int.Parse(split[1]) - 1;  // level starts with 1
                dataObject.cookTime[levelIndex] = value.cookTime;
                dataObject.upgradeMoney[levelIndex] = value.upgradeValue;
            }

            EditorUtility.SetDirty(dataObject);
            machineDataList.AddMachine(dataObject);
        }
        EditorUtility.SetDirty(machineDataList);
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
#endif
    }
    [MenuItem("SrpObject/CreateCharacterByDatabase")]
    public static void CreateScriptablCharacterObjects()
    {
        Team5DataTable_Type.CharacterTypeData.Load();
        Team5DataTable_Value.CharacterValueData.Load();
        List<Team5DataTable_Type.CharacterTypeData> characterTypes = Team5DataTable_Type.CharacterTypeData.CharacterTypeDataList;
        List<Team5DataTable_Value.CharacterValueData> characterValues = Team5DataTable_Value.CharacterValueData.CharacterValueDataList;
        string characterDataListPath = "Assets/Resources/CharacterDataList.asset";
        CharacterDataList characterDataList = AssetDatabase.LoadAssetAtPath<CharacterDataList>(characterDataListPath);
        characterDataList.cleanList();
        foreach (var characterType in characterTypes)
        {
            string assetPath = $"Assets/2.Scripts/ScriptableObject/Characters/SO_{characterType.characterName}.asset";
#if UNITY_EDITOR
            ScriptableCharacter dataObject = AssetDatabase.LoadAssetAtPath<ScriptableCharacter>(assetPath);
#endif
            if (dataObject == null)
            {
                dataObject = ScriptableObject.CreateInstance<ScriptableCharacter>();
                AssetDatabase.CreateAsset(dataObject, assetPath);
            }

            dataObject.index = characterType.index;
            dataObject.characterName = characterType.characterName;
            dataObject.characterNameInKorean = characterType.characterNameInKorean; 

            // foodValues를 필터링하여 dataObject에 저장
            var filteredValues = characterValues.Where(v => v.characterName.StartsWith(characterType.characterName)).ToList();
            dataObject.profitGrowthRate = new float[filteredValues.Count];
            dataObject.upgradeMoney = new int[filteredValues.Count];

            foreach (var value in filteredValues)
            {
                string[] split = value.characterName.Split('_');
                int levelIndex = int.Parse(split[1]) - 1;  // Level Starts with 1
                dataObject.profitGrowthRate[levelIndex] = value.profitGrowthRate;
                dataObject.upgradeMoney[levelIndex] = value.upgradeValue;
            }

            EditorUtility.SetDirty(dataObject);
            characterDataList.AddCharacter(dataObject);
        }
        EditorUtility.SetDirty(characterDataList);
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
#endif
    }

        [MenuItem("SrpObject/CreateMissionByDatabase")]
    public static void CreateScriptablMissionObjects()
    {
        Team5DataTable_Mission.Data.Load();
        List<Team5DataTable_Mission.Data> missionDatas = Team5DataTable_Mission.Data.DataList;
        Debug.Log("Data load Complete");
        string missionDataListPath = "Assets/Resources/MissionDataList.asset";
        MissionDataList missionDataList = AssetDatabase.LoadAssetAtPath<MissionDataList>(missionDataListPath);
        missionDataList.cleanList();
        if (missionDataList == null)
        {
            Debug.LogError("MissionDataList not found!");
            return;
        }
        foreach (var missionData in missionDatas)
        {
            string assetPath = $"Assets/2.Scripts/ScriptableObject/Missions/{missionData.stageToUse}Stage/SO_{missionData.missionContent}.asset";
#if UNITY_EDITOR
            ScriptableMission dataObject = AssetDatabase.LoadAssetAtPath<ScriptableMission>(assetPath);
#endif      
            Debug.Log(missionData.missionContent);
            if (dataObject == null)
            {
                dataObject = ScriptableObject.CreateInstance<ScriptableMission>();
                AssetDatabase.CreateAsset(dataObject, assetPath);
                Debug.Log("Mission Created");
            }
            dataObject.index=missionData.index;
            if (Enum.TryParse(missionData.missionType, true, out MissionType missionTypeEnum))
            {
                dataObject.missionType = missionTypeEnum;
                Debug.Log(missionData.missionType+" "+missionTypeEnum);
            }
            else
            {
                Debug.LogError($"Invalid mission content string: {missionData.missionType}");
            }
            
            dataObject.stageToAppear = missionData.stageToUse;
            string content = missionData.missionContent;
            if (dataObject.missionType==MissionType.Reward){
                int activatedIndex = content.IndexOf("Activated");
                int levelIndex = content.IndexOf("Level");

                if (activatedIndex != -1)
                {
                    dataObject.targetName = content.Substring(0, activatedIndex).Trim();
                    dataObject.missionContent = MissionContent.ActivatedCheck;
                }
                else if (levelIndex != -1)
                {
                    dataObject.targetName = content.Substring(0, levelIndex).Trim();
                    dataObject.missionContent = MissionContent.LevelCheck;
                }
                else if (content.Contains("Customer"))
                {
                    dataObject.missionContent = MissionContent.CustomerCheck;
                }
                else if (content.Contains("Sales"))
                {
                    dataObject.missionContent = MissionContent.SalesCheck;
                }
                else if(content.Contains("Machine")){
                    dataObject.missionContent = MissionContent.MachineNumberCheck;
                }
                else
                {
                    Debug.LogError($"Invalid mission content format: {content}");
                }
            }
            else{
                int addIndex = content.IndexOf("Add");
                if (addIndex != -1&&content.Contains("Table"))
                {
                    dataObject.missionContent = MissionContent.TableAdd;
                }
                else if(addIndex != -1 && content.Contains("Machine")){
                    dataObject.targetName = content.Substring(0, addIndex).Trim();
                    dataObject.missionContent = MissionContent.MachineAdd;
                }
                else{
                    dataObject.missionContent = MissionContent.Speedup;
                }
            }
            dataObject.criteria = missionData.criteria;
            dataObject.cost = missionData.cost;
            dataObject.progress=missionData.progress;
            dataObject.description= missionData.description;
            EditorUtility.SetDirty(dataObject);
            missionDataList.AddMission(dataObject);
        }
        EditorUtility.SetDirty(missionDataList);


#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
#endif
    }
    
}
