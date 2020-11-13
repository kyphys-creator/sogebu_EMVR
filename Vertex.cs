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
    Vector3 centralobjectposworldframe3;
    Vector4 centralobjectposworldframe4;
    Vector3 centralobjectvelworld3;
    Vector4 centralobjectvelworld4;
    cameraMove cM;
    cubeMove cuM;
    private Matrix4x4 Lplayer;
    private Matrix4x4 Lplayerinverse;
    private Vector4 vertexposplayrrestframe4;//vertexposplayrrestframe4
    private Vector3 objectposworldframe4;//object's world frame position

    public void Awake()
    {
        cam = Camera.main;
        cM = cam.GetComponent<cameraMove>();
        cuM = centralObject.GetComponent<cubeMove>();
        //importing the position of central object in world frame from cubeMove's four vector x4
        centralobjectposworldframe4 = cuM.x4;
        centralobjectposworldframe3 = centralobjectposworldframe4;
        Lplayer = cM.Lplayer;
        Lplayerinverse = cM.Lplayerinverse;
        this.meshFilter = this.GetComponent<MeshFilter>();
        this.originalvertices = this.meshFilter.mesh.vertices;
        this.vertices = this.meshFilter.mesh.vertices;

        //o4 = new Vector4(this.transform.position.x, this.transform.position.y, this.transform.position.z, -this.transform.position.magnitude);
        //x3 = cuM.x4;
        objectposworldframe4 = Lplayerinverse * centralobjectposworldframe4;//body's position in world frame

        centralobjectvelworld3 = new Vector3(0.0f, 0.0f, 0.0f);
        centralobjectvelworld4 = centralobjectvelworld3;
        centralobjectvelworld4.w = 1.0f;
    }

    public void LateUpdate()
    {
        centralobjectposworldframe4 = cuM.x4;
        centralobjectposworldframe3 = centralobjectposworldframe4;
        //Object's center point in player's rest frame
        //o4 = new Vector4(this.transform.position.x, this.transform.position.y, this.transform.position.z, -this.transform.position.magnitude);
        Lplayer = cM.Lplayer;//world frame to player's rest frame
        Lplayerinverse = cM.Lplayerinverse;
        objectposworldframe4 = Lplayerinverse * centralobjectposworldframe4;
        //cM.xx4 is the player's position in player's rest frame

        this.targetVertices = new List<Vector3>();
        for (int i = 0; i < vertices.Length; ++i)
        {
            Vector3 vertex = originalvertices[i];
            //vertObject.TransformPoint(vertex);
            Vector3 vv = objectposworldframe4 + vertex;
            //vv4 in world frame
            Vector4 vv4 = vv;
            vv4.w = cM.playrposworldframe4.w - (cM.playrposworldframe3 - vv).magnitude;
            //position of vertices in World Frame4
            /*if (!this.originalVertices.Contains(vertex))
            {*/
            //
            vertexposplayrrestframe4 = Lplayer * vv4;
            this.targetVertices.Add(new Vector3(vertexposplayrrestframe4.x, vertexposplayrrestframe4.y, vertexposplayrrestframe4.z));
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
