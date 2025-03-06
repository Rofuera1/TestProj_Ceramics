using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image ProgressSlider;
    [SerializeField] private Button ContinueProgressButton;
    [SerializeField] private Toggle StopAtFoundResults;
    [SerializeField] private GameObject EndWindow;

    public System.Action ContinueCalculations;

    public void UpdateProgress(float Progress)
    {
        ProgressSlider.fillAmount = Progress;
    }

    public void OnStopAtResult()
    {
        if(StopAtFoundResults.isOn)
        {
            OnPressedAtContinue();
            return;
        }

        ContinueProgressButton.interactable = true;
    }

    public void OnPressedAtContinue()
    {
        ContinueProgressButton.interactable = false;

        ContinueCalculations?.Invoke();
    }

    public void OnEnded()
    {
        EndWindow.SetActive(true);
    }
}
