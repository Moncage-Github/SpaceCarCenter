using System;
using UnityEngine;

namespace Util
{
    public class Stopwatch
    {
        private float _startTime;
        private float _pausedTime;
        private bool _isRunning;
        private bool _isPaused;

        /// <summary>
        /// 스톱워치를 시작합니다.
        /// </summary>
        public void Start()
        {
            if (_isRunning)
            {
                Debug.LogWarning("스톱워치가 이미 실행 중입니다.");
                return;
            }

            _startTime = GetCurrentTime();
            _isRunning = true;
            _isPaused = false;
            _pausedTime = 0f;
            Debug.Log("스톱워치 시작.");
        }

        /// <summary>
        /// 스톱워치를 종료하고 경과 시간을 리턴합니다.
        /// </summary>
        /// <returns>시작부터 종료까지의 경과 시간(초)</returns>
        public float Stop()
        {
            if (!_isRunning)
            {
                Debug.LogWarning("스톱워치가 실행 중이지 않습니다.");
                return 0f;
            }

            float elapsedTime;
            if (_isPaused)
            {
                elapsedTime = _pausedTime;
            }
            else
            {
                elapsedTime = GetCurrentTime() - _startTime;
                elapsedTime = Mathf.Max(0f, elapsedTime); // Prevent negative elapsed time due to clock precision issues
            }

            _isRunning = false;
            _isPaused = false;

            Debug.Log($"스톱워치 종료. 경과 시간: {elapsedTime}초");
            return elapsedTime;
        }

        /// <summary>
        /// 스톱워치를 일시 정지합니다.
        /// </summary>
        public void Pause()
        {
            if (!_isRunning || _isPaused)
            {
                Debug.LogWarning("스톱워치가 실행 중이지 않거나 이미 일시 정지 상태입니다.");
                return;
            }

            _pausedTime = GetCurrentTime() - _startTime;
            _isPaused = true;
            Debug.Log("스톱워치 일시 정지.");
        }

        /// <summary>
        /// 일시 정지된 스톱워치를 재개합니다.
        /// </summary>
        public void Resume()
        {
            if (!_isRunning || !_isPaused)
            {
                Debug.LogWarning("스톱워치가 실행 중이 아니거나 일시 정지 상태가 아닙니다.");
                return;
            }

            _startTime = GetCurrentTime() - _pausedTime;
            _isPaused = false;
            Debug.Log("스톱워치 재개.");
        }

        /// <summary>
        /// 스톱워치가 실행 중인지 확인합니다.
        /// </summary>
        /// <returns>스톱워치 실행 여부</returns>
        public bool IsRunning()
        {
            return _isRunning;
        }

        /// <summary>
        /// 스톱워치가 일시 정지 상태인지 확인합니다.
        /// </summary>
        /// <returns>스톱워치 일시 정지 여부</returns>
        public bool IsPaused()
        {
            return _isPaused;
        }

        /// <summary>
        /// 현재 흐른 시간을 초 단위로 반환합니다.
        /// </summary>
        /// <returns>현재까지의 경과 시간(초)</returns>
        public float GetElapsedTime()
        {
            if (!_isRunning)
            {
                Debug.LogWarning("스톱워치가 실행 중이지 않습니다.");
                return 0f;
            }

            if (_isPaused)
            {
                return _pausedTime;
            }

            return GetCurrentTime() - _startTime;
        }

        /// <summary>
        /// 현재 시간을 초 단위로 가져옵니다.
        /// </summary>
        /// <returns>현재 시간(초)</returns>
        private float GetCurrentTime()
        {
            return Time.time;
        }

        /// <summary>
        /// 초를 입력받아 "분:초" 형식의 문자열로 변환하는 함수
        /// </summary>
        /// <returns>"분:초 문자열"</returns>
        public static System.Func<int, string> FormatTimeToMinutesAndSeconds = (totalSeconds) =>
        {
            int minutes = totalSeconds / 60; // 분 계산
            int seconds = totalSeconds % 60; // 초 계산

            // "분:초" 형식의 문자열로 반환
            return string.Format("{0}:{1:D2}", minutes, seconds);
        };
    }
}