using BLINK.RPGBuilder.Managers;
using UnityEngine;
using UnityEngine.UI;
using BLINK.RPGBuilder.Characters;

namespace BLINK.RPGBuilder.World
{
    public class CraftingStation : MonoBehaviour, IPlayerInteractable
    {
        public RPGCraftingStation station;
        public string skillName;
        public float useDistanceMax;
        public float interactableUIoffsetY = 2;
        private float CurrentInteractionProgress = 0;
        private bool interact = false;
        public Image interactionBar;

        private void LateUpdate()
        {
            if (interact)
                HandleObjectInteraction();

        }

        public virtual bool rpgIsSkillKnown(string skillName)
        {
            int skillID = GetSkillID(skillName);
            if (skillID == -1) return false;
            return Character.Instance.CharacterData.Skills.Find(skill => skill.skillID == skillID) != null;
        }


        public virtual double rpgGetSkillLevel(string skillName)
        {
            int skillID = GetSkillID(skillName);
            foreach (var t in Character.Instance.CharacterData.Skills)
            {
                if (t.skillID == skillID)
                {
                    return t.currentSkillLevel;
                }
            }
            return 0;
        }

        protected virtual int GetSkillID(string skillName)
        {
            var skill = GetSkill(skillName);
            return (skill != null) ? skill.ID : -1;
        }

        protected virtual RPGSkill GetSkill(string skillName)
        {
            foreach (RPGSkill skill in GameDatabase.Instance.GetSkills().Values)
            {
                if (skill != null && (string.Equals(skill.name.Replace("_SKILL", ""), skillName)))
                {
                    return skill;
                }
            }
            Debug.LogWarning($"Dialogue System: Can't find RPG Builder skill named '{skillName}'");
            return null;
        }

        private void HandleObjectInteraction()
        {
            CurrentInteractionProgress += Time.deltaTime;
            //Debug.Log("IN" + CurrentInteractionProgress);
            if (interact && CurrentInteractionProgress <=1)
            {
                WorldInteractableDisplayManager.Instance.UpdateInteractionBar(CurrentInteractionProgress,
                    1);
            }
            else if(CurrentInteractionProgress >=1)
            {
                interact = false;
                CurrentInteractionProgress = 0;
                if (rpgIsSkillKnown(skillName))
                {
                    InitCraftingStation();
                }
                else
                {
                    Color red = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    SimpleCustomAlrtUI.Instance.SendCustomMessage("Skill is not known.", "red");
                }

            }

        }
            private void OnMouseOver()
        {
            if (UIEvents.Instance.CursorHoverUI)
            {
                UIEvents.Instance.OnSetCursorToDefault();
                return;
            }
            if (Input.GetMouseButtonUp(1))
                if (Vector3.Distance(transform.position, GameState.playerEntity.transform.position) <=
                    useDistanceMax)
                {
                    if (!UIEvents.Instance.IsPanelOpen("Crafting"))
                    {
                        interact = true;
                        WorldInteractableDisplayManager.Instance.ResetInteractionBarBar();
                    }

                    //InitCraftingStation();
                }
                else
                {
                    if (GameState.playerEntity.controllerEssentials.GETControllerType() ==
                        RPGBuilderGeneralSettings.ControllerTypes.TopDownClickToMove)
                    {

                    }
                    else
                    {
                        UIEvents.Instance.OnShowAlertMessage("This is too far", 3);
                    }
                }

            UIEvents.Instance.OnSetNewCursor(CursorType.CraftingStation);
        }

        private void OnMouseExit()
        {
            UIEvents.Instance.OnSetCursorToDefault();
        }

        private void InitCraftingStation()
        {
            GeneralEvents.Instance.OnInitCraftingStation(this);
        }

        public void Interact()
        {
            if (UIEvents.Instance.CursorHoverUI) return;
            if (!(Vector3.Distance(transform.position, GameState.playerEntity.transform.position) <= useDistanceMax)) return;
            if (!UIEvents.Instance.IsPanelOpen("Crafting"))
            {
               //interact = true; 
            }
        }

        public void ShowInteractableUI()
        {
            var pos = transform;
            Vector3 worldPos = new Vector3(pos.position.x, pos.position.y + interactableUIoffsetY, pos.position.z);
            var screenPos = Camera.main.WorldToScreenPoint(worldPos);
            WorldInteractableDisplayManager.Instance.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);
            
            WorldInteractableDisplayManager.Instance.Show(this);
            interactionBar = WorldInteractableDisplayManager.Instance.interactionBar;
        }

        public string getInteractableName()
        {
            return station.entryDisplayName;
        }

        public bool isReadyToInteract()
        {
            return true;
        }

        public RPGCombatDATA.INTERACTABLE_TYPE getInteractableType()
        {
            return RPGCombatDATA.INTERACTABLE_TYPE.CraftingStation;
        }
    }
}