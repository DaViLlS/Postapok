using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Main.NavigationUI
{
    public class NavigationButton : MonoBehaviour, IPointerEnterHandler
    {
        public event Action<NavigationButton> OnMouseEnterEvent;

        [SerializeField] private TMP_Text buttonText;
        [SerializeField] private RectTransform buttonRect;
        [SerializeField] private Vector2 targetScale;

        private Sequence _currentSequence;

        public void Initialize()
        {
            buttonRect.localScale = new Vector2(0, 1);
            buttonText.gameObject.SetActive(false);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            OnMouseEnterEvent?.Invoke(this);
        }

        public Tween Show(Action onFinish = null)
        {
            return buttonRect.DOScale(targetScale, 0.5f).OnComplete(() =>
            {
                buttonText.gameObject.SetActive(true);
                onFinish?.Invoke();
            });
        }

        public Tween Hide(Action onFinish = null)
        {
            return buttonRect.DOScale(new Vector2(0, 1), 0.5f).OnComplete(() =>
            {
                buttonText.gameObject.SetActive(false);
                onFinish?.Invoke();
            });
        }
    }
}
