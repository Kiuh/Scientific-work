using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServerSpeaker
{
    public static string login;
    #region Вход в аккаунт
    public class LoginData
    {
        public string login;
        public string password;
    }
    public static bool Login(LoginData login_data, ref string error)
    {
        login = login_data.login;
        return true;
    }
    #endregion
    #region Регестрация
    public class RegistrationData
    {
        public string login;
        public string email;
        public string password;
    }
    public static bool Registration(RegistrationData registration_data, ref string error)
    {
        return true;
    }
    #endregion
    #region Получить все имена и правила жизни генераций 
    public class GenerationData1
    {
        public string name;
        public string life_type;
    }
    public static List<GenerationData1> GetGenerations_Names_Types()
    {
        return new List<GenerationData1>() { new GenerationData1() { name = "Generation 1", life_type = "DEATH" } };
    }
    #endregion
    #region Получить Карту, Частоту и Комментарии одной генерации
    public class GenerationData2
    {
        public string map_name;
        public string tick;
        public string comment;
    }
    public static GenerationData2 GetGeneration_Map_Tick_Comments(string gen_name)
    {
        return new GenerationData2() { map_name = "Map_1", tick = "0.1", comment = "Some comments to discribe generstion." };
    }
    #endregion
    #region Получить общее время генерации
    public static GenerationTime GetGeneration_Time(string gen_name)
    {
        return new GenerationTime() { time = 0.1f };
    }
    public class GenerationTime
    {
        public float time;
    }
    #endregion
    #region Получить количество вымираний в генерации
    public static GenerationLifeEnds GetGeneration_LifeEnds(string gen_name)
    {
        return new GenerationLifeEnds() { LifeEnds = 2 }; ;
    }
    public class GenerationLifeEnds
    {
        public int LifeEnds;
    }
    #endregion
    #region Удалить генерацию
    public static void DeleteGeneration(string gen_name)
    {

    }
    #endregion
    #region Получить имена всех карт
    public static List<string> GetMapsNames()
    {
        return new List<string>() { "Map_1" };
    }
    #endregion
    #region Получить все правила жизней
    public static List<string> GetLifeRulesNames()
    {
        return new List<string>() { "StandartLifeRule" };
    }
    #endregion
    #region Получить все правила кормления
    public static List<string> GetFeedingRulesNames()
    {
        return new List<string>() { "StandartFeedingRule" };
    }
    #endregion
    #region Получить все частоты
    public static List<float> GetTicksNames()
    {
        return new List<float>() { 0.1f };
    }
    #endregion
    #region Получить все Правила первого разведения и их описания
    public class StartGenerationRule
    {
        public string name;
        public string json;
    }
    public static List<StartGenerationRule> GetGenerationSetupsAndJsons()
    {
        return new List<StartGenerationRule>() { 
            new StartGenerationRule() { 
                name = "Randon_Generation_Input",
                json = ""
            }
        };
    }
    #endregion
    #region Создать генерацию
    public class CreateGenerationData
    {
        public string name;
        public string map;
        public string life_rule;
        public string feeding_rule;
        public string start_options;
        public string comments;
        public string tick;
        public string start_options_json;
    }
    public static bool CreateNewGeneration(CreateGenerationData create_generation_data)
    {
        return true;
    }
    #endregion
    #region Получить описание генерации
    public static GenerationDescription GetGenerationDescr(string gen_name)
    {
        return new GenerationDescription() { description = "Test generation" };
    }
    public class GenerationDescription
    {
        public string description;
    }
    #endregion
    #region Задать новое имя и описание генерации
    public class UpdateGenerationData
    {
        private string old_name;
        private string new_name;
        private string desctiption;
    }
    public static bool SetNewGenerationNameAndDescr(UpdateGenerationData update_generation_data)
    {
        return true;
    }
    #endregion
    #region Получить информацию о генерации
    public class GenerationData3
    {
        public string map;
        public string life_type;
        public string feed_type;
        public string setup_type;
        public float tick;
        public long last_send_num;
        public string setup_json;
        public long last_cell_num;
    }
    public static GenerationData3 GetGenerationInfoForStart(string gen_name)
    {
        return new GenerationData3() { map = "StandartMap", life_type = "RepeatStart", feed_type = "Standart", last_send_num = 0, setup_type = "", tick = 0.1f, setup_json = "" };
    }
    #endregion
}
