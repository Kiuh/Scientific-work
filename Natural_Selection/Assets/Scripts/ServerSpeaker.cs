using System.Collections.Generic;
using System.Net.Http;

public static class ServerSpeaker
{
    static string URI = "";
    static string current_login = "";
    static HttpClient client = new HttpClient();
    
    #region Вход в аккаунт
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
    public static bool Login(LoginData data)
    {
        //current_login = data.login;
        //var res = client.PatchAsync(URI + "/User", new StringContent(JsonUtility.ToJson(data))).Result;
        //if (res.IsSuccessStatusCode) 
        //    return true;
        //else
        //    return false;
        return true;
    }
    #endregion
    #region Регестрация
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
    public static bool Registration(RegistrationData data)
    {
        //var res = client.PutAsync(URI + "/User", new StringContent(JsonUtility.ToJson(data))).Result;
        //if (res.IsSuccessStatusCode)
        //    return true;
        //else
        //    return false;
        return true;
    }
    #endregion
    
    #region Получить все генерации (с информацией)
    public static GenerationsResponse GetGenerations()
    {
        //string query = "/?login=" + current_login;
        //var res = client.GetAsync(URI + "/Generations" + query).Result;
        //return JsonUtility.FromJson<GenerationsResponse>(res.Content.ReadAsStringAsync().Result);
        return new GenerationsResponse()
        {
            generations = new List<GenerationData>() 
            {
                new GenerationData() 
                {
                    name = "Generation1",
                    map = "StandartMap",
                    life_type = "RepeatSetupLifeType",
                    feed_type = "StandartFeeding",
                    setup_type = "Random_Generation",
                    tick = 0.1f,
                    last_send_num = 0,
                    setup_json = "{\"start_cells_count\":40,\"description\":\"Some description\"}",
                    last_cell_num = 0,
                    description = "Test description"
                } 
            }
        };
    }
    public class GenerationsResponse
    {
        public List<GenerationData> generations;
    }
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
    public static bool DeleteGeneration(string gen_name)
    {
        //string query = "/?login=" + current_login;
        //var res = client.GetAsync(URI + "/Generations/" + gen_name + query).Result;
        //if (res.IsSuccessStatusCode)
        //    return true;
        //else
        //    return false;
        return true;
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
    public static bool UpdateGeneration(UpdateGenerationData data, string gen_name)
    {
        //string query = "/?login=" + current_login;
        //var res = client.PutAsync(URI + "/Generation/" + gen_name + query, new StringContent(JsonUtility.ToJson(data))).Result;
        //if (res.IsSuccessStatusCode)
        //    return true;
        //else
        //    return false;
        return true;
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
    public static bool CreateNewGeneration(CreateGenerationData data)
    {
        //string query = "/?login=" + current_login;
        //var res = client.PutAsync(URI + "/Generation" + query, new StringContent(JsonUtility.ToJson(data))).Result;
        //if (res.IsSuccessStatusCode)
        //    return true;
        //else
        //    return false;
        return true;
    }
    #endregion

    #region Получить общее время генерации
    public static GenerationTimeResponse GetGenerationTime(string gen_name)
    {
        //string query = "/?login=" + current_login;
        //var res = client.GetAsync(URI + "/Generations/" + gen_name + "/Time" + query).Result;
        //return JsonUtility.FromJson<GenerationTimeResponse>(res.Content.ReadAsStringAsync().Result);
        return new GenerationTimeResponse()
        {
            time = 0
        };
    }
    public class GenerationTimeResponse
    {
        public float time;
    }
    #endregion
    #region Получить количество вымираний в генерации
    public static GenerationLifeEndsResponse GetGenerationLifeEnds(string gen_name)
    {
        //string query = "/?login=" + current_login;
        //var res = client.GetAsync(URI + "/Generations/" + gen_name + "/LifeEnds" + query).Result;
        //return JsonUtility.FromJson<GenerationLifeEndsResponse>(res.Content.ReadAsStringAsync().Result);
        return new GenerationLifeEndsResponse()
        {
            life_ends = 0
        };
    }
    public class GenerationLifeEndsResponse
    {
        public int life_ends;
    }
    #endregion

    #region Получить все данные для создания генерации
    public static CreationsVariantsResponse GetCreationVariants()
    {
        //string query = "/?login=" + current_login;
        //var res = client.GetAsync(URI + "/CreationVariants" + query).Result;
        //return JsonUtility.FromJson<CreationsVariantsResponse>(res.Content.ReadAsStringAsync().Result);
        return new CreationsVariantsResponse()
        {
            map_names = new List<string>() { "StandartMap" },
            life_types = new List<string>() { "RepeatSetupLifeType" },
            feed_types = new List<string>() { "StandartFeeding" },
            ticks = new List<float>() { 0.1f , 0.5f },
            setup_types = new List<SetupTypeData>()
            {
                new SetupTypeData()
                {
                    name = "Random_Generation",
                    json = "{\"start_cells_count\":40,\"description\":\"Some description\"}"
                }
            }
        };
    }

    public class CreationsVariantsResponse
    {
        public List<string> map_names;
        public List<string> life_types;
        public List<string> feed_types;
        public List<float> ticks;
        public List<SetupTypeData> setup_types;
    }

    public class SetupTypeData
    {
        public string name;
        public string json;
    }
    #endregion
}
