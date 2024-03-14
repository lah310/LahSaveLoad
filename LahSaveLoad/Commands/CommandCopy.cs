using LahSaveLoad.Models;
using LahSaveLoad.Services;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace LahSaveLoad.Commands
{
    public class CommandCopy : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "copy";

        public string Help => "copy items";

        public string Syntax => "/copy <player>";

        public List<string> Aliases => new List<string> { "c" };

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            var instance = LahSaveLoadPlugin.Instance;
            if (command.Length < 1)
            {
                ChatManager.serverSendMessage(Syntax, Color.red, null, player.SteamPlayer(), EChatMode.GLOBAL, instance.Configuration.Instance.Icon, true);
                return;
            }

            UnturnedPlayer target = UnturnedPlayer.FromName(command[0]);
            if (target == null)
            {
                ChatManager.serverSendMessage("No se ha encontrado el jugador", Color.red, null, player.SteamPlayer(), EChatMode.GLOBAL, instance.Configuration.Instance.Icon, true);
                return;
            }
            else
            {
                Loadout loadout = instance.saveLoadProvider.GetLoadout(target.Id);

                if (loadout == null)
                {
                    ChatManager.serverSendMessage("Este jugador no tiene nada para copiar :(", Color.red, null, player.SteamPlayer(), EChatMode.GLOBAL, instance.Configuration.Instance.Icon, true);
                    return;
                }
                else
                {
                    // Clear player's inventory before copying items
                    ClearInventory(player.Inventory);

                    // Copy items from target player to caller player
                    SaveLoading.DropLoadout(loadout, player);
                }
            }
        }

        // Method to clear player's inventory
        private void ClearInventory(PlayerInventory inventory)
        {
            for (byte page = 0; page < PlayerInventory.PAGES; page++)
            {
                if (page == PlayerInventory.AREA)
                    continue;

                var count = inventory.getItemCount(page);

                for (byte index = 0; index < count; index++)
                {
                    inventory.removeItem(page, index);
                }
            }
        }
    }
}
