using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using TMPro;
using System;
using System.Linq;


[Serializable]
public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// データをすべて消す
    /// </summary>
    [SerializeField] private bool isReset;

    /// <summary>
    /// 現在スコアを1度だけセーブするための変数
    /// </summary>
    public bool isFirstTime = true;

    /// <summary>
    /// ファイルパス
    /// </summary>
    string filepath;

    /// <summary>
    /// ファイル名
    /// </summary>
    string fileName = "ScoreData.json";

    /// <summary>
    /// scoreText一つあたりの行数
    /// ランキング表示数＝一つ当たり行数*scoreTextの数
    /// </summary>
    [SerializeField] private int numOfLines;

    /// <summary>
    /// スコアを表示するテキストUI
    /// </summary>
    [SerializeField] private GameObject[] scoreText;

    /// <summary>
    /// 結果を表示するテキストUI
    /// </summary>
    [SerializeField] private GameObject resultText;

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
        if (panel.activeSelf && isFirstTime)
        {
            isFirstTime = false;

            scoreData.scores.Add(score);

            string resultStr = "";
            var scores = scoreData.scores;
            scores = scores.OrderByDescending(a => a).ToList(); //降順にソート

            int pre_i = 0;
            for (int i0 = 0; i0 < scoreText.Length; i0++)
            {
                string scoreStr = "";
                for (int i = i0 * numOfLines; i < (i0 + 1) * numOfLines; i++)
                {
                    if (i >= scores.Count || i >= numOfLines * scoreText.Length)
                    {
                        break;
                    }

                    if (scores[i] != scores[pre_i])
                    {
                        pre_i = i;
                    }

                    var str_addend = "";
                    if (scores[i] == score)
                    {
                        str_addend = "<color=\"red\">" + (pre_i + 1) + "位 : " + scores[i] + Environment.NewLine + "</color>";
                        resultStr = "あなたの結果は... <size=150><color=\"red\">" + (pre_i + 1) + "位</size></color>です！";
                    }
                    else
                    {
                        str_addend = "<color=\"black\">" + (pre_i + 1) + "位 : " + scores[i] + Environment.NewLine + "</color>";
                    }

                    scoreStr += str_addend;
                }
                scoreText[i0].GetComponent<TMP_Text>().SetText(scoreStr);
            }
            resultText.GetComponent<TMP_Text>().SetText(resultStr);
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
        if (!isReset)
        {
            Save(scoreData);
        }
        else
        {
            Save(new ScoreData(new List<float>()));
        }
    }

}