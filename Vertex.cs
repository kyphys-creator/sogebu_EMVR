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
    cameraMove2 cameraMove2;
    cubeMove2 cubeMove2;
    public Matrix4x4 R;
    private Matrix4x4 Lplayer;
    private Matrix4x4 Lplayerinverse;
    private Vector4 vertexposplayrrestframe4;//vertexposplayrrestframe4
    private Vector3 objectposworldframe4;//object's world frame position

    public void Awake()
    {
        Quaternion q = centralObject.transform.rotation.normalized;
        //Defining Rotation Matrix by using Quartanion
        R = Matrix4x4.identity;
        R.m00 = q.x * q.x - q.y * q.y - q.z * q.z + q.w * q.w;
        R.m01 = 2 * (q.x * q.y - q.z * q.w);
        R.m02 = 2 * (q.x * q.z + q.y * q.w);
        R.m10 = 2 * (q.x * q.z + q.y * q.w);
        R.m11 = -q.x * q.x + q.y * q.y - q.z * q.z + q.w * q.w;
        R.m12 = 2 * (q.y * q.z - q.x * q.w);
        R.m20 = 2 * (q.x * q.z - q.y * q.w);
        R.m21 = 2 * (q.y * q.z + q.x * q.w);
        R.m22 = -q.x * q.x - q.y * q.y + q.z * q.z + q.w * q.w;

        cam = Camera.main;
        cameraMove2 = cam.GetComponent<cameraMove2>();
        cubeMove2 = centralObject.GetComponent<cubeMove2>();

        //importing the position of central object in world frame from cubeMove's four vector x4
        centralobjectposworldframe4 = cubeMove2.objposworldframe4;
        centralobjectposworldframe3 = centralobjectposworldframe4;

        //importing Lorentz transformation(Lplayer) and its inverse transformation from cameraMove
        Lplayer = cameraMove2.Lplayer;
        Lplayerinverse = cameraMove2.Lplayer.inverse;

        //importing basic meshdata(vertex list)
        this.meshFilter = this.GetComponent<MeshFilter>();
        this.originalvertices = this.meshFilter.mesh.vertices;
        this.vertices = this.meshFilter.mesh.vertices;

        //objectposworldframe4 = Lplayerinverse * centralobjectposworldframe4;//body's position in world frame

        centralobjectvelworld3 = new Vector3(0.0f, 0.0f, 0.0f);
        centralobjectvelworld4 = centralobjectvelworld3;
        centralobjectvelworld4.w = 1.0f;
    }

    public void LateUpdate()
    {
        centralobjectposworldframe4 = cubeMove2.objposworldframe4;
        centralobjectposworldframe3 = centralobjectposworldframe4;
        //Object's center point in player's rest frame
        //o4 = new Vector4(this.transform.position.x, this.transform.position.y, this.transform.position.z, -this.transform.position.magnitude);
        Lplayer = cameraMove2.Lplayer;//world frame to player's rest frame
        Lplayerinverse = cameraMove2.Lplayer.inverse;
        //objectposworldframe4 = centralobjectposworldframe4;
        //cM.xx4 is the player's position in player's rest frame

        this.targetVertices = new List<Vector3>();
        for (int i = 0; i < vertices.Length; ++i)
        {
            Vector3 vertex = originalvertices[i];
            //vertObject.TransformPoint(vertex);
            Vector3 vv = centralobjectposworldframe3 + vertex;
            //vv4 in world frame
            Vector4 vv4 = vv;
            vv4.w = cameraMove2.playrposworldframe4.w - (cameraMove2.playrposworldframe3 - vv).magnitude;
            //position of vertices in World Frame4
            /*if (!this.originalVertices.Contains(vertex))
            {*/
            //
            vertexposplayrrestframe4 = Lplayer * vv4 - cameraMove2.Lplayer * cubeMove2.objposworldframe4;
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
