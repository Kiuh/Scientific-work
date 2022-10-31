using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    List<Cell_WithID> cells = new();

    List<long> dead_cells = new();
    List<long> created_cells = new();

    string generation_name;
    ServerSpeaker.GenerationData generation_data;

    float tick_counter;

    ILifeType lifeType;
    ISceneSetup sceneSetup;

    ServerSpeaker ss;

    void Awake()
    {
        ss = FindObjectOfType<ServerSpeaker>();
        GenInfo go = FindObjectOfType<GenInfo>();
        generation_name = go.Generation_name;
        Destroy(go);

        ss.GetGenerations(ContinueAwaking);
    }

    bool _lock = true;

    void ContinueAwaking(ServerSpeaker.GenerationsResponse all_generations)
    {
        generation_data = all_generations.generations.Where(x => x.name == generation_name).FirstOrDefault();

        tick_counter = generation_data.tick;

        SetupMap();
        SetupFeeding();
        SetupLifeType();
        if (generation_data.last_send_num == 0)
        {
            SetIds_Pack(CreateFirstCells());
        }
        else
        {
            // Get cells from db and spawn them
        }
        _lock = false;
    }

    public void FixedUpdate()
    {
        if (_lock)
            return;
        tick_counter -= Time.fixedDeltaTime;

        foreach (var cell in cells)
            if (cell.cell == null)
                cells.Remove(cell);

        if (tick_counter <= 0)
        {
            tick_counter = generation_data.tick;
            if (cells.Count == 0)
            {
                SetIds_Pack(lifeType.CreateNewCells());
            }
            PushChangesToServer();
        }
    }
    public void RememberDead(long id)
    {
        dead_cells.Add(id);
    }
    public void CreateNewLife(long patent_id, CreateCellParameters parameters)
    {
        SetIds_Pack(new List<Cell>() { new CellCreator().CreateCell(parameters) } );
    }
    void PushChangesToServer()
    {
        List<ServerSpeaker.CellData> created = new();
        foreach (var item in created_cells)
        {
            Cell_WithID buffer = cells.Find((x) => x.id == item);
            if (buffer != null)
                created.Add(buffer.GetCellData());
        }
        created_cells.Clear();
        List<ServerSpeaker.ModuleDataWithId> changes = new();
        foreach (var item in cells)
        {
            changes.AddRange(item.GetPositionChanges());
        }
        ServerSpeaker.SendTickChangesData sendTickChangesData = new(created, changes, dead_cells);
        //ss.SendTickChanges(generation_name, generation_data.last_send_num, sendTickChangesData, ChangesAnsver);

        generation_data.last_send_num++;
        dead_cells.Clear();
    }

    public void ChangesAnsver(bool value)
    {
        if (value)
        {
            Debug.Log("Changes of number " + generation_data.last_send_num-- + " sucsessful.");
        }
        else
        {
            Debug.LogError("Changes of number " + generation_data.last_send_num-- + " bad.");
        }
    }

    void SetupMap()
    {
        GameObject go = Resources.Load("Maps/" + generation_data.map) as GameObject;
        Instantiate(go);
    }
    void SetupFeeding()
    {
        GameObject go = Resources.Load("Feeding/" + generation_data.feed_type) as GameObject;
        Instantiate(go);
    }
    void SetupLifeType()
    {
        GameObject go = Resources.Load("LifeTypes/" + generation_data.life_type) as GameObject;
        //lifeType = go.GetComponents<Component>().Where(x => x is ILifeType).Cast<ILifeType>().First();
        lifeType = go.GetComponent<ILifeType>();
        //Debug.Log(lifeType);
        Instantiate(go);
    }
    public List<Cell> CreateFirstCells()
    {
        if (sceneSetup == null)
        {
            GameObject go = Resources.Load("GenerationSetups/SetupsScripts/" + generation_data.setup_type) as GameObject;
            GameObject gameO = Instantiate(go, gameObject.transform);
            List<Component> components = gameO.GetComponents<Component>().ToList();
            sceneSetup = components.Where(x => x is ISceneSetup).First() as ISceneSetup;
            sceneSetup.FillWithJson(generation_data.setup_json);
        }
        return sceneSetup.CreateFirstCells();
    }
    void SetIds_Pack(List<Cell> cells)
    {
        List<Cell_WithID> cell_WithIDs = new();
        foreach (Cell cell in cells)
        {
            cell_WithIDs.Add(new Cell_WithID(-1, generation_data.last_cell_num, cell, RememberDead, CreateNewLife));
            generation_data.last_cell_num++;
        }
        created_cells.AddRange(cell_WithIDs.Select(x => x.id));
        this.cells.AddRange(cell_WithIDs);
    }
}

class Cell_WithID
{
    public long parent_id;
    public long id;
    public Cell cell;
    Action<long> death;
    Action<long, CreateCellParameters> birthNew;
    public Cell_WithID(long parent_id, long id, Cell cell, Action<long> death, Action<long, CreateCellParameters> birthNew)
    {
        this.parent_id = parent_id;
        this.id = id;
        this.cell = cell;
        this.death = death;
        this.birthNew = birthNew;
        this.cell.birth_trigger += BirthNewCell;
    }
    void BirthNewCell(CreateCellParameters cellParameters)
    {
        birthNew(id, cellParameters);
    }

    public ServerSpeaker.CellData GetCellData()
    {
        List<ServerSpeaker.ModuleData> modulesData = cell.modulesData;
        ServerSpeaker.IntellectData intellectData = cell.Intellect.IntellectData;
        ServerSpeaker.CellData data = new(parent_id, id, modulesData, intellectData);
        return data;
    } 

    public List<ServerSpeaker.ModuleDataWithId> GetPositionChanges()
    {
        List<ServerSpeaker.ModuleData> list = cell.GetPositionsData();
        return list.Select(x => new ServerSpeaker.ModuleDataWithId(id, x.name, x.value)).ToList();
    }

    ~Cell_WithID()
    {
        death(id);
    }
}