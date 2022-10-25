using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatSetupLifeType : MonoBehaviour, ILifeType
{
    GenerationManager generationManager;

    private void Start()
    {
        generationManager = GameObject.FindObjectOfType<GenerationManager>();
    }

    public List<Cell> CreateNewCells()
    {
        return generationManager.CreateFirstCells();
    }
}
