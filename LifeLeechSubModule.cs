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

		public override void OnMissionBehaviourInitialize(Mission mission) {
			var serializer = new XmlSerializer(typeof(Config));

			var reader = new StreamReader(BasePath.Name + "Modules/LifeLeech/bin/Win64_Shipping_Client/config.xml");
			config = (Config)serializer.Deserialize(reader);
			reader.Close();

			if (config.DebugOutput) {
				InformationManager.DisplayMessage(new InformationMessage($"Life Leech mod activated." +
				$"\nmultiplerHealing = {config.MultiplerHealing.Enabled}@{config.MultiplerHealing.Value:N1}" +
				$"\nstaticHealing = {config.StaticHealing.Enabled}@{config.StaticHealing.Value:N1}" +
				$"\nplayerOnly = {config.PlayerOnly}" +
				$"\nkillBased = {config.KillBased}" +
				$"\nexcludeCavalry = {config.ExcludeCavalry}"));
			}
			mission.AddMissionBehaviour(new LeechBehavior(config));
			base.OnMissionBehaviourInitialize(mission);
		}
	}
}
