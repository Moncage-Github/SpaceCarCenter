using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuySell
{
    void Buy(int currentGold);
    void Sell(int currentGold);
}
