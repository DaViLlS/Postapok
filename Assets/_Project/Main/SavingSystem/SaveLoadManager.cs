using UnityEngine;
using Zenject;

namespace _Project.Main.SavingSystem
{
    public class SaveLoadManager : MonoBehaviour
    {
        [Inject] private readonly ISaveLoader[] _saveLoaders;

        private void Awake()
        {
            LoadGame();
        }

        private void OnDestroy()
        {
            SaveGame();
        }

        public void LoadGame()
        {
            Repository.LoadState();

            foreach (var saveLoader in _saveLoaders)
            {
                saveLoader.LoadData(); 
            }
        }

        public void SaveGame()
        {
            foreach (var saveLoader in _saveLoaders)
            {
                saveLoader.SaveData();
            }
        
            Repository.SaveState();
        }
    }
}