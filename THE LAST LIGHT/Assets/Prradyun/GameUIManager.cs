using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameUIManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button startButton;
    public Button quitButton;

    [Header("Click Animation Settings")]
    public float clickScaleAmount = 0.9f;
    public float clickAnimDuration = 0.1f;

    private void Start()
    {
        startButton.onClick.AddListener(OnStartClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
    }

    private void OnStartClicked()
    {
        StartCoroutine(ClickAnim(startButton.transform, () =>
        {
            SceneManager.LoadScene("Main_Scene");
        }));
    }

    private void OnQuitClicked()
    {
        StartCoroutine(ClickAnim(quitButton.transform, () =>
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }));
    }

    private IEnumerator ClickAnim(Transform target, System.Action onComplete)
    {
        Vector3 original = target.localScale;
        Vector3 shrunk = original * clickScaleAmount;

        float t = 0f;
        while (t < clickAnimDuration)
        {
            t += Time.deltaTime;
            target.localScale = Vector3.Lerp(original, shrunk, t / clickAnimDuration);
            yield return null;
        }

        t = 0f;
        while (t < clickAnimDuration)
        {
            t += Time.deltaTime;
            target.localScale = Vector3.Lerp(shrunk, original, t / clickAnimDuration);
            yield return null;
        }

        target.localScale = original;
        onComplete?.Invoke();
    }
}
