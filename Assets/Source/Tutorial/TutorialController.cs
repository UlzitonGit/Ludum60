using System;
using UnityEngine;
using Zenject;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private TutorialStage[] stages;

    public Action<int> OnStageChanged;
    
    private int stageIndex = -1;
    private InputHandler _inputHandler;

    private bool _isTutorial;
    

    [Inject]
    public void Construct(InputHandler inputHandler)
    {
        _isTutorial = true;
        _inputHandler = inputHandler;
        ChangeStage();
    }

    public void ChangeStage()
    {
        if (stageIndex > 0)
        {
            stages[stageIndex].gameObject.SetActive(false);
        }
        if(stageIndex == stages.Length) return;
        
        stageIndex++;
        stages[stageIndex].Initialize(this);
        stages[stageIndex].gameObject.SetActive(true);
        OnStageChanged?.Invoke(stageIndex);
    }

    public bool IsTutorial()
    {
        return _isTutorial;
    }

    public int GetStageIndex()
    {
        return stageIndex;
    }
}
