using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tuning
{
    public abstract class PartsBase : MonoBehaviour
    {
        protected Rigidbody2D Rigidbody;
        protected SpriteRenderer Renderer;

        public BoxCollider2D Collider { get; private set; }

        private bool _select;
        public bool Select
        {
            get => _select;
            set
            {
                _select = value;
                if (_select == false)
                {
                    Renderer.color = Color.white;
                }
            }
        }


        public void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            Rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<BoxCollider2D>();
            _select = false;
        }

        private void OnEnable()
        {
            PartsPool.Instance.AddParts(this);
        }
        private void OnDisable()
        {
            PartsPool.Instance?.RemoveParts(this);
        }

        public virtual void Drop()
        {
            Renderer.sortingOrder = 0;
            Rigidbody.bodyType = RigidbodyType2D.Dynamic;
            Rigidbody.transform.parent = null;
        }
        public virtual void Pickup()
        {
            Renderer.sortingOrder = 2;
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.bodyType = RigidbodyType2D.Kinematic;
            Rigidbody.transform.parent = null;
        }

        private void FixedUpdate()
        {
            OnSelected();
        }

        public void OnSelected()
        {
            if (!Select) return;
            float t = Mathf.PingPong(Time.time / 0.5f, 1f);
            Color color1 = Color.white;
            Color color2 = Color.gray;

            Color newColor = Color.Lerp(color1, color2, t);
            Renderer.color = newColor;
        }
    }
}   