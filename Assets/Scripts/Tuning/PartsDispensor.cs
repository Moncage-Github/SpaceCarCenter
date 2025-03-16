using System.Collections;
using System.Collections.Generic;
using Tuning;
using UnityEngine;
using UnityEngine.UI;

namespace Tuning
{
    public class PartsDispensor : MonoBehaviour
    {
        [SerializeField] private List<PartsData> _queue;
        [SerializeField] private Transform _placeHolder;

        [SerializeField] private DetectionArea _detectionArea;
        [SerializeField] private Button _button;

        private bool _canDispense = true;


        private void Awake()
        {
            _button.onClick.AddListener(() =>
            {
                if (!_canDispense) return;
                if (_queue.Count <= 0) return;   

                _canDispense = false;

                var data = _queue[0];

                var parts = PartsPool.Instance.CreateParts(data);

                parts.transform.position = _placeHolder.transform.position;
                var r = parts.transform.eulerAngles;
                r.z = Random.Range(0.0f, 180.0f);
                parts.transform.eulerAngles = r;
                
                _queue.RemoveAt(0);

                Invoke("SetInteractable", 0.5f);
            });

            _detectionArea.OnEnterDetectionArea += OnEnterDetectionArea;
            _detectionArea.OnExitDetectionArea += OnExitDetectionArea;
        }

        private void SetInteractable()
        {
            _canDispense = true;
        }

        public void OnEnterDetectionArea(Collider2D collider)
        {
            if (collider.TryGetComponent(out TuningPlayer player))
            {
                _button.interactable = true;
            }
        }

        public void OnExitDetectionArea(Collider2D collider)
        {
            if (collider.TryGetComponent(out TuningPlayer player))
            {
                _button.interactable = false;
            }
        }
    }
}