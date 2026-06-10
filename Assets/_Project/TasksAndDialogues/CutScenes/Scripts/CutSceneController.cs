using System;
using _Project.MainCharacter.Scripts;
using _Project.TasksAndDialogues.Dialogues.Scripts;
using UnityEngine;
using UnityEngine.Playables;

namespace _Project.TasksAndDialogues.CutScenes.Scripts
{
    public class CutSceneController : MonoBehaviour
    {
        public event Action OnCutSceneFinished;
        
        [SerializeField] private PlayableDirector director;
        [SerializeField] private MainCharacterController mainCharacter;
        [SerializeField] private Rigidbody2D blackCatRb;
        [SerializeField] private Transform startPosition;
        [SerializeField] private DialoguesController dialoguesController;
        [SerializeField] private DialogueConfig[] dialogues;

        private bool _isCharacterMoving = false;
        private bool _isCharacterRunning = false;
        private bool _blackCatMoving = false;
        private Vector2 _direction;
        private Vector2 _catDirection;

        private int _currentDialogueIndex;
        
        public void LaunchCutScene()
        {
            mainCharacter.transform.position = startPosition.position;
            
            director.Play();
        }

        public void FinishCutScene()
        {
            OnCutSceneFinished?.Invoke();
        }

        private void FixedUpdate()
        {
            if (_isCharacterMoving)
            {
                mainCharacter.Rb.linearVelocity = new Vector2(_direction.x, _direction.y) * 4f;
            }
            
            if (_isCharacterRunning)
            {
                mainCharacter.Rb.linearVelocity = new Vector2(_direction.x, _direction.y) * 6f;
            }

            if (_blackCatMoving)
            {
                blackCatRb.linearVelocity = new Vector2(_catDirection.x, _catDirection.y) * 3f;
            }
        }

        public void StartDialogue()
        {
            director.Pause();
            dialoguesController.OnDialogueCompleted += ContinueCutScene;
            dialoguesController.StartDialogue(dialogues[_currentDialogueIndex]);
        }

        private void ContinueCutScene()
        {
            _currentDialogueIndex++;
            dialoguesController.OnDialogueCompleted -= ContinueCutScene;
            director.Play();
        }

        public void StopCharacter()
        {
            _isCharacterMoving = false;
            mainCharacter.Rb.linearVelocity = Vector2.zero;
            _direction = Vector2.zero;
        }

        public void StopRunning()
        {
            _isCharacterRunning = false;
            mainCharacter.Rb.linearVelocity = Vector2.zero;
            _direction = Vector2.zero;
        }

        public void MoveCharacterToLeft()
        {
            _direction = Vector2.left;
            _isCharacterMoving = true;
        }

        public void MoveCharacterToRight()
        {
            _direction = Vector2.right;
            _isCharacterMoving = true;
        }

        public void MoveCharacterToTop()
        {
            _direction = Vector2.up;
            _isCharacterMoving = true;
        }

        public void MoveCharacterToBottom()
        {
            _direction = Vector2.down;
            _isCharacterMoving = true;
        }
        
        public void RunCharacterToLeft()
        {
            _direction = Vector2.left;
            _isCharacterRunning = true;
        }

        public void RunCharacterToRight()
        {
            _direction = Vector2.right;
            _isCharacterRunning = true;
        }

        public void RunCharacterToTop()
        {
            _direction = Vector2.up;
            _isCharacterRunning = true;
        }

        public void RunCharacterToBottom()
        {
            _direction = Vector2.down;
            _isCharacterRunning = true;
        }

        public void MoveCatLeft()
        {
            _catDirection = Vector2.left;
            _blackCatMoving = true;
        }

        public void StopCat()
        {
            _blackCatMoving = false;
            blackCatRb.linearVelocity = Vector2.zero;
            _catDirection = Vector2.zero;
        }
    }
}