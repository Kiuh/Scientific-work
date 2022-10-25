using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerationInfo : MonoBehaviour
{
    [SerializeField]
    TMP_Text gen_name;
    [SerializeField]
    TMP_Text gen_map;
    [SerializeField]
    TMP_Text gen_type;
    [SerializeField]
    TMP_Text gen_ticks;
    [SerializeField]
    TMP_Text gen_comments;
    public void InitInfo(string name, string map, string type, string ticks, string comments)
    {
        gameObject.SetActive(true);
        gen_name.text = name;
        gen_map.text = map;
        gen_type.text = type;
        gen_ticks.text = ticks;
        gen_comments.text = comments;
    }
}
