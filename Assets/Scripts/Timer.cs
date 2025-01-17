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
        /// Ÿ�̸Ӹ� �����մϴ�.
        /// </summary>
        /// <param name="duration">Ÿ�̸� ���� �ð�(��)</param>
        public void Start(float duration)
        {
            if (_isRunning)
            {
                Debug.LogWarning("Ÿ�̸Ӱ� �̹� ���� ���Դϴ�.");
                return;
            }

            if (duration <= 0)
            {
                Debug.LogError("���� �ð��� 0���� Ŀ�� �մϴ�.");
                return;
            }

            _duration = duration;
            _endTime = GetCurrentTime() + duration;
            _isRunning = true;
            Debug.Log($"Ÿ�̸� ����. {duration}�� �� ����˴ϴ�.");
        }

        /// <summary>
        /// Ÿ�̸Ӱ� �Ϸ�Ǿ����� Ȯ���մϴ�.
        /// </summary>
        /// <returns>Ÿ�̸� �Ϸ� ����</returns>
        public bool IsComplete()
        {
            if (!_isRunning)
            {
                Debug.LogWarning("Ÿ�̸Ӱ� ���� ������ �ʽ��ϴ�.");
                return false;
            }

            if (GetCurrentTime() >= _endTime)
            {
                _isRunning = false;
                Debug.Log("Ÿ�̸� �Ϸ�.");

                // �̺�Ʈ ����
                OnTimerComplete?.Invoke();

                return true;
            }

            return false;
        }

        /// <summary>
        /// ���� Ÿ�̸� �ð��� �����ɴϴ�.
        /// </summary>
        /// <returns>���� �ð�(��). Ÿ�̸Ӱ� ���� ������ ������ 0 ��ȯ.</returns>
        public float GetRemainingTime()
        {
            if (!_isRunning)
            {
                Debug.LogWarning("Ÿ�̸Ӱ� ���� ������ �ʽ��ϴ�.");
                return 0f;
            }

            float remainingTime = _endTime - GetCurrentTime();
            return remainingTime > 0 ? remainingTime : 0f;
        }

        /// <summary>
        /// ���� �ð��� �� ������ �����ɴϴ�.
        /// </summary>
        /// <returns>���� �ð�(��)</returns>
        private float GetCurrentTime()
        {
            return (float)DateTime.Now.Subtract(DateTime.MinValue.AddYears(1969)).TotalSeconds;
        }
    }
}