using UnityEngine;
using Zenject;

public class FishingRodController : MonoBehaviour
{
    [SerializeField] private Animator animatorRod;

    [SerializeField] private Rigidbody hookRb;

    [SerializeField] private Transform castSpp;
    
    [SerializeField] private Transform hookSpp;
    
    private InputHandler _inputHandler;

    private bool _startCasting;

    private bool _isCasted;
    
    [Inject]
    public void Construct(InputHandler input)
    {
        _inputHandler = input;

        _inputHandler.OnCast += Cast;
    }

    private void Cast(bool cast)
    {

        if (_isCasted && cast)
        {
            _isCasted = false;
            
            hookRb.transform.position = hookSpp.transform.position;
            
            hookRb.transform.parent = hookSpp.transform;
            
            hookRb.isKinematic = true;
            
        }
        else if (cast && !_startCasting)
        {
            _startCasting = true;

            _isCasted = false;
            
            animatorRod.SetBool("Casting", true);
        }
        else if (!cast && _startCasting)
        {
            _startCasting = false;
            
            _isCasted = true;
            
            animatorRod.SetBool("Casting", false);
            
            hookRb.isKinematic = false;
            hookRb.transform.parent = null;
            
            hookRb.linearVelocity = Vector3.zero;
            hookRb.AddForce(castSpp.forward * 5, ForceMode.Impulse);
        }
    }
}
