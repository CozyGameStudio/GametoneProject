#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScriptableObjectCreatorWithInterior
{
    [MenuItem("ScriptObject/CreateInteriorByDatabase")]
    public static void CreateScriptableInteriorObjects()
    {
        Team5DataTable_Interior.InteriorData.Load();
        List<Team5DataTable_Interior.InteriorData> interiors = Team5DataTable_Interior.InteriorData.InteriorDataList;
        Debug.Log($"Interior Count: {interiors.Count}");
        string InteriorDataListPath = "Assets/Resources/InteriorDataList.asset";
        InteriorDataList interiorDataList = AssetDatabase.LoadAssetAtPath<InteriorDataList>( InteriorDataListPath );
        interiorDataList.CleanList();


        foreach (var interior in interiors)
        {
            string assetPath = $"Assets/2.Scripts/ScriptableObject/Interiors/Interior/{interior.stageToUse}Stage/SO_{interior.interirorName}.asset";

#if UNITY_EDITOR
            ScriptableInterior dataObject = AssetDatabase.LoadAssetAtPath<ScriptableInterior>(assetPath);
#endif
            if(dataObject == null )
            {
                dataObject = ScriptableObject.CreateInstance<ScriptableInterior>();
                AssetDatabase.CreateAsset(dataObject,assetPath);
            }

            dataObject.index = interior.index;
            dataObject.interiorName = interior.interirorName;
            dataObject.interiorNameInKorean = interior.interiorNameInKorean;
            dataObject.stageToUse = interior.stageToUse;
            dataObject.presetNumber = interior.presetNumber;
            dataObject.position = interior.position;

            EditorUtility.SetDirty( dataObject );
            interiorDataList.AddInterior(dataObject);
        }

#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        Debug.Log("save Completed to Interior");
#endif

    }

    [MenuItem("ScriptObject/CreatePresetByDatabase")]
    public static void CreateScriptlablePresetObjects()
    {
        Team5DataTable_Interior.PresetData.Load();
        List<Team5DataTable_Interior.PresetData> presets = Team5DataTable_Interior.PresetData.PresetDataList;
        Debug.Log($"Preset Count: {presets.Count}");
        string presetDataListPath = "Assets/Resources/presetDataList.asset";
        PresetDataList presetDataList = AssetDatabase.LoadAssetAtPath<PresetDataList>(presetDataListPath);
        presetDataList.CleanList();

        foreach (var preset in presets)
        {
            string assetPath = $"Assets/2.Scripts/ScriptableObject/Interiors/Preset/{preset.stageToUse}Stage/SO_{preset.presetNumber}.asset";

#if UNITY_EDITOR
            ScriptablePreset dataObject = AssetDatabase.LoadAssetAtPath<ScriptablePreset>(assetPath);
#endif
            if(dataObject == null )
            {
                dataObject = ScriptableObject.CreateInstance<ScriptablePreset>();
                AssetDatabase.CreateAsset(dataObject, assetPath);
            }

            dataObject.Index = preset.index;
            dataObject.presetNumber = preset.presetNumber;
            dataObject.stageToUse = preset.stageToUse;
            //dataObject.money = preset.money;

            EditorUtility.SetDirty( dataObject );
            presetDataList.AddPreset(dataObject);
        }

#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        Debug.Log("save Completed to preset");
#endif

    }

    [MenuItem("ScriptObject/CreatePositionByDatabase")]
    public static void CreateScriptlablePositionObjects()
    {
        Team5DataTable_Interior.PositionData.Load();
        List<Team5DataTable_Interior.PositionData> positions = Team5DataTable_Interior.PositionData.PositionDataList;
        Debug.Log($"Interior Count: {positions.Count}");
        string positionDataListPath = "Assets/Resources/positionDataList.asset";
        PositionDataList positionDataList = AssetDatabase.LoadAssetAtPath<PositionDataList>(positionDataListPath);
        positionDataList.CleanList();

        foreach (var position in positions)
        {
            string assetPath = $"Assets/2.Scripts/ScriptableObject/Interiors/Position/{position.stageToUse}Stage/SO_preset{position.presetNumber}position{position.positionNumber}.asset";

#if UNITY_EDITOR
            ScriptablePosition dataObject = AssetDatabase.LoadAssetAtPath<ScriptablePosition>(assetPath);
#endif
            if (dataObject == null)
            {
                dataObject = ScriptableObject.CreateInstance<ScriptablePosition>();
                AssetDatabase.CreateAsset(dataObject, assetPath);
            }

            dataObject.index = position.index;
            dataObject.stageToUse = position.stageToUse;
            dataObject.presetNumber = position.presetNumber;
            dataObject.positionNumber = position.positionNumber;

            EditorUtility.SetDirty(dataObject);
            positionDataList.AddPosition(dataObject);
        }

#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        Debug.Log("save Completed to preset");
#endif

    }
}
