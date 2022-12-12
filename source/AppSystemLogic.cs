using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Unigine;

namespace UnigineApp
{
	class AppSystemLogic : SystemLogic
	{
		// System logic, it exists during the application life cycle.
		// These methods are called right after corresponding system script's (UnigineScript) methods.

		public AppSystemLogic()
		{
		}

		public override bool Init()
		{
			// Write here code to be called on engine initialization.
			LoadingScreen.BackgroundColor = new vec4(0, 0, 0, 0);
			LoadingScreen.TexturePath = "catss.png";
			LoadingScreen.Transform = new vec4(0.5f, 0.5f, 0.5f, 0.5f);
			LoadingScreen.Threshold = 30;

			LoadingScreen.Text = @"
			<p> align=""left"">
				<font color=""#ffffff"" size=""64"">
					<xy x=""%8"" y=""%7.5""/>Mao_Good_Transite
				</font>
			</p>";

			LoadingScreen.MessageLoadingWorld = @"
			<p> align=""right"">
				<font color=""#ffffff"" size=""24"">
					<xy x=""%75"" y=""%86""/>Loading
				</font>
			</p>";
			return true;
		}

		// start of the main loop
		public override bool Update()
		{
			// Write here code to be called before updating each render frame.

			return true;
		}

		public override bool PostUpdate()
		{
			// Write here code to be called after updating each render frame.

			return true;
		}
		// end of the main loop

		public override bool Shutdown()
		{
			// Write here code to be called on engine shutdown.

			return true;
		}
	}
}
