using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.Runtime.Gameplay.Inventory.Items;
using UnityEngine;
using System;

namespace Sources.Runtime.Gameplay.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        private const float _cellsCreateAnimationDuration = 0.35f;
        private const float _cellsDelayBetweenAnimations = 0.5f;
        
        private InventoryRoot _root;
        
        public void Initialize(InventoryRoot root)
        {
            _root = root;

            _root.OnCellsCreated += ShowCellsCreateAnimation;
            _root.OnItemAdded += SetupItem;
            _root.OnItemRemoved += ClearItemParent;
        }

        private void OnDestroy()
        {
            _root.OnCellsCreated += ShowCellsCreateAnimation;
            _root.OnItemAdded -= SetupItem;
            _root.OnItemRemoved -= ClearItemParent;
        }
        
        private void ShowCellsCreateAnimation(InventoryCell[,] cells) =>
            ShowCellsCreateAnimationAsync(cells).Forget();
        
        private async UniTask ShowCellsCreateAnimationAsync(InventoryCell[,] cells)
        {
            foreach (var cell in cells)
            {
                cell.ToggleImage(transform);

                cell.transform.localScale = Vector3.zero;
                cell.transform.localRotation = Quaternion.Euler(0, 0, 90);

                cell.transform
                    .DOScale(Vector3.one, _cellsCreateAnimationDuration)
                    .SetEase(Ease.OutBack);

                cell.transform
                    .DORotate(Vector3.zero, _cellsCreateAnimationDuration)
                    .SetEase(Ease.OutCubic);

                await UniTask.Delay(
                    TimeSpan.FromMilliseconds(_cellsDelayBetweenAnimations),
                    cancellationToken: this.GetCancellationTokenOnDestroy()
                );
            }
        }

        private void SetupItem(ItemViewRoot itemViewRoot, InventoryCell cell)
        {
            itemViewRoot.transform.SetParent(cell.transform, false);
            itemViewRoot.transform.localPosition = Vector3.zero;
        }

        private void ClearItemParent(ItemViewRoot itemViewRoot)
        {
            itemViewRoot.transform.SetParent(null);
        }
    }
}