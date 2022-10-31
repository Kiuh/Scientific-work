using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneSetup
{
    public void FillWithJson(string json);
    public List<Cell> CreateFirstCells();
}
