using UnityEngine;

public class MenuHooks : MonoBehaviour
{
    [SerializeField] private Transform[] _poses;
    [SerializeField] private Animator _anim;


    public void SetButton(int pos)
    {
        transform.position = _poses[pos].position;
        _anim.SetTrigger("Hook");
    }

    public void EndHook()
    {
        transform.position = _poses[0].position;
        _anim.SetTrigger("EndHook");
    }
}
