using System;
using UnityEngine;

namespace Util
{
    public class Timer
    {
        private float _duration;
        private float _endTime;
        private bool _isRunning;
        public Action OnTimerComplete;

        /// <summary>
        /// 타이머를 시작합니다.
        /// </summary>
        /// <param name="duration">타이머 지속 시간(초)</param>
        public void Start(float duration)
        {
            if (_isRunning)
            {
                Debug.LogWarning("타이머가 이미 실행 중입니다.");
                return;
            }

            if (duration <= 0)
            {
                Debug.LogError("지속 시간은 0보다 커야 합니다.");
                return;
            }

            _duration = duration;
            _endTime = GetCurrentTime() + duration;
            _isRunning = true;
            Debug.Log($"타이머 시작. {duration}초 후 종료됩니다.");
        }

        /// <summary>
        /// 타이머가 완료되었는지 확인합니다.
        /// </summary>
        /// <returns>타이머 완료 여부</returns>
        public bool IsComplete()
        {
            if (!_isRunning)
            {
                Debug.LogWarning("타이머가 실행 중이지 않습니다.");
                return false;
            }

            if (GetCurrentTime() >= _endTime)
            {
                _isRunning = false;
                Debug.Log("타이머 완료.");

                // 이벤트 실행
                OnTimerComplete?.Invoke();

                return true;
            }

            return false;
        }

        /// <summary>
        /// 남은 타이머 시간을 가져옵니다.
        /// </summary>
        /// <returns>남은 시간(초). 타이머가 실행 중이지 않으면 0 반환.</returns>
        public float GetRemainingTime()
        {
            if (!_isRunning)
            {
                Debug.LogWarning("타이머가 실행 중이지 않습니다.");
                return 0f;
            }

            float remainingTime = _endTime - GetCurrentTime();
            return remainingTime > 0 ? remainingTime : 0f;
        }

        /// <summary>
        /// 현재 시간을 초 단위로 가져옵니다.
        /// </summary>
        /// <returns>현재 시간(초)</returns>
        private float GetCurrentTime()
        {
            return (float)DateTime.Now.Subtract(DateTime.MinValue.AddYears(1969)).TotalSeconds;
        }
    }
}