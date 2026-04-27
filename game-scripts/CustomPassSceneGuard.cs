using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.HighDefinition;

public class CustomPassSceneGuard : MonoBehaviour
{
    [Tooltip("How long to keep passes disabled during scene switches.")]
    public float disableSeconds = 0.2f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    void OnActiveSceneChanged(Scene from, Scene to)
    {
        StartCoroutine(DisableTemporarily());
    }

    IEnumerator DisableTemporarily()
    {
        // Disable everything that currently exists
        var volumes = FindObjectsOfType<CustomPassVolume>(includeInactive: true);
        foreach (var v in volumes)
        {
            if (v != null) v.enabled = false;
        }

        // Wait a couple of frames + extra time for new scene setup
        yield return null;
        yield return null;
        yield return new WaitForSeconds(disableSeconds);

        // Re-enable everything that exists *now* (including new scene volumes)
        volumes = FindObjectsOfType<CustomPassVolume>(includeInactive: true);
        foreach (var v in volumes)
        {
            if (v != null) v.enabled = true;
        }
    }
}
