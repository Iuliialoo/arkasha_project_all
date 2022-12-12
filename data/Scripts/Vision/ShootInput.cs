using System;
using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "7318c075bccc5750c469426fa3168d0245f4e879")]
public class ShootInput : Component
{
	public bool IsShooting()
	{
		//return Input.IsMouseButtonDown(Input.MOUSE_BUTTON.LEFT);

		return Input.IsMouseButtonPressed(Input.MOUSE_BUTTON.LEFT);
	}
}