using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tuning
{
    public class ToolInventory : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<TuningTool.Type, Sprite> _toolImages;
        [SerializeField] private Image _invenImage;

        private TuningTool _curTool;
        public TuningTool CurTool 
        { 
            get => _curTool; 
            private set
            {
                _curTool = value;
                if(_toolImages.TryGetValue(_curTool.ToolType, out var sprite))
                {
                    _invenImage.sprite = sprite;
                }
            }
        }

        private TuningTool _driver = new TuningTool(TuningTool.Type.Driver);
        private TuningTool _hammer = new TuningTool(TuningTool.Type.Hammer);
        private TuningTool _hand = new TuningTool(TuningTool.Type.Hand);
        private TuningTool _paintGun = new TuningTool(TuningTool.Type.PaintingGun);

        public void ChangeTool(TuningTool.Type type)
        {
            switch (type)
            {
                case TuningTool.Type.Hammer:
                    CurTool = _hammer;
                    break;
                case TuningTool.Type.Driver:
                    CurTool = _driver;
                    break;
                case TuningTool.Type.Hand:
                    CurTool = _hand;
                    break;
                case TuningTool.Type.PaintingGun:
                    CurTool = _paintGun;
                    break;
            }
        }
    }
}