using UnityEngine;
using Zenject;

public class FishingMinigame : MonoBehaviour
{ 
    [SerializeField] private MiniGameArrow _minigameArrow;
    
    [SerializeField] private RectTransform _catchZone;
    
    [SerializeField] private Hook _hook;
    
    private InputHandler _inputHandler;

    [Inject]

    public void Construct(InputHandler inputHandler)
    {
        _inputHandler = inputHandler;

        _inputHandler.OnCast += Strike;
        
        gameObject.SetActive(false);
    }

    private void Strike(bool strike)
    {
        if(!strike || !gameObject.activeInHierarchy) return;
        
        print("strike");
        
        if (_minigameArrow.OnZone())
        {
            _hook.ApplyImpulseInRod(1, 4);
            
        }
        else
        {
            _hook.ApplyImpulseInRod(-1, 4);
        }

        _catchZone.localPosition = new Vector3(Random.Range(-400f, 400f), 0, 0);
    }
    
    
    private void OnDisable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnCast -= Strike;
        }
    }

    private void OnEnable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnCast += Strike;
        }
        _catchZone.localPosition = new Vector3(Random.Range(-400f, 400f), 0, 0);
    }
}
