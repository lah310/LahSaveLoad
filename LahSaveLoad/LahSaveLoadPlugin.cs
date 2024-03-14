using LahSaveLoad.Configuration;
using LahSaveLoad.Database;
using LahSaveLoad.Models;
using LahSaveLoad.Services;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace LahSaveLoad
{
    public class LahSaveLoadPlugin : RocketPlugin<LahSaveLoadConfiguration>
    {
        public static LahSaveLoadPlugin Instance { get; set; }
        public SaveLoadProvider saveLoadProvider { get; set; }

        protected override void Load()
        {
            Instance = this;
            if (Configuration.Instance.AutoLoadOnDeath)
            {
                UnturnedPlayerEvents.OnPlayerRevive += OnRevive;
            }

            Logger.Log("LahSaveLoad - By Margarita#8172 (Database Json)");
            Logger.Log("El Plugin original es de ImperialsPlugins solo que este tiene una database con Json");
            Logger.Log(@"
░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░
░░░░░████░░░░░░░░░░░░░░░████░░░░░
░░░░███░░░░░░░░░░░░░░░░░░░███░░░░
░░░███░░░░░░░░░░░░░░░░░░░░░███░░░
░░███░░░░░░░░░░░░░░░░░░░░░░░███░░
░███░░░░░░░░░░░░░░░░░░░░░░░░░███░
████░░░░░░░░░░░░░░░░░░░░░░░░░████
████░░░░░░░░░░░░░░░░░░░░░░░░░████
██████░░░░░░░███████░░░░░░░██████
█████████████████████████████████
█████████████████████████████████
░███████████████████████████████░
░░████░███████████████████░████░░
░░░░░░░███▀███████████▀███░░░░░░░
░░░░░░████──▀███████▀──████░░░░░░
░░░░░░█████───█████───█████░░░░░░
░░░░░░███████▄█████▄███████░░░░░░
░░░░░░░███████████████████░░░░░░░
░░░░░░░░█████████████████░░░░░░░░
░░░░░░░░░███████████████░░░░░░░░░
░░░░░░░░░░█████████████░░░░░░░░░░
░░░░░░░░░░░███████████░░░░░░░░░░░
░░░░░░░░░░███──▀▀▀──███░░░░░░░░░░
░░░░░░░░░░███─█████─███░░░░░░░░░░
░░░░░░░░░░░███─███─███░░░░░░░░░░░
░░░░░░░░░░░░█████████░░░░░░░░░░░░
░░░░░░Plugin Mejorado by lah░░░░░
░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░");

            saveLoadProvider = new SaveLoadProvider();
            saveLoadProvider.Reload();
        }

        private void OnRevive(UnturnedPlayer player, Vector3 position, byte angle)
        {
            try
            {
                Loadout loadout = saveLoadProvider.GetLoadout(player.Id);
                if (loadout == null) throw new Exception();

                var playerInventory = player.Inventory;

                // Restaurar los elementos al inventario del jugador
                foreach (var itemWrapper in loadout.Items)
                {
                    // Obtener el ItemAsset del ItemConfig
                    var itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, itemWrapper.ItemConfig.Id);

                    if (itemAsset == null)
                    {
                        Logger.LogError($"No se encontró un Asset para el Item con ID: {itemWrapper.ItemConfig.Id}");
                        continue;
                    }

                    // Agregar el item al inventario del jugador
                    playerInventory.tryAddItem(new Item(itemAsset.id, true), true);
                }

                player.MaxSkills();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error al restaurar el inventario del jugador después de revivir: {ex.Message}");
            }
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                TranslationList translationLists = new TranslationList();

                translationLists.Add("noload", "No tienes nada en el inventario virtual usa /save");
                translationLists.Add("saved", "Has guardado tu inventario correctamente");
                return translationLists;
            }
        }

        protected override void Unload()
        {
            try
            {
                UnturnedPlayerEvents.OnPlayerRevive -= OnRevive;
            }
            catch { }
        }
    }
}
