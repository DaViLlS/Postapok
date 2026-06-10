using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Project.Main.NavigationUI
{
    public class NavigationPanel : MonoBehaviour
    {
        [SerializeField] private Vector2 hidePosition;
        [SerializeField] private Vector2 showPosition;
        [SerializeField] private GameObject arrows;
        [SerializeField] private List<NavigationButton> navigationButtons;
        
        private int _hiddenButtonsCount;
        
        public bool IsAnimating { get; private set; }

        public void Initialize()
        {
            IsAnimating = false;
            arrows.SetActive(false);
            
            foreach (var navigationButton in navigationButtons)
            {
                navigationButton.Initialize();
            }
        }

        public void Show()
        {
            if (IsAnimating)
                return;
            
            IsAnimating = true;
            
            transform.DOLocalMoveX(showPosition.x, 0.2f).OnComplete(() =>
            {
                var sequence = DOTween.Sequence();
                
                foreach (var navigationButton in navigationButtons)
                {
                    sequence.Append(navigationButton.Show(() =>
                    {
                        navigationButton.OnMouseEnterEvent += MoveArrows;
                    })).OnComplete(() =>
                    {
                        arrows.SetActive(true);
                        IsAnimating = false;
                    });
                } 
            });
        }

        public void Hide()
        {
            if (IsAnimating)
                return;
            
            IsAnimating = true;
            arrows.SetActive(false);
            var sequence = DOTween.Sequence();
            
            foreach (var navigationButton in navigationButtons)
            {
                navigationButton.OnMouseEnterEvent -= MoveArrows;
                sequence.Append(navigationButton.Hide()).OnComplete(() =>
                {
                    transform.DOLocalMoveX(hidePosition.x, 0.1f);
                    IsAnimating = false;
                });
            }
        }

        private void MoveArrows(NavigationButton navigationButton)
        {
            arrows.transform.position = navigationButton.transform.position;
        }
    }
}
