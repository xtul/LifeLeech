using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;

namespace LifeLeech {
	internal partial class LeechBehavior : MissionBehaviour {
		private readonly Config config;

		public LeechBehavior(Config config) {
			this.config = config;
		}

		public override MissionBehaviourType BehaviourType => MissionBehaviourType.Other;

		public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, int damage, int weaponKind, int currentWeaponUsageIndex) {
			if (config.KillBased) {
				return;
			}
			if (affectorAgent.Character == null) {
				if (config.DebugOutput)
					InformationManager.DisplayMessage(new InformationMessage($"{affectorAgent.Name} isn't a character/is a mount."));
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

			DoHealing(affectorAgent);
		}

		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow) {
			if (!config.KillBased) { 
				return;
			}
			if (affectorAgent.Character == null) {
				if (config.DebugOutput)
					InformationManager.DisplayMessage(new InformationMessage($"{affectorAgent.Name} isn't a character/is a mount."));
				return;
			}
			if (!blow.IsValid) {
				return;
			}
			if (config.PlayerOnly && !affectorAgent.IsPlayerControlled) {
				if (config.DebugOutput)
					InformationManager.DisplayMessage(new InformationMessage($"{affectorAgent.Name} isn't a player."));
				return;
			}
			if (config.ExcludeCavalry && affectorAgent.HasMount) {
				if (config.DebugOutput)
					InformationManager.DisplayMessage(new InformationMessage($"{affectorAgent.Name} is mounted - no healing."));
				return;
			}

			DoHealing(affectorAgent);
		}
	}
}
