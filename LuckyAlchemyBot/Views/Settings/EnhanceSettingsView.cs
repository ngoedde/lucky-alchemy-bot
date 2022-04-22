﻿using LuckyAlchemyBot.Bot;
using RSBot.Core.Event;
using RSBot.Core.Objects;
using System.Linq;
using System.Windows.Forms;

namespace LuckyAlchemyBot.Views.Settings
{
    public partial class EnhanceSettingsView : UserControl
    {
        internal class ElixirComboboxItem
        {
            /// <summary>
            /// Gets or sets the inventory item
            /// </summary>
            public InventoryItem Item { get; set; }

            public ElixirComboboxItem(InventoryItem item)
            {
                Item = item;
            }

            public override string ToString()
            {
                return $"{Item.Amount}x {Item.Record.GetRealName()}";
            }
        }

        #region Member

        private InventoryItem _selectedItem = null;

        #endregion Member

        #region Constructor

        /// <summary>
        /// Subscribes several events
        /// </summary>
        public EnhanceSettingsView()
        {
            InitializeComponent();

            EventManager.SubscribeEvent("OnEnterGame", SubscribeMainFormEvents);
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Subscribes the ItemChanged event
        /// </summary>
        private void SubscribeMainFormEvents()
        {
            if (Globals.View != null)
                Globals.View.ItemChanged += View_ItemChanged;
        }

        /// <summary>
        /// General UI update logic
        /// </summary>
        private void PopulateView()
        {
            if (Globals.View == null || Globals.View.SelectedItem == null)
            {
                Enabled = false;

                return;
            }

            _selectedItem = Globals.View.SelectedItem;
            lblCurrentOptLevel.Text = _selectedItem == null ? "+0" : $"+{Globals.View.SelectedItem.OptLevel}";

            var type = Helper.AlchemyItemHelper.ElixirType.Unspecified;

            var accessorryTypeId3 = new byte[] { 5, 12 };
            var armorTypeId3 = new byte[] { 1, 2, 3, 9, 10, 11 };

            if (_selectedItem.Record.TypeID3 == 4 && _selectedItem.Record.TypeID2 == 1)
                type = Helper.AlchemyItemHelper.ElixirType.Shield;

            if (_selectedItem.Record.TypeID3 == 6 && _selectedItem.Record.TypeID2 == 1)
                type = Helper.AlchemyItemHelper.ElixirType.Weapon;

            if (accessorryTypeId3.Contains(_selectedItem.Record.TypeID3) && _selectedItem.Record.TypeID2 == 1)
                type = Helper.AlchemyItemHelper.ElixirType.Accessory;

            if (armorTypeId3.Contains(_selectedItem.Record.TypeID3) && _selectedItem.Record.TypeID2 == 1)
                type = Helper.AlchemyItemHelper.ElixirType.Protector;

            var matchingElixirs = Helper.AlchemyItemHelper.GetElixirItems(type);

            comboElixir.Items.Clear();

            foreach (var item in matchingElixirs)
                comboElixir.Items.Add(new ElixirComboboxItem(item));

            if (comboElixir.Items.Count > 0)
                comboElixir.SelectedIndex = 0;

            var luckyPowders = Helper.AlchemyItemHelper.GetLuckyPowder(_selectedItem);
            lblLuckyPowderCount.Text = luckyPowders == null ? "x0" : $"x{luckyPowders.Amount}";

            var luckyStones = Helper.AlchemyItemHelper.GetLuckyStone(_selectedItem);
            checkUseLuckyStones.Enabled = luckyStones != null && luckyStones.Amount > 0;

            if (luckyStones == null)
                checkUseLuckyStones.Checked = false;
            lblLuckyCount.Text = luckyStones == null ? "x0" : $"x{luckyStones.Amount}";

            var astralStones = Helper.AlchemyItemHelper.GetAstralStone(_selectedItem);
            if (astralStones == null)
                checkUseAstralStones.Checked = false;
            checkUseAstralStones.Enabled = astralStones != null && astralStones.Amount > 0;
            lblAstralCount.Text = astralStones == null ? "x0" : $"x{astralStones.Amount}";

            var immortalStones = Helper.AlchemyItemHelper.GetImmortalStone(_selectedItem);
            if (immortalStones == null)
                checkUseAstralStones.Checked = false;
            checkUseImmortalStones.Enabled = immortalStones != null && immortalStones.Amount > 0;

            lblImmortalCount.Text = immortalStones == null ? "x0" : $"x{immortalStones.Amount}";

            var steadyStones = Helper.AlchemyItemHelper.GetSteadyStone(_selectedItem);
            checkUseSteadyStones.Enabled = steadyStones != null && steadyStones.Amount > 0;
            lblSteadyStonesCount.Text = steadyStones == null ? "x0" : $"x{steadyStones.Amount}";

            Enabled = true;
        }

        #endregion Methods

        #region Events

        /// <summary>
        /// Will be triggered when the user click on the refresh link
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkRefreshItemList_Click(object sender, System.EventArgs e)
        {
            PopulateView();
        }

        /// <summary>
        /// Will be triggered when the selected item changed
        /// </summary>
        /// <param name="item">The new item</param>
        private void View_ItemChanged(InventoryItem item)
        {
            PopulateView();
        }

        /// <summary>
        /// Will be triggered when the user changed a setting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void config_CheckedChange(object sender, System.EventArgs e)
        {
            if (Globals.Botbase == null || Globals.Botbase.Engine != Engine.Enhancement)
                return;

            Globals.Botbase.EnhancementConfig = new Bot.EnhancementConfig
            {
                Item = Globals.View.SelectedItem,
                UseAstralStones = checkUseAstralStones.Checked,
                UseLuckyStones = checkUseLuckyStones.Checked,
                UseImmortalStones = checkUseImmortalStones.Checked,
                UseSteadyStones = checkUseSteadyStones.Checked,
                Elixir = (comboElixir.SelectedItem as ElixirComboboxItem)?.Item,
                MaxOptLevel = (byte)numMaxEnhancement.Value,
                StopIfLuckyPowderEmpty = checkStopLuckyPowder.Checked
            };
        }

        #endregion Events
    }
}