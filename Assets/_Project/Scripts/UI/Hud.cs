using Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class Hud : MonoBehaviour
    {
        [Inject] private WindowManager _windowManager;
        [Inject] private UIAudioPlayer _audioPlayer;

        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _questLogButton;
        [SerializeField] private Button _inventoryButton;

        private void Awake()
        {
            _inventoryButton.onClick.AddListener(OpenInventory);
            _questLogButton.onClick.AddListener(OpenQuestLog);
            _settingsButton.onClick.AddListener(OpenPause);
        }

        private void OpenInventory()
        {
            _audioPlayer.PlayButtonSound();
            _windowManager.ShowInventoryWindow();
        }

        private void OpenQuestLog()
        {
            _audioPlayer.PlayButtonSound();
            _windowManager.ShowQuestLogWindow();
        }

        private void OpenPause()
        {
            _audioPlayer.PlayButtonSound();
            _windowManager.ShowPauseWindow();
        }

        private void SwitchView(bool state)
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(state);
        }

        public void DisableView() => SwitchView(false);

        public void EnableView() => SwitchView(true);
    }
}