using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour, IAction
{
    public int queue_number
    {
        get
        {
            return queue_number;
        }
    }

    public void DoAction(List<float> floats)
    {
        throw new System.NotImplementedException();
    }

    public int GetInputCount()
    {
        return 3;
    }

    void IAction.FindNeededPropertys(List<IProperty> properties)
    {
        throw new System.NotImplementedException();
    }
}
