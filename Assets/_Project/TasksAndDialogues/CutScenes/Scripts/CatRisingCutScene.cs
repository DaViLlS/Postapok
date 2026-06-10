using _Project.MainCharacter.Scripts;
using UnityEngine;

namespace _Project.TasksAndDialogues.CutScenes.Scripts
{
    public class CatRisingCutScene : MonoBehaviour
    {
        [SerializeField] private MainCharacterController character;

        [SerializeField] private Transform firstTargetTransform;
        [SerializeField] private Transform secondTargetTransform;
    }
}