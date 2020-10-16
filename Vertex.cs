using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    //MeshFilter
    private MeshFilter meshFilter;
    //Vertices of MeshFilter data
    private Vector3[] orgvertices;
    private Vector3[] vertices;

    /**
     * 動かす際のターゲットとなる重複のない頂点データ。
     */
    private List<Vector3> targetVertices = new List<Vector3>();

    Camera cam;
    public GameObject Car;
    Vector3 carpos3;
    Vector4 carpos4;
    cameraMove cM;
    cubeMove cuM;
    private Matrix4x4 l;
    private Vector4 k;//player's rest frame
    private Vector4 o4;//player's rest frame vector4
    private Vector4 v4;//world frame vector4
    private Vector3 x3;//object's world frame position

    public void Awake()
    {
        cam = Camera.main;
        cM = cam.GetComponent<cameraMove>();
        cuM = this.GetComponent<cubeMove>();
        carpos3 = Car.transform.position;
        carpos4 = new Vector4(carpos3.x, carpos3.y, carpos3.z, -carpos3.magnitude);
        l = cM.L;
        this.meshFilter = this.GetComponent<MeshFilter>();
        this.orgvertices = this.meshFilter.mesh.vertices;
        this.vertices = this.meshFilter.mesh.vertices;

        o4 = new Vector4(this.transform.position.x, this.transform.position.y, this.transform.position.z, -this.transform.position.magnitude);
        x3 = cuM.x4;
    }

    public void Update()
    {
        //Object's center point in player's rest frame
        o4 = new Vector4(this.transform.position.x, this.transform.position.y, this.transform.position.z, -this.transform.position.magnitude);
        l = cM.L;//world frame to player's rest frame
        //cM.xx4 is the player's position in player's rest frame
        for (int i = 0; i < vertices.Length; ++i)
        {
            Vector3 vertex = orgvertices[i];
            Vector3 vv = x3 + vertex;
            //vv4 in world frame
            Vector4 vv4 = vv;
            vv4.w = cM.xx4.w - (cM.xx3 - vv).magnitude;
            //position of vertices in World Frame4
            /*if (!this.originalVertices.Contains(vertex))
            {*/
            //
            k = l * vv4;
            this.targetVertices.Add(new Vector3(k.x, k.y, k.z));
        }

        // 現在位置の更新
        for (int i = 0; i < this.targetVertices.Count; ++i)
        {
            Vector3 targetPos = this.targetVertices[i];
            this.targetVertices[i] = targetPos;
        }

        // verticesに渡す頂点を作成
        for (int i = 0; i < this.vertices.Length; ++i)
        {
            this.vertices[i] = this.targetVertices[i];
        }

        // 頂点を渡す
        this.meshFilter.mesh.vertices = this.vertices;
    }
}
