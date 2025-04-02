using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tuning
{   
    public class DetectionArea : MonoBehaviour
    {
        public Action<Collider2D> OnEnterDetectionArea;
        public Action<Collider2D> OnExitDetectionArea;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnEnterDetectionArea?.Invoke(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            OnExitDetectionArea?.Invoke(collision);
        }
    }
}