
/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorUnityEngine.cs'  */
using GoogleSheet.Protocol.v2.Res;
using GoogleSheet.Protocol.v2.Req;
using UGS;
using System;
using UGS.IO;
using GoogleSheet;
using System.Collections.Generic;
using System.IO;
using GoogleSheet.Type;
using System.Reflection;
using UnityEngine;


namespace Team5DataTable_Interior
{
    [GoogleSheet.Attribute.TableStruct]
    public partial class PositionData : ITable
    { 

        public delegate void OnLoadedFromGoogleSheets(List<PositionData> loadedList, Dictionary<int, PositionData> loadedDictionary);

        static bool isLoaded = false;
        static string spreadSheetID = "16WTecGpY3Q3I1-fERJZ0fOEtWLCq6ZhKweSUqLxZJJA"; // it is file id
        static string sheetID = "324117979"; // it is sheet id
        static UnityFileReader reader = new UnityFileReader();

/* Your Loaded Data Storage. */
    
        public static Dictionary<int, PositionData> PositionDataMap = new Dictionary<int, PositionData>();  
        public static List<PositionData> PositionDataList = new List<PositionData>();   

        /// <summary>
        /// Get PositionData List 
        /// Auto Load
        /// </summary>
        public static List<PositionData> GetList()
        {{
           if (isLoaded == false) Load();
           return PositionDataList;
        }}

        /// <summary>
        /// Get PositionData Dictionary, keyType is your sheet A1 field type.
        /// - Auto Load
        /// </summary>
        public static Dictionary<int, PositionData>  GetDictionary()
        {{
           if (isLoaded == false) Load();
           return PositionDataMap;
        }}

    

/* Fields. */

		public System.Int32 index;
		public System.Int32 stageToUse;
		public System.Int32 presetNumber;
		public System.Int32 positionNumber;
  

#region fuctions


        public static void Load(bool forceReload = false)
        {
            if(isLoaded && forceReload == false)
            {
#if UGS_DEBUG
                 Debug.Log("PositionData is already loaded! if you want reload then, forceReload parameter set true");
#endif
                 return;
            }

            string text = reader.ReadData("Team5DataTable_Interior"); 
            if (text != null)
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ReadSpreadSheetResult>(text);
                CommonLoad(result.jsonObject, forceReload); 
                if(!isLoaded)isLoaded = true;
            }
      
        }
 

        public static void LoadFromGoogle(System.Action<List<PositionData>, Dictionary<int, PositionData>> onLoaded, bool updateCurrentData = false)
        {      
                IHttpProtcol webInstance = null;
    #if UNITY_EDITOR
                if (Application.isPlaying == false)
                {
                    webInstance = UnityEditorWebRequest.Instance as IHttpProtcol;
                }
                else 
                {
                    webInstance = UnityPlayerWebRequest.Instance as IHttpProtcol;
                }
    #endif
    #if !UNITY_EDITOR
                     webInstance = UnityPlayerWebRequest.Instance as IHttpProtcol;
    #endif
          
 
                var mdl = new ReadSpreadSheetReqModel(spreadSheetID);
                webInstance.ReadSpreadSheet(mdl, OnError, (data) => {
                    var loaded = CommonLoad(data.jsonObject, updateCurrentData); 
                    onLoaded?.Invoke(loaded.list, loaded.map);
                });
        }

               


    public static (List<PositionData> list, Dictionary<int, PositionData> map) CommonLoad(Dictionary<string, Dictionary<string, List<string>>> jsonObject, bool forceReload){
            Dictionary<int, PositionData> Map = new Dictionary<int, PositionData>();
            List<PositionData> List = new List<PositionData>();     
            TypeMap.Init();
            FieldInfo[] fields = typeof(PositionData).GetFields(BindingFlags.Public | BindingFlags.Instance);
            List<(string original, string propertyName, string type)> typeInfos = new List<(string, string, string)>(); 
            List<List<string>> rows = new List<List<string>>();
            var sheet = jsonObject["PositionData"];

            foreach (var column in sheet.Keys)
            {
                string[] split = column.Replace(" ", null).Split(':');
                         string column_field = split[0];
                string   column_type = split[1];

                typeInfos.Add((column, column_field, column_type));
                          List<string> typeValues = sheet[column];
                rows.Add(typeValues);
            }

          // 실제 데이터 로드
                    if (rows.Count != 0)
                    {
                        int rowCount = rows[0].Count;
                        for (int i = 0; i < rowCount; i++)
                        {
                            PositionData instance = new PositionData();
                            for (int j = 0; j < typeInfos.Count; j++)
                            {
                                try
                                {
                                    var typeInfo = TypeMap.StrMap[typeInfos[j].type];
                                    //int, float, List<..> etc
                                    string type = typeInfos[j].type;
                                    if (type.StartsWith(" < ") && type.Substring(1, 4) == "Enum" && type.EndsWith(">"))
                                    {
                                         Debug.Log("It's Enum");
                                    }

                                    var readedValue = TypeMap.Map[typeInfo].Read(rows[j][i]);
                                    fields[j].SetValue(instance, readedValue);

                                }
                                catch (Exception e)
                                {
                                    if (e is UGSValueParseException)
                                    {
                                        Debug.LogError("<color=red> UGS Value Parse Failed! </color>");
                                        Debug.LogError(e);
                                        return (null, null);
                                    }

                                    //enum parse
                                    var type = typeInfos[j].type;
                                    type = type.Replace("Enum<", null);
                                    type = type.Replace(">", null);

                                    var readedValue = TypeMap.EnumMap[type].Read(rows[j][i]);
                                    fields[j].SetValue(instance, readedValue); 
                                }
                              
                            }
                            List.Add(instance); 
                            Map.Add(instance.index, instance);
                        }
                        if(isLoaded == false || forceReload)
                        { 
                            PositionDataList = List;
                            PositionDataMap = Map;
                            isLoaded = true;
                        }
                    } 
                    return (List, Map); 
}


 

        public static void Write(PositionData data, System.Action<WriteObjectResult> onWriteCallback = null)
        { 
            TypeMap.Init();
            FieldInfo[] fields = typeof(PositionData).GetFields(BindingFlags.Public | BindingFlags.Instance);
            var datas = new string[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                var type = fields[i].FieldType;
                string writeRule = null;
                if(type.IsEnum)
                {
                    writeRule = TypeMap.EnumMap[type.Name].Write(fields[i].GetValue(data));
                }
                else
                {
                    writeRule = TypeMap.Map[type].Write(fields[i].GetValue(data));
                } 
                datas[i] = writeRule; 
            }  
           
#if UNITY_EDITOR
if(Application.isPlaying == false)
{
                UnityPlayerWebRequest.Instance.WriteObject(new WriteObjectReqModel(spreadSheetID, sheetID, datas[0], datas), OnError, onWriteCallback);

}
else
{
            UnityPlayerWebRequest.Instance.WriteObject(new  WriteObjectReqModel(spreadSheetID, sheetID, datas[0], datas), OnError, onWriteCallback);

}
#endif

#if !UNITY_EDITOR
   UnityPlayerWebRequest.Instance.WriteObject(new  WriteObjectReqModel(spreadSheetID, sheetID, datas[0], datas), OnError, onWriteCallback);

#endif
        } 
          

 


#endregion

#region OdinInsepctorExtentions
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.Button("UploadToSheet")]
    public void Upload()
    {
        Write(this);
    }
 
    
#endif


 
#endregion
    public static void OnError(System.Exception e){
         UnityGoogleSheet.OnTableError(e);
    }
 
    }
}
        