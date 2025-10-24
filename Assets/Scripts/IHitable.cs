using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitable
{
    public void hit(int damage, float xPos);
}
