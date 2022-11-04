using System;
using System.Collections.Generic;
using UnityEngine;

public class RepeatSetupLifeType : MonoBehaviour, ILifeType
{
    public void CreateNewCells(Action<Component> death, Action<Component> birth)
    {
        FindObjectOfType<GenerationManager>().CreateFirstCells();
    }
}
