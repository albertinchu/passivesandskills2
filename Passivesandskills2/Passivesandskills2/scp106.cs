
using System.Collections.Generic;
using Smod2;
using Smod2.EventHandlers;
using Smod2.Events;
using Smod2.API;
using scp4aiur;

namespace Passivesandskills2
{
	partial class scp106 : IEventHandlerSetRole, IEventHandlerPocketDimensionDie, IEventHandlerPlayerHurt, IEventHandlerWaitingForPlayers
	{
	
		static Dictionary<string, int> Scp106 = new Dictionary<string, int>();
        
       
     

     

        public void OnPlayerHurt(PlayerHurtEvent ev)
		{
			//[Golpe Crítico]//
			if ((ev.Attacker.TeamRole.Role == Role.SCP_106))
			{
				Scp106[ev.Attacker.SteamId] += 1;
				if (Scp106[ev.Attacker.SteamId] >= 5)
				{
					ev.Player.Kill(DamageType.SCP_106);
					Scp106[ev.Attacker.SteamId] = 0;
				}
			}
		}

		public void OnPocketDimensionDie(PlayerPocketDimensionDieEvent ev)
		{
            foreach (Player player in PluginManager.Manager.Server.GetPlayers())
            {
                if (player.TeamRole.Role == Role.SCP_106) { player.AddHealth(20); }
            }
        }

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
            if ((ev.Player.TeamRole.Role == Role.SCP_106) && (!Scp106.ContainsKey(ev.Player.SteamId)))
            {
                Scp106.Add(ev.Player.SteamId, 0);
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Digestión]: Te curas cuando alguien muere en tu dimensión [Golpe Crítico]: Tu quinta victima muere al instante.", false);
            }
           
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			Scp106.Clear();
		}

       
    }
}
