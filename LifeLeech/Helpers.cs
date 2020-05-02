using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Library;

namespace LifeLeech {
	internal partial class LeechBehavior : MissionBehaviour {
		/// <summary>
		/// Shortcut method to send a debug message in chat box.
		/// </summary>
		/// <param name="text">Text to display.</param>
		void Say(string text) {
			if (config.DebugOutput)
				InformationManager.DisplayMessage(new InformationMessage(text, new Color(0.5f, 0.5f, 0.5f)));
		}

		void SayGreen(string text) {
			InformationManager.DisplayMessage(new InformationMessage(text, new Color(0, 1, 0)));
		}
	}
}
