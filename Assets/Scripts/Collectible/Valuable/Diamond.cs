using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Diamond : AbstractValuable
{
	protected override void SetObjValue()
	{
		_value = 50;
	}
}