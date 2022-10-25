using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISetup
{
    public void PushInformation(string json);
    public string GetNewInformation();
}
