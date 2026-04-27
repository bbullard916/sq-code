using UnityEngine;
using UnityEngine.UI;
using PixelCrushers;
using PixelCrushers.QuestMachine;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.RPGBuilderSupport;

namespace PixelCrushers.QuestMachine.DialogueSystemSupport
{
    [RequireComponent(typeof(LevelWillBeChanged))]
    [RequireComponent(typeof(HandleWorldMapAction))]
    public class HandleMapTransition : MonoBehaviour, IMessageHandler
    {
        // -----------------------
        // Inspector
        // -----------------------
        [Header("Quest Conditions")]
        [Tooltip("Quest (ID/Name) to gate usage. Leave empty to ignore quest gating.")]
        public string questName;

        [Tooltip("Quest Node ID to check. Leave empty to gate on whole quest or ignore gating.")]
        public string questNodeName;

        [Header("Transition")]
        [Tooltip("World-map transition key. Leave blank for 'open world map' behavior.")]
        public string singleTransition;

        [Header("UI")]
        [Tooltip("Optional arrow Image whose color reflects quest node state.")]
        public Image arrowImage;

        [Header("References (Optional - will auto-resolve if not set)")]
        [Tooltip("World map handler (tagged 'World Map' if not assigned).")]
        public CustomWorldMapHandler worldMapHandler;

        // -----------------------
        // Private
        // -----------------------
        private LevelWillBeChanged _levelWillBeChanged;
        private HandleWorldMapAction _worldMapAction;
        private DialogueSystemRPGBuilderBridge _rpgBridge;

        // Consistent UI colors
        private static readonly Color32 ColorIdleNoNode = new Color32(128, 141, 150, 255);  // Blue-ish
        private static readonly Color32 ColorActiveNode = new Color32(84, 200, 18, 71);  // Green-ish
        private static readonly Color32 ColorInactiveNode = new Color32(128, 141, 150, 114);  // Purplish

        // -----------------------
        // Unity lifecycle
        // -----------------------
        private void Awake()
        {
            // Required components
            _levelWillBeChanged = GetComponent<LevelWillBeChanged>();
            _worldMapAction = GetComponent<HandleWorldMapAction>();

            // Optional reference (only if present)
            var rpgGO = GameObject.Find("DialogueManager_RPGBuilder");
            if (rpgGO != null) _rpgBridge = rpgGO.GetComponent<DialogueSystemRPGBuilderBridge>();

            // World map handler: prefer assigned one, otherwise try tag fallback.
            if (worldMapHandler == null)
            {
                var tagged = GameObject.FindGameObjectWithTag("World Map");
                if (tagged != null)
                {
                    worldMapHandler = tagged.GetComponent<CustomWorldMapHandler>();
                }
            }
        }

        private void OnEnable()
        {
            MessageSystem.AddListener(this, QuestMachineMessages.QuestStateChangedMessage, string.Empty);
        }

        private void OnDisable()
        {
            MessageSystem.RemoveListener(this, QuestMachineMessages.QuestStateChangedMessage, string.Empty);
        }

        private void Start()
        {
            // Validate critical refs
            if (_levelWillBeChanged == null)
                Debug.LogError($"{nameof(HandleMapTransition)}: Missing {nameof(LevelWillBeChanged)}.", this);
            if (_worldMapAction == null)
                Debug.LogError($"{nameof(HandleMapTransition)}: Missing {nameof(HandleWorldMapAction)}.", this);
            if (worldMapHandler == null)
                Debug.LogWarning($"{nameof(HandleMapTransition)}: Missing {nameof(CustomWorldMapHandler)}. Transitions will be disabled.", this);

            // Initialize arrow color
            if (arrowImage != null)
            {
                if (string.IsNullOrEmpty(questNodeName))
                {
                    arrowImage.color = ColorIdleNoNode;
                }
                else
                {
                    arrowImage.color = IsQuestNodeActive() ? ColorActiveNode : ColorInactiveNode;
                }
            }
        }

        // -----------------------
        // IMessageHandler
        // -----------------------
        public void OnMessage(MessageArgs args)
        {
            // Message: "Quest State Changed"
            // parameter: quest ID, values[0]: StringField node ID or null, values[1]: QuestState/QuestNodeState
            var changedQuestID = args.parameter;

            // If a specific node is provided
            if (args.values != null && args.values.Length >= 2 && args.values[0] is StringField nodeField)
            {
                var nodeId = StringField.GetStringValue(nodeField);
                var nodeState = (QuestNodeState)args.values[1];

                // Only react to our configured quest+node
                if (!string.IsNullOrEmpty(questName) &&
                    !string.IsNullOrEmpty(questNodeName) &&
                    changedQuestID == questName &&
                    nodeId == questNodeName)
                {
                    // Update arrow color based on node state
                    if (arrowImage != null)
                    {
                        arrowImage.color = nodeState == QuestNodeState.Active ? ColorActiveNode : ColorInactiveNode;
                    }
                }
            }
            else
            {
                // Main quest state changed (no node). You can add handling here if needed.
                // var questState = (QuestState)args.values[1];
                // Debug.Log($"Quest {changedQuestID} changed to {questState}");
            }
        }

        // -----------------------
        // Trigger logic
        // -----------------------
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            Debug.Log($"{other.name} entered {name}'s trigger.");

            // If no quest gating
            if (string.IsNullOrEmpty(questNodeName))
            {
                // If no specific transition, open world map action (index 0 by convention)
                if (IsNone(singleTransition))
                {
                    OpenWorldMapPanel();
                    return;
                }

                // Else perform named transition
                TryTransition(singleTransition);
                return;
            }

            // Quest node gating present
            if (IsQuestNodeActive() || IsQuestNodeSuccess())
            {
                if (!IsNone(singleTransition))
                {
                    // Perform named transition
                    _levelWillBeChanged?.levelWillChange();
                    TryTransition(singleTransition);
                }
                else
                {
                    // Open world map action
                    OpenWorldMapPanel();
                }
            }
            else
            {
                UIInfoWindow.Instance?.SendGeneralMessage("You cannot travel at this at this time.");
                SimpleCustomAlrtUI.Instance.SendCustomMessage("You cannot travel at this at this time.", "green");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            Debug.Log($"{other.name} exited {name}'s trigger.");
            // Close panel if we were in a quest-gated area and showing world map UI
            if (!IsNone(singleTransition) || string.IsNullOrEmpty(questNodeName)) return;
            _worldMapAction?.ClosePanel();
        }

        // -----------------------
        // Helpers
        // -----------------------
        private void OpenWorldMapPanel()
        {
            Debug.Log("Opening World Map panel.");
            _levelWillBeChanged?.levelWillChange();
            _worldMapAction?.HandleWorldMapActionCall(0);
        }

        private void TryTransition(string transitionKey)
        {
            if (worldMapHandler == null)
            {
                Debug.LogWarning("Transition requested but WorldMapHandler is not assigned.", this);
                return;
            }
            Debug.Log($"Transition: {transitionKey}");
            _levelWillBeChanged?.levelWillChange();
            worldMapHandler.HandleTransition(transitionKey);
        }

        private static bool IsNone(string value)
        {
            // Treat null/empty/"None" (case-insensitive) as no transition configured.
            return string.IsNullOrEmpty(value) || value.Equals("None", System.StringComparison.OrdinalIgnoreCase);
        }

        private bool IsQuestNodeActive()
        {
            if (string.IsNullOrEmpty(questName) || string.IsNullOrEmpty(questNodeName)) return false;

            var quest = GetQuestInstance(questName, "Player");
            if (quest == null) return false;

            var node = quest.GetNode(questNodeName);
            if (node == null) return false;

            return node.GetState() == QuestNodeState.Active;
        }

        private bool IsQuestNodeSuccess()
        {
            if (string.IsNullOrEmpty(questName) || string.IsNullOrEmpty(questNodeName)) return false;

            var quest = GetQuestInstance(questName, "Player");
            if (quest == null) return false;

            var node = quest.GetNode(questNodeName);
            if (node == null) return false;

            return node.GetState() == QuestNodeState.True;
        }

        private Quest GetQuestInstance(string questID, string questerID)
        {
            var originalDebug = QuestMachine.debug;
            QuestMachine.debug = false;

            var quester = QuestMachine.GetQuestJournal(questerID);
            Quest result =
                quester?.FindQuest(questID) ??
                QuestMachine.GetQuestInstance(questID, StringField.GetStringValue(quester?.id)) ??
                QuestMachine.GetQuestInstance(questID, questerID);

            QuestMachine.debug = originalDebug;
            return result;
        }
    }
}
