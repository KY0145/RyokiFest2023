using System;
using UnityEngine;
using UnityEngine.UI;


[ExecuteAlways]
public class CreateUI_LockOn : Graphic
{
    [SerializeField] private GameObject player;

    [SerializeField] private Color32 normalColor;
    [SerializeField] private Color32 lockonColor;


    private void Update()
    {
        bool bl = player.GetComponent<MouseLockOnShooting>().targetEnemiesList.Count != 0;

        if (bl)
        {
            SetColor(lockonColor);
        }
        else
        {
            SetColor(normalColor);
        }

        SetVerticesDirty();
    }

    /// <summary>
    /// 頂点情報
    /// </summary>
    [Serializable]
    private struct VertexInfo
    {
        public Vector2 position; // 位置
        public Color32 color;    // カラー
    }
    [SerializeField]
    private VertexInfo[] vertexInfos = new VertexInfo[]
    {
        new VertexInfo
        {
            position = new Vector2(-50f, -50f),
            color = new Color32(255,255, 255, 255)
        },
        new VertexInfo
        {
            position = new Vector2(-50f, 50f),
            color = new Color32(255,255, 255, 255)
        },
        new VertexInfo
        {
            position = new Vector2(50f, 50f),
            color = new Color32(255,255, 255, 255)
        },
        new VertexInfo
        {
            position = new Vector2(50f, -50f),
            color = new Color32(255,255, 255, 255)
        },

        new VertexInfo
        {
            position = new Vector2(-45f, -45f),
            color = new Color32(255,255, 255, 255)
        },
        new VertexInfo
        {
            position = new Vector2(-45f, 45f),
            color = new Color32(255,255, 255, 255)
        },
        new VertexInfo
        {
            position = new Vector2(45f, 45f),
            color = new Color32(255,255, 255, 255)
        },
        new VertexInfo
        {
            position = new Vector2(45f, -45f),
            color = new Color32(255,255, 255, 255)
        }
    };

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        // 頂点情報をクリア
        vh.Clear();

        // 頂点座標を設定
        for (var i = 0; i < vertexInfos.Length; i++)
        {
            var v = new UIVertex
            {
                position = vertexInfos[i].position,
                color = vertexInfos[i].color
            };
            vh.AddVert(v);
        }

        // 三角形を描画
        vh.AddTriangle(0, 1, 5);
        vh.AddTriangle(0, 5, 4);
        vh.AddTriangle(1, 2, 5);
        vh.AddTriangle(5, 2, 6);
        vh.AddTriangle(7, 6, 2);
        vh.AddTriangle(7, 2, 3);
        vh.AddTriangle(0, 4, 7);
        vh.AddTriangle(0, 7, 3);
    }

    private void SetColor(Color32 color)
    {
        for (int i = 0; i < vertexInfos.Length; ++i)
        {
            vertexInfos[i].color = color;
        }
    }
}