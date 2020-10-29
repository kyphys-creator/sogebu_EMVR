using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    //MeshFilter
    private MeshFilter meshFilter;
    //Vertices of MeshFilter data
    private Vector3[] originalvertices;
    private Vector3[] vertices;

    /*
     * 動かす際のターゲットとなる重複のない頂点データ。
     */
    private List<Vector3> targetVertices = new List<Vector3>();

    Camera cam;
    public GameObject centralObject;
    Vector3 centralobjectpos3;
    Vector4 centralobjectpos4;
    Vector3 centralobjectvelworld3;
    Vector4 centralobjectvelworld4;
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
        centralobjectpos3 = centralObject.transform.position;
        centralobjectpos4 = new Vector4(centralobjectpos3.x, centralobjectpos3.y, centralobjectpos3.z, -centralobjectpos3.magnitude);
        l = cM.Lplayer;
        Matrix4x4 linv = cM.Lplayerinverse;
        this.meshFilter = this.GetComponent<MeshFilter>();
        this.originalvertices = this.meshFilter.mesh.vertices;
        this.vertices = this.meshFilter.mesh.vertices;

        //o4 = new Vector4(this.transform.position.x, this.transform.position.y, this.transform.position.z, -this.transform.position.magnitude);
        //x3 = cuM.x4;
        x3 = linv * centralobjectpos4;//body's position in world frame

        centralobjectvelworld3 = new Vector3(0.0f, 0.0f, 0.0f);
        centralobjectvelworld4 = centralobjectvelworld3;
        centralobjectvelworld4.w = 1.0f;
    }

    public void LateUpdate()
    {
        centralobjectpos3 = centralObject.transform.position;
        centralobjectpos4 = new Vector4(centralobjectpos3.x, centralobjectpos3.y, centralobjectpos3.z, -centralobjectpos3.magnitude);
        //Object's center point in player's rest frame
        //o4 = new Vector4(this.transform.position.x, this.transform.position.y, this.transform.position.z, -this.transform.position.magnitude);
        l = cM.Lplayer;//world frame to player's rest frame
        Matrix4x4 linv = cM.Lplayerinverse;
        x3 = linv * centralobjectpos4;
        //cM.xx4 is the player's position in player's rest frame

        this.targetVertices = new List<Vector3>();
        for (int i = 0; i < vertices.Length; ++i)
        {
            Vector3 vertex = originalvertices[i];
            //vertObject.TransformPoint(vertex);
            Vector3 vv = x3 + vertex;
            //vv4 in world frame
            Vector4 vv4 = vv;
            vv4.w = cM.playrposworldframe4.w - (cM.playrposworldframe3 - vv).magnitude;
            //position of vertices in World Frame4
            /*if (!this.originalVertices.Contains(vertex))
            {*/
            //
            k = l * vv4;
            this.targetVertices.Add(new Vector3(k.x, k.y, k.z));
        }

        // 現在位置の更新
        /*for (int i = 0; i < this.targetVertices.Count; ++i)
        {
            Vector3 targetPos = this.targetVertices[i];
            this.targetVertices[i] = targetPos;
        }*/

        // verticesに渡す頂点を作成
        for (int i = 0; i < this.vertices.Length; ++i)
        {
            this.vertices[i] = this.targetVertices[i];
        }

        // 頂点を渡す
        this.meshFilter.mesh.vertices = this.vertices;
    }
}
