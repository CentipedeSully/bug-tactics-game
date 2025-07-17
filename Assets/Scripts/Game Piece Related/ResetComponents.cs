using SullysToolkit.TableTop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IResettable
{
    void ResetState();
}

public class ResetComponents : MonoBehaviour
{
    private List<IResettable> _resettableComponents=new();



    private void Awake()
    {
        IResettable[] resettableableComponents= GetComponents<IResettable>();
        foreach (IResettable components in resettableableComponents)
            _resettableComponents.Add(components);
    }

    public void ResetResettableComponents()
    {
        foreach (IResettable component in _resettableComponents)
            component.ResetState();
    }

    public void AddComponentToResettableList(IResettable component)
    {
        //ignore empties
        if (component == null)
            return;

        //Make sure it's not already included before adding
        if (!_resettableComponents.Contains(component))
            _resettableComponents.Add (component);
    }
}
