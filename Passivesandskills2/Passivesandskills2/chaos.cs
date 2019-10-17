﻿
using Smod2.EventHandlers;
using Smod2.Events;
using Smod2.API;


namespace Passivesandskills2
{
	partial class chaos : IEventHandlerPlayerDie, IEventHandlerSetRole, IEventHandlerPlayerHurt
		{ 
	
        //  hacen daño adicional = a la vida que les falta/2 y al matar NTF obtienen botiquines 
		public void OnPlayerDie(PlayerDeathEvent ev)
		{
			// Chaos - [Carroñero] //
			if ((ev.Killer.TeamRole.Role == Role.CHAOS_INSURGENCY) && (ev.Player.TeamRole.Team == Team.NINETAILFOX))
			{
				ev.Killer.GiveItem(ItemType.MEDKIT);
			}
		}

		public void OnPlayerHurt(PlayerHurtEvent ev)
		{
			//chaos - [Luchador de doble filo]//
			if (ev.Attacker.TeamRole.Role == Role.CHAOS_INSURGENCY)
			{
                if ((ev.Player.TeamRole.Team == Team.NINETAILFOX) || (ev.Player.TeamRole.Team == Team.SCIENTIST) || (ev.Player.TeamRole.Team == Team.CLASSD))

                { ev.Damage += ((120 - ev.Attacker.GetHealth()) / 2); }
                                       
            }
		}

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			if (ev.Player.TeamRole.Role == Role.CHAOS_INSURGENCY)
			{
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Carroñero]: Recibes un botiquin por cada NTF que asesines [Luchador de doble filo]: La vida que te falte la inflinges como daño adicional entre 2.", false);
                ev.Player.PersonalBroadcast(10, "Luchador de doble filo no afecta a SCPS", false);
            }
		}
	}

}
