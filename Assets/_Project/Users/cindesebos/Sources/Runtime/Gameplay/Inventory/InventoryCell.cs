using UnityEngine;
using UnityEngine.UI;

namespace Sources.Runtime.Gameplay.Inventory
{
    public class InventoryCell : MonoBehaviour
    {
        [field: SerializeField] public bool IsOccupied { get; private set; }
        [field: SerializeField] public RectTransform RectTransform { get; private set; }

        public Vector2Int Position => new Vector2Int(_x, _y);

        [SerializeField] private Image _image;

        private int _x;
        private int _y;
        private InventoryRoot _inventory;

        private Color _defaultColor;
        private Color _validColor = new Color(0f, 1f, 0f, 0.4f);
        private Color _invalidColor = new Color(1f, 0f, 0f, 0.4f);

        private void OnValidate()
        {
            RectTransform ??= GetComponent<RectTransform>();
            _image ??= GetComponent<Image>();
        }

        public void Initialize(int indexByX, int indexByY, float cellSize, InventoryRoot inventory)
        {
            _defaultColor = _image.color;

            _x = indexByX;
            _y = indexByY;
            _inventory = inventory;

            transform.localScale = Vector3.one;
        }

        public void SetOccupied(bool state)
        {
            IsOccupied = state;

            if (IsOccupied == false)
                ClearHighlight();
        }

        public void HighlightValid()
        {
            _image.color = _validColor;
        }

        public void HighlightInvalid()
        {
            _image.color = _invalidColor;
        }

        public void ClearHighlight()
        {
            _image.color = _defaultColor;
        }
    }
}
