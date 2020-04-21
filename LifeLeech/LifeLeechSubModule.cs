using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;
using System.Linq;
using System.Text;
using TaleWorlds.Core;
using TaleWorlds.Library;
using System.Xml.Serialization;
using System.IO;

namespace LifeLeech {
	public class LifeLeechSubModule : MBSubModuleBase {
		public Config config;

		/// <summary>
		/// Runs when a mission (ie. field battle, arena, siege) is initialized.
		/// </summary>
		public override void OnMissionBehaviourInitialize(Mission mission) {
			// reads config file and serializes it into Config object
			// doing this on mission initialization allows us to change settings when game is still running
			var serializer = new XmlSerializer(typeof(Config));
			var reader = new StreamReader(BasePath.Name + "Modules/LifeLeech/bin/Win64_Shipping_Client/config.xml");
			config = (Config)serializer.Deserialize(reader);
			reader.Close();

			// if debug output is enabled in config, prints config details
			if (config.DebugOutput) {
				InformationManager.DisplayMessage(new InformationMessage($"Life Leech mod activated." +
				$"\nmultiplerHealing = {config.MultiplerHealing.Enabled} @ {config.MultiplerHealing.Value:N1}" +
				$"\nstaticHealing = {config.StaticHealing.Enabled} @ {config.StaticHealing.Value:N1}" +
				$"\nplayerOnly = {config.PlayerOnly}" +
				$"\nkillBased = {config.KillBased}" +
				$"\nexcludeCavalry = {config.ExcludeCavalry}"));
			}

			// adds custom mission behavior - a mission script, so to speak.
			mission.AddMissionBehaviour(new LeechBehavior(config));
			base.OnMissionBehaviourInitialize(mission);
		}
	}
}
