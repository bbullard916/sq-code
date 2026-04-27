using System.Collections;
using TMPro;
using UnityEngine;
public class LevelUpNotification : MonoBehaviour
{
    public AnimationCurve animationCurve;
    public float fadingSpeed = 5f;
    public TextMeshProUGUI LevelText;

    public enum Direction { FadeIn, FadeOut };
    public void OnEnable()
    {
        GameEvents.CharacterLevelChanged += LevelChanged;
    }

    public void OnDisable()
    {
        GameEvents.CharacterLevelChanged -= LevelChanged;
    }

    public void LevelChanged(int amount)
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) Debug.LogError("Please assign a canvas group to the canvas!");

        if (animationCurve.length == 0)
        {
            Debug.Log("Animation curve not assigned: Create a default animation curve");
            animationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        }
        LevelText.text = "Level " + amount.ToString();
        StartCoroutine(FadeCanvas(canvasGroup, Direction.FadeIn, fadingSpeed));
        StartCoroutine(FadeCanvas(canvasGroup, Direction.FadeOut, fadingSpeed));
    }

    public IEnumerator FadeCanvas(CanvasGroup canvasGroup,  Direction direction, float duration)
    {
        // keep track of when the fading started, when it should finish, and how long it has been running
        var startTime = Time.time;
        var endTime = Time.time + duration;
        var elapsedTime = 0f;

        // set the canvas to the start alpha – this ensures that the canvas is ‘reset’ if you fade it multiple times
        if (direction == Direction.FadeIn) canvasGroup.alpha = animationCurve.Evaluate(0f);
        else canvasGroup.alpha = animationCurve.Evaluate(1f);

        // loop repeatedly until the previously calculated end time
        while (Time.time <= endTime)
        {
            elapsedTime = Time.time - startTime; // update the elapsed time
            var percentage = 1 / (duration / elapsedTime); // calculate how far along the timeline we are
            if ((direction == Direction.FadeOut)) // if we are fading out
            {
                canvasGroup.alpha = animationCurve.Evaluate(1f - percentage);
            }
            else // if we are fading in/up
            {
                canvasGroup.alpha = animationCurve.Evaluate(percentage);
            }

            yield return new WaitForEndOfFrame(); // wait for the next frame before continuing the loop
        }

        // force the alpha to the end alpha before finishing – this is here to mitigate any rounding errors, e.g. leaving the alpha at 0.01 instead of 0
        if (direction == Direction.FadeIn) canvasGroup.alpha = animationCurve.Evaluate(1f);
        else canvasGroup.alpha = animationCurve.Evaluate(0f);
    }
}
