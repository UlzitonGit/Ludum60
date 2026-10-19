using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainHeart : MonoBehaviour
{
    [SerializeField] private Image oxygenBar;
    [SerializeField] private Image feBar;
    
    [SerializeField] private float maxOxygen;
    
    [SerializeField] private TutorialStage tutorialStage;
    
    
    [SerializeField] private float maxFe;

    private bool _tutor;
    
    private InputHandler _inputHandler;
    
    private TutorialController _tutorialController;

    private float _fishActivity = 1;

    private float _rodPower = 1;

    private float _decreaseSpeed = 1;
    
    private Hook _hook;
    
    private float oxygen;
    private float fe;
    
    private bool inZone;

    [Inject]
    public void Construct(InputHandler inputHandler, Hook hook, TutorialController tutorialController)
    {
        
        _tutorialController = tutorialController;
        oxygen = maxOxygen;
        fe = maxFe;
        
        _hook = hook;

        _inputHandler = inputHandler;

        _inputHandler.OnInteract += TryToFeed;
        
        StartCoroutine(DecreaseStats());
    }

    public void TryToFeed(bool click)
    {
        if (inZone && _hook.HasFish() && _tutorialController.GetStageIndex() > 4)
        {
            AddFee(_hook.Getfish().GetFishData().bloodLevel);
            AddOxygen(_hook.Getfish().GetFishData().oxygenLevel);
            
           Fish fish = _hook.ThrowFish(transform);
           Destroy(fish.gameObject);
           if (!_tutor)
           {
               tutorialStage.NextStage();
               _tutor = true;
           }
        }
    }

    public void AddOxygen(int amount)
    {
        oxygen += amount;
        if(oxygen > maxOxygen) oxygen = maxOxygen;
        
        oxygenBar.fillAmount = oxygen / maxOxygen;
    }

    public void AddFee(int amount)
    {
        fe += amount;
        if(fe > maxFe) fe = maxFe;
        
        feBar.fillAmount = fe / maxFe;
    }

    public void SetFishActivity(float fishActivity)
    {
        this._fishActivity = fishActivity;
    }

    public void SetRodPower(float rodPower)
    {
        this._rodPower = rodPower;
    }

    public void SetDecreaseSpeed(float decreaseSpeed)
    {
        this._decreaseSpeed = decreaseSpeed;
    }

    public float GetFishActivity()
    {
        return this._fishActivity;
    }

    public float GetRodPower()
    {
        return this._rodPower;
    }
    

    IEnumerator DecreaseStats()
    {
        yield return new WaitForSeconds(2f);
        oxygen -= _decreaseSpeed;
        fe -= _decreaseSpeed;
        
        print(oxygen / maxOxygen);
        
        oxygenBar.fillAmount = oxygen / maxOxygen;
        feBar.fillAmount = fe / maxFe;
        StartCoroutine(DecreaseStats());
    }
    private void OnDisable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnInteract -= TryToFeed;
        }
    }

    private void OnEnable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnInteract += TryToFeed;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = false;
        }
    }
}
