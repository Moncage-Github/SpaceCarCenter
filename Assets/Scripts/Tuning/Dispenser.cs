using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tuning
{
    public class Dispenser : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _placeHolder;
        private Button _button;
        private bool _canDispense = true;

        // Start is called before the first frame update
        void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() =>
            {
                if (!_canDispense) return;
                GameObject obj = Instantiate(_prefab);
                obj.transform.position = _placeHolder.transform.position;
                _canDispense = false;
                Invoke("Interactable", 0.5f);
            });
        }

        private void Interactable()
        {
            _canDispense = true;
        }

    }
}