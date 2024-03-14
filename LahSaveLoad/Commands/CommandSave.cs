using LahSaveLoad.Models;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static LahSaveLoad.Models.Loadout;

namespace LahSaveLoad.Commands
{
    internal class CommandSave : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "save";

        public string Help => "guarda tu inventario";

        public string Syntax => string.Empty;

        public List<string> Aliases => new List<string> { "save", "guardar", "s" };

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            ExecuteCommand(player);
        }

        public void ExecuteCommand(UnturnedPlayer player)
        {

            var instance = LahSaveLoadPlugin.Instance;
            List<ItemWrapper> items = new List<ItemWrapper>();

            try
            {
                var x = instance.saveLoadProvider;
                var load = x.GetLoadout(player.Id);
                if (load == null)
                {
                    GetClothing(player, items);
                    getItems(player, items);
                    Loadout load2 = new Loadout
                    {
                        Id = player.Id,
                        Items = items
                    };
                    x.AddLoadout(load2);
                    ChatManager.serverSendMessage(instance.Translate("saved"), Color.white, null, player.SteamPlayer(), EChatMode.GLOBAL, instance.Configuration.Instance.Icon, true);
                }
                else
                {
                    x.RemoveLoadout(load);
                    ExecuteCommand(player);
                }
            }
            catch
            {

            }
        }

        private void GetClothing(UnturnedPlayer player, List<ItemWrapper> items)
        {

            PlayerClothing clothing = player.Player.clothing;
            if (clothing.backpack != 0)
            {
                ItemWrapper item = new ItemWrapper
                {
                    IsClothing = true,
                    //Item = new Item(clothing.backpack, 1, clothing.backpackQuality, clothing.backpackState)
                    ItemConfig = new ItemWrapper2
                    {
                        Id = clothing.backpack,
                        Durability = clothing.backpackQuality,
                        Metadata = clothing.backpackState
                    }
                };
                items.Add(item);
            }
            if (clothing.glasses != 0)
            {
                ItemWrapper item2 = new ItemWrapper
                {
                    IsClothing = true,
                    //Item = new Item(clothing.glasses, 1, clothing.glassesQuality, clothing.glassesState)
                    ItemConfig = new ItemWrapper2
                    {
                        Id = clothing.glasses,
                        Durability = clothing.glassesQuality,
                        Metadata = clothing.glassesState
                    }
                };
                items.Add(item2);
            }
            if (clothing.hat != 0)
            {
                ItemWrapper item3 = new ItemWrapper
                {
                    IsClothing = true,
                    //Item = new Item(clothing.hat, 1, clothing.hatQuality, clothing.hatState)
                    ItemConfig = new ItemWrapper2
                    {
                        Id = clothing.hat,
                        Durability = clothing.hatQuality,
                        Metadata = clothing.hatState
                    }
                };
                items.Add(item3);
            }
            if (clothing.mask != 0)
            {
                ItemWrapper item4 = new ItemWrapper
                {
                    IsClothing = true,
                    //Item = new Item(clothing.mask, 1, clothing.maskQuality, clothing.maskState)
                    ItemConfig = new ItemWrapper2
                    {
                        Id = clothing.mask,
                        Durability = clothing.maskQuality,
                        Metadata = clothing.maskState
                    }
                };
                items.Add(item4);
            }
            if (clothing.shirt != 0)
            {
                ItemWrapper item5 = new ItemWrapper
                {
                    IsClothing = true,
                    //Item = new Item(clothing.shirt, 1, clothing.shirtQuality, clothing.shirtState)
                    ItemConfig = new ItemWrapper2
                    {
                        Id = clothing.shirt,
                        Durability = clothing.shirtQuality,
                        Metadata = clothing.shirtState
                    }
                };
                items.Add(item5);
            }
            if (clothing.vest != 0)
            {
                ItemWrapper item6 = new ItemWrapper
                {
                    IsClothing = true,
                    //Item = new Item(clothing.vest, 1, clothing.vestQuality, clothing.vestState)
                    ItemConfig = new ItemWrapper2
                    {
                        Id = clothing.vest,
                        Durability = clothing.vestQuality,
                        Metadata = clothing.vestState
                    }
                };
                items.Add(item6);
            }
            if (clothing.pants != 0)
            {
                ItemWrapper item7 = new ItemWrapper
                {
                    IsClothing = true,
                    //Item = new Item(clothing.pants, 1, clothing.pantsQuality, clothing.pantsState)
                    ItemConfig = new ItemWrapper2
                    {
                        Id = clothing.pants,
                        Durability = clothing.pantsQuality,
                        Metadata = clothing.pantsState
                    }
                };
                items.Add(item7);
            }
        }
        private void getItems(UnturnedPlayer player, List<ItemWrapper> items)
        {
            foreach (Items items2 in player.Inventory.items)
            {
                if (items2 != null)
                {
                    for (byte b = 0; b < items2.getItemCount(); b += 1)
                    {
                        ItemJar item = items2.getItem(b);
                        if (((item != null) ? item.item : null) != null)
                        {
                            ItemWrapper item2 = new ItemWrapper
                            {

                                PosX = item.x,
                                PosY = item.y,
                                Page = items2.page,
                                Rotation = item.rot,

                                ItemConfig = new ItemWrapper2
                                {
                                    Id = item.item.id,
                                    Durability = item.item.durability,
                                    Metadata = item.item.metadata,
                                    Amount = item.item.amount
                                }
                            };
                            items.Add(item2);
                        }
                    }
                }
            }
        }
    }
}