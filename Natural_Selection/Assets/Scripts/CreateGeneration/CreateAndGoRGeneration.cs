using System.Collections;
using System.Collections.Generic;
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
    public void OnPointerClick(PointerEventData eventData)
    {
        ServerSpeaker.CreateGenerationData data = new()
        {
            name = gen_name_input.text,
            map = maps_dd.itemText.text,
            life_rule = life_rule_dd.itemText.text,
            feeding_rule = feeding_rule_dd.itemText.text,
            start_options = start_options_dd.itemText.text,
            comments = gen_comments_input.text,
            tick = ticks_dd.itemText.text,
            start_options_json = RGS.current_show.GetComponent<ISetup>().GetNewInformation()
        };
        if (ServerSpeaker.CreateNewGeneration(data))
        {
            SceneManager.LoadScene("Generations");
        }
        else
        {

        }
    }
}
