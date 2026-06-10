using System.Collections;
using UnityEngine;

namespace _Project.Tutorial.Scripts
{
    public class FightImitations : MonoBehaviour
    {
        private Transform[] _fightImitations;
        
        private void Awake()
        {
            _fightImitations = GetComponentsInChildren<Transform>();

            for (var i = 1; i < _fightImitations.Length; i++)
            {
                _fightImitations[i].gameObject.SetActive(false);
            }

            StartCoroutine(EnableWIthCooldown());
        }

        private IEnumerator EnableWIthCooldown()
        {
            for (var i = 1; i < _fightImitations.Length; i++)
            {
                _fightImitations[i].gameObject.SetActive(true);
                
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}