using UnityEngine;

namespace _Project.WorldClicking.Scripts
{
    public interface IInteractable
    {
        public void Interact();
        public Transform GetTransform();
    }
}