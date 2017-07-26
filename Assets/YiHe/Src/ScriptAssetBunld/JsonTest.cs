using UnityEngine;
using System.Collections;

public class JsonTest : MonoBehaviour
{
    public string _jsonT;
    // Use this for initialization
    void Start()
    {
        var jsonModelData = new JsonModelData();
        jsonModelData.jsonInfo = new ModelHolo.JsonInfo();
        jsonModelData.jsonInfo.json_date = "2017-06-30 14:48:11";

        jsonModelData.jsonInfo.modelsInfo = new ModelHolo.ModelInfo[2];
        jsonModelData.jsonInfo.modelsInfo[0] = new ModelHolo.ModelInfo
        {
            id = 1,
            name = "Chair2.assetbundle",
            url = "http://101.37.149.220/HololensWebYiHe/web/uploads/Chair2.assetbundle",
            imageUrl  = "http://101.37.149.220/HololensWebYiHe/web/images/v2-8c306d636da249bfeec95f4633e6d29b_b.jpg",
            updateDate = "2017-06-30 00:00:00"
        };
        jsonModelData.jsonInfo.modelsInfo[1] = new ModelHolo.ModelInfo
        {
            id = 2,
            name = "Chair2.assetbundle",
            url = "http://101.37.149.220/HololensWebYiHe/web/uploads/Chair2.assetbundle",
            imageUrl = "http://101.37.149.220/HololensWebYiHe/web/images/v2-8c306d636da249bfeec95f4633e6d29b_b.jpg",
            updateDate = "2017-06-30 04:04:29"
        };
        var gjson = JsonUtility.ToJson(jsonModelData);
        Debug.Log(JsonUtility.ToJson(jsonModelData) == _jsonT);
        var data = JsonUtility.FromJson(gjson, typeof(JsonModelData));
    }
}
