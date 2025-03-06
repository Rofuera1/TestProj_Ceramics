using UnityEngine;

[System.Serializable]
public class MatrixConverter
{
    [SerializeField]
    public StringMatrix[] Matrixes;

    public MatrixConverter(StringMatrix[] matrixes)
    {
        Matrixes = matrixes;
    }

    public MatrixConverter(Matrix4x4[] matrixes)
    {
        Matrixes = new StringMatrix[matrixes.Length];
        for (int i = 0; i < matrixes.Length; i++)
        {
            Matrixes[i] = new StringMatrix(matrixes[i]);
        }
    }

    public Matrix4x4[] ReturnAsMatrix4x4()
    {
        Matrix4x4[] Result = new Matrix4x4[Matrixes.Length];
        for (int i = 0; i < Matrixes.Length; i++)
            Result[i] = Matrixes[i].ToUnityMatrix();
        return Result;
    }
}

[System.Serializable]
public class StringMatrix
{
    public float m00, m01, m02, m03;
    public float m10, m11, m12, m13;
    public float m20, m21, m22, m23;
    public float m30, m31, m32, m33;

    public StringMatrix(Matrix4x4 matr)
    {
        m00 = matr.m00; m01 = matr.m01; m02 = matr.m02; m03 = matr.m03;
        m10 = matr.m10; m11 = matr.m11; m12 = matr.m12; m13 = matr.m13;
        m20 = matr.m20; m21 = matr.m21; m22 = matr.m22; m23 = matr.m23;
        m30 = matr.m30; m31 = matr.m31; m32 = matr.m32; m33 = matr.m33;
    }

    public Matrix4x4 ToUnityMatrix()
    {
        Matrix4x4 mat = new Matrix4x4();
        mat.m00 = m00; mat.m01 = m01; mat.m02 = m02; mat.m03 = m03;
        mat.m10 = m10; mat.m11 = m11; mat.m12 = m12; mat.m13 = m13;
        mat.m20 = m20; mat.m21 = m21; mat.m22 = m22; mat.m23 = m23;
        mat.m30 = m30; mat.m31 = m31; mat.m32 = m32; mat.m33 = m33;
        return mat;
    }
}