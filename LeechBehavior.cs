using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;

namespace LifeLeech {
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
			if (config.MultiplerHealing.Enabled) {
				HealUsingMultipler(affectorAgent);
			}
			if (config.StaticHealing.Enabled) {
				HealUsingStatic(affectorAgent);
			}			
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
			if (config.MultiplerHealing.Enabled) {
				HealUsingMultipler(affectorAgent);
			}
			if (config.StaticHealing.Enabled) {
				HealUsingStatic(affectorAgent);
			}
		}

		private void HealUsingMultipler(Agent agent) {
			float athletics = agent.Character.GetSkillValue(DefaultSkills.Athletics);
			float medicine = agent.Character.GetSkillValue(DefaultSkills.Medicine);
			float maxHealth = agent.HealthLimit;

			var result = ((athletics + medicine) / 2f) * config.MultiplerHealing.Value;
			if (result < 0.1f) {
				result = 0.1f;
			}

			agent.Health += (float)result;
			if (agent.Health > maxHealth && config.LimitedByMaxHealth) {
				agent.Health = maxHealth;
			}

			if (config.DebugOutput) {
				InformationManager.DisplayMessage(new InformationMessage($"{agent.Name} was granted {result:N1} formula-based hit points.\n" +
				$" - Athletics: {athletics}\n" +
				$" - Medicine: {medicine}\n" +
				$" - Multipler: {config.MultiplerHealing.Value:N2}"));
			}
			return;
		}

		private void HealUsingStatic(Agent agent) {
			float maxHealth = agent.HealthLimit;
			var result = config.StaticHealing.Value;

			agent.Health += (float)result;
			if (agent.Health > maxHealth && config.LimitedByMaxHealth) {
				agent.Health = maxHealth;
			}

			if (config.DebugOutput) {
				InformationManager.DisplayMessage(new InformationMessage($"{agent.Name} was granted {result:N1} static hit points."));
			}
			return;
		}
	}
}
