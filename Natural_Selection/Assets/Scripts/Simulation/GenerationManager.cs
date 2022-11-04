using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    [SerializeField]
    List<Component> cells = new();

    [SerializeField]
    List<long> dead_cells = new();
    [SerializeField]
    List<long> created_cells = new();

    [SerializeField]
    string generation_name;
    [SerializeField]
    ServerSpeaker.GenerationData generation_data;

    [SerializeField]
    float tick_counter;

    [SerializeField]
    Component lifeType;
    [SerializeField]
    Component sceneSetup;

    [SerializeField]
    ServerSpeaker ss;

    [SerializeField]
    bool _lock = true;

    void Awake()
    {
        ss = FindObjectOfType<ServerSpeaker>();

        GenInfo go = FindObjectOfType<GenInfo>();
        generation_name = go.Generation_name;
        Destroy(go.gameObject);

        ss.GetGenerations(ContinueAwaking);
    }
    void ContinueAwaking(ServerSpeaker.GenerationsResponse all_generations)
    {
        generation_data = all_generations.generations.Where(x => x.name == generation_name).FirstOrDefault();

        tick_counter = generation_data.tick;

        SetupMap();
        SetupFeeding();
        SetupLifeType();
        if (generation_data.last_send_num == 0)
        {
            CreateFirstCells();
            _lock = false;
        }
        else
        {
            //ss.GetCellsFromTick(generation_name, generation_data.last_send_num - 1, CompleteAwaking);
        }
    }
    public void CompleteAwaking(ServerSpeaker.CellsFromTickResponse response)
    {
        foreach (var item in response.cells)
        {
            CellCreator.CreateCellFromData(item, RememberDead, RememberBirth, RememberLoad);
        }
        _lock = false;
    }

    public void FixedUpdate()
    {
        if (_lock)
            return;

        tick_counter -= Time.fixedDeltaTime;

        if (tick_counter <= 0)
        {
            tick_counter = generation_data.tick;
            if (cells.Count == 0)
            {
                (lifeType as ILifeType).CreateNewCells(RememberDead, RememberBirth);
            }
            PushChangesToServer();
        }
    }
    public void RememberDead(Component cell)
    {
        dead_cells.Add((cell as Cell).ID);
        cells.Remove(cell);
    }
    public void RememberLoad(Component cell)
    {
        cells.Add(cell);
    }
    public void RememberBirth(Component cell)
    {
        (cell as Cell).ID = generation_data.last_cell_num;
        generation_data.last_cell_num++;
        cells.Add(cell);
        created_cells.Add((cell as Cell).ID);
    }
    void PushChangesToServer()
    {
        foreach (var cell in dead_cells)
        {
            if (created_cells.Contains(cell))
            {
                created_cells.Remove(cell);
                dead_cells.Remove(cell);
            }
        }

        List<ServerSpeaker.CellData> created = new();
        foreach (var item in created_cells)
        {
            Component buffer = cells.Find((x) => (x as Cell).ID == item);
            if (buffer != null)
                created.Add((buffer as Cell).GetCellData());
        }
        

        List<ServerSpeaker.ModuleDataWithId> changes = new();
        foreach (var item in cells)
        {
            if (!created_cells.Contains((item as Cell).ID)) {
                List<ServerSpeaker.ModuleData> md = (item as Cell).GetPositionsData();
                changes.AddRange(md.Select(x => new ServerSpeaker.ModuleDataWithId((item as Cell).ID, x.name, x.value)));
            }
        }

        created_cells.Clear();
        ServerSpeaker.SendTickChangesData sendTickChangesData = new(created, changes, dead_cells);
        ss.SendTickChanges(generation_name, generation_data.last_send_num, sendTickChangesData, ChangesAnsver);

        generation_data.last_send_num++;
        dead_cells.Clear();
    }
    public void ChangesAnsver(bool value)
    {
        if (value)
        {
            Debug.Log("Changes of number " + (generation_data.last_send_num - 1) + " sucsessful.");
        }
        else
        {
            Debug.Log("Changes of number " + (generation_data.last_send_num - 1) + " bad.");
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
        Instantiate(go);
        lifeType = go.GetComponents<Component>().Where(x => x is ILifeType).First();
    }
    public void CreateFirstCells()
    {
        if (sceneSetup == null)
        {
            GameObject go = Resources.Load("GenerationSetups/SetupsScripts/" + generation_data.setup_type) as GameObject;
            GameObject gameO = Instantiate(go);
            sceneSetup = gameO.GetComponents<Component>().ToList().Where(x => x is ISceneSetup).First();
            (sceneSetup as ISceneSetup).FillWithJson(generation_data.setup_json);
        }
        (sceneSetup as ISceneSetup).CreateFirstCells(RememberDead, RememberBirth);
    }
}