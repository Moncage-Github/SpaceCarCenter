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
        /// �����ġ�� �����մϴ�.
        /// </summary>
        public void Start()
        {
            if (_isRunning)
            {
                Debug.LogWarning("�����ġ�� �̹� ���� ���Դϴ�.");
                return;
            }

            _startTime = GetCurrentTime();
            _isRunning = true;
            _isPaused = false;
            _pausedTime = 0f;
            Debug.Log("�����ġ ����.");
        }

        /// <summary>
        /// �����ġ�� �����ϰ� ��� �ð��� �����մϴ�.
        /// </summary>
        /// <returns>���ۺ��� ��������� ��� �ð�(��)</returns>
        public float Stop()
        {
            if (!_isRunning)
            {
                Debug.LogWarning("�����ġ�� ���� ������ �ʽ��ϴ�.");
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

            Debug.Log($"�����ġ ����. ��� �ð�: {elapsedTime}��");
            return elapsedTime;
        }

        /// <summary>
        /// �����ġ�� �Ͻ� �����մϴ�.
        /// </summary>
        public void Pause()
        {
            if (!_isRunning || _isPaused)
            {
                Debug.LogWarning("�����ġ�� ���� ������ �ʰų� �̹� �Ͻ� ���� �����Դϴ�.");
                return;
            }

            _pausedTime = GetCurrentTime() - _startTime;
            _isPaused = true;
            Debug.Log("�����ġ �Ͻ� ����.");
        }

        /// <summary>
        /// �Ͻ� ������ �����ġ�� �簳�մϴ�.
        /// </summary>
        public void Resume()
        {
            if (!_isRunning || !_isPaused)
            {
                Debug.LogWarning("�����ġ�� ���� ���� �ƴϰų� �Ͻ� ���� ���°� �ƴմϴ�.");
                return;
            }

            _startTime = GetCurrentTime() - _pausedTime;
            _isPaused = false;
            Debug.Log("�����ġ �簳.");
        }

        /// <summary>
        /// �����ġ�� ���� ������ Ȯ���մϴ�.
        /// </summary>
        /// <returns>�����ġ ���� ����</returns>
        public bool IsRunning()
        {
            return _isRunning;
        }

        /// <summary>
        /// �����ġ�� �Ͻ� ���� �������� Ȯ���մϴ�.
        /// </summary>
        /// <returns>�����ġ �Ͻ� ���� ����</returns>
        public bool IsPaused()
        {
            return _isPaused;
        }

        /// <summary>
        /// ���� �帥 �ð��� �� ������ ��ȯ�մϴ�.
        /// </summary>
        /// <returns>��������� ��� �ð�(��)</returns>
        public float GetElapsedTime()
        {
            if (!_isRunning)
            {
                Debug.LogWarning("�����ġ�� ���� ������ �ʽ��ϴ�.");
                return 0f;
            }

            if (_isPaused)
            {
                return _pausedTime;
            }

            return GetCurrentTime() - _startTime;
        }

        /// <summary>
        /// ���� �ð��� �� ������ �����ɴϴ�.
        /// </summary>
        /// <returns>���� �ð�(��)</returns>
        private float GetCurrentTime()
        {
            return Time.time;
        }

        /// <summary>
        /// �ʸ� �Է¹޾� "��:��" ������ ���ڿ��� ��ȯ�ϴ� �Լ�
        /// </summary>
        /// <returns>"��:�� ���ڿ�"</returns>
        public static System.Func<int, string> FormatTimeToMinutesAndSeconds = (totalSeconds) =>
        {
            int minutes = totalSeconds / 60; // �� ���
            int seconds = totalSeconds % 60; // �� ���

            // "��:��" ������ ���ڿ��� ��ȯ
            return string.Format("{0}:{1:D2}", minutes, seconds);
        };
    }
}