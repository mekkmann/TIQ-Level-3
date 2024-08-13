using UnityEngine;

namespace MonsterQuest
{
    public class ConsolePanelPresenter : MonoBehaviour
    {
        [SerializeField] private float normalHeight;
        [SerializeField] private float fullHeight;

        private bool _isFullHeight;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            SetHeight(normalHeight);
        }

        private void SetHeight(float height)
        {
            Vector2 sizeDelta = _rectTransform.sizeDelta;
            sizeDelta.y = height;
            _rectTransform.sizeDelta = sizeDelta;
        }

        public void OnPointerDown()
        {
            _isFullHeight = !_isFullHeight;
            SetHeight(_isFullHeight ? fullHeight : normalHeight);
        }
    }
}
