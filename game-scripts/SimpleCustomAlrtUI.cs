using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SimpleCustomAlrtUI : MonoBehaviour
{
    private CanvasGroup UICanvas;
    private CanvasGroup _canvasGroup;
    public TextMeshProUGUI Text;
    private static SimpleCustomAlrtUI _instance;
    [SerializeField] private float _duration = 1f;

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
    public static SimpleCustomAlrtUI Instance
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
        if(to == 1)
          StartCoroutine(Wait());
    }

    public void SendCustomMessage(string message, string color, Vector3 position = default(Vector3))
    {
        Color currentColor = Color.blue;
        if (position != default(Vector3))
          Text.gameObject.transform.position = position;
        
       
        if(color == "green")
        {
            Text.color = Color.green;
        }
        else if(color == "red")
        {
            Text.color = Color.red;
        }
        else if (color == "blue")
        {
            Text.color = Color.blue;
        }
        Text.text = message;
        StartCoroutine(FadeCGAlpha(0f, 1f, _duration));
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(FadeCGAlpha(1f, 0f, _duration));
    }
}
