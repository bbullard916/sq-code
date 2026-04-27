using UnityEngine;
using TMPro;

public class WorldInteractableHoverText : MonoBehaviour
{
    Vector2 hotSpot = new Vector2(0, 0);
    Ray ray;
    RaycastHit hit;
    public Transform UI;
    public float yAdjustment = 0f;
    public Vector3 UIsize;
    public GameObject WorldInteractTextObj;
    GameObject go = null;
    public string Text;

    void LateUpdate()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "ToolTipText")
            {
                GameObject Player = GameObject.FindGameObjectWithTag("Player");
                Vector3 delta = hit.transform.position - Player.transform.position;
                if (delta.magnitude <= 3 && go == null && hit.transform.name == this.gameObject.name)
                {
                    go = Instantiate(WorldInteractTextObj) as GameObject;
                    go.transform.SetParent(UI, false);
                    go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y+yAdjustment, go.transform.position.z);
                    go.GetComponentInChildren<TextMeshProUGUI>().text = Text;
                }
            }
            else
            {
                DestroyImmediate(go);
            }
        }
    }
}
