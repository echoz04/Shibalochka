using System;

namespace Sources.UI.Services.Fishing
{
    public class FishingBarrier
    {
        private int _taps;
        private int _currentTaps;

        private float _positionValue;
        public float PositionValue => _positionValue;
        public int Taps => _currentTaps;

        private Action<int> _onBarrierBreak;

        private int _index;
        public int Index => _index;
        
        public FishingBarrier(int index, float positionValue, int taps, Action<int> onBarrierBreak)
        {
            _index = index;
            _positionValue = positionValue;
            _taps = taps;
            _currentTaps = taps;
            _onBarrierBreak = onBarrierBreak;
        }

        public void DecreaseTap()
        {
            if (_currentTaps > 0)
            {
                _currentTaps--;
            }
            else
            {
                _onBarrierBreak?.Invoke(_index);
            }
        }
    }
}