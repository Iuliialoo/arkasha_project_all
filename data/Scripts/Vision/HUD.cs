using System;
using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "77438698575559176082a38a28b43d611c83b74c")]
public class HUD : Component
{
	// crosshair parameters
	[ParameterFile]
	public string crosshairImage = null;
	public int crosshairSize = 10;
	bool switcher = false;

	//GuiButton manager = new GuiButton();

	WidgetSprite sprite;
	//GuiButton button;

	Gui screenGui = Gui.Get();
	private void Init()
	{
		sprite = new WidgetSprite(screenGui, crosshairImage);

		// get an instance of the screen Gui
		//Gui screenGui = Gui.GetCurrent();
		
		// add a sprite widget
		// set the sprite size
		sprite.Width = crosshairSize;
		sprite.Height = crosshairSize;

		// make the sprite stay in the screen center and overlap the other widgets
		screenGui.AddChild(sprite, Gui.ALIGN_CENTER | Gui.ALIGN_OVERLAP);
		//sprite.Hidden = true;
		
	}
	
	private void Update()
	{
		// write here code to be called before updating each render frame
		int width = App.GetWidth(), height = App.GetHeight();
		sprite.SetPosition(width / 2 - 8, height / 2 - 8);
		// if (Input.IsKeyDown(Input.KEY.ESC))
        // {
        //     sprite.Hidden = true;
        //     switcher = true;
        // }
        // else if(Input.IsMouseButtonPressed(Input.MOUSE_BUTTON.LEFT))
        // {
        //     sprite.Hidden = false;
        //     switcher = false;
        // }
		
		
	}
}