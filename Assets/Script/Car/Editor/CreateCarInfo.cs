using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateCarInfo : EditorWindow
{
    private TextAsset csvFile;// CSVファイル
    private string exportFolder;// 出力先フォルダ
    private byte startRowLine = 1;// 何行目から読み込むか

    private CarInfo _carInfo = null;

    private const string FILENAME = "CarInfo";

    [MenuItem("Sample3DGame/CreateCarInfo")]
    private static void ShowWindow() 
    {
        var window = GetWindow<CreateCarInfo>();
        window.titleContent = new GUIContent("車両の情報作成");
        window.Show();
    }

    private void OnGUI() 
    {
        GUILayout.Label("新規車両情報の追加", EditorStyles.boldLabel);
        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSVファイル", csvFile, typeof(TextAsset), false);
        exportFolder = EditorGUILayout.TextField("出力先フォルダ", exportFolder);
        if (GUILayout.Button("出力先フォルダの選択"))
        {
            if (!string.IsNullOrEmpty(exportFolder)) 
            {
                if (!EditorUtility.DisplayDialog("出力先フォルダの上書き", "フォルダ名の上書きをおこなってもよろしいでしょうか？", "Yes", "No")) return;
            }
            exportFolder = EditorUtility.OpenFolderPanel("出力先フォルダの選択", "", "");
        }
        startRowLine = (byte)EditorGUILayout.IntField("読み取り開始行番号", startRowLine);

        //元のInspector部分の下にボタンを表示
        if (GUILayout.Button("データの設定")) CreateData();
    }

    private void CreateData()
    {
        // CSVファイルが設定されていない場合
        if (csvFile == null) 
        {
            string csvMessage = "読み込むCSVファイルがセットされていません。";
            CustomDebugger.ColorLog(csvMessage, GameConst.LogLevel.Yellow);
            EditorUtility.DisplayDialog("注意", csvMessage, "OK");
			return;
        }
        // 出力先フォルダが設定されていない場合
        if (string.IsNullOrEmpty(exportFolder)) 
        {
            string exportMessage = "出力先フォルダが設定されていません。";
            CustomDebugger.ColorLog(exportMessage, GameConst.LogLevel.Yellow);
            EditorUtility.DisplayDialog("注意", exportMessage, "OK");
            return;
        }
        // ユーザーに確認させる
        if (!EditorUtility.DisplayDialog("新規車両情報作成", "新規車両情報を作成します。宜しいでしょうか？", "Yes", "No"))
            return;

        // csvファイルをstring形式に変換
		string csvText = csvFile.text;
        // 改行ごとにパース
		string[] afterParse = csvText.Split('\n');

        for (int i = startRowLine; i < afterParse.Length; i++) 
        {
            // ヘッダー行は無視
            if (i == 0) continue;

            string[] parseByComma = afterParse[i].Split(',');
            
            _carInfo = CreateCarData(parseByComma);
            CreateAssetData(i);
        }

        string message = csvFile.name + "のマスターデータの作成が完了しました。";
        EditorUtility.DisplayDialog("確認", message, "OK");
        CustomDebugger.ColorLog(message, GameConst.LogLevel.Lime);
    }

    private CarInfo CreateCarData(string[] datas)
    {
        var result = ScriptableObject.CreateInstance<CarInfo>();
        byte column = 0;
        int num = 0;

        result.CarId = int.TryParse(datas[column], out num) ? num : 0;

        column++;
        result.moveSpeed = float.TryParse(datas[column], out float speed) ? speed : 0f;

        column++;
        result.spinSpeed = float.TryParse(datas[column], out speed) ? speed : 0f;

        column++;
        result.weight = int.TryParse(datas[column], out int weight) ? weight : 0;

        column++;
        result.boostSpeed = float.TryParse(datas[column], out speed) ? speed : 0f;

        column++;
        result.drag = float.TryParse(datas[column], out speed) ? speed : 0f;

        column++;
        result.angularDrag = float.TryParse(datas[column], out speed) ? speed : 0f;

        return result;
    }

    private void CreateAssetData(int num)
    {
        // 行数をIDとしてファイルを作成
        string fileName = FILENAME + num.ToString("D3") + ".asset";
        string path = $"{exportFolder}/{fileName}";

        // インスタンス化したものをアセットとして保存
        var asset = (CarInfo)AssetDatabase.LoadAssetAtPath(path, typeof(CarInfo));
        if (asset == null) AssetDatabase.CreateAsset(_carInfo, path);
        else 
        {
            // 指定のパスに既に同名のファイルが存在する場合は更新
            EditorUtility.CopySerialized(_carInfo, asset);
            AssetDatabase.SaveAssets();
        }
    }
}
