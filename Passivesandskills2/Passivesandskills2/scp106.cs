
using System.Collections.Generic;
using Smod2;
using Smod2.EventHandlers;
using Smod2.Events;
using Smod2.API;
using scp4aiur;

namespace Passivesandskills2
{
	partial class scp106 : IEventHandlerSetRole, IEventHandlerPocketDimensionDie, IEventHandlerPlayerHurt, IEventHandlerWaitingForPlayers, IEventHandler106CreatePortal, IEventHandler106Teleport
	{
	
		static Dictionary<string, int> Scp106 = new Dictionary<string, int>();
        static Dictionary<string, Vector> Portales = new Dictionary<string, Vector>();

        public void On106CreatePortal(Player106CreatePortalEvent ev)
        {
            Portales[ev.Player.SteamId] = ev.Player.GetPosition();
        }

        public void On106Teleport(Player106TeleportEvent ev)
        {
            Timing.Run(Portaltp(ev.Player));
        }

        public static IEnumerable<float> Portaltp(Player player)
        {

            yield return 5f;
            foreach (KeyValuePair<string, Vector> key in Portales)
            {
                if (player.SteamId == key.Key) { player.Teleport(key.Value); }
            }
        }

        public void OnPlayerHurt(PlayerHurtEvent ev)
		{
			//[Golpe Crítico]//
			if ((ev.Attacker.TeamRole.Role == Role.SCP_106))
			{
				Scp106[ev.Attacker.SteamId] += 1;
				if (Scp106[ev.Attacker.SteamId] == 5)
				{
					ev.Player.Kill(DamageType.SCP_106);
					Scp106[ev.Attacker.SteamId] = 0;
				}
			}
		}

		public void OnPocketDimensionDie(PlayerPocketDimensionDieEvent ev)
		{
			if ((ev.Player.TeamRole.Role == Role.SCP_106) && (!Scp106.ContainsKey(ev.Player.SteamId)))
			{
				Scp106.Add(ev.Player.SteamId, 0);
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Digestión]: Te curas cuando alguien muere en tu dimensión [Golpe Crítico]: Tu quinta victima muere al instante.", false);
			}
		}

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			foreach (Player player in PluginManager.Manager.Server.GetPlayers())
			{
				if (player.TeamRole.Role == Role.SCP_106) { player.AddHealth(20); }
			}
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			Scp106.Clear();
		}
	}
}
