using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    /**
     * 揺らぐ距離間。
     */
    public float swingDistance = 0.1f;

    /**
     * MeshFilter.
     */
    private MeshFilter meshFilter;

    /**
     * メッシュデータの頂点。
     */
    private Vector3[] vertices;

    /**
     * 重複のないオリジナル頂点データ。
     */
    private List<Vector3> originalVertices = new List<Vector3>();

    /**
     * 現在の重複のない頂点データ。
     */
    private List<Vector3> currentVertices = new List<Vector3>();

    /**
     * 動かす際のターゲットとなる重複のない頂点データ。
     */
    private List<Vector3> targetlVertices = new List<Vector3>();

    /**
     * 頂点ごとの時間の差。
     */
    private List<float> timeGap = new List<float>();

    /**
     * 頂点との対応表。
     */
    private Dictionary<int, int> correspondenceTable = new Dictionary<int, int>();

    Camera cam;
    cameraMove cM;
    private Matrix4x4 l;

    /**
     * Awake.
     */
    public void Awake()
    {
        cam = Camera.main;
        cM = cam.GetComponent<cameraMove>();

        l = cM.L;
        this.meshFilter = this.GetComponent<MeshFilter>();
        this.vertices = this.meshFilter.mesh.vertices;

        for (int i = 0; i < vertices.Length; ++i)
        {
            Vector3 vertex = vertices[i];

            if (!this.originalVertices.Contains(vertex))
            {
                this.correspondenceTable[i] = this.originalVertices.Count;
                this.originalVertices.Add(vertex);
                this.currentVertices.Add(vertex);

                // ランダムな位置を作る
                this.targetlVertices.Add(new Vector3(
                    vertex.x + Random.Range(-this.swingDistance, this.swingDistance),
                    vertex.y + Random.Range(-this.swingDistance, this.swingDistance),
                    vertex.z + Random.Range(-this.swingDistance, this.swingDistance)
                ));
            }
            else
            {
                this.correspondenceTable[i] = this.originalVertices.FindIndex(vec => vec == vertex);
            }
        }

        for (int i = 0; i < this.originalVertices.Count; ++i)
        {
            this.timeGap.Add(Random.Range(0.0f, 1.0f));
        }
    }

    /**
     * Update.
     */
    public void Update()
    {
        // 現在位置の更新
        for (int i = 0; i < this.currentVertices.Count; ++i)
        {
            Vector3 originalPos = this.originalVertices[i];
            Vector3 targetPos = this.targetlVertices[i];
            Vector3 currentPos = this.currentVertices[i];

            this.timeGap[i] += Time.deltaTime * 1.6f;
            currentPos = Vector3.Slerp(originalPos, targetPos, Mathf.PingPong(this.timeGap[i], 1.0f));

            this.currentVertices[i] = currentPos;
        }

        // verticesに渡す頂点を作成
        for (int i = 0; i < this.vertices.Length; ++i)
        {
            int vid = this.correspondenceTable[i];
            this.vertices[i] = this.currentVertices[vid];
        }

        // 回転演出
        this.transform.Rotate(Vector3.up * Time.deltaTime * 12.0f, Space.World);

        // 頂点を渡す
        this.meshFilter.mesh.vertices = this.vertices;
    }
}
