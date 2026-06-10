using UnityEngine;

namespace _Project.TasksAndDialogues.Dialogues.Scripts
{
    [CreateAssetMenu(fileName = "DialogueConfig", menuName = "Dialogues/Dialogue Config")]
    public class DialogueConfig : ScriptableObject
    {
        [SerializeField] private DialogueData[] dialogues;
        
        public DialogueData[] Dialogues => dialogues;
        
        public DialogueData GetDialogue(int dialogueIndex) => dialogues[dialogueIndex];
        public bool IsEnd(int dialogueIndex) => dialogueIndex > dialogues.Length - 1;
    }
}