using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    float fade_speed = 0.02f;
    [SerializeField]
    GameObject panel;
    [SerializeField]
    Canvas canvas;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadScene(string scene_name)
    {
        StartCoroutine(SceneEnd(scene_name));
    }

    IEnumerator SceneEnd(string scene_name)
    {
        Image image = Instantiate(panel, canvas.transform).GetComponent<Image>();
        for (float visible = 0f; visible < 1; visible += fade_speed)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, visible);
            yield return null;
        }
        SceneManager.LoadScene(scene_name);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SceneStart());
    }

    IEnumerator SceneStart()
    {
        canvas = FindObjectOfType<Canvas>();
        Image image = Instantiate(panel, canvas.transform).GetComponent<Image>();
        for (float visible = 1f; visible > 0; visible -= fade_speed)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, visible);
            yield return null;
        }
        Destroy(image.gameObject);
    }
}
