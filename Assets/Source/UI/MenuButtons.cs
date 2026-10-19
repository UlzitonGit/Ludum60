using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtons : MonoBehaviour, IPointerEnterHandler,  IPointerExitHandler
{
    [SerializeField] private int _pos;
    [SerializeField] private MenuHooks _menuHooks;
    [SerializeField] private AudioPlayer _audioPlayer;
    [SerializeField] private AudioClip _audioClip;

    private bool enter;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != gameObject) return;
        
        
        print(enter);
        _menuHooks.SetButton(_pos);
        _audioPlayer.Play(_audioClip);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        enter = false;
    }
}
