using System.Collections;
using System.Collections.Generic;
using Tuning;
using UnityEngine;
using UnityEngine.UI;

namespace Tuning
{
    public class PartsDispensor : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _objectsQueue;
        [SerializeField] private Transform _placeHolder;

        [SerializeField] private DetectionArea _detectionArea;
        [SerializeField] private Button _button;

        private bool _canDispense = true;


        private void Awake()
        {
            _button.onClick.AddListener(() =>
            {
                if (!_canDispense) return;
                if (_objectsQueue.Count <= 0) return;   

                _canDispense = false;
                GameObject obj = _objectsQueue[0];
                obj.SetActive(true);

                obj.transform.position = _placeHolder.transform.position;
                _objectsQueue.RemoveAt(0);

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