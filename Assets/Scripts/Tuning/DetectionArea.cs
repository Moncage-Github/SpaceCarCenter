using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tuning
{
    public class DetectionArea : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out TuningPlayer player))
            {
                _button.interactable = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out TuningPlayer player))
            {
                _button.interactable = false;
            }
        }
    }
}