using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CustomAlertOverlay : MonoBehaviour
{
    private CanvasGroup UICanvas;
    public Sprite LevelUpImage;
    private CanvasGroup _canvasGroup;
    public Sprite[] MiscIcons;
    public TextMeshProUGUI Text;
    public Image Icon;
    private static CustomAlertOverlay _instance;
    [SerializeField] private float _duration = 0.3f;

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public static CustomAlertOverlay Instance
    {
        get { return _instance; }
    }
    public void Start()
    {
        UICanvas = GetComponent<CanvasGroup>();
    }
    private IEnumerator FadeCGAlpha(float from, float to, float duration)
    {
        float elaspedTime = 0f;
        while (elaspedTime <= duration)
        {
            elaspedTime += Time.deltaTime;
            UICanvas.alpha = Mathf.Lerp(from, to, elaspedTime / duration);
            yield return null;
        }
        UICanvas.alpha = to;
        if(to >= 1)
          StartCoroutine(Wait());
    }
    private void OnEnable()
    {
        GameEvents.CharacterLevelChanged += LevelUp;
        GameEvents.PlayerLearnedAbility += LearnedAbility;
    }

    private void OnDisable()
    {
        GameEvents.CharacterLevelChanged -= LevelUp;
        GameEvents.PlayerLearnedAbility  -= LearnedAbility;
    }

    private void LevelUp(int lvl)
    {
        Debug.Log("Level UP " + lvl);
        Icon.sprite = LevelUpImage;
        Text.text = "You attained level " + lvl;
        SoundManager.Instance.PlayLevelUpStinger();
        StartCoroutine(FadeCGAlpha(0f, 1f, _duration));
    }

    private void LearnedAbility(RPGAbility Ability)
    {
        Debug.Log("Learned Ability" + Ability.entryIcon);
        Text.text = "You learned a new ability " + Ability.entryName;
        Icon.sprite = Ability.entryIcon;
        StartCoroutine(FadeCGAlpha(0f, 1f, _duration));
        SoundManager.Instance.PlayAbilityStinger();
    }

    public void SendCustomMessage(string message, double IconID)
    {
        Text.text = message;
        Icon.sprite = MiscIcons[Mathf.RoundToInt((float)IconID)];
        StartCoroutine(FadeCGAlpha(0f, 1f, 1f));
        SoundManager.Instance.PlayAbilityStinger();
    }

    private IEnumerator Wait()
    {

        StartCoroutine(FadeCGAlpha(1f, 0f, _duration));
        yield return new WaitForSeconds(1);
    }
}
