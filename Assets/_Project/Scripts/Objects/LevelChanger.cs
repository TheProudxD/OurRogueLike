using System;
using System.Collections;
using Storage;
using Player;
using StorageService;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Objects
{
    public class LevelChanger : MonoBehaviour
    {
        [Inject] private LevelManager _levelManager;
        [Inject] private Hud _hud;
        private const string Fade = "Fade";

        private Animator _fadeAnimator;
        private Image _loadingBar;

        private void Awake()
        {
            _fadeAnimator = _hud.FadeAnimator;
            _loadingBar = _hud.LoadingBar;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerAttacking player))
                StartTransition();
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerAttacking player))
                StopTransition();
        }

        private void StopTransition()
        {
            StopAllCoroutines();
            _loadingBar.fillAmount = 0f;
        }

        private void StartTransition() => StartCoroutine(LoadingTimer());

        private IEnumerator LoadingTimer()
        {
            while (_loadingBar.fillAmount < 1f)
            {
                _loadingBar.fillAmount += 0.1f;
                yield return new WaitForSecondsRealtime(0.15f);
            }
            _loadingBar.fillAmount = 0f;
            
            
            _fadeAnimator.SetTrigger(Fade);
            yield return new WaitForSecondsRealtime(1f);
            _levelManager.StartNextLevel();
            _fadeAnimator.SetTrigger(Fade);
        }
    }
}