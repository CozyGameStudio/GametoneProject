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
            string assetPath = $"Assets/2.Scripts/Food/TestScriptableObject/FoodType_{foodType.foodName}.asset";
#if UNITY_EDITOR
            FoodTypeData dataObject = AssetDatabase.LoadAssetAtPath<FoodTypeData>(assetPath);
#endif
            if (dataObject == null)
            {
                dataObject = ScriptableObject.CreateInstance<FoodTypeData>();
                AssetDatabase.CreateAsset(dataObject, assetPath);
            }

            dataObject.index = foodType.index;
            dataObject.foodName = foodType.foodName;
            dataObject.foodPrice = foodType.foodPrice;
            dataObject.cookTime = foodType.cookTime;
            dataObject.stageToUse = foodType.stageToUse;

            EditorUtility.SetDirty(dataObject);
        }
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
#endif
    }
}
