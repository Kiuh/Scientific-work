using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Massager : MonoBehaviour
{
    [SerializeField]
    float fade_speed = 0.01f;
    [SerializeField]
    float show_time = 30;
    [SerializeField]
    TMP_Text text;
    public void Awake()
    {
        text = gameObject.GetComponent<TMP_Text>();
        text.text = "";
        text.color = Color.red;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }
    public void ShowMassage(string massage)
    {
        text.text = massage;
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        for (float i = show_time; i > 0; i -= 0.1f)
        {
            yield return null;
        }
        for (float visible = 1f; visible > 0; visible -= fade_speed)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, visible);
            yield return null;
        }
    }
}
