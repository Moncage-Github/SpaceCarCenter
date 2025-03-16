using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInfoUIShowable 
{
    public void OnSelected();
    public void OnUnselected();

    public bool IsMouseEnter(Vector2 mousePos);

    public void SetPartsInfo(InfoPanel infoPanel);
}
