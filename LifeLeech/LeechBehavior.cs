using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;
using System;

namespace LifeLeech {
	internal partial class LeechBehavior : MissionBehaviour {
		private readonly Config config;

		public LeechBehavior(Config config) {
			this.config = config;
		}

		public override MissionBehaviourType BehaviourType => MissionBehaviourType.Other;

		/// <summary>
		/// Runs when any agent in mission is hit. Includes mounts.
		/// </summary>
		public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, int damage, int weaponKind, int currentWeaponUsageIndex) {
			try {
				if (config.KillBased) {
					return;
				}
				if (affectorAgent.Character == null || affectedAgent.Character == null) {
					Say($"{affectorAgent.Name} isn't a character/is a mount.");
					return;
				}
				if (damage == 0) {
					Say($"{affectorAgent.Name} dealt no damage.");
					return;
				}
				if (config.OnlyNamedCharacters && !affectorAgent.IsHero) {
					Say($"{affectorAgent.Name} isn't a hero.");
					return;
				}
				if (config.ExcludeCavalry && affectorAgent.HasMount) {
					Say($"{affectorAgent.Name} is mounted - no healing.");
					return;
				}
				if (config.PlayerOnly && !affectorAgent.IsPlayerControlled) {
					Say($"{affectorAgent.Name} isn't a player.");
					return;
				}

				DoHealing(affectorAgent);
			} catch (Exception ex) { Say($"{ex.Message}"); return; };
		}

		/// <summary>
		/// Runs when any agent in mission is removed in some way. Includes mounts. Includes kills, but method also handles removal by other means.
		/// </summary>
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow) {
			try {
				if (!config.KillBased) {
					return;
				}
				if (!blow.IsValid) {
					return;
				}
				if (affectorAgent.Character == null || affectedAgent.Character == null) {
					Say($"{affectorAgent.Name} isn't a character/is a mount.");
					return;
				}
				if (config.OnlyNamedCharacters && !affectorAgent.IsHero) {
					Say($"{affectorAgent.Name} isn't a hero.");
					return;
				}
				if (config.PlayerOnly && !affectorAgent.IsPlayerControlled) {
					Say($"{affectorAgent.Name} isn't a player.");
					return;
				}
				if (config.ExcludeCavalry && affectorAgent.HasMount) {
					Say($"{affectorAgent.Name} is mounted - no healing.");
					return;
				}

				DoHealing(affectorAgent);
			} catch (Exception ex) { Say($"{ex.Message}"); return; };
		}
	}
}
