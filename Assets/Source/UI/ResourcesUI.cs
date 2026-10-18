using System;
using TMPro;
using UnityEngine;
using Zenject;

public class ResourcesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nuts;
    [SerializeField] private TextMeshProUGUI _wire;
    [SerializeField] private TextMeshProUGUI _bolts;
    
    private ResourcesManager _resourcesManager;

    [Inject]
    public void Construct(ResourcesManager resourcesManager)
    {
        _resourcesManager = resourcesManager;
        _resourcesManager.updateResources += UpdateText;
    }

    private void UpdateText()
    {
        _nuts.text = _resourcesManager.GetNuts().ToString();
        _wire.text = _resourcesManager.GetWires().ToString();
        _bolts.text = _resourcesManager.GetBolts().ToString();
    }

    private void OnDisable()
    {
        if (_resourcesManager != null)
        {
            _resourcesManager.updateResources -= UpdateText;
        }
    }

    private void OnEnable()
    {
        if (_resourcesManager != null)
        {
            _resourcesManager.updateResources += UpdateText;
            UpdateText();
        }
    }
}
