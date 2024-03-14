using LahSaveLoad.Models;
using LahSaveLoad.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LahSaveLoad.Database
{
    public class SaveLoadProvider
    {
        private List<Loadout> Data;
        private DataStorage<List<Loadout>> ItemStorage { get; set; }
        public SaveLoadProvider()
        {
            this.ItemStorage = new DataStorage<List<Loadout>>(LahSaveLoadPlugin.Instance.Directory, "Database.json");
        }


        public void Reload()
        {
            Data = ItemStorage.Read();
            if(Data == null)
            {
                Data = new List<Loadout>();
                ItemStorage.Save(Data);
            }
        }

        public void AddLoadout(Loadout item)
        {
            this.Data.Add(item);
            ItemStorage.Save(this.Data);
        }

        public void RemoveLoadout(Loadout item)
        {
            this.Data.Remove(item);
            ItemStorage.Save(this.Data);
        }

        public Loadout GetLoadout(string Id)
        {
            return this.Data.Find(x => x.Id == Id);
        }


    }
}
