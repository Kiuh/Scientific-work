using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ServerSpeaker : MonoBehaviour
{
    readonly string URI = "http://localhost:27503";
    string current_login = "";
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    #region Вход в аккаунт
    [Serializable]
    public class LoginData
    {
        public string login;
        public string password;
        public LoginData(string login, string password)
        {
            this.login = login;
            this.password = password;
        }
    }

    public void Login(LoginData data, Action<bool> action)
    {
        StartCoroutine(LoginSmart(data, action));
    }
    IEnumerator LoginSmart(LoginData data, Action<bool> action)
    {
        current_login = data.login;

        UnityWebRequest webRequest = new(URI + "/User", "POST");
        webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
        webRequest.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

        yield return webRequest.SendWebRequest();

        action(webRequest.responseCode == 200);
        webRequest.Dispose();
    }
    #endregion
    #region Регестрация
    [Serializable]
    public class RegistrationData
    {
        public string login;
        public string email;
        public string password;
        public RegistrationData(string login, string email, string password)
        {
            this.login = login;
            this.email = email;
            this.password = password;
        }
    }
    public void Registration(RegistrationData data, Action<bool> action)
    {
        StartCoroutine(RegistrationSmart(data, action));
    }
    IEnumerator RegistrationSmart(RegistrationData data, Action<bool> action)
    {
        UnityWebRequest webRequest = new(URI + "/User", "PUT");
        webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
        webRequest.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

        yield return webRequest.SendWebRequest();

        action(webRequest.responseCode == 200);
        webRequest.Dispose();
    }
    #endregion

    #region Получить все генерации (с информацией)
    public void GetGenerations(Action<GenerationsResponse> action)
    {
        StartCoroutine(GetGenerationsSmart(action));
    }
    IEnumerator GetGenerationsSmart(Action<GenerationsResponse> action)
    {
        string query = "?login=" + current_login;

        UnityWebRequest webRequest = new(URI + "/Generations" + query, "GET");
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        action(JsonUtility.FromJson<GenerationsResponse>(webRequest.downloadHandler.text));
        webRequest.Dispose();
    }
    [Serializable]
    public class GenerationsResponse
    {
        public List<GenerationData> generations;
    }
    [Serializable]
    public class GenerationData
    {
        public string name;
        public string map;
        public string life_type;
        public string feed_type;
        public string setup_type;
        public float tick;
        public long last_send_num;
        public string setup_json;
        public long last_cell_num;
        public string description;
    }
    #endregion
    #region Удалить генерацию
    public void DeleteGeneration(string gen_name, Action<bool> action)
    {
        StartCoroutine(DeleteGenerationSmart(gen_name, action));
    }
    IEnumerator DeleteGenerationSmart(string gen_name, Action<bool> action)
    {
        string query = "?login=" + current_login;
        UnityWebRequest webRequest = new(URI + "/Generation/" + gen_name + query, "DELETE");

        yield return webRequest.SendWebRequest();

        action(webRequest.responseCode == 200);
        webRequest.Dispose();
    }
    #endregion
    #region Обновить генерацию
    public class UpdateGenerationData
    {
        private string name;
        private string desctiption;
        public UpdateGenerationData(string name, string desctiption)
        {
            this.name = name;
            this.desctiption = desctiption;
        }
    }
    public void UpdateGeneration(UpdateGenerationData data, string gen_name, Action<bool> action)
    {
        StartCoroutine(UpdateGenerationSmart(data, gen_name, action));
    }
    IEnumerator UpdateGenerationSmart(UpdateGenerationData data, string gen_name, Action<bool> action)
    {
        string query = "?login=" + current_login;

        UnityWebRequest webRequest = new(URI + "/Generation/" + gen_name + query, "PUT");
        webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
        webRequest.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

        yield return webRequest.SendWebRequest();

        action(webRequest.responseCode == 200);
        webRequest.Dispose();
    }
    #endregion
    #region Создать генерацию
    public class CreateGenerationData
    {
        public string name;
        public string map;
        public string feed_type;
        public string setup_type;
        public string life_type;
        public string description;
        public string tick;
        public string setup_json;
        public CreateGenerationData(string name, string map, string feed_type, string setup_type, string life_type, string description, string tick, string setup_json)
        {
            this.name = name;
            this.map = map;
            this.feed_type = feed_type;
            this.setup_type = setup_type;
            this.life_type = life_type;
            this.description = description;
            this.tick = tick;
            this.setup_json = setup_json;
        }
    }
    public void CreateNewGeneration(CreateGenerationData data, Action<bool> action)
    {
        StartCoroutine(CreateNewGenerationSmart(data, action));
    }
    IEnumerator CreateNewGenerationSmart(CreateGenerationData data, Action<bool> action)
    {
        string str = URI + "/Generation" + $"?login={current_login}";
        UnityWebRequest webRequest = new(str, "PUT");

        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
        webRequest.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

        yield return webRequest.SendWebRequest();

        action(webRequest.responseCode == 200);
        webRequest.Dispose();
    }
    #endregion

    #region Получить общее время генерации
    public void GetGenerationTime(string gen_name, Action<GenerationTimeResponse> action)
    {
        StartCoroutine(GetGenerationTimeSmart(gen_name, action));
    }
    IEnumerator GetGenerationTimeSmart(string gen_name, Action<GenerationTimeResponse> action)
    {
        string query = "?login=" + current_login;

        string str = URI + "/Generation/" + gen_name + "/Time" + query;

        UnityWebRequest webRequest = new(str, "GET");
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        //action(JsonUtility.FromJson<GenerationTimeResponse>(webRequest.downloadHandler.text));
        action(new GenerationTimeResponse() { time = 0 });
        webRequest.Dispose();
    }
    public class GenerationTimeResponse
    {
        public float time;
    }
    #endregion
    #region Получить количество вымираний в генерации
    public void GetGenerationLifeEnds(string gen_name, Action<GenerationLifeEndsResponse> action)
    {
        StartCoroutine(GetGenerationLifeEndsSmart(gen_name, action));
    }
    IEnumerator GetGenerationLifeEndsSmart(string gen_name, Action<GenerationLifeEndsResponse> action)
    {
        string query = "?login=" + current_login;

        UnityWebRequest webRequest = new(URI + "/Generation/" + gen_name + "/LifeEnds" + query, "GET");
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        //action(JsonUtility.FromJson<GenerationLifeEndsResponse>(webRequest.downloadHandler.text));
        action(new GenerationLifeEndsResponse() { life_ends = 0 });
        webRequest.Dispose();
    }
    public class GenerationLifeEndsResponse
    {
        public int life_ends;
    }
    #endregion

    #region Получить все данные для создания генерации
    public void GetCreationVariants(Action<CreationsVariantsResponse> action)
    {
        StartCoroutine(GetCreationVariantsSmart(action));
    }
    IEnumerator GetCreationVariantsSmart(Action<CreationsVariantsResponse> action)
    {
        UnityWebRequest webRequest = new(URI + "/CreationVariants", "GET");
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        action(JsonUtility.FromJson<CreationsVariantsResponse>(webRequest.downloadHandler.text));
        webRequest.Dispose();
    }
    [Serializable]
    public class CreationsVariantsResponse
    {
        public List<string> map_names;
        public List<string> life_types;
        public List<string> feed_types;
        public List<float> ticks;
        public List<SetupTypeData> setup_types;
    }
    [Serializable]
    public class SetupTypeData
    {
        public string name;
        public string json;
    }
    #endregion

    #region Получить все клетки в конкретный тик
    public void GetCellsFromTick(string gen_name, long send_id, Action<CellsFromTickResponse> action)
    {
        StartCoroutine(GetCellsFromTickSmart(gen_name, send_id, action));
    }

    IEnumerator GetCellsFromTickSmart(string gen_name, long send_id, Action<CellsFromTickResponse> action)
    {
        string query = "?login=" + current_login;

        UnityWebRequest webRequest = new(URI + "/Generation/" + gen_name + "/Cells/" + send_id + query, "GET");
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        action(JsonUtility.FromJson<CellsFromTickResponse>(webRequest.downloadHandler.text));
        webRequest.Dispose();
    }

    public class CellsFromTickResponse
    {
        public List<CellData> cells;
    }

    #endregion
    #region Запомнить состояние клеток в генерации на тике
    [Serializable]
    public class SendTickChangesData
    {
        public List<CellData> added;
        public List<ModuleDataWithId> changes;
        public List<long> deleted;
        public SendTickChangesData(List<CellData> added, List<ModuleDataWithId> changes, List<long> deleted)
        {
            this.added = added;
            this.changes = changes;
            this.deleted = deleted;
        }
    }
    [Serializable]
    public class ModuleDataWithId
    {
        public long cell_id;
        public string name;
        public float? value;
        public ModuleDataWithId(long cell_id, string name, float? value)
        {
            this.cell_id = cell_id;
            this.name = name;
            this.value = value;
        }
    }

    public void SendTickChanges(string gen_name, long send_id, SendTickChangesData data, Action<bool> action)
    {
        StartCoroutine(SendTickChangesSmart(gen_name, send_id, data, action));
    }

    IEnumerator SendTickChangesSmart(string gen_name, long send_id, SendTickChangesData data, Action<bool> action)
    {
        string query = "?login=" + current_login;

        UnityWebRequest webRequest = new(URI + "/Generation/" + gen_name + "/Cells/" + send_id + query, "PATCH");
        webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

        yield return webRequest.SendWebRequest();

        action(webRequest.responseCode == 200);
        webRequest.Dispose();
    }
    #endregion

    #region Класс клетки
    [Serializable]
    public class CellData
    {
        public long parent_id;
        public long own_id;
        public List<ModuleData> modules;
        public IntellectData intellect;
        public CellData(long parent_id, long own_id, List<ModuleData> modules, IntellectData intellect)
        {
            this.parent_id = parent_id;
            this.own_id = own_id;
            this.modules = modules;
            this.intellect = intellect;
        }
    }
    [Serializable]
    public class ModuleData
    {
        public string name;
        public float? value; 
        public ModuleData(string name, float? value)
        {
            this.name = name;
            this.value = value;
        }
    }
    [Serializable]
    public class IntellectData
    {
        public int neurons_count;
        public int gens_count;
        public int input_neurons_count;
        public int output_neurons_count;
        public List<NeuronData> neurons;
        public List<SynapsData> gens;
        public IntellectData(int neurons_count, int gens_count, int input_neurons_count, int output_neurons_count, List<NeuronData> neurons, List<SynapsData> gens)
        {
            this.neurons_count = neurons_count;
            this.gens_count = gens_count;
            this.input_neurons_count = input_neurons_count;
            this.output_neurons_count = output_neurons_count;
            this.neurons = neurons;
            this.gens = gens;
        }
    }
    [Serializable]
    public class NeuronData
    {
        public float bias;
        public NeuronData(float bias)
        {
            this.bias = bias;
        }
    }
    [Serializable]
    public class SynapsData
    {
        public int el_neur_number;
        public int fin_neur_number;
        public float weight;
        public SynapsData(int el_neur_number, int fin_neur_number, float weight)
        {
            this.el_neur_number = el_neur_number;
            this.fin_neur_number = fin_neur_number;
            this.weight = weight;
        }
    }
    #endregion
}
