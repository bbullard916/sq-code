using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;

    private CanvasGroup canvasGroup;
    private Coroutine currentFade;
    // ScreenFader.Instance.FadeToBlack();
    // ScreenFader.Instance.FadeFromBlack();
    void Awake()
    {
        // Singleton enforcement
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    // ---------- PUBLIC API ----------

    public void FadeToBlack()
    {
        StartFade(1f);
    }

    public void FadeFromBlack()
    {
        StartFade(0f);
    }

    public void Fade(float targetAlpha)
    {
        StartFade(targetAlpha);
    }

    public IEnumerator FadeOutRoutine()
    {
        yield return FadeRoutine(1f);
    }

    public IEnumerator FadeInRoutine()
    {
        yield return FadeRoutine(0f);
    }

    // ---------- INTERNAL ----------

    private void StartFade(float target)
    {
        if (currentFade != null)
            StopCoroutine(currentFade);

        currentFade = StartCoroutine(FadeRoutine(target));
    }

    private IEnumerator FadeRoutine(float target)
    {
        canvasGroup.blocksRaycasts = true;

        float start = canvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(start, target, time / fadeDuration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.alpha = target;
        canvasGroup.blocksRaycasts = target > 0f;
        currentFade = null;
    }
}
