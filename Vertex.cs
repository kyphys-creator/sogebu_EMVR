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
    private Vector4 k;
    private Vector4 O4;
    private Vector4 vv;

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

        O4 = new Vector4(this.transform.position.x, this.transform.position.y, this.transform.position.z, -Mathf.Sqrt(this.transform.position.x * this.transform.position.x + this.transform.position.y * this.transform.position.y + this.transform.position.z * this.transform.position.z));

        for (int i = 0; i < vertices.Length; ++i)
        {
            Vector3 vertex = vertices[i];
            vv = new Vector4(vertex.x, vertex.y, vertex.z, - Mathf.Sqrt(vertex.x * vertex.x + vertex.y * vertex.y + vertex.z * vertex.z));

            if (!this.originalVertices.Contains(vertex))
            {
                k = cM.Linv * (O4 - cM.xx4) + vv;
                this.correspondenceTable[i] = this.originalVertices.Count;
                this.originalVertices.Add(l * k - (O4 - cM.xx4));
                this.currentVertices.Add(l * k - (O4 - cM.xx4));

                this.targetlVertices.Add(l * k - (O4 - cM.xx4));
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
        l = cM.L;
        // 現在位置の更新
        for (int i = 0; i < this.currentVertices.Count; ++i)
        {
            Vector3 originalPos = this.originalVertices[i];
            Vector3 targetPos = this.targetlVertices[i];
            Vector3 currentPos = this.currentVertices[i];

            this.timeGap[i] += Time.deltaTime * cM.u4.z;
            currentPos = Vector3.Slerp(originalPos, targetPos, Mathf.PingPong(this.timeGap[i], 1.0f));

            this.currentVertices[i] = currentPos;
        }

        // verticesに渡す頂点を作成
        for (int i = 0; i < this.vertices.Length; ++i)
        {
            int vid = this.correspondenceTable[i];
            this.vertices[i] = this.currentVertices[vid];
        }

        // 頂点を渡す
        this.meshFilter.mesh.vertices = this.vertices;
    }
}
