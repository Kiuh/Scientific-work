using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    List<Cell_WithID> cells = new List<Cell_WithID>();

    List<long> dead_cells = new List<long>();
    List<long> created_cells = new List<long>();

    string generation_name;
    ServerSpeaker.GenerationData3 generation_data;

    float tick_counter;

    ILifeType lifeType;
    ISceneSetup sceneSetup;

    void Start()
    {
        GenInfo go = FindObjectOfType<GenInfo>();
        generation_name = go.Generation_name;
        Destroy(go);

        generation_data = ServerSpeaker.GetGenerationInfoForStart(generation_name);
        tick_counter = generation_data.tick;

        SetupMap();
        SetupFeeding();
        SetupLifeType();
        if (generation_data.last_send_num == 0)
        {
            List<Cell_WithID> new_cells = SetIds(CreateFirstCells(), ref generation_data.last_cell_num);
            created_cells.AddRange(new_cells.Select(x => x.id));
            cells.AddRange(new_cells);
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
                List<Cell_WithID> new_cells = SetIds(lifeType.CreateNewCells(), ref generation_data.last_cell_num);
                created_cells.AddRange(new_cells.Select(x => x.id));
                cells.AddRange(new_cells);
            }
            PushChangesToServer();
        }
    }

    public void RememberDead(long id)
    {
        dead_cells.Add(id);
    }
    public void RememberLive(long id)
    {
        created_cells.Add(id);
    }

    public void PushChangesToServer()
    {
        // Send to server
    }

    public void SetupMap()
    {
        GameObject go = Resources.Load("Maps/" + generation_data.map) as GameObject;
        Instantiate(go);
    }
    public void SetupFeeding()
    {
        GameObject go = Resources.Load("Feeding/" + generation_data.feed_type) as GameObject;
        Instantiate(go);
    }
    public void SetupLifeType()
    {
        GameObject go = Resources.Load("LifeTypes/" + generation_data.life_type) as GameObject;
        lifeType = go.GetComponent<ILifeType>();
        Instantiate(go);
    }
    public List<Cell> CreateFirstCells()
    {
        if (sceneSetup == null)
        {
            GameObject go = Resources.Load("GenerationSetups/SetupsScripts/" + generation_data.setup_type) as GameObject;
            sceneSetup = Instantiate(go, gameObject.transform).GetComponent<ISceneSetup>();
            sceneSetup.FillWithJson(generation_data.setup_json);
        }
        return sceneSetup.CreateFirstCells();
    }
    public List<Cell_WithID> SetIds(List<Cell> cells, ref long last_id)
    {
        List<Cell_WithID> cell_WithIDs = new List<Cell_WithID>();
        foreach (Cell cell in cells)
        {
            cell_WithIDs.Add(new Cell_WithID(last_id, cell, RememberDead));
            last_id++;
        }
        return cell_WithIDs;
    }
}

public class Cell_WithID
{
    public delegate void Death(long id);
    public long id;
    public Cell cell;
    Death death;
    public Cell_WithID(long id, Cell cell, Death death)
    {
        this.id = id;
        this.cell = cell;
        this.death = death;
    }
    ~Cell_WithID()
    {
        death(id);
    }
}
