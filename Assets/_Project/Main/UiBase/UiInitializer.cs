using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Main.UiBase
{
    public class UiInitializer : MonoBehaviour
    {
        [SerializeField] private Image darkImage;
        [SerializeField] private List<GameScreen> gameScreens = new List<GameScreen>();
        [SerializeField] private List<GamePopup> gamePopups = new List<GamePopup>();

        private void Start()
        {
            darkImage.DOFade(0f, 2f).OnComplete(Initialize);
        }

        private void Initialize()
        {
            darkImage.gameObject.SetActive(false);
            
            foreach (var gameScreen in gameScreens)
            {
                gameScreen.Initialize();
            }
            
            foreach (var gamePopup in gamePopups)
            {
                gamePopup.Initialize();
            }
        }

        private void OnDestroy()
        {
            foreach (var gameScreen in gameScreens)
            {
                gameScreen.Dispose();
            }
        }
    }
}