using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace LifeLeech {
	internal partial class LeechBehavior : MissionBehaviour {

		public void DoHealing(Agent agent) {
			if (config.MultiplerHealing.Enabled) {
				HealUsingMultipler(agent);
			}
			if (config.StaticHealing.Enabled) {
				HealUsingStatic(agent);
			}
		}

		public void HealUsingMultipler(Agent agent) {
			decimal? athletics = agent.Character.GetSkillValue(DefaultSkills.Athletics);
			decimal? medicine = agent.Character.GetSkillValue(DefaultSkills.Medicine);
			float maxHealth = agent.HealthLimit;

			var result = (float)((athletics + medicine) / 2 * config.MultiplerHealing.Value);
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

		public void HealUsingStatic(Agent agent) {
			float maxHealth = agent.HealthLimit;
			var result = config.StaticHealing.Value;

			agent.Health += result;
			if (config.LimitedByMaxHealth && agent.Health > maxHealth) {
				agent.Health = maxHealth;
			}

			if (config.DebugOutput) {
				InformationManager.DisplayMessage(new InformationMessage($"{agent.Name} was granted {result:N1} static hit points."));
			}
			return;
		}
	}
}
