using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ServerSpeaker;

public class GenerationManager : MonoBehaviour
{
    List<Cell_WithID> cells = new();

    List<long> dead_cells = new();
    List<long> created_cells = new();

    string generation_name;
    GenerationData generation_data;

    float tick_counter;

    ILifeType lifeType;
    ISceneSetup sceneSetup;

    void Awake()
    {
        GenInfo go = FindObjectOfType<GenInfo>();
        generation_name = go.Generation_name;
        Destroy(go);

        var all_generations = GetGenerations();
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
    }
    public void FixedUpdate()
    {
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
        // Send to server
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
        Debug.Log(lifeType);
        Instantiate(go);
    }
    public List<Cell> CreateFirstCells()
    {
        if (sceneSetup == null)
        {
            GameObject go = Resources.Load("GenerationSetups/SetupsScripts/" + generation_data.setup_type) as GameObject;
            //Debug.Log("GenerationSetups/SetupsScripts/" + generation_data.setup_type);
            sceneSetup = Instantiate(go, gameObject.transform).GetComponent<ISceneSetup>();
            sceneSetup.FillWithJson(generation_data.setup_json);
        }
        return sceneSetup.CreateFirstCells();
    }
    void SetIds_Pack(List<Cell> cells)
    {
        List<Cell_WithID> cell_WithIDs = new List<Cell_WithID>();
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
    public delegate void Death(long id);
    public delegate void BirthNew(long parent_id, CreateCellParameters cellParameters);
    public long parent_id;
    public long id;
    public Cell cell;
    Death death;
    BirthNew birthNew;
    public Cell_WithID(long parent_id, long id, Cell cell, Death death, BirthNew birthNew)
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
    ~Cell_WithID()
    {
        death(id);
    }
}