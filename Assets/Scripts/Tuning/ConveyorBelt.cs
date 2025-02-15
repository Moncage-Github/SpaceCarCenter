using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tuning
{
    public class ConveyorBelt : MonoBehaviour
    {
        [SerializeField] float _beltSpeed;
        List<GameObject> _beltList = new List<GameObject>();

        //private void Update()
        //{
        //    foreach (var belt in _beltList)
        //    {
        //        belt.transform.Translate(-_beltSpeed * Time.deltaTime, 0, 0);
        //    }
        //}

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out Parts parts))
            {
                if (parts.CurState != Parts.State.Dropped) return;
            }

            BoxCollider2D boxCollider = collision as BoxCollider2D;
            if (boxCollider == null) return;
            float otherDown = collision.transform.position.y - boxCollider.size.y / 2;
            float beltUp = transform.position.y;

            if (otherDown >= beltUp)
            {
                collision.transform.Translate(-_beltSpeed * Time.fixedDeltaTime, 0, 0);

            }
        }

        //private void OnCollisionExit2D(Collision2D collision)
        //{
        //    if (_beltList.Contains(collision.gameObject))
        //        _beltList.Remove(collision.gameObject);
        //}
    }
}