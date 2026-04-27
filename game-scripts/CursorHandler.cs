using UnityEngine;
using EPOOutline;
using System.Collections;
using BLINK.RPGBuilder.Characters;
using BLINK.RPGBuilder.Managers;
public class CursorHandler : MonoBehaviour
{
    public Outlinable outline;
    public Texture2D defaultMouseCursor;
    public Texture2D GroundMouseCursor;
    public Texture2D attackMouseCursor;
    public Texture2D attackMouseCursorFlash;
    public Texture2D talkMouseCursor;
    public Texture2D lootMouseCursor;
    public Texture2D useMouseCursor;
    public Texture2D MapTransition;
    public Texture2D Merchant;
    public Texture2D BlackSmith;
    public Texture2D Alchemy;
    public Texture2D Mining;
    public Texture2D Projectile;
    public LayerMask ignoreLayer;
    public GameObject Player;
    Vector2 hotSpot = new Vector2(0, 0);
    CursorMode cursorMode = CursorMode.Auto;
    Ray ray;
    RaycastHit hit;
    GameObject CurrentHighlighted;
    Outlinable currentOutlinable;

    private void Start()
    {
        SetCursor("default");
        Cursor.visible = true;
        ignoreLayer = 16;
    }


    void LateUpdate()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 5000))
        {
            if (hit.transform != null)
            {
                Player = GameObject.FindGameObjectWithTag("Player");
                Vector3 delta = hit.transform.position - Player.transform.position;
                if (hit.collider.tag.Contains("loot"))
                {
                    if (delta.magnitude <= 3)
                    {
                        SetCursor("loot");
                        currentOutlinable = hit.collider.GetComponentInChildren<Outlinable>();
                        currentOutlinable.DrawingMode = OutlinableDrawingMode.Normal;
                    }
                }
                else if (hit.collider.tag.Contains("loot"))
                {
                    if (delta.magnitude <= 3)
                    {
                        SetCursor("loot");
                        currentOutlinable = hit.collider.GetComponent<Outlinable>();
                        currentOutlinable.DrawingMode = OutlinableDrawingMode.Normal;
                    }
                }
                else if (hit.collider.tag.Contains("door"))
                {
                    if (delta.magnitude <= 3)
                    {
                        SetCursor("use");
                    }
                }
                else if (hit.collider.tag.Contains("map"))
                {
                    if (delta.magnitude <= 4)
                    {
                        SetCursor("map_transition");
                    }
                }
                else if (hit.collider.tag.Contains("BlackSmith"))
                {
                    if (delta.magnitude <= 3)
                    {
                        SetCursor("black_smith");
                    }
                }
                else if (hit.collider.tag.Contains("Alchemy"))
                {
                    if (delta.magnitude <= 3)
                    {
                        SetCursor("alchemy");
                    }
                }
                else if (hit.collider.tag.Contains("enemy"))
                {
                    if (Vector3.Distance(hit.transform.position, Player.transform.position) < 3)
                    {
                        if (Input.GetMouseButton(0))
                        {
                            SetCursor("attackglow");
                        }
                        else
                        {
                            SetCursor("attack");
                        }
                    }
                    else if (Vector3.Distance(hit.transform.position, Player.transform.position) > 10 && Vector3.Distance(hit.transform.position, Player.transform.position) < 75)
                    {
                        foreach (var slot in Character.Instance.CharacterData.WeaponsEquipped)
                        {
                            if (slot.itemID > 0)
                            {
                                RPGItem item = GameDatabase.Instance.GetItems()[slot.itemID];
                                if (item.WeaponSlot.entryName == "OFF HAND" && item.name.Contains("bow"))
                                {
                                    SetCursor("projectile");
                                }
                            }
                        }
                    }
                }
                else if (hit.collider.tag.Contains("friendly") && Vector3.Distance(hit.transform.position, Player.transform.position) < 3)
                {
                    SetCursor("talk");
                    if(hit.collider.GetComponentInChildren<Outlinable>())
                    {
                        currentOutlinable = hit.collider.GetComponentInChildren<Outlinable>();
                        currentOutlinable.DrawingMode = OutlinableDrawingMode.Normal;
                    }
                }
                else if (hit.collider.tag.Contains("mining"))
                {
                    SetCursor("mining");
                }
                else if (hit.collider.tag.Contains("interaction"))
                {
                    if (delta.magnitude <= 3)
                    {
                        if(hit.collider.GetComponentInChildren<Outlinable>())
                        {
                            currentOutlinable = hit.collider.GetComponentInChildren<Outlinable>();
                            currentOutlinable.DrawingMode = OutlinableDrawingMode.Normal;
                        }
                        SetCursor("use");
                    }
                }
                else
                {
                    if (Input.GetMouseButton(0))
                    {
                        SetCursor("defaultground");
                    }
                    else
                    {
                        if (currentOutlinable != null && currentOutlinable.gameObject.tag.Contains("friendly"))
                            currentOutlinable.DrawingMode = OutlinableDrawingMode.ZOnly;
                        if (currentOutlinable != null && currentOutlinable.gameObject.tag.Contains("loot"))
                            currentOutlinable.DrawingMode = OutlinableDrawingMode.ZOnly;
                        if (currentOutlinable != null && currentOutlinable.gameObject.tag.Contains("interaction"))
                            currentOutlinable.DrawingMode = OutlinableDrawingMode.ZOnly;
                        SetCursor("default");
                    }
                }
            }
        }  
    }
    public void SetCursor(string cursorName)
    {
        if (cursorName == "default")
        {
            Cursor.SetCursor(defaultMouseCursor, hotSpot, cursorMode);
        }
        else if (cursorName == "defaultground")
        {
            Cursor.SetCursor(GroundMouseCursor, hotSpot, cursorMode);
        }
        else if (cursorName == "attackglow")
        {
            Cursor.SetCursor(attackMouseCursorFlash, hotSpot, cursorMode);
        }
        else if (cursorName == "attack")
        {
            Cursor.SetCursor(attackMouseCursor, hotSpot, cursorMode);
        }
        else if (cursorName == "talk")
        {
            Cursor.SetCursor(talkMouseCursor, hotSpot, cursorMode);
        }
        else if (cursorName == "use")
        {
            Cursor.SetCursor(useMouseCursor, hotSpot, cursorMode);
        }
        else if (cursorName == "loot")
        {
            Cursor.SetCursor(lootMouseCursor, hotSpot, cursorMode);
        }
        else if (cursorName == "map_transition")
        {
            Cursor.SetCursor(MapTransition, hotSpot, cursorMode);
        }
        else if (cursorName == "black_smith")
        {
            Cursor.SetCursor(BlackSmith, hotSpot, cursorMode);
        }
        else if (cursorName == "alchemy")
        {
            Cursor.SetCursor(Alchemy, hotSpot, cursorMode);
        }
        else if (cursorName == "mining")
        {
            Cursor.SetCursor(Mining, hotSpot, cursorMode);
        }
        else if (cursorName == "projectile")
        {
            Cursor.SetCursor(Projectile, hotSpot, cursorMode);
        }
    }
}
