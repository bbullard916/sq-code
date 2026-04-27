// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using BLINK.RPGBuilder.Characters;

namespace PixelCrushers.DialogueSystem
{

    public class ConversationInfoText : MonoBehaviour
    {

        [Tooltip("Log player lines in this color.")]
        public Color playerColor = Color.blue;
        private bool isDebug = true;
        [Tooltip("Log NPC lines in this color.")]
        public Color npcColor = Color.green;
        public UIInfoWindow _UIInfoWindow;

        public void Start()
        {
            _UIInfoWindow = GameObject.FindGameObjectWithTag("info-text-window").GetComponent<UIInfoWindow>();
        }
        public void OnConversationStart(Transform actor)
        {
            if(isDebug)
              Debug.Log(string.Format("{0}: Starting conversation with {1}", new object[] { name, GetActorName(actor) }));
            this.GetComponent<InteractionSoundFX>().PlayInteractionSound();
        }

        public void OnConversationLine(Subtitle subtitle)
        {
            string _currentColor = "green";
            if (subtitle == null | subtitle.formattedText == null | string.IsNullOrEmpty(subtitle.formattedText.text)) return;
              string speakerName = (subtitle.speakerInfo != null && subtitle.speakerInfo.transform != null) ? subtitle.speakerInfo.transform.name : "(null speaker)";
            
            if(subtitle.speakerInfo.transform.name.Contains("Player"))
            {
                speakerName = Character.Instance.CharacterData.CharacterName;
            }
            else
            {
                _currentColor = "#3B9AFFFF";
            }
            
            if (isDebug)
                Debug.Log(string.Format("<color={0}>{1}: {2}</color>", new object[] { GetActorColor(subtitle), speakerName, subtitle.formattedText.text }));
            //_UIInfoWindow.ShowDialogueText(string.Format("<color={0}>{1}: {2}</color>", new object[] { GetActorColor(subtitle), speakerName, subtitle.formattedText.text }));
            _UIInfoWindow.ShowDialogueText(string.Format("<color="+_currentColor+">"+speakerName + ": "+"</color>" + "<color=#FF9400>" + subtitle.formattedText.text+"</color>" ));
        }

        public void OnConversationEnd(Transform actor)
        {
            if (isDebug)
                Debug.Log(string.Format("{0}: Ending conversation with {1}", name, GetActorName(actor)));
        }

        private string GetActorName(Transform actor)
        {
            return (actor != null) ? actor.name : "(null transform)";
        }

        private string GetActorColor(Subtitle subtitle)
        {
            if (subtitle == null | subtitle.speakerInfo == null) return "white";
            return Tools.ToWebColor(subtitle.speakerInfo.isPlayer ? playerColor : npcColor);
        }


        public void OnPrepareConversationLine(DialogueEntry entry)
        {
            if (entry == null) return;
            if (isDebug)
                Debug.Log(string.Format("Preparing line {0}", entry.currentDialogueText));
        }

        public void OnConversationLineCancelled(Subtitle subtitle)
        {
            if (subtitle == null | subtitle.formattedText == null | string.IsNullOrEmpty(subtitle.formattedText.text)) return;
            string speakerName = (subtitle.speakerInfo != null && subtitle.speakerInfo.transform != null) ? subtitle.speakerInfo.transform.name : "(null speaker)";
            if (isDebug)
                Debug.Log(string.Format("<color={0}>Line cancelled - {1}: {2}</color>", new object[] { GetActorColor(subtitle), speakerName, subtitle.formattedText.text }));
        }

        public void OnConversationLineEnd(Subtitle subtitle)
        {
            if (subtitle == null | subtitle.formattedText == null | string.IsNullOrEmpty(subtitle.formattedText.text)) return;
            string speakerName = (subtitle.speakerInfo != null && subtitle.speakerInfo.transform != null) ? subtitle.speakerInfo.transform.name : "(null speaker)";
            if (isDebug)
                Debug.Log(string.Format("<color={0}>Line ended - {1}: {2}</color>", new object[] { GetActorColor(subtitle), speakerName, subtitle.formattedText.text }));
        }

        public void OnConversationResponseMenu(Response[] responses)
        {
            if (isDebug)
                Debug.Log("Showing conversation response menu.");
        }

        public void OnConversationTimeout()
        {
            if (isDebug)
                Debug.Log("Conversation timed out.");
        }

        public void OnLinkedConversationStart(Transform actor)
        {
            if (isDebug)
                Debug.Log("Starting linked conversation.");
        }

    }

}
