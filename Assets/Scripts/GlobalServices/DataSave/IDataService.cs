using UnityEngine;

namespace GlobalServices.DataSave
{
    public interface IDataService
    {
        public void Save();
        public void Load();

        public string DataID { get; set; }
    }
}