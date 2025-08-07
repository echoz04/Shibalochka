using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.Runtime.Gameplay.Configs;
using VContainer;
using VContainer.Unity;

namespace Sources.Runtime.Gameplay.Inventory
{
    public class InventoryView : MonoBehaviour, IInitializable
    {
        private InventoryRoot _root;
        private ProjectConfig _projectConfig;

        [Inject]
        private void Construct(ProjectConfig projectConfig, InventoryRoot root)
        {
            _projectConfig = projectConfig;
            _root = root;
        }

        void IInitializable.Initialize()
        {
            if (!_root)
                return;

            _root.OnBuildCells += BuildCells;
        }

        private void BuildCells(List<InventoryCell> cells) =>
            AnimateCellsAsync(cells).Forget();


        private async UniTaskVoid AnimateCellsAsync(List<InventoryCell> cells)
        {
            float animationDuration = _projectConfig.InventoryConfig.CellsSpawnAnimationDuration;
            float delay = _projectConfig.InventoryConfig.DelayBetweenCellsSpawnAnimation;

            foreach (var cell in cells)
            {
                cell.RectTransform.localScale = Vector3.zero;
                cell.RectTransform.localRotation = Quaternion.Euler(0, 0, 90);
            }

            foreach (var cell in cells)
            {
                cell.RectTransform
                    .DOScale(Vector3.one, animationDuration)
                    .SetEase(Ease.OutBack);

                cell.RectTransform
                    .DORotate(Vector3.zero, animationDuration)
                    .SetEase(Ease.OutCubic);

                await UniTask.Delay(
                    System.TimeSpan.FromMilliseconds(delay),
                    cancellationToken: this.GetCancellationTokenOnDestroy()
                );
            }
        }

        private void OnDestroy()
        {
            _root.OnBuildCells -= BuildCells;
        }
    }
}
