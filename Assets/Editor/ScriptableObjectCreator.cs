#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

//     [MenuItem("SrpObject/CreateMachineByDatabase")]
//     public static void CreateScriptableMachineObjects()
//     {
//         List<Team5DataTable_Type.MachineTypeData> machineTypes = Team5DataTable_Type.MachineTypeData.MachineTypeDataList;
//         List<Team5DataTable_Value.MachineValueData> machineValues = Team5DataTable_Value.MachineValueData.MachineValueDataList;

//         foreach (var machineType in machineTypes)
//         {
//             string assetPath = $"Assets/2.Scripts/ScriptableObject/Machines/SO_{machineType.foodName}.asset";
// #if UNITY_EDITOR
//             ScriptableMachine dataObject = AssetDatabase.LoadAssetAtPath<ScriptableMachine>(assetPath);
// #endif
//             if (dataObject == null)
//             {
//                 dataObject = ScriptableObject.CreateInstance<ScriptableMachine>();
//                 AssetDatabase.CreateAsset(dataObject, assetPath);
//             }

//             dataObject.index = machineType.index;
//             dataObject.machineName = machineType.foodName;
//             dataObject.machineNameInKorean = machineType.machineNameInKorean; // 필요한 경우 추가
//             dataObject.stageToUse = machineType.stageToUse;

//             // machineValues를 필터링하여 dataObject에 저장
//             var filteredValues = machineValues.Where(v => v.machineName == machineType.machineName).ToList();
//             dataObject.cookTime = new int[filteredValues.Count];
//             dataObject.upgradeMoney = new int[filteredValues.Count];

//             for (int i = 0; i < filteredValues.Count; i++)
//             {
//                 dataObject.foodPrice[i] = filteredValues[i].cookTime;
//                 dataObject.upgradeMoney[i] = filteredValues[i].upgradeValue;
//             }

//             EditorUtility.SetDirty(dataObject);
//         }
// #if UNITY_EDITOR
//         AssetDatabase.SaveAssets();
// #endif
//     }
}
