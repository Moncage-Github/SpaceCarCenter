using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tuning
{
    [RequireComponent(typeof(Text))]
    public class Timer : MonoBehaviour
    {
        [SerializeField] private int _startTime;
        private int _curTime;
        private Text _text;

        public UnityEvent OnFinishTimer;

        // Start is called before the first frame update
        void Start()
        {
            StartTimer();
        }

        public void StartTimer()
        {
            _text = GetComponent<Text>();

            _curTime = _startTime;
            _text.text = Util.Stopwatch.FormatTimeToMinutesAndSeconds(_curTime);

            WaitForSeconds waitForSeconds = new WaitForSeconds(1);
            StartCoroutine(TimerRoutine(waitForSeconds));
        }

        private IEnumerator TimerRoutine(WaitForSeconds wait)
        {
            while (_curTime != 0)
            {
                yield return wait;

                _curTime -= 1;
                _text.text = Util.Stopwatch.FormatTimeToMinutesAndSeconds(_curTime);
            }

            OnFinishTimer?.Invoke();
        }
    }
}