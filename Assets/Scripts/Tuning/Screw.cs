using System.Collections;
using System.Collections.Generic;
using Tuning;
using UnityEngine;

namespace Tuning
{
    public class Screw : MonoBehaviour, IInfoUIShowable
    {
        private Collider2D _collider;
        private BlinkObject _blinkObject;
        private SpriteRenderer _renderer; 

        public Sprite GetSprite() => _renderer.sprite;

        public void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _blinkObject = GetComponent<BlinkObject>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            PartsPool.Instance.AddObjectAtPool(this);
        }

        private void OnDisable()
        {
            PartsPool.Instance?.RemoveObjectAtPool(this);
        }


        public void OnSelected()
        {
            _blinkObject.StartBlink();
        }

        public void OnUnselected()
        {
            _blinkObject.StopBlink();
        }

        public bool IsMouseEnter(Vector2 mousePos)
        {
            return _collider.OverlapPoint(mousePos);
        }

        public void SetPartsInfo(InfoPanel infoPanel)
        {
            infoPanel.SetName("³ª»ç");
            infoPanel.Stat1Lable.gameObject.SetActive(false);   
            infoPanel.Stat2Lable.gameObject.SetActive(false);   
            infoPanel.Stat3Lable.gameObject.SetActive(false);   
            infoPanel.Stat4Lable.gameObject.SetActive(false);   

            infoPanel.QualiltyPanel.gameObject.SetActive(false);

            infoPanel.Icon.gameObject.SetActive(false);
        }
    }
}