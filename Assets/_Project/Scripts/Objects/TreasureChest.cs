using System.Collections;
using Managers;
using Player;
using Tools;
using UnityEngine;
using Zenject;

namespace Objects
{
    public class TreasureChest : Interactable
    {
        private const string OPEN_STATE = "opened";
        private static readonly int Opened = Animator.StringToHash(OPEN_STATE);

        [Inject] protected WindowManager WindowManager;
        [Inject] private PlayerInteraction _playerInteraction;
        [SerializeField] private InventoryItem[] _items;

        private Animator _animator;
        private readonly float _openDuration = 1f;
        private bool _opened;

        private void OnEnable()
        {
            if (_opened)
                OpenAnimation();
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private IEnumerator OpenChest()
        {
            OpenAnimation();
            _opened = true;
            Context.Raise();
            yield return new WaitForSeconds(_openDuration);
            Context.Raise();

            var dialogWindow = WindowManager.ShowDialogBox();
            foreach (var item in _items)
            {
                dialogWindow.Text.SetText(item.Description);
                _playerInteraction.DisplayPickupItem(item);
                yield return new WaitUntil(() => Input.anyKey);
            }

            WindowManager.CloseDialogBox();
            _playerInteraction.RemovePickupItem();
        }

        private void OpenAnimation() => _animator.SetBool(Opened, true);

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (_opened)
                return;

            if (InsightUtils.IsItPlayer(other) == false)
                return;

            StartCoroutine(OpenChest());
        }

        protected override void OnTriggerExit2D(Collider2D other)
        {
            if (InsightUtils.IsItPlayer(other) == false)
                return;

            PlayerInRange = false;
        }
    }
}