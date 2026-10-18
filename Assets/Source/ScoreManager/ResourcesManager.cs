using System;
using UnityEngine;

public class ResourcesManager
{
    public Action updateResources;
    
    private int nuts;

    private int bolts;

    private int wires;

    public void AddNuts(int nuts)
    {
        this.nuts += nuts;
        
        updateResources?.Invoke();
    }

    public void AddBolts(int bolts)
    {
        this.bolts += bolts;
        
        updateResources?.Invoke();
    }

    public void AddWires(int wires)
    {
        this.wires += wires;
        
        updateResources?.Invoke();
    }

    public int GetNuts()
    {
        return nuts;
    }

    public int GetBolts()
    {
        return bolts;
    }

    public int GetWires()
    {
        return wires;
    }
}
