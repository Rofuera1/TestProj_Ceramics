using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Model : MonoBehaviour
{
    [SerializeField] private Mesh Mesh;
    [SerializeField] private Material MaterialAligned;
    [SerializeField] private Material MaterialUnaligned;

    [Zenject.Inject] private Space Space;

    private List<Matrix4x4> MatrixiesAligned;
    private List<Matrix4x4> MatrixiesUnaligned;
    private IEnumerator continuosDrawing;

    public Matrix4x4[] MatrixesAtStart { get; private set; }

    private const string PATH = "Assets/Data/model.json";

    public async Task LoadAll()
    {
        await LoadMatrix();
    }

    private async Task LoadMatrix()
    {
        MatrixesAtStart = await Reader.LoadAddressableText(PATH);
    }

    public void ContinueDrawingMeshes()
    {
        StartCoroutine(continuosDrawing = drawingExistingMeshes());
    }

    public void StopDrawingContinuesMeshes()
    {
        if(continuosDrawing != null)
            StopCoroutine(continuosDrawing);
    }

    private IEnumerator drawingExistingMeshes()
    {
        while(true)
        {
            Graphics.DrawMeshInstanced(Mesh, 0, MaterialAligned, MatrixiesAligned);
            yield return null;
        }
    }

    /// <summary>
    /// Draws all suitable variables, returns result if all boxes fit
    /// </summary>
    /// <param name="Offset"></param>
    /// <returns></returns>
    public bool DrawMatrixies(Matrix4x4 Offset)
    {
        MatrixiesAligned = new List<Matrix4x4>();
        MatrixiesUnaligned = new List<Matrix4x4>();

        for(int i = 0; i < MatrixesAtStart.Length; i++)
        {
            Matrix4x4 OffsettedMatrix = Offset * MatrixesAtStart[i];
            if (Space.DoesContain(OffsettedMatrix))
            {
                MatrixiesAligned.Add(OffsettedMatrix);
            }
            else
            {
                MatrixiesUnaligned.Add(OffsettedMatrix);
                break;
            }
        }

        Graphics.DrawMeshInstanced(Mesh, 0, MaterialAligned, MatrixiesAligned);
        Graphics.DrawMeshInstanced(Mesh, 0, MaterialUnaligned, MatrixiesUnaligned);

        return MatrixiesUnaligned.Count == 0;
    }
}
