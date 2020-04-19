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
				$"\n{config.Multipler:N2} multipler" +
				$"\nplayeronly = {config.PlayerOnly}" +
				$"\nkillbased = {config.KillBased}" +
				$"\nexcludecavalry = {config.ExcludeCavalry}"));
			}
			mission.AddMissionBehaviour(new LeechBehavior(config));
			base.OnMissionBehaviourInitialize(mission);
		}
	}

	internal class LeechBehavior : MissionBehaviour {
		private readonly Config config;

		public LeechBehavior(Config skills) {
			this.config = skills;
		}

		public override MissionBehaviourType BehaviourType => MissionBehaviourType.Other;

		public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, int damage, int weaponKind, int currentWeaponUsageIndex) {
			if (config.ExcludeCavalry && affectorAgent.HasMount) {
				if (config.DebugOutput)
					InformationManager.DisplayMessage(new InformationMessage($"{affectorAgent.Name} is mounted - no healing."));
				return;
			}
			if (config.PlayerOnly && !affectorAgent.IsPlayerControlled) {
				if (config.DebugOutput)
					InformationManager.DisplayMessage(new InformationMessage($"{affectorAgent.Name} isn't a player."));
				return;
			}
			if (config.KillBased) {
				return;
			}
			HealUnit(affectorAgent);
			
		}

		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow) {
			if (!blow.IsValid) {
				if (config.DebugOutput)
					InformationManager.DisplayMessage(new InformationMessage($"Invalid blow."));
				return;
			}
			if (config.ExcludeCavalry && affectorAgent.HasMount) {
				if (config.DebugOutput)
					InformationManager.DisplayMessage(new InformationMessage($"{affectorAgent.Name} is mounted - no healing."));
				return;
			}
			if (config.PlayerOnly && !affectorAgent.IsPlayerControlled) {
				if (config.DebugOutput)
					InformationManager.DisplayMessage(new InformationMessage($"{affectorAgent.Name} isn't a player."));
				return;
			}
			if (!config.KillBased) { 
				return;
			}
			HealUnit(affectorAgent);
		}

		private void HealUnit(Agent agent) {
			float athletics = agent.Character.GetSkillValue(DefaultSkills.Athletics);
			float medicine = agent.Character.GetSkillValue(DefaultSkills.Medicine);
			float maxHealth = agent.HealthLimit;

			var result = ((athletics + medicine) / 2f) * config.Multipler;
			if (result < 0.1f) {
				result = 0.1f;
			}

			agent.Health += (float)result;
			if (agent.Health > maxHealth && config.LimitedByMaxHealth) {
				agent.Health = maxHealth;
			}

			if (config.DebugOutput) {
				InformationManager.DisplayMessage(new InformationMessage($"{agent.Name} was granted {result:N1} hit points." +
				$" - Athletics: {athletics}" +
				$" - Medicine: {medicine}" +
				$" - Multipler: {config.Multipler:N2}"));
			}
			return;
		}
	}

	[Serializable()]
	[XmlRoot(ElementName = "Config")]
	public class Config {
		[XmlElement(ElementName = "Multipler")]
		public double Multipler { get; set; }
		[XmlElement(ElementName = "ExcludeCavalry")]
		public bool ExcludeCavalry { get; set; }
		[XmlElement(ElementName = "PlayerOnly")]
		public bool PlayerOnly { get; set; }
		[XmlElement(ElementName = "KillBased")]
		public bool KillBased { get; set; }
		[XmlElement(ElementName = "LimitedByMaxHealth")]
		public bool LimitedByMaxHealth { get; set; }
		[XmlElement(ElementName = "DebugOutput")]
		public bool DebugOutput { get; set; }
	}
}
