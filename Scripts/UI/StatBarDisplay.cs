using BLINK.RPGBuilder.Combat;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BLINK.RPGBuilder.DisplayHandler
{
    public class StatBarDisplay : MonoBehaviour
    {
        private Coroutine flashCoroutine;
        [SerializeField] private Image fillBar;
        [SerializeField] public Image flashBar;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private RPGStat stat;

        private void OnEnable()
        {
            CombatEvents.StatValueChanged += UpdateBar;
        }
        
        private void OnDisable()
        {
            CombatEvents.StatValueChanged -= UpdateBar;
        }

        private float previousFillAmount = -1f; // Initialized to -1 to ensure first update logs nothing

        protected virtual void UpdateBar(CombatEntity combatEntity, RPGStat statChanged, float currentValue, float maxValue)
        {
            if (combatEntity != GameState.playerEntity || stat != statChanged) return;

            float newFill = currentValue / maxValue;

            if (fillBar != null)
            {
                if (previousFillAmount >= 0f && newFill < previousFillAmount)
                {
                    //Debug.Log($"fillAmount decreased: {previousFillAmount} -> {newFill}");
                    HandleHighlight();
                }

                fillBar.fillAmount = newFill;
                previousFillAmount = newFill;
            }

            if (amountText != null)
            {
                amountText.text = $"{(int)currentValue} / {(int)maxValue}";
            }
        }

        public void HandleHighlight()
        {
            if (flashCoroutine != null) StopCoroutine(flashCoroutine);
            flashCoroutine = StartCoroutine(FadeFlashBar());
        }

        private IEnumerator FadeFlashBar()
        {
            float duration = 0.5f; // fade duration
            float elapsed = 0f;

            Color originalColor = flashBar.color;
            originalColor.a = 1f;
            flashBar.color = originalColor;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                flashBar.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }

            // Ensure it's fully transparent at the end
            flashBar.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        }

    }
}
