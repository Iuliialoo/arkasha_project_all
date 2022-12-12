using System;
using System.Collections;
using System.Collections.Generic;
using Unigine;


[Component(PropertyGuid = "02f4d20b71a0ffee25d2c67584a2a550eb5f386f")]
public class Shoot : Component
{
	public PlayerDummy shootingCamera = null;
	public ShootInput shootInput = null;

	//public ObjectGui menuGui = null;

	Gui gui;
	WidgetCanvas textOfObject;
	
	string myText;


	// Корректное выделение объекта
	Unigine.Object lastSelected = null;
	static vec4 lastSelectedColor;
	static float lastVector;
	static vec3 deltaObject;
	static float angleObject;

	Unigine.Object mainHitObject;

	[ParameterMask(MaskType = ParameterMaskAttribute.TYPE.INTERSECTION)]
	public int mask = ~0;

	int x, y;
	public void addText(string text){
		//textOfObject.Clear();
		textOfObject.SetTextText(x, text);
	}

	// define world intersection instance

	// create a counter to show the message once
	private void Init()
	{
		// write here code to be called on component initialization
		//ControlsApp.MouseHandle = Input.MOUSE_HANDLE.USER;

		gui = Gui.Get();
		textOfObject = new WidgetCanvas(gui);
		textOfObject.Clear();

		x = textOfObject.AddText(1);

		vec4 color = new vec4(x, 1, 1, 0.5);
		textOfObject.SetTextColor(x, color);
		textOfObject.SetTextSize(x, 30);
		textOfObject.SetTextPosition(x, new vec2(10, 10));
		
		y = textOfObject.AddPolygon(0);
		textOfObject.SetPolygonColor(y, new vec4(0, 0, 0, 0.5));

		textOfObject.AddPolygonPoint(y, new vec3(0, 0, 0));
		textOfObject.AddPolygonPoint(y, new vec3(300, 0, 0));
		textOfObject.AddPolygonPoint(y, new vec3(300, 100, 0));
		textOfObject.AddPolygonPoint(y, new vec3(0, 100, 0));

		gui.AddChild(textOfObject, Gui.ALIGN_EXPAND);
		//textOfObject.Clear();
		textOfObject.Hidden = true;

	}

	public void DirectionShoot(Unigine.Object Object, vec3 p0, vec3 p1)
	{
		if (Object != null)
		{
			if (Object.RootNode.Name == "dynamic_content")
			{
				WorldIntersectionNormal hitInfo = new WorldIntersectionNormal();
				Unigine.Object hitObject = World.GetIntersection(p0, p1, mask, hitInfo);

				Object.WorldPosition = p0 - deltaObject + ((p1 - p0).Normalized * lastVector);

				// Object.SetWorldDirection(deltaObjectDirection - (p1 - p0).Normalized, Object.GetWorldDirection(MathLib.AXIS.Z), MathLib.AXIS.Y);
				// Object.SetWorldDirection(deltaObjectDirection + (new vec3 (1, 1, -1)) * (p1 - p0).Normalized, new vec3(0.0f,0.0f,1.0f), MathLib.AXIS.Z);

				Log.Message("vector camera: {0}\n", (p1 - p0).Normalized);

				vec3 deltaObjectDirection = (p0 - p1).Normalized;

				// vector deviated from deltaObjectDirection by an angle (angleObject) 
				vec3 vectorDeviation = new vec3(deltaObjectDirection.x * Math.Cos(angleObject) - deltaObjectDirection.y * Math.Sin(angleObject),
												deltaObjectDirection.x * Math.Sin(angleObject) + deltaObjectDirection.y * Math.Cos(angleObject),
												deltaObjectDirection.z);

				textOfObject.Hidden = false;
				string info = $"{Object.Name}";
				addText(info);

				Visualizer.RenderMessage3D(Object.WorldPosition + new vec3(0, 0, 1.1f), 0, info, vec4.GREEN, 0, 20, 0);

				// Object.SetWorldDirection(vectorDeviation, Object.GetWorldDirection(MathLib.AXIS.Z), MathLib.AXIS.Y);

				// My method of rotation

				// Object.SetDirection(deltaObjectDirection, new vec3(0.0f,0.0f,1.0f), MathLib.AXIS.Y);


				// the object must have one side facing the camera without changing its local direction

				// Object.SetRotation(new quat(new vec3 (1, 1, -1) * (p1 - p0).Normalized, 0), true);
			}
		}
	}


	public void SelecteObject(Unigine.Object Object)
	{
		// if (lastSelected)
		// lastSelected.GetMaterial(0).SetParameterFloat4("albedo_color", lastSelectedColor);

		// lastSelected = Object;
		// lastSelectedColor = lastSelected.GetMaterial(0).GetParameterFloat4("albedo_color");

		if (Object.RootNode.Name == "dynamic_content")
		{
			// Object.GetMaterial(0).SetParameterFloat4("albedo_color", vec4.WHITE);

			Visualizer.RenderObjectSurfaceBoundBox(Object, 0, vec4.BLUE, 0.05f);
			// Visualizer.RenderObject(Object, vec4.BLUE, 0.05f);

			textOfObject.Hidden = false;
			string info = $"{Object.Name}";
			addText(info);

			Visualizer.RenderMessage3D(Object.WorldPosition + new vec3( 0, 0, 1.1f), 0, info, vec4.GREEN, 0, 20, 0);
		}
		else{
			textOfObject.Hidden = true;
		}
	} 


	public void intersectionObject(bool choseObject)
	{
		vec3 p0, p1;
		shootingCamera.GetDirectionFromScreen(out p0, out p1);

		WorldIntersectionNormal hitInfo = new WorldIntersectionNormal();
		Unigine.Object hitObject = World.GetIntersection(p0, p1, mask, hitInfo);

		if (choseObject)
			DirectionShoot(mainHitObject, p0, p1);
		else if (hitObject)
			SelecteObject(hitObject);
	}


	public void Selection()
	{
		intersectionObject(false);
	}


	public void Shooting()
	{
		intersectionObject(true);
	}


	public void trackingObject(bool trackObject)
	{
		if (trackObject)
		{
			vec3 p0, p1;
			shootingCamera.GetDirectionFromScreen(out p0, out p1);

			WorldIntersectionNormal hitInfo = new WorldIntersectionNormal();
			Unigine.Object hitObject = World.GetIntersection(p0, p1, mask, hitInfo);

			if (hitObject)
			{
				if (hitObject.RootNode.Name == "dynamic_content")
				{
					lastVector = (hitObject.WorldPosition - p0).Length;
					deltaObject = (p1 - p0).Normalized*lastVector - (hitObject.WorldPosition - p0).Normalized*lastVector;
		
					// angle between two vectors (hitObject.GetWorldDirection(MathLib.AXIS.Y) and camera direction (p1 - p0).Normalized) 
					vec3 v1 = hitObject.GetWorldDirection(MathLib.AXIS.Y);
					vec3 v2 = (p0 - p1).Normalized;

					angleObject = MathLib.Acos((v1.x * v2.x + v1.y * v2.y + v1.z * v2.z) / (v1.Length * v2.Length));
	
					// deltaObjectDirection = hitObject.GetWorldDirection(MathLib.AXIS.Y);

					mainHitObject = hitObject;
				}
				
			}
			
		}
		else
		{
			mainHitObject = null;
		}
	}


	private void Update()
	{
		Visualizer.Enabled = true;


		if (Input.IsMouseButtonDown(Input.MOUSE_BUTTON.LEFT))
			trackingObject(true);

		if (Input.IsMouseButtonUp(Input.MOUSE_BUTTON.LEFT))
			trackingObject(false);

		if (shootInput.IsShooting())
		{
			Shooting();
		}
		else
			Selection();

	}
}