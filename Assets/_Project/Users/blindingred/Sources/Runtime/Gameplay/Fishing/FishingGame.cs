
using System;
using System.Collections.Generic;
using Sources.Signals;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Sources.UI.Services.Fishing
{
    public class FishingGame: IInitializable, ITickable
    {
        private enum FishingState
        {
            Startup,
            InProcess,
            HittingBarrier,
            WaitingFishRage,
            End
        }

        private ISignalBus _signalBus;
        private FishingData _fishingData;
        
        private FishingState _currentState;
        private bool _isStarted;
        private bool _isPlayerPulling;
        private bool _isFishCalm;
        private bool _isRunning;
        
        private float _playerValue;
        private float _failValue;

        public FishingData FishingData => _fishingData;
        public float PlayerValue => _playerValue;
        public float FailValue => _failValue;

        private List<FishingBarrier> _barriers;

        private FishingBarrier _currentBarrier;

        public List<FishingBarrier> Barriers => _barriers;
        
        [Inject]
        private void Construct(ISignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        void IInitializable.Initialize()
        {
            _currentState = FishingState.Startup;
        }

        public void StartGame(FishingData fishingData, Action onComplete = null)
        {
            _fishingData = fishingData;
            _barriers = new List<FishingBarrier>();
            
            _playerValue = 0.05f;
            _failValue = 0f;

            for (var index = 0; index < fishingData.BarriersData.Length; index++)
            {
                var barrierData = fishingData.BarriersData[index];
                var positionValue = Random.Range(barrierData.MinBarrierPosition, barrierData.MaxBarrierPosition);
                var tapsCount = Random.Range(barrierData.MinBarrierTaps, barrierData.MaxBarrierTaps);
                _barriers.Add(new FishingBarrier(index, positionValue, tapsCount, OnBarrierBreak));
            }

            _currentBarrier = _barriers[0];
            
            _signalBus.Fire(new ScreenChangeSignal(typeof(FishingScreen)));
            _currentState = FishingState.InProcess;
            _isRunning = true;
            
            onComplete?.Invoke();
        }

        public void SetPullingFlag(bool flag)
        {
            _isPlayerPulling = flag;
        }
        
        void ITickable.Tick()
        {
            if (!_isRunning) return;
            
            switch (_currentState)
            {
                case FishingState.Startup:
                    
                    break;
                case FishingState.InProcess:
                    
                    if (_isPlayerPulling)
                    {
                        _playerValue += 0.1f * Time.deltaTime;
                    }

                    if (_currentBarrier != null && _playerValue >= _currentBarrier.PositionValue)
                    {
                        Debug.Log($"Switching to Hitting Barrier state. Player: {_playerValue}, Barrier #{_currentBarrier.Index}: {_currentBarrier.PositionValue}");
                        _currentState = FishingState.HittingBarrier;
                    }
        
                    _failValue += 0.02f * Time.deltaTime;
                    break;
                case FishingState.HittingBarrier:
                    
                    _failValue += 0.02f * Time.deltaTime;
                    break;
                case FishingState.WaitingFishRage:
                    
                    break;
                case FishingState.End:
                    _isRunning = false;
                    _signalBus.Fire(new ScreenChangeSignal(typeof(MainScreen)));
                    break;
            }
            
            if (_playerValue >= 1f)
            {
                Debug.Log("WIN");
                _currentState = FishingState.End;
            }
            
            if (_failValue > _playerValue)
            {
               Debug.Log("FAIL");
               _currentState = FishingState.End;
            }
        }

        public void SmashBarrier()
        {
            if (_currentState == FishingState.HittingBarrier)
            {
                Debug.Log($"SmashBarrier #{_currentBarrier.Index}");
                _currentBarrier.DecreaseTap();
            }
            else
            {
                Debug.Log("SmashBarrier fail");
            }
        } 

        void OnBarrierBreak(int index)
        {
            if (index + 1 >= _barriers.Count)
            {
                Debug.Log($"All barriers has been broken");
                _currentBarrier = null;
            }
            Debug.Log($"Barrier break {index}");
            index += 1;
            _currentBarrier = _barriers[index];
            _currentState = FishingState.InProcess;
        }
    }
}