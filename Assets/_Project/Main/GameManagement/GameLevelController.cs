using System;
using _Project.Enemies.Scripts;
using _Project.Localization;
using _Project.Main.UiBase;
using _Project.MainCharacter.Scripts;
using _Project.TasksAndDialogues.Tasks.Scripts;
using _Project.WorldClicking.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace _Project.Main.GameManagement
{
    public abstract class GameLevelController : MonoBehaviour
    {
        public event Action OnLevelTaskChanged;
        public event Action OnLevelCompleted;
        public event Action OnLevelFailed;

        [Inject] private TasksController _tasksController;
        [Inject] private LocalizationController _localizationController;
        [Inject] private MainCharacterData _mainCharacterData;
        [Inject] private PointersController _pointersController;
        
        [SerializeField] protected string taskTextMask;
        [SerializeField] protected TextData[] taskTextMasks;
        [SerializeField] protected EnemiesManager enemiesManager;
        [SerializeField] protected MainCharacterController mainCharacterController;
        [SerializeField] private Transform graveyardPoint;
        [SerializeField] private Sprite graveyardSprite;
        
        public SoftValueCounter SoftValueCounter { get; private set; }
        
        private bool _isLevelFailed;

        protected string GetTaskTextMask()
        {
            for (var i = 0; i < taskTextMasks.Length; i++)
            {
                if (taskTextMasks[i].language == _localizationController.ChosenLanguage)
                {
                    return taskTextMasks[i].text;
                }
            }
            
            return string.Empty;
        }

        protected virtual void Awake()
        {
            _isLevelFailed = false;

            _pointersController.SetupPointer(graveyardPoint, graveyardSprite, graveyardSprite, false);
            
            if (enemiesManager != null)
            {
                enemiesManager.Initialize();
                enemiesManager.OnBossKilled += CheckEndGameAvailability;
                
                SoftValueCounter = new SoftValueCounter(enemiesManager);
            }
            
            mainCharacterController.OnKilled += MainCharacterKilled;
        }

        private void Start()
        {
            CheckTutorial();
        }

        protected virtual void CheckTutorial()
        {
            
        }

        protected virtual void OnDestroy()
        {
            if (enemiesManager != null)
                enemiesManager.OnBossKilled -= CheckEndGameAvailability;
            
            mainCharacterController.OnKilled -= MainCharacterKilled;
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                CompleteLevel();
            }
            
            if (Input.GetKeyUp(KeyCode.G))
            {
                FailLevel();
            }
        }
#endif
        protected abstract void CheckEndGameAvailability();
        
        private void MainCharacterKilled(IAttackable attackable)
        {
            FailLevel();
        }

        public abstract string GetLevelTaskText();
        
        protected virtual void CompleteLevel()
        {
            var levelName = SceneManager.GetActiveScene().name;
        }

        private void PerformDialogueEnded()
        {
            OnLevelCompleted?.Invoke();
        }

        public void UpdateLevelTask()
        {
            OnLevelTaskChanged?.Invoke();
        }

        protected virtual void FailLevel()
        {
            if (_isLevelFailed)
                return;

            _isLevelFailed = true;
            
            OnLevelFailed?.Invoke();
        }
    }
}