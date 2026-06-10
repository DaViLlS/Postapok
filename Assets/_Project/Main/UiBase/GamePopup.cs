using UnityEngine;

namespace _Project.Main.UiBase
{
    public abstract class GamePopup : MonoBehaviour
    {
        public abstract void Initialize();
        
        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
    }
}