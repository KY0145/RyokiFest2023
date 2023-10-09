using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using TMPro;
using System;

[Serializable]
public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// ファイルパス
    /// </summary>
    string filepath;

    /// <summary>
    /// ファイル名
    /// </summary>
    string fileName = "ScoreData.json";

    /// <summary>
    /// スコアを表示するテキストUI
    /// </summary>
    [SerializeField] private GameObject scoreText;

    /// <summary>
    /// scoreTextの親オブジェクトになっているパネルUI
    /// </summary>
    [SerializeField] private GameObject panel;

    /// <summary>
    /// 現在のスコア
    /// </summary>
    public float score;

    /// <summary>
    /// 過去のスコア
    /// </summary>
    private ScoreData scoreData;

    /// <summary>
    /// 保存するデータを格納するクラス
    /// </summary>
    private class ScoreData
    {
        public List<float> scores;

        public ScoreData(List<float> scores)
        {
            this.scores = scores;
        }
    }


    void Awake()
    {
        score = 0;

        // パス名取得
        filepath = Application.dataPath + "/" + fileName;

        // ファイルがないとき、ファイル作成
        if (!File.Exists(filepath))
        {
            Save(new ScoreData(new List<float> { -1 }));
        }

        // ファイルを読み込んでdataに格納
        scoreData = Load(filepath);
    }


    private void Update()
    {
        if (panel.activeSelf)
        {
            string str = "";
            foreach (var s in scoreData.scores)
            {
                str += "," + s.ToString();
            }
            str += "," + score;
            scoreText.GetComponent<TMP_Text>().SetText(str);
        }
    }


    public void AddScore(float addend)
    {
        score += addend;
    }



    /// <summary>
    /// jsonとしてデータを保存
    /// </summary>
    /// <param name="data">保存するデータ</param>
    void Save(ScoreData data)
    {
        StreamWriter writer = new StreamWriter(filepath, false);    // ファイル書き込み指定
        string json = JsonUtility.ToJson(data);                     // jsonとして変換
        writer.WriteLine(json);                                     // json変換した情報を書き込み
        writer.Close();                                             // ファイル閉じる
    }

    /// <summary>
    /// jsonファイル読み込み
    /// </summary>
    /// <param name="path">読み込むファイルのパス</param>
    /// <returns>読み込んだデータ</returns>
    ScoreData Load(string path)
    {
        StreamReader reader = new StreamReader(path);               // ファイル読み込み指定
        string json = reader.ReadToEnd();                           // ファイル内容全て読み込む
        reader.Close();                                             // ファイル閉じる

        return JsonUtility.FromJson<ScoreData>(json);               // jsonファイルを型に戻して返す
    }



    // シーン遷移後に保存
    void OnDestroy()
    {
        scoreData.scores.Add(score);
        Save(scoreData);
    }

}