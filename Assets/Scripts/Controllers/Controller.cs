using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Zenject.Inject] private Space Space;
    [Zenject.Inject] private Model Model;
    [Zenject.Inject] private UIManager UI;

    public Action OnFoundGoodResult;
    public Action OnEnded;
    public Action<float> OnProgress;
    public List<Matrix4x4> ResultOffsets { get; private set; } = new List<Matrix4x4>();

    private int CurrentID;
    private int AllSpaceElementsCount; // It's known to be a 100, but using it not like a const is better

    private void Awake()
    {
        SubscribeEveryoneToActions();

        LoadAll();
    }

    private void SubscribeEveryoneToActions()
    {
        OnFoundGoodResult += UI.OnStopAtResult;
        OnProgress += UI.UpdateProgress;
        OnEnded += UI.OnEnded;
        OnEnded += SaveResult;

        UI.ContinueCalculations += ContinueIterating;
    }

    private async void SaveResult()
    {
        await Saver.SaveText("Result.json", new MatrixConverter(ResultOffsets.ToArray()));
    }

    public void ContinueIterating()
    {
        if (CurrentID >= AllSpaceElementsCount - 1) Debug.LogWarning("Trying to load more iterations at end");

        Model.StopDrawingContinuesMeshes();
        StartCoroutine(IterateWithDrawing());
    }

    private async void LoadAll()
    {
        Task SpaceTask = Space.LoadAll();
        Task ModelTask = Model.LoadAll();

        await Task.WhenAll(SpaceTask, ModelTask);

        CurrentID = 0;
        AllSpaceElementsCount = Space.MatrixesAtStart.Length;

        StartCoroutine(IterateWithDrawing());
    }

    private IEnumerator IterateWithDrawing()
    {
        Matrix4x4 MatrixAsBase = Model.MatrixesAtStart[0];

        while (CurrentID < AllSpaceElementsCount)
        {
            yield return null;

            Matrix4x4 Offset = NewOffset(Space.MatrixesAtStart[CurrentID], MatrixAsBase);
            CurrentID++;
            OnProgress?.Invoke((float)CurrentID / AllSpaceElementsCount);

            if (Model.DrawMatrixies(Offset))
            {
                OnFoundAppliccableResult(Offset);
                break;
            }
        }

        if(CurrentID >= AllSpaceElementsCount)
            OnEnded?.Invoke();
    }

    private void OnFoundAppliccableResult(Matrix4x4 Result)
    {
        ResultOffsets.Add(Result);
        OnFoundGoodResult?.Invoke();

        Model.ContinueDrawingMeshes();
    }

    private Matrix4x4 NewOffset(Matrix4x4 Space, Matrix4x4 Model)
    {
        Matrix4x4 Lin = Space * Model.inverse;
        return Lin;
    }
}
