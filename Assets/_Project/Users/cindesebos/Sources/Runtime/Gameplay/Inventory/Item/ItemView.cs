using DG.Tweening;
using Sources.Runtime.Gameplay.Configs;
using Sources.Runtime.Gameplay.Configs.Items;
using Sources.Runtime.Services.ProjectConfigLoader;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Sources.Runtime.Gameplay.Inventory.Item
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private ItemRoot _root;
        [SerializeField] private Image _view;
        [SerializeField] private RectTransform _parent;
        [SerializeField] private Sprite _arrowSprite;
        [SerializeField] private Vector2 _arrrowSize = new Vector2(62f, 62f);
        [SerializeField] private float _offSet;

        private GameObject[] _currentArrows = new GameObject[4];
        private RectTransform _target;
        private ItemConfig _config;
        private InventoryConfig _inventoryConfig;

        private void OnValidate()
        {
            _root ??= GetComponent<ItemRoot>();
            _view ??= GetComponentInChildren<Image>();
        }

        [Inject]
        private void Construct(IProjectConfigLoader projectConfigLoader)
        {
            _inventoryConfig = projectConfigLoader.ProjectConfig.InventoryConfig;
        }

        private void Start()
        {
            _config = _root.Config;
            _target = GetComponent<RectTransform>();

            _view.sprite = _config.Icon;
            _view.alphaHitTestMinimumThreshold = 0.1f;
            _view.SetNativeSize();

            //CreateArrows();
            //UpdateArrowPositions();
            //ToggleArrows(false);

            _root.OnBeginDragging += OnDragBegan;
            _root.OnDragging += OnDragged;
            _root.OnEndDragging += OnDragEnded;
            _root.OnSelected += OnSelected;
        }

        private void OnDestroy()
        {
            if (_root == null) return;

            _root.OnBeginDragging -= OnDragBegan;
            _root.OnDragging -= OnDragged;
            _root.OnEndDragging -= OnDragEnded;
            _root.OnSelected -= OnSelected;
        }

        public void OnDragBegan()
        {
            _target.DOKill();
            _target.DOScale(_inventoryConfig.ItemDraggingScale, _inventoryConfig.ItemAnimationsDuration);
        }

        public void OnDragged()
        {
        }

        public void OnDragEnded()
        {
            _target.DOKill();
            _target.DOScale(1f, _inventoryConfig.ItemAnimationsDuration);
        }

        public void OnSelected(bool value)
        {
            //ToggleArrows(value);
        }

        private void ToggleArrows(bool value)
        {
            foreach (var arrow in _currentArrows)
                arrow.SetActive(value);
        }

        private void CreateArrows()
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject arrow = new GameObject("Arrow_" + i, typeof(Image));
                arrow.transform.SetParent(_parent, false);

                Image instance = arrow.GetComponent<Image>();
                instance.sprite = _arrowSprite;
                instance.rectTransform.sizeDelta = _arrrowSize;

                float angle = i switch
                {
                    0 => 90f,
                    1 => 0f,
                    2 => 180f,
                    3 => -90f,
                    _ => 0f
                };

                instance.rectTransform.localRotation = Quaternion.Euler(0, 0, angle);

                _currentArrows[i] = arrow;
            }
        }

        private void UpdateArrowPositions()
        {
            Vector3[] corners = new Vector3[4];
            _target.GetWorldCorners(corners);

            Vector3 topLeft = corners[1];
            Vector3 topRight = corners[2];
            Vector3 bottomRight = corners[3];
            Vector3 bottomLeft = corners[0];

            Vector3 localTL = transform.InverseTransformPoint(topLeft);
            Vector3 localTR = transform.InverseTransformPoint(topRight);
            Vector3 localBR = transform.InverseTransformPoint(bottomRight);
            Vector3 localBL = transform.InverseTransformPoint(bottomLeft);

            _currentArrows[0].transform.localPosition = localTL + new Vector3(-_offSet, _offSet, 0);
            _currentArrows[1].transform.localPosition = localTR + new Vector3(_offSet, _offSet, 0);
            _currentArrows[2].transform.localPosition = localBL + new Vector3(-_offSet, -_offSet, 0);
            _currentArrows[3].transform.localPosition = localBR + new Vector3(_offSet, -_offSet, 0);
        }
    }
}
