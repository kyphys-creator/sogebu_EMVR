using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralRelmetric : MonoBehaviour
{
    public Matrix4x4 metrictensor;
    private Matrix4x4 specialrelativitymetric;
    private Matrix4x4 perturbationmetric;
    // Start is called before the first frame update
    void Start()
    {
        specialrelativitymetric = Matrix4x4.identity;
        specialrelativitymetric.m33 = -1;
        perturbationmetric = Matrix4x4.zero;
        metrictensor = Sum(specialrelativitymetric, perturbationmetric);
    }

    // Update is called once per frame
    void Update()
    {
        specialrelativitymetric = Matrix4x4.identity;
        specialrelativitymetric.m33 = -1;
        perturbationmetric = Matrix4x4.zero;
        metrictensor = Sum(specialrelativitymetric, perturbationmetric);
    }
    public Matrix4x4 Sum(Matrix4x4 M1, Matrix4x4 M2)
    {
        Matrix4x4 Sum = Matrix4x4.identity;
        Sum.m00 = M1.m00 + M2.m00;
        Sum.m01 = M1.m01 + M2.m01;
        Sum.m02 = M1.m02 + M2.m02;
        Sum.m03 = M1.m03 + M2.m03;
        Sum.m10 = M1.m10 + M2.m10;
        Sum.m11 = M1.m11 + M2.m11;
        Sum.m12 = M1.m12 + M2.m12;
        Sum.m13 = M1.m13 + M2.m13;
        Sum.m20 = M1.m20 + M2.m20;
        Sum.m21 = M1.m21 + M2.m21;
        Sum.m22 = M1.m22 + M2.m22;
        Sum.m23 = M1.m23 + M2.m23;
        Sum.m30 = M1.m30 + M2.m30;
        Sum.m31 = M1.m31 + M2.m31;
        Sum.m32 = M1.m32 + M2.m32;
        Sum.m33 = M1.m33 + M2.m33;
        return Sum;
    }
}
