using System.Collections;
using TMPro;
using UnityEngine;

namespace General
{
    public class LoadingPause : MonoBehaviour
    {
        [SerializeField]
        private GameObject loadingPausePrefab;
        [SerializeField]
        private float delaySeconds;
        private TMP_Text loadingText;
        private int dotsCount = 0;

        public static LoadingPause Instance { get; private set; } = null;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void ShowLoading(string text)
        {
            GameObject instance = Canvas.Instantiate(loadingPausePrefab);
            loadingText = instance.GetComponentInChildren<TMP_Text>();
            _ = loadingText.StartCoroutine(LoadingRoutine(text, loadingText));
        }

        public void HideLoading()
        {
            Destroy(loadingText.transform.parent.gameObject);
            dotsCount = 0;
        }

        private IEnumerator LoadingRoutine(string text, MonoBehaviour initiator)
        {
            dotsCount++;
            dotsCount %= 3;
            loadingText.text = text + new string('.', dotsCount);
            yield return new WaitForSeconds(delaySeconds);
            _ = initiator.StartCoroutine(LoadingRoutine(text, initiator));
        }
    }
}
