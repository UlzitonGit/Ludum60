using UnityEngine;
using Zenject;

public class TutorialStage : MonoBehaviour
{
    [SerializeField] private GameObject[] _stages;
    [SerializeField] private int stageIndex;
    
    private int stage;
    
    private TutorialController _tutorialController;
    
    public void Initialize(TutorialController tutorialController)
    {
        _tutorialController = tutorialController;
        
        _stages[0].gameObject.SetActive(true);
        _stages[0].GetComponent<IStage>().StartStage();
    }

    public void NextStage()
    {
        _stages[stage].GetComponent<IStage>().EndStage();
        _stages[stage].SetActive(false);
        stage++;
        if (stage >= _stages.Length - 1)
        {
            _tutorialController.ChangeStage();
            return;
        }
        _stages[stage].SetActive(true);
        _stages[stage].GetComponent<IStage>().StartStage();
    }
}
