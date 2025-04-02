using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tuning
{
    public class ScrewDispenser : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _placeHolder;
        private Button _button;
        private bool _canDispense = true;

        [SerializeField] private DetectionArea _detectionArea;

        void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() =>
            {
                if (!_canDispense) return;
                GameObject obj = Instantiate(_prefab);
                obj.transform.position = _placeHolder.transform.position;
                _canDispense = false;
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
            if(collider.gameObject.TryGetComponent(out TuningPlayer player))
            {
                _button.interactable = true;
            }
        }

        public void OnExitDetectionArea(Collider2D collider)
        {
            if (collider.gameObject.TryGetComponent(out TuningPlayer player))
            {
                _button.interactable = false;
            }
        }
    }
}