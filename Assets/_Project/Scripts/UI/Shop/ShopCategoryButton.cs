using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    public class ShopCategoryButton : MonoBehaviour
    {
        public Action Click;

        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _categoryTitle;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _unselectedColor;
        [SerializeField] private Image _focus;

        private void OnEnable() => _button.onClick.AddListener(OnClick);

        private void OnDisable() => _button.onClick.RemoveListener(OnClick);

        public void Select()
        {
            _categoryTitle.color = _selectedColor;

            var button = _button.GetComponent<RectTransform>().rect;
            _focus.transform.localPosition =
                new Vector2(_button.transform.localPosition.x, button.center.y - 1.5f * button.height);
        }

        public void Unselect() => _categoryTitle.color = _unselectedColor;

        private void OnClick() => Click?.Invoke();
    }
}