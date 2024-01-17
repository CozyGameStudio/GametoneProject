#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;

public static class FoodTypeScriptableObjectCreator
{
    [MenuItem("SrpObject/CreatebyDatabase")]
    public static void CreateFoodTypeDataObjects()
    {
        List<Team5DataTable_FoodType.Data> foodTypes = Team5DataTable_FoodType.Data.DataList;

        foreach (var foodType in foodTypes)
        {
            FoodTypeData dataObject = ScriptableObject.CreateInstance<FoodTypeData>();
            Debug.Log(dataObject);
            dataObject.index = foodType.index;
            dataObject.foodName = foodType.foodName;
            dataObject.foodPrice = foodType.foodPrice;
            dataObject.cookTime = foodType.cookTime;
            dataObject.stageToUse=foodType.stageToUse;

#if UNITY_EDITOR
            string assetPath = $"Assets/2.Scripts/Food/TestScriptableObject/FoodType_{foodType.foodName}.asset";
            AssetDatabase.CreateAsset(dataObject, assetPath);
#endif
        }

#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
#endif
    }
}
