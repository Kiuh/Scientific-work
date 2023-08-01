using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private float fadeSpeed = 0.02f;

    [SerializeField]
    private GameObject panel;

    private Canvas canvas;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadScene(string scene_name)
    {
        _ = StartCoroutine(SceneEnd(scene_name));
    }

    private IEnumerator SceneEnd(string scene_name)
    {
        Image image = Instantiate(panel, canvas.transform).GetComponent<Image>();
        for (float visible = 0f; visible < 1; visible += fadeSpeed)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, visible);
            yield return null;
        }
        SceneManager.LoadScene(scene_name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        canvas = FindObjectOfType<Canvas>();
        _ = StartCoroutine(SceneStart());
    }

    private IEnumerator SceneStart()
    {
        canvas = FindObjectOfType<Canvas>();
        Image image = Instantiate(panel, canvas.transform).GetComponent<Image>();
        for (float visible = 1f; visible > 0; visible -= fadeSpeed)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, visible);
            yield return null;
        }
        Destroy(image.gameObject);
    }
}
