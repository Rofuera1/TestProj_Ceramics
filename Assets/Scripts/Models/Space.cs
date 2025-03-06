using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Space : MonoBehaviour
{
    [SerializeField] private Mesh Mesh;
    [SerializeField] private Material Material;

    public Matrix4x4[] MatrixesAtStart { get; private set; }
    private Dictionary<Vector3Int, List<Matrix4x4>> SortedMatrixies; // Trying to optimize comparing

    private const string PATH = "Assets/Data/space.json";

    public async Task LoadAll()
    {
        await LoadMatrix();
        StartCoroutine(DrawMatrixiesAlways());
    }

    private async Task LoadMatrix()
    {
        MatrixesAtStart = await Reader.LoadAddressableText(PATH);
        SortedMatrixies = new Dictionary<Vector3Int, List<Matrix4x4>>();

        foreach (var Mat in MatrixesAtStart)
        {
            Vector3Int Key = KeyFromMatrix(Mat);
            if (!SortedMatrixies.ContainsKey(Key)) SortedMatrixies.Add(Key, new List<Matrix4x4>());

            SortedMatrixies[Key].Add(Mat);
        }
        //Debug.Log(MatrixiesSorted.Count); // Checked, if there are any matches - there are not :(
    }

    private Vector3Int KeyFromMatrix(Matrix4x4 Mat)
    {
        return new Vector3Int((int)Mat.m00, (int)Mat.m11, (int)Mat.m22);
    }

    private IEnumerator DrawMatrixiesAlways()
    {
        while (true)
        {
            yield return null;

            DrawMatrixies();
        }
    }

    private void DrawMatrixies()
    {
        Graphics.DrawMeshInstanced(Mesh, 0, Material, MatrixesAtStart);
    }

    public bool DoesContain(Matrix4x4 Matrix)
    {
        if (!SortedMatrixies.ContainsKey(KeyFromMatrix(Matrix))) return false;

        foreach (Matrix4x4 m in SortedMatrixies[KeyFromMatrix(Matrix)])
            if (ApproximatelyEqual(m, Matrix))
                return true;
        return false;
    }

    public bool ApproximatelyEqual(Matrix4x4 a, Matrix4x4 b, float epsilon = 0.001f)
    {
        for (int i = 0; i < 16; i++)
        {
            if (Mathf.Abs(a[i] - b[i]) > epsilon)
                return false;
        }
        return true;
    }
}
