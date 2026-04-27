using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.HighDefinition;
public class SetPlayerCustomization : MonoBehaviour
{
    public GameObject[] Hair;
    public GameObject[] FacialHair;
    public GameObject[] Skin;
    public GameObject Eyebrow;
    private bool Debugging = false;
    public string CurrentHairChoice = "Hair0";
    public string CurrentFaceHairChoice = "Hair0";
    public string CurrentSkinColorChoice = "Skin0";
    public Color HairColor = Color.yellow;
    public Color SkinColor = Color.white;


    void Start()
    {
        DisableHair();
        DisableFacialHair();
        string sceneName = SceneManager.GetActiveScene().name;
        if(sceneName.Contains("Main"))
        {
            string savedHairColorChoice = CustomDataProvider.Instance.LoadStringData("HairColor");
            if (!string.IsNullOrEmpty(savedHairColorChoice))
            {
                SetHairColor(HexToColor(savedHairColorChoice));
            }
        }
        else
        {
            string savedHairChoice = CustomDataProvider.Instance.LoadStringData("Hair");
            if (!string.IsNullOrEmpty(savedHairChoice))
            {
                SetHair(savedHairChoice);
            }
            string savedFacialHairChoice = CustomDataProvider.Instance.LoadStringData("FacialHair");
            if (!string.IsNullOrEmpty(savedHairChoice))
            {
                SetFaceHair(savedFacialHairChoice);
            }
            string savedHairColorChoice = CustomDataProvider.Instance.LoadStringData("HairColor");
            if (!string.IsNullOrEmpty(savedHairColorChoice))
            {
                SetHairColor(HexToColor(savedHairColorChoice));
            }
            string savedSkinColorChoice = CustomDataProvider.Instance.LoadStringData("SkinColor");
            if (!string.IsNullOrEmpty(savedSkinColorChoice))
            {
                SetSkinColor(HexToColor(savedSkinColorChoice));
            }
        }
    }

    private void DisableHair()
    {
        foreach (var hair in Hair)
        {
            if (hair != null)
                hair.SetActive(false);
        }
    }
    private void DisableFacialHair()
    {
        foreach (var hair in FacialHair)
        {
            if (hair != null)
                hair.SetActive(false);
        }
    }

    public void SetHair(string hairChoice)
    {
        if (Debugging)
            Debug.Log("Setting Hair to " + hairChoice);

        DisableHair();

        int hairIndex = ParseHairChoice(hairChoice);
        if (hairIndex >= 0 && hairIndex < Hair.Length && Hair[hairIndex] != null)
        {
            Hair[hairIndex].SetActive(true);
            CurrentHairChoice = hairChoice;
            //SetHairColor(HexToColor(HairColor));
        }
        else
        {
            Debug.LogError("Invalid hair choice: " + hairChoice);
        }
    }

    public void SetFaceHair(string hairChoice)
    {
        if (Debugging)
            Debug.Log("Setting Hair to " + hairChoice);

        DisableFacialHair();

        int hairIndex = ParseFaceHairChoice(hairChoice);
        if (hairIndex >= 0 && hairIndex < FacialHair.Length && FacialHair[hairIndex] != null)
        {
            FacialHair[hairIndex].SetActive(true);
            CurrentFaceHairChoice = hairChoice;
        }
        else
        {
            Debug.LogError("Invalid hair choice: " + hairChoice);
        }
    }


    public void SetHairColor(Color color)
    {
        int hairIndex = ParseHairChoice(CurrentHairChoice);
        if (hairIndex >= 0 && hairIndex < Hair.Length && Hair[hairIndex] != null)
        {
            Renderer[] renderers = Hair[hairIndex].GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                if (renderer != null)
                {
                    foreach (Material material in renderer.materials)
                    {
                        material.SetColor("_BaseColor", color);
                    }
                }
                else
                {
                    Debug.LogError("Renderer component not found!");
                }
            }
            Renderer[] renderers2 = Eyebrow.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers2)
            {
                if (renderer != null)
                {
                    foreach (Material material in renderer.materials)
                    {
                        material.SetColor("_BaseColor", color);
                    }
                }
                else
                {
                    Debug.LogError("Renderer component not found!");
                }
            }
        }
        else
        {
            Debug.LogError("Invalid current hair choice: " + CurrentHairChoice);
        }
        int hairIndex2 = ParseFaceHairChoice(CurrentFaceHairChoice);
        if (hairIndex2 >= 0 && hairIndex2 < FacialHair.Length && FacialHair[hairIndex2] != null)
        {
            Renderer[] renderers = FacialHair[hairIndex2].GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                if (renderer != null)
                {
                    foreach (Material material in renderer.materials)
                    {
                        material.SetColor("_BaseColor", color);
                    }
                }
                else
                {
                    Debug.LogError("Renderer component not found!");
                }
            }
        }
        else
        {
            Debug.LogError("Invalid current hair choice: " + CurrentHairChoice);
        }
    }

    public void SetSkinColor(Color color)
    {
        int skinIndex = ParseSkinChoice(CurrentSkinColorChoice);
        Renderer[] renderers = Skin[skinIndex].GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                if (renderer != null)
                {
                    foreach (Material material in renderer.materials)
                    {
                        material.SetColor("_BaseColor", color);
                    }
                }
            }

    }
    private int ParseHairChoice(string hairChoice)
    {
        if (hairChoice.StartsWith("Hair") && int.TryParse(hairChoice.Substring(4), out int index))
        {
            return index;
        }
        return -1; // Invalid choice
    }
    private int ParseFaceHairChoice(string hairChoice)
    {
        if (hairChoice.StartsWith("FaceHair") && int.TryParse(hairChoice.Substring(8), out int index))
        {
            return index;
        }
        return -1; // Invalid choice
    }
    private int ParseSkinChoice(string skinChoice)
    {
        if (skinChoice.StartsWith("Skin") && int.TryParse(skinChoice.Substring(4), out int index))
        {
            return index;
        }
        return -1; // Invalid choice
    }
    public void HelmetEquipped()
    {
        DisableFacialHair();
        DisableHair();
    }
    public void HelmetUnEquipped()
    {
        string savedHairChoice = CustomDataProvider.Instance.LoadStringData("Hair");
        if (!string.IsNullOrEmpty(savedHairChoice))
        {
            SetHair(savedHairChoice);
        }
        string savedFacialHairChoice = CustomDataProvider.Instance.LoadStringData("FacialHair");
        if (!string.IsNullOrEmpty(savedHairChoice))
        {
            SetFaceHair(savedFacialHairChoice);
        }
    }
    public static Color HexToColor(string hex)
    {

        if (ColorUtility.TryParseHtmlString("#" + hex, out Color color))
        {
            return color;
        }
        else
        {
            Debug.LogError("Invalid hex color: " + hex);
            return Color.white; // Default fallback color
        }
    }
}
