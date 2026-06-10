using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Tutorial.Scripts
{
    public class EntrancePlot : MonoBehaviour
    {
        public event Action OnEntranceEnded;

        [SerializeField] private Image background;
        [SerializeField] private TMP_Text[] plotTexts;
        [SerializeField] private bool isPermanentlyOpened;
        [SerializeField] private bool needToHide;
        [SerializeField] private TMP_Text skipText;

        private bool _isActive;

        private void Start()
        {
            if (isPermanentlyOpened)
            {
                _isActive = true;
                StartCoroutine(WaitBeforeNewText());
            }
            else
            {
                background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
                gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (!_isActive)
                return;
            
            if (Input.anyKeyDown)
            {
                skipText.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isActive = false;
                CompleteEntrance();
            }
        }

        public void Show()
        {
            _isActive = true;
            gameObject.SetActive(true);
            background.DOColor(new Color(background.color.r, background.color.g, background.color.b, 1f), 1f).OnComplete(
                () =>
                {
                    StartCoroutine(WaitBeforeNewText());
                });
        }
        
        private IEnumerator WaitBeforeNewText()
        {
            for (var i = 0; i < plotTexts.Length; i++)
            {
                plotTexts[i].DOColor(new Color(plotTexts[i].color.r, plotTexts[i].color.g, plotTexts[i].color.b, 1f), 0.6f);

                yield return new WaitForSeconds(2f);
            }

            yield return new WaitForSeconds(2f);

            CompleteEntrance();
        }

        private void CompleteEntrance()
        {
            for (var i = 0; i < plotTexts.Length; i++)
            {
                plotTexts[i].DOColor(new Color(plotTexts[i].color.r, plotTexts[i].color.g, plotTexts[i].color.b, 0f), 1f);
            }
            
            _isActive = false;
            
            if (needToHide)
            {
                OnEntranceEnded?.Invoke();
                background.DOColor(new Color(background.color.r, background.color.g, background.color.b, 0f), 1f);
            }
            else
            {
                OnEntranceEnded?.Invoke();
            }
        }
    }
}