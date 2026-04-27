using System;
using System.Linq;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutlineListener : MonoBehaviour
{
    [Header("Behavior")]
    [Tooltip("Recreate on every scene load (level load).")]
    public bool runOnSceneLoaded = true;

    [Tooltip("Seconds to wait between destroy and add (set to 0 for immediate).")]
    public float waitSeconds = 2f;

    [Header("What to add after removing Outliner")]
    [Tooltip("If true, remove Outliner then re-add Outliner.")]
    public bool recreateOutliner = true;

    [Tooltip("If recreateOutliner is false, remove Outliner and add this type instead (e.g. 'UnityEngine.UI.Outline' or 'MyNamespace.MyOutline').")]
    public string addTypeName = "UnityEngine.UI.Outline";

    void OnEnable()
    {
        if (runOnSceneLoaded)
            SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        // Run once for the first scene too (optional)
        // StartCoroutine(DestroyAndReAdd());
    }

    void OnDisable()
    {
        if (runOnSceneLoaded)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DestroyAndReAdd());
    }

    private IEnumerator DestroyAndReAdd()
    {
        bool removedAny = RemoveOutliner();

        if (waitSeconds > 0f)
            yield return new WaitForSeconds(waitSeconds);

        if (recreateOutliner)
        {
            // Remove then re-add Outliner
            var outlinerType = FindTypeByName("Outliner");
            if (outlinerType != null)
            {
                if (GetComponent(outlinerType) == null)
                    gameObject.AddComponent(outlinerType);
            }
            else if (removedAny)
            {
                Debug.LogWarning("[OutlineListener] 'Outliner' type not found to re-add.");
            }
        }
        else
        {
            // Remove Outliner, add something else
            AddByTypeName(addTypeName);
        }
    }

    private bool RemoveOutliner()
    {
        bool removed = false;
        var toRemove = GetComponents<Component>()
            .Where(c => c != null && string.Equals(c.GetType().Name, "Outliner", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        foreach (var c in toRemove)
        {
            Destroy(c);
            removed = true;
        }
        return removed;
    }

    private void AddByTypeName(string typeName)
    {
        if (string.IsNullOrWhiteSpace(typeName)) return;

        var t = FindTypeByName(typeName);
        if (t == null)
        {
            Debug.LogWarning($"[OutlineListener] Could not find type '{typeName}'. Check the namespace/assembly.");
            return;
        }

        if (GetComponent(t) == null)
            gameObject.AddComponent(t);
    }

    private static Type FindTypeByName(string typeName)
    {
        // Try fully-qualified name first
        var t = Type.GetType(typeName, throwOnError: false);
        if (t != null) return t;

        // Search all loaded assemblies
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                t = asm.GetTypes().FirstOrDefault(x => x != null && (x.FullName == typeName || x.Name == typeName));
                if (t != null) return t;
            }
            catch (ReflectionTypeLoadException e)
            {
                if (e?.Types != null)
                {
                    foreach (var tt in e.Types)
                    {
                        if (tt != null && (tt.FullName == typeName || tt.Name == typeName))
                            return tt;
                    }
                }
            }
        }
        return null;
    }
}
