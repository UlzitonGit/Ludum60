using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MainHeart : MonoBehaviour
{
    [SerializeField] private Image oxygenBar;
    [SerializeField] private Image feBar;
    
    [SerializeField] private float maxOxygen;
    
    [SerializeField] private TutorialStage tutorialStage;
    
    [SerializeField] private Animator animator;

    [SerializeField] private FloatingShop _floatingShop;
    
    [SerializeField] private Transform _spp;

    [SerializeField] private GameObject _hint;
    [SerializeField] private GameObject _hint2;

    [SerializeField] private GameObject _playerCam;
    [SerializeField] private GameObject _animCam;
    [SerializeField] private Animator _bloodAnimator;

    [SerializeField] private GameObject _deathPanel;
    
    
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


    private bool isFeeding;
    private bool isEnding;
    
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

        _inputHandler.OnKnife += TryToEnd;
        
        StartCoroutine(DecreaseStats());
    }

    public void TryToFeed(bool click)
    {
        if (inZone && _hook.HasFish() && _tutorialController.GetStageIndex() > 4 && !isFeeding)
        {
            StartCoroutine(Feed());
        }
    }

    private void TryToEnd(bool click)
    {
        if (_floatingShop.GetHasKnife() && !isEnding && !isFeeding)
        {
            StartCoroutine(End());
        }
    }

    IEnumerator Feed()
    {
        _hint.SetActive(false);
        isFeeding = true;
        _hint2.SetActive(false);
        _inputHandler.SetStopMovement(true);
        
        
        animator.SetTrigger("Feed");
        
        _playerCam.SetActive(false);
        _animCam.SetActive(true);
        
        AddFee(_hook.Getfish().GetFishData().bloodLevel);
        AddOxygen(_hook.Getfish().GetFishData().oxygenLevel);
            
        
        Fish fish = _hook.ThrowFish(_spp);
        
        yield return new WaitForSeconds(3f);
        
        _bloodAnimator.SetTrigger("Blood");
        yield return new WaitForSeconds(2f);
        
        _animCam.SetActive(false);
        _inputHandler.SetStopMovement(false);
        isFeeding = false;
        
        
        
        _playerCam.SetActive(true);
        
        if (!_tutor)
        {
            tutorialStage.NextStage();
            _tutor = true;
        }
        
        Destroy(fish.gameObject);
    }
    IEnumerator End()
    {
        _hint.SetActive(false);
        _hint2.SetActive(false);
        isEnding = true;
        _inputHandler.SetStopMovement(true);
        
        
        
        _playerCam.SetActive(false);
        _animCam.SetActive(true);
        animator.SetTrigger("End");
        
        yield return new WaitForSeconds(26f);

        isEnding = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(2);
        
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
        yield return new WaitForSeconds(4f);
        oxygen -= _decreaseSpeed;
        fe -= _decreaseSpeed;
        if (fe <= 0 || oxygen <= 0)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _deathPanel.SetActive(true);
        }
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
            _inputHandler.OnKnife -= TryToEnd;
        }
    }

    private void OnEnable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnInteract += TryToFeed;
            _inputHandler.OnKnife += TryToEnd;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = true;
            if (_hook.HasFish())
            {
                _hint.SetActive(true);
            }
            if (_floatingShop.GetHasKnife())
            {
                _hint2.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = false;
            _hint.SetActive(false);
            _hint2.SetActive(false);
        }
    }
}
