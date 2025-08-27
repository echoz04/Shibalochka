using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.Runtime.Gameplay.Configs.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Runtime.Gameplay.Inventory.Item.Configurer
{
    public class ItemCellsDrawer : MonoBehaviour
    {
        [SerializeField, Required] private ItemConfig _config;
        [SerializeField, Required] private Transform _parent;
        [SerializeField] private Image _image;

        [Button("Initialize")]
        private void Initialize()
        {
            if (!_image)
                return;

            _image.sprite = _config.Icon;
            _image.SetNativeSize();

            DrawCells();
        }

        private void DrawCells()
        {
            ClearOldCells();

            foreach (var localPos in _config.CellPointsPosition)
            {
                var cell = Instantiate(_config.ItemCellPointPrefab, _parent);
                cell.transform.localPosition = localPos;
                cell.name = "CellPoint";
            }
        }

        [Button("Clear Cells")]
        private void ClearOldCells()
        {
            var children = new List<Transform>();
            foreach (Transform child in _parent)
                children.Add(child);

            foreach (var child in children)
            {
#if UNITY_EDITOR
                if (Application.isEditor && !Application.isPlaying)
                    DestroyImmediate(child.gameObject);
                else
#endif
                    Destroy(child.gameObject);
            }
        }
    }
}