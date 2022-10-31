using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CreateAndGoRGeneration : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    TMP_Text gen_name_input;
    [SerializeField]
    TMP_Dropdown maps_dd;
    [SerializeField]
    TMP_Dropdown life_rule_dd;
    [SerializeField]
    TMP_Dropdown feeding_rule_dd;
    [SerializeField]
    TMP_Dropdown ticks_dd;
    [SerializeField]
    TMP_Dropdown start_options_dd;
    [SerializeField]
    TMP_Text gen_comments_input;

    [SerializeField]
    SetupRGenStart RGS;

    [SerializeField]
    TMP_Text error;

    ServerSpeaker ss;

    public void Start()
    {
        ss = FindObjectOfType<ServerSpeaker>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ServerSpeaker.CreateGenerationData data = new(
            gen_name_input.text,
            maps_dd.options[maps_dd.value].text,
            feeding_rule_dd.options[maps_dd.value].text,
            start_options_dd.options[maps_dd.value].text,
            life_rule_dd.options[maps_dd.value].text,
            gen_comments_input.text,
            ticks_dd.options[maps_dd.value].text,
            RGS.current_show.GetComponent<ISetup>().GetNewInformation()
        );
        ss.CreateNewGeneration(data, OnPointerClick_Continue);
    }

    public void OnPointerClick_Continue(bool value)
    {
        if (value)
        {
            SceneManager.LoadScene("Generations");
        }
        else
        {
            error.text = "Fail to create generation";
        }
    }
}
