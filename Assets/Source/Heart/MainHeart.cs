using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainHeart : MonoBehaviour
{
    [SerializeField] private Image oxygenBar;
    [SerializeField] private Image feBar;
    
    [SerializeField] private float maxOxygen;
    
    [SerializeField] private float maxFe;
    
    private InputHandler _inputHandler;
    
    private Hook _hook;
    
    private float oxygen;
    private float fe;
    
    private bool inZone;

    [Inject]
    public void Construct(InputHandler inputHandler, Hook hook)
    {
        oxygen = maxOxygen;
        fe = maxFe;
        
        _hook = hook;

        _inputHandler = inputHandler;

        _inputHandler.OnInteract += TryToFeed;
        
        StartCoroutine(DecreaseStats());
    }

    public void TryToFeed(bool click)
    {
        if (inZone && _hook.HasFish())
        {
            AddFee(_hook.Getfish().GetFishData().bloodLevel);
            AddOxygen(_hook.Getfish().GetFishData().oxygenLevel);
            
           Fish fish = _hook.ThrowFish(transform);
           Destroy(fish.gameObject);
        }
    }

    private void AddOxygen(int amount)
    {
        oxygen += amount;
        if(oxygen > maxOxygen) oxygen = maxOxygen;
        
        oxygenBar.fillAmount = oxygen / maxOxygen;
    }

    private void AddFee(int amount)
    {
        fe += amount;
        if(fe > maxFe) fe = maxFe;
        
        feBar.fillAmount = fe / maxFe;
    }

    IEnumerator DecreaseStats()
    {
        yield return new WaitForSeconds(2f);
        oxygen -= 1;
        fe -= 1;
        
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
