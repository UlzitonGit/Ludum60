using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FishCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject fishCardPrefab;
    
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _weight;
    
    [SerializeField] private Image _sprite;

    [SerializeField] private TextMeshProUGUI _bloodLevel;
    [SerializeField] private TextMeshProUGUI _oxygenLevel;
    
    [SerializeField]  private Image panelImage;
    
    private bool isSelected = false;
    
    
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow; 

    private int _id;
    
    private BucketUIController _bucketUIController;

    public void Init(int id, BucketUIController bucketUIController)
    {
        _id = id;
        
        _bucketUIController = bucketUIController;
    }

    public void ShowInfo(string name, string description, Sprite sprite, float weight, float oxygenLevel,
        float bloodLevel)
    {
        
        fishCardPrefab.SetActive(true);
        
        _name.text = name;
        
        _weight.text = weight.ToString();
        _description.text = description;
        _sprite.sprite = sprite;
        _bloodLevel.text = bloodLevel.ToString();
        _oxygenLevel.text = oxygenLevel.ToString();
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!fishCardPrefab.activeInHierarchy) return;
        isSelected = !isSelected; 
        panelImage.color = isSelected ? selectedColor : normalColor;

        _bucketUIController.FishCardPicked(_id);
    }

    public void UnSelect()
    {
        isSelected = false;
        panelImage.color = normalColor;
    }

    public void HideInfo()
    {
        fishCardPrefab.SetActive(false);
    }
    
    public int GetId()
    {
        return _id;
    }
}
