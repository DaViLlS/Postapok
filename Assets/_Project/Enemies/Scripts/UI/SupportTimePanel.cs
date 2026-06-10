using System.Collections;
using TMPro;
using UnityEngine;

namespace _Project.Enemies.Scripts.UI
{
    public class SupportTimePanel : MonoBehaviour
    {
        [SerializeField] private EnemiesManager enemiesManager;
        [SerializeField] private TMP_Text timerText;

        private void Awake()
        {
            gameObject.SetActive(false);
            
            if (enemiesManager == null)
                return;
            
            enemiesManager.OnSupportStarted += EnablePanel;
            enemiesManager.OnTimerChanged += StartCountingTimer;
            enemiesManager.OnSupportEnded += DisablePanel;
        }

        private void OnDestroy()
        {
            if (enemiesManager == null)
                return;
            
            enemiesManager.OnSupportStarted -= EnablePanel;
            enemiesManager.OnTimerChanged -= StartCountingTimer;
            enemiesManager.OnSupportEnded -= DisablePanel;
        }

        private void EnablePanel()
        {
            gameObject.SetActive(true);
        }
        
        private void StartCountingTimer(float timeInSeconds)
        {
            var minutes = Mathf.FloorToInt(timeInSeconds / 60f);
            var seconds = Mathf.FloorToInt(timeInSeconds % 60f);
            
            timerText.text = $"{minutes:00}:{seconds:00}";
            
            StopAllCoroutines();

            StartCoroutine(CountTime(timeInSeconds));
        }

        private IEnumerator CountTime(float timeInSeconds)
        {
            while (timeInSeconds > 0)
            {
                yield return new WaitForEndOfFrame();
                
                timeInSeconds -= Time.deltaTime;
                
                var minutes = Mathf.FloorToInt(timeInSeconds / 60f);
                var seconds = Mathf.FloorToInt(timeInSeconds % 60f);
            
                timerText.text = $"{minutes:00}:{seconds:00}";

                yield return null;
            }
        }

        private void DisablePanel()
        {
            gameObject.SetActive(false);
        }
    }
}