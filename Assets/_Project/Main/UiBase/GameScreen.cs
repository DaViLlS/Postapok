using UnityEngine;

namespace _Project.Main.UiBase
{
    public abstract class GameScreen : MonoBehaviour
    {
        public abstract void Initialize();
        public abstract void Dispose();
        
        public bool IsOpened { get; protected set; }

        public virtual void Open()
        {
            gameObject.SetActive(true);
            IsOpened = true;
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
            IsOpened = false;
        }
    }
}