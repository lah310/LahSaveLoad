using LahSaveLoad.Models;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LahSaveLoad.Models.Loadout;

namespace LahSaveLoad.Services
{
    public static class SaveLoading
    {
        public static void DropLoadout(Loadout load, UnturnedPlayer player)
        {
            ClearInventory(player);
            List<ItemWrapper> list = new List<ItemWrapper>();
            foreach (ItemWrapper itemWrapper in load.Items)
            {

                if (itemWrapper.IsClothing)
                {
                    //player.Inventory.forceAddItem(itemWrapper.Item, true);
                    Item item = new Item(itemWrapper.ItemConfig.Id, 1, itemWrapper.ItemConfig.Durability, itemWrapper.ItemConfig.Metadata);
                    player.Inventory.forceAddItem(item, true);
                }
                else if (!player.Inventory.tryAddItem(getItem(itemWrapper), itemWrapper.PosX, itemWrapper.PosY, itemWrapper.Page, itemWrapper.Rotation))
                {
                    list.Add(itemWrapper);
                }
            }
            foreach (ItemWrapper itemWrapper2 in list)
            {
                if (!player.Inventory.tryAddItem(getItem(itemWrapper2), itemWrapper2.PosX, itemWrapper2.PosY, itemWrapper2.Page, itemWrapper2.Rotation))
                {
                    player.Inventory.forceAddItem(getItem(itemWrapper2), true);
                }
            }
        }

        private static void ClearInventory(UnturnedPlayer player)
        {
            byte[] EMPTY_BYTE_ARRAY = new byte[0];
            var playerInv = player.Inventory;

            // "Remove "models" of items from player "body""
            for (byte index = 0; index < player.Inventory.getItemCount(0); index++)
            {
                var item = player.Inventory.getItem(0, index);
                if (item != null)
                    player.Inventory.removeItem(0, index);
            }

            player.Player.equipment.sendSlot(0);
            for (byte index = 0; index < player.Inventory.getItemCount(1); index++)
            {
                var item = player.Inventory.getItem(1, index);
                if (item != null)
                    player.Inventory.removeItem(1, index);
            }

            player.Player.equipment.sendSlot(1);

            // Remove items
            for (byte page = 0; page < PlayerInventory.PAGES; page++)
            {
                if (page == PlayerInventory.AREA)
                    continue;

                var count = playerInv.getItemCount(page);

                for (byte index = 0; index < count; index++)
                    playerInv.removeItem(page, 0);
            }

            // Remove clothes

            // Remove unequipped cloths
            void RemoveUnequipped()
            {
                for (byte i = 0; i < playerInv.getItemCount(2); i++)
                    playerInv.removeItem(2, 0);
            }

            // Unequip & remove from inventory
            player.Player.clothing.askWearBackpack(0, 0, EMPTY_BYTE_ARRAY, true);
            RemoveUnequipped();

            player.Player.clothing.askWearGlasses(0, 0, EMPTY_BYTE_ARRAY, true);
            RemoveUnequipped();

            player.Player.clothing.askWearHat(0, 0, EMPTY_BYTE_ARRAY, true);
            RemoveUnequipped();

            player.Player.clothing.askWearPants(0, 0, EMPTY_BYTE_ARRAY, true);
            RemoveUnequipped();

            player.Player.clothing.askWearMask(0, 0, EMPTY_BYTE_ARRAY, true);
            RemoveUnequipped();

            player.Player.clothing.askWearShirt(0, 0, EMPTY_BYTE_ARRAY, true);
            RemoveUnequipped();

            player.Player.clothing.askWearVest(0, 0, EMPTY_BYTE_ARRAY, true);
            RemoveUnequipped();
        }

        private static Item getItem(ItemWrapper itemWrapper)
        {
            Item item = new Item(itemWrapper.ItemConfig.Id, itemWrapper.ItemConfig.Amount, itemWrapper.ItemConfig.Durability, itemWrapper.ItemConfig.Metadata);
            return item;
        }
    }
}
