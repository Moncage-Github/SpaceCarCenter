using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tuning
{
    public class ConveyorBelt : MonoBehaviour
    {
        [SerializeField] float _beltSpeed;
        List<GameObject> _beltList = new List<GameObject>();

        private void Update()
        {
            foreach (var belt in _beltList)
            {
                belt.transform.Translate(-_beltSpeed * Time.deltaTime, 0, 0);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.position.y > transform.position.y)
            {
                _beltList.Add(collision.gameObject);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if(_beltList.Contains(collision.gameObject))
                _beltList.Remove(collision.gameObject);
        }
    }
}