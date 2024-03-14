using LahSaveLoad.Models;
using LahSaveLoad.Services;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LahSaveLoad.Commands
{
    public class CommandLoadout : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "load";

        public string Help => "load items";

        public string Syntax => String.Empty;

        public List<string> Aliases => new List<string> { "l" };

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            var instance = LahSaveLoadPlugin.Instance;

            try
            {
                // Clear player's inventory before loading saved items
                ClearInventory(player.Inventory);

                Loadout load = instance.saveLoadProvider.GetLoadout(player.Id);
                if (load == null) throw new Exception("No se encontro un loadout ");

                foreach (var itemWrapper in load.Items)
                {
                    if (itemWrapper.IsClothing)
                    {
                        // Agregar la ropa al jugador
                        Item clothingItem = new Item(itemWrapper.ItemConfig.Id, 1, itemWrapper.ItemConfig.Durability, itemWrapper.ItemConfig.Metadata);
                        player.Inventory.tryAddItem(clothingItem, true);
                    }
                    else
                    {
                        // Agregar el item al inventario del jugador
                        Item item = new Item(itemWrapper.ItemConfig.Id, itemWrapper.ItemConfig.Amount, itemWrapper.ItemConfig.Durability, itemWrapper.ItemConfig.Metadata);
                        player.Inventory.tryAddItem(item, true);
                    }
                }

                ChatManager.serverSendMessage(instance.Translate("se a cargado correctamente el Loadout"), Color.white, null, player.SteamPlayer(), EChatMode.GLOBAL, instance.Configuration.Instance.Icon, true);
            }
            catch (Exception e)
            {
                // Use alternative logging mechanism provided by Rocket framework or plugin structure
                Rocket.Core.Logging.Logger.LogError(e.Message);

                ChatManager.serverSendMessage(instance.Translate("noload"), Color.red, null, player.SteamPlayer(), EChatMode.GLOBAL, instance.Configuration.Instance.Icon, true);
            }
        }

        // ClearInventory method adjusted to avoid assigning to read-only property
        private void ClearInventory(PlayerInventory inventory)
        {
            // Iterate backwards to avoid index shifting issues
            for (byte page = 0; page < PlayerInventory.PAGES; page++)
            {
                if (page == PlayerInventory.AREA)
                    continue;

                var count = inventory.getItemCount(page);

                for (byte index = 0; index < count; index++)
                {
                    inventory.removeItem(page, 0);
                }
            }
        }
    }
}