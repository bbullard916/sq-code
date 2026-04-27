using System.Collections.Generic;
using System.Linq;
using BLINK.RPGBuilder.Characters;
using BLINK.RPGBuilder.Combat;
using BLINK.RPGBuilder.Data;
using BLINK.RPGBuilder.DisplayHandler;
using BLINK.RPGBuilder.UIElements;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BLINK.RPGBuilder.Managers
{
    public enum CustomCharacterInfoTypes
    {
        GEAR,
        INFO,
        STATS,
        ATTRIBUTES,
        FACTIONS
    }

    public class CustomCharacterPanel : DisplayPanel
    {
        [SerializeField] private CanvasGroup thisCG, CharGearCG, CharInfoCG, CharStatsCG, CharAttributesCG, CharFactionsCG, StatTooltipCG;
        [SerializeField] private TextMeshProUGUI CharacterNameText, RaceNameText, ClassNameText, LevelText, ExperienceText, StatTooltipText;
        [SerializeField] private GameObject classTalentTreeButtonGO;
        [SerializeField] private GameObject StatTitlePrefab, StatTextPrefab, CombatTreeSlotPrefab, factionSlotPrefab;
        [SerializeField] private Transform StatTextsParent, CombatTreeSlotsParent, factionSlotsParent;
        [SerializeField] private List<GameObject> statTextGO = new List<GameObject>();
        [SerializeField] private List<GameObject> cbtTreeSlots = new List<GameObject>();
        [SerializeField] private List<GameObject> factionSlots = new List<GameObject>();
        [SerializeField] private Animator talentCategoryAnimator;
        [SerializeField] private Color defaultCategoryColor, selectedCategoryColor;
        [SerializeField] private Sprite defaultCategorySprite, selectedCategorySprite;
        [SerializeField] private Image gearCategoryImage, statsCategoryImage, talentsCategoryImage, factionsCategorryImage;
        [SerializeField] private Button gearCategoryButton, statsCategoryButton, talentsCategoryButton, factionsCategorryButton;
        [SerializeField] private GameObject statSlotPrefab;
        [SerializeField] private Transform statSlotsParent;
        private readonly List<StatAllocationSlot> curStatSlots = new List<StatAllocationSlot>();
        [SerializeField] private TextMeshProUGUI currentPointsText;
        [SerializeField] private List<EquipmentItemSlotDisplayHandler> ArmorSlots;
        [SerializeField] private List<EquipmentItemSlotDisplayHandler> WeaponSlots;



        private CustomCharacterInfoTypes curCharInfoType;
        private static readonly int glowing = Animator.StringToHash("glowing");
        public DuloGames.UI.UITab[] Paneltabs;
        private void OnEnable()
        {
            GameEvents.NewGameSceneLoaded += Register;
            GameEvents.BackToMainMenu += ResetCategoryButtons;
            GeneralEvents.PlayerEquippedItem += PlayerEquippedItem;
            GeneralEvents.PlayerUnequippedItem += PlayerUnequippedItem;
            UIEvents.ShowCharacterPanelStatTooltip += ShowStatTooltipPanel;
            UIEvents.HideCharacterPanelStatTooltip += HideStatTooltipPanel;
            CombatEvents.StatsChanged += StatsChanged;
            UIEvents.UpdateStatAllocationPanel += UpdateCurrentPointText;

            if (GameState.IsInGame())
            {
                Register();
                if (UIEvents.Instance.IsPanelOpen("Character")) Show();
            }
        }

        private void OnDisable()
        {
            GameEvents.NewGameSceneLoaded -= Register;
            GameEvents.BackToMainMenu -= ResetCategoryButtons;
            GeneralEvents.PlayerEquippedItem -= PlayerEquippedItem;
            GeneralEvents.PlayerUnequippedItem -= PlayerUnequippedItem;
            UIEvents.ShowCharacterPanelStatTooltip -= ShowStatTooltipPanel;
            UIEvents.HideCharacterPanelStatTooltip -= HideStatTooltipPanel;
            CombatEvents.StatsChanged -= StatsChanged;
            Unregister();
        }

        protected override void Register()
        {
            Dictionary<string, object> panelData = new Dictionary<string, object> { { "ArmorSlots", ArmorSlots }, { "WeaponSlots", WeaponSlots }, { "allSlots", curStatSlots } };
            UIEvents.Instance.AddPanelEntry(this, gameObject.name, panelData);
        }

        protected override void Unregister()
        {
            UIEvents.Instance.RemovePanelEntry(this, gameObject.name);
        }

        public override bool IsOpen()
        {
            return opened;
        }

        private void disableAllCharCategoriesCG()
        {
            RPGBuilderUtilities.DisableCG(CharInfoCG);
            RPGBuilderUtilities.DisableCG(CharGearCG);
            RPGBuilderUtilities.DisableCG(CharStatsCG);
            RPGBuilderUtilities.DisableCG(CharAttributesCG);
            RPGBuilderUtilities.DisableCG(CharFactionsCG);
        }

        private void ResetCategoryButtons()
        {
            setButtonAppearance(gearCategoryButton, gearCategoryImage, false);
            setButtonAppearance(statsCategoryButton, statsCategoryImage, false);
            setButtonAppearance(factionsCategorryButton, factionsCategorryImage, false);
            setButtonAppearance(talentsCategoryButton, talentsCategoryImage, false);
        }

        private void setButtonAppearance(Button button, Image image, bool selected)
        {
            ColorBlock colorblock = button.colors;
            colorblock.normalColor = selected ? selectedCategoryColor : defaultCategoryColor;
            button.colors = colorblock;
            image.sprite = selected ? selectedCategorySprite : defaultCategorySprite;
        }

        public void InitCharacterCategory(string newCategory)
        {
            var parsedEnum = (CustomCharacterInfoTypes)System.Enum.Parse(typeof(CustomCharacterInfoTypes), newCategory); 
            disableAllCharCategoriesCG();
            ResetCategoryButtons();
            ClearStatText();
            InitCharacterInfo();
            RPGBuilderUtilities.EnableCG(CharInfoCG);
            switch (parsedEnum)
            {
                case CustomCharacterInfoTypes.GEAR:
                    curCharInfoType = CustomCharacterInfoTypes.GEAR;
                    RPGBuilderUtilities.EnableCG(CharGearCG);
                    setButtonAppearance(gearCategoryButton, gearCategoryImage, true);
                    InitCharEquippedItems();
                    curCharInfoType = CustomCharacterInfoTypes.STATS;
                    RPGBuilderUtilities.EnableCG(CharStatsCG);
                    setButtonAppearance(statsCategoryButton, statsCategoryImage, true);
                    //InitCharStats();
                    break;
                case CustomCharacterInfoTypes.INFO:
                    curCharInfoType = CustomCharacterInfoTypes.INFO;
                    break;
                case CustomCharacterInfoTypes.STATS:
                    curCharInfoType = CustomCharacterInfoTypes.STATS;
                    RPGBuilderUtilities.EnableCG(CharStatsCG);
                    setButtonAppearance(statsCategoryButton, statsCategoryImage, true);
                    //InitCharStats();
                    break;
                case CustomCharacterInfoTypes.ATTRIBUTES:
                    curCharInfoType = CustomCharacterInfoTypes.ATTRIBUTES;
                    RPGBuilderUtilities.EnableCG(CharAttributesCG);
                    setButtonAppearance(talentsCategoryButton, talentsCategoryImage, true);
                    //InitCharCombatTrees();
                    //InitStatList();
                    break;
                case CustomCharacterInfoTypes.FACTIONS:
                    curCharInfoType = CustomCharacterInfoTypes.FACTIONS;
                    RPGBuilderUtilities.EnableCG(CharFactionsCG);
                    setButtonAppearance(factionsCategorryButton, factionsCategorryImage, true);
                    InitCharFactions();
                    break;
            }

            if (!GameDatabase.Instance.GetCharacterSettings().NoClasses && talentCategoryAnimator != null) talentCategoryAnimator.SetBool(glowing, RPGBuilderUtilities.hasPointsToSpendInClassTrees());
        }

        private void UpdateCurrentPointText()
        {
            currentPointsText.text = "Points: " + Character.Instance.getTreePointsAmountByPoint(GameDatabase.Instance.GetCharacterSettings().StatAllocationPointID);
        }

        public void ClearAllStatSlots()
        {
            foreach (var t in curStatSlots)
                Destroy(t.gameObject);

            curStatSlots.Clear();
        }

        private void InitAttribsInfo()
        {
            ClearAllStatSlots();

            List<CharacterEntries.AllocatedStatEntry> allStats = getAllStats();

            foreach (var statAllocationEntry in allStats)
            {
                StatAllocationManager.Instance.SpawnStatAllocationSlot(statAllocationEntry, statSlotPrefab,
                    statSlotsParent, curStatSlots, StatAllocationSlot.SlotType.Game);
            }


            foreach (var allocatedStatSlot in curStatSlots)
            {
                float currentValue = StatAllocationManager.Instance.getAllocatedStatValue(allocatedStatSlot.thisStat.ID, StatAllocationSlot.SlotType.Game);

                float max = StatAllocationManager.Instance.getMaxAllocatedStatValue(allocatedStatSlot.thisStat);
                allocatedStatSlot.curValueText.text = max > 0 ? currentValue + " / " + max :
                    currentValue.ToString();
            }

            UpdateCurrentPointText();

            Dictionary<string, object> panelData = new Dictionary<string, object> { { "allSlots", curStatSlots } };
            UIEvents.Instance.UpdatePanelEntryData(this, gameObject.name, panelData);

            StatAllocationManager.Instance.HandleStatAllocationButtons(Character.Instance.getTreePointsAmountByPoint(GameDatabase.Instance.GetCharacterSettings().StatAllocationPointID), 0, curStatSlots, StatAllocationSlot.SlotType.Game);
        }

        public void InitStatList()
        {
            ClearStatText();
            ClearAllStatSlots();

            List<CharacterEntries.AllocatedStatEntry> allStats = getAllStats();

            foreach (var statAllocationEntry in allStats)
            {
                StatAllocationManager.Instance.SpawnStatAllocationSlot(statAllocationEntry, statSlotPrefab,
                    statSlotsParent, curStatSlots, StatAllocationSlot.SlotType.Game);
            }


            foreach (var allocatedStatSlot in curStatSlots)
            {
                float currentValue = StatAllocationManager.Instance.getAllocatedStatValue(allocatedStatSlot.thisStat.ID, StatAllocationSlot.SlotType.Game);

                float max = StatAllocationManager.Instance.getMaxAllocatedStatValue(allocatedStatSlot.thisStat);
                allocatedStatSlot.curValueText.text = max > 0 ? currentValue + " / " + max :
                    currentValue.ToString();
            }

            UpdateCurrentPointText();

            Dictionary<string, object> panelData = new Dictionary<string, object> { { "allSlots", curStatSlots } };
            UIEvents.Instance.UpdatePanelEntryData(this, gameObject.name, panelData);

            StatAllocationManager.Instance.HandleStatAllocationButtons(Character.Instance.getTreePointsAmountByPoint(GameDatabase.Instance.GetCharacterSettings().StatAllocationPointID), 0, curStatSlots, StatAllocationSlot.SlotType.Game);
        }
        private void InitCharacterInfo()
        {
            CharacterNameText.text = Character.Instance.CharacterData.CharacterName;
            RaceNameText.text = "Race: " + GameDatabase.Instance.GetRaces()[Character.Instance.CharacterData.RaceID].entryDisplayName;

            if (!GameDatabase.Instance.GetCharacterSettings().NoClasses)
            {
                ClassNameText.text = "Class: " + GameDatabase.Instance.GetClasses()[Character.Instance.CharacterData.ClassID].entryDisplayName;
            }
            else
            {
                ClassNameText.text = "";
            }

            LevelText.text = "Level: " + Character.Instance.CharacterData.Level;
            ExperienceText.text = "Experience: " + Character.Instance.CharacterData.CurrentExperience + " / " + Character.Instance.CharacterData.ExperienceNeeded;
        }

        private List<CharacterEntries.AllocatedStatEntry> getAllStats()
        {
            List<CharacterEntries.AllocatedStatEntry> allStats = new List<CharacterEntries.AllocatedStatEntry>();

            foreach (var skillREF in Character.Instance.CharacterData.Skills.Select(skill => GameDatabase.Instance.GetSkills()[skill.skillID]))
            {
                allStats.AddRange(skillREF.allocatedStatsEntriesGame.Where(stat => stat.statID != -1));
            }
            foreach (var weaponTemplateREF in Character.Instance.CharacterData.WeaponTemplates.Select(weaponTemplate => GameDatabase.Instance.GetWeaponTemplates()[weaponTemplate.weaponTemplateID]))
            {
                allStats.AddRange(weaponTemplateREF.allocatedStatsEntriesGame.Where(stat => stat.statID != -1));
            }

            if (!GameDatabase.Instance.GetCharacterSettings().NoClasses)
            {
                RPGClass classREF = GameDatabase.Instance.GetClasses()[Character.Instance.CharacterData.ClassID];
                allStats.AddRange(classREF.allocatedStatsEntriesGame.Where(stat => stat.statID != -1));
            }

            return allStats;
        }
        private void ClearCombatTreeSlots()
        {
            foreach (var t in cbtTreeSlots) Destroy(t);

            cbtTreeSlots.Clear();
        }

        private void InitCharCombatTrees()
        {
            ClearCombatTreeSlots();
            foreach (var t in GameDatabase.Instance.GetClasses()[Character.Instance.CharacterData.ClassID].talentTrees)
            {
                var cbtTree = Instantiate(CombatTreeSlotPrefab, CombatTreeSlotsParent);
                cbtTreeSlots.Add(cbtTree);
                var slotREF = cbtTree.GetComponent<CombatTreeSlot>();
                slotREF.InitSlot(GameDatabase.Instance.GetTalentTrees()[t.talentTreeID]);
            }
        }

        private void InitAttributesPanel()
        {

        }
        private void PlayerEquippedItem(RPGItem itemEquipped)
        {
            InitCharEquippedItems();
        }
        private void PlayerUnequippedItem(RPGItem itemEquipped)
        {
            InitCharEquippedItems();
        }

        private void StatsChanged(CombatEntity statEntity)
        {
            if (!statEntity.IsPlayer()) return;
            //if (opened && curCharInfoType == CustomCharacterInfoTypes.STATS) InitCharStats();
        }

        private void InitCharEquippedItems()
        {
            ArmorSlots.Clear();
            WeaponSlots.Clear();

            foreach (var slot in GetComponentsInChildren<EquipmentItemSlotDisplayHandler>())
            {
                if (slot.EquipFunction == EconomyData.EquipFunction.Armor)
                {
                    ArmorSlots.Add(slot);
                }
                else if (slot.EquipFunction == EconomyData.EquipFunction.Weapon)
                {
                    WeaponSlots.Add(slot);
                }
            }

            foreach (var armorSlot in ArmorSlots)
            {
                foreach (var equippedArmorSlot in GameState.playerEntity.equippedArmors)
                {
                    if (armorSlot.ArmorSlot != equippedArmorSlot.ArmorSlot) continue;
                    if (equippedArmorSlot.item != null)
                        armorSlot.InitItem(equippedArmorSlot.item, equippedArmorSlot.temporaryItemDataID);
                    else
                        armorSlot.ResetItem();
                }
            }

            for (var i = 0; i < WeaponSlots.Count; i++)
            {
                var weaponSlot = WeaponSlots[i];
                for (var index = 0; index < GameState.playerEntity.equippedWeapons.Count; index++)
                {
                    var equippedWeaponSlot = GameState.playerEntity.equippedWeapons[index];
                    if (i != index) continue;
                    if (equippedWeaponSlot.item != null)
                        weaponSlot.InitItem(equippedWeaponSlot.item, equippedWeaponSlot.temporaryItemDataID);
                    else
                        weaponSlot.ResetItem();
                }
            }

        }

        public void InitCharStats()
        {
            ClearStatText();
            foreach (var t in GameDatabase.Instance.GetStatCategories().Values)
            {
                if (t.entryName.Contains("Attributes"))
                {
                    var statTitle = Instantiate(StatTitlePrefab, StatTextsParent);
                    statTextGO.Add(statTitle);
                    statTitle.GetComponent<TextMeshProUGUI>().text = t.entryDisplayName;
                    foreach (var t1 in GameState.playerEntity.GetStats())
                    {
                        if (t1.Key == 27)
                        {
                            var statText = Instantiate(StatTextPrefab, StatTextsParent);
                            statTextGO.Add(statText);
                            StatDataHolder statREF = statText.GetComponent<StatDataHolder>();
                            if (statREF != null)
                            {
                                statREF.InitStatText(t1.Value);
                            }
                        }
                    }
                    foreach (var t1 in GameState.playerEntity.GetStats())
                    {
                        if (t1.Key == 44)
                        {
                            var statText = Instantiate(StatTextPrefab, StatTextsParent);
                            statTextGO.Add(statText);
                            StatDataHolder statREF = statText.GetComponent<StatDataHolder>();
                            if (statREF != null)
                            {
                                statREF.InitStatText(t1.Value);
                            }
                        }
                    }
                    foreach (var t1 in GameState.playerEntity.GetStats())
                    {
                        if (t1.Key == 45)
                        {
                            var statText = Instantiate(StatTextPrefab, StatTextsParent);
                            statTextGO.Add(statText);
                            StatDataHolder statREF = statText.GetComponent<StatDataHolder>();
                            if (statREF != null)
                            {
                                statREF.InitStatText(t1.Value);
                            }
                        }
                    }
                    foreach (var t1 in GameState.playerEntity.GetStats())
                    {
                        if (t1.Key == 28)
                        {
                            var statText = Instantiate(StatTextPrefab, StatTextsParent);
                            statTextGO.Add(statText);
                            StatDataHolder statREF = statText.GetComponent<StatDataHolder>();
                            if (statREF != null)
                            {
                                statREF.InitStatText(t1.Value);
                            }
                        }
                    }
                    foreach (var t1 in GameState.playerEntity.GetStats())
                    {
                        if (t1.Key == 46)
                        {
                            var statText = Instantiate(StatTextPrefab, StatTextsParent);
                            statTextGO.Add(statText);
                            StatDataHolder statREF = statText.GetComponent<StatDataHolder>();
                            if (statREF != null)
                            {
                                statREF.InitStatText(t1.Value);
                            }
                        }
                    }
                }
            }
                foreach (var t in GameDatabase.Instance.GetStatCategories().Values)
            {
                if (t == null) continue;
                if(!t.entryName.Contains("Attributes"))
                {
                    var statTitle = Instantiate(StatTitlePrefab, StatTextsParent);
                    statTextGO.Add(statTitle);
                    statTitle.GetComponent<TextMeshProUGUI>().text = t.entryDisplayName;

                    foreach (var t1 in GameState.playerEntity.GetStats())
                    {
                        if (t1.Value.stat.StatCategory != t) continue;
                        var statText = Instantiate(StatTextPrefab, StatTextsParent);
                        statTextGO.Add(statText);
                        StatDataHolder statREF = statText.GetComponent<StatDataHolder>();
                        if (statREF != null)
                        {
                            statREF.InitStatText(t1.Value);
                        }
                    }
                }
            }
        }

        private void ClearFactionSlots()
        {
            foreach (var t in factionSlots) Destroy(t);
            factionSlots.Clear();
        }

        private void InitCharFactions()
        {
            ClearFactionSlots();
            foreach (var t in Character.Instance.CharacterData.Factions)
            {
                var factionSlot = Instantiate(factionSlotPrefab, factionSlotsParent);
                factionSlots.Add(factionSlot);
                FactionSlotDataHolder factionSlotREF = factionSlot.GetComponent<FactionSlotDataHolder>();
                if (factionSlotREF != null)
                {
                    factionSlotREF.Init(t);
                }
            }
        }

        private void ShowStatTooltipPanel(RPGStat stat)
        {
            StatTooltipCG.alpha = 1;
            StatTooltipText.text = stat.entryDescription;
        }

        private void HideStatTooltipPanel()
        {
            StatTooltipCG.alpha = 0;
            StatTooltipText.text = "";
        }

        public void ClearStatText()
        {
            foreach (var t in statTextGO) Destroy(t);
            statTextGO.Clear();
        }

        public override void Show()
        {
            base.Show();
            CharacterNameText.text = Character.Instance.CharacterData.CharacterName;
            classTalentTreeButtonGO.SetActive(!GameDatabase.Instance.GetCharacterSettings().NoClasses);
            RPGBuilderUtilities.EnableCG(thisCG);
            transform.SetAsLastSibling();
            InitCharacterCategory(curCharInfoType.ToString());
            UIEvents.Instance.OnClosePanel("Skill_Book");
            UIEvents.Instance.OnClosePanel("Weapon_Templates");
            CustomInputManager.Instance.AddOpenedPanel(thisCG);

            if (GameState.playerEntity != null) GameState.playerEntity.controllerEssentials.GameUIPanelAction(opened);
            if (PauseGame) Invoke("StartPause", 0);
            for (int i = 0; i < Paneltabs.Length; i++)
            {
                if (Paneltabs[i].isOn && Paneltabs[i].name.Contains("GEAR"))
                {
                    InitCharacterCategory("GEAR");
                }
                if (Paneltabs[i].isOn && Paneltabs[i].name.Contains("ATTRIBUTES"))
                {
                    InitCharacterCategory("ATTRIBUTES");
                }
                if (Paneltabs[i].isOn && Paneltabs[i].name.Contains("FACTIONS"))
                {
                    InitCharacterCategory("FACTIONS");
                }
            }
        }

        public override void Hide()
        {
            base.Hide();
            if (PauseGame) Invoke("EndPause", 0);
            gameObject.transform.SetAsFirstSibling();
            RPGBuilderUtilities.DisableCG(thisCG);
            if (CustomInputManager.Instance != null) CustomInputManager.Instance.HandleUIPanelClose(thisCG);
        }
    }
}
