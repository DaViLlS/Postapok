using System;
using TMPro;
using UnityEngine;

namespace _Project.Main.GameManagement.UI
{
    public class LevelTaskPanel : MonoBehaviour
    {
        [SerializeField] private GameLevelController gameLevelController;
        [SerializeField] private TMP_Text currentTask;

        private void Awake()
        {
            if (gameLevelController != null)
                gameLevelController.OnLevelTaskChanged += UpdateTaskText;
        }

        private void Start()
        {
            if (gameLevelController == null)
                return;
                
            
            if (gameLevelController.GetLevelTaskText() == string.Empty)
            {
                gameLevelController.OnLevelTaskChanged -= UpdateTaskText;
                gameObject.SetActive(false);
                
                return;
            }
            
            currentTask.text = gameLevelController.GetLevelTaskText();
        }

        private void OnDestroy()
        {
            if (gameLevelController != null)
                gameLevelController.OnLevelTaskChanged -= UpdateTaskText;
        }

        private void UpdateTaskText()
        {
            currentTask.text = gameLevelController?.GetLevelTaskText();
        }
    }
}