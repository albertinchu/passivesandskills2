using System.Collections.Generic;
using System.Linq;
using Smod2;
using Smod2.EventHandlers;
using scp4aiur;
using Smod2.Events;
using Smod2.API;

namespace Passivesandskills2
{
	partial class scp079 : IEventHandler079AddExp, IEventHandlerPlayerDie, IEventHandlerGeneratorFinish, IEventHandler079LevelUp, 
		IEventHandlerSetRole, IEventHandlerWaitingForPlayers, IEventHandlerWarheadDetonate, IEventHandlerWarheadStartCountdown, IEventHandlerWarheadStopCountdown
	{
		
		bool Nuket = false;
		bool Boom = false;
		int gen = 0;
		int level = 0;
		static bool overcharge = false;
		string computerchan;
		static Dictionary<string, bool> Computerr = new Dictionary<string, bool>();
		Vector posicionteni;
		static Dictionary<string, Player> Pasivaa = new Dictionary<string, Player>();

		public static IEnumerable<float> Secondboom()
		{
			yield return 5f;
			PluginManager.Manager.Server.Map.DetonateWarhead();
			yield return 2f;
			PluginManager.Manager.Server.Map.DetonateWarhead();
			yield return 2f;
			PluginManager.Manager.Server.Map.DetonateWarhead();
			yield return 2f;
			PluginManager.Manager.Server.Map.DetonateWarhead();

		}


		public static IEnumerable<float> Computer(Player player)
		{
			yield return 0.5f;
			player.ChangeRole(Role.SCP_079);
		}


		public static IEnumerable<float> Pcoff()
		{
			yield return 60f;
			overcharge = true;
		}

		public void On079AddExp(Player079AddExpEvent ev)
		{
			float Xp;
			Xp = ev.ExpToAdd;
			if (level > 3)
			{
				ev.Player.Scp079Data.APPerSecond = (ev.Player.Scp079Data.APPerSecond + (Xp / 25));
				ev.Player.Scp079Data.MaxAP = ev.Player.Scp079Data.MaxAP + (Xp / 10);

			}
		}

		public void On079LevelUp(Player079LevelUpEvent ev)
		{
			level = ev.Player.Scp079Data.Level;
			if (level == 3)
			{
				ev.Player.Scp079Data.MaxAP = 200;
			}
			if ((level > 3))
			{
				ev.Player.Scp079Data.MaxAP = 300;
				PluginManager.Manager.Server.Map.AnnounceCustomMessage("G G . SCP 079 LEVEL 5");
			}
		}

		public void OnGeneratorFinish(GeneratorFinishEvent ev)
		{
			gen += 1;
			if (gen == 5) { Timing.Run(Pcoff()); }
		}

		public void OnPlayerDie(PlayerDeathEvent ev)
		{
			if (overcharge == false)
			{
				if ((Computerr.ContainsKey(ev.Killer.SteamId))) { PluginManager.Manager.Server.Map.Broadcast(1, "079 mató a un jugador", true); }
				posicionteni = ev.Player.GetPosition();

				if ((Boom == false) && (Computerr.ContainsKey(ev.Player.SteamId))) { Timing.Run(Computer(ev.Player)); }

				if ((ev.Player.SteamId == ev.Killer.SteamId) && (Nuket == true) && (ev.DamageTypeVar == DamageType.TESLA) && (ev.Player.TeamRole.Role != Role.SCP_096))
				{
					var nueva = ev.Player.TeamRole.Role;
					ev.SpawnRagdoll = false;
					foreach (KeyValuePair<string, Player> keyValue in Pasivaa)
					{
						if ((keyValue.Value.TeamRole.Role == Role.SCP_079) && (nueva != Role.SCP_173)) { keyValue.Value.ChangeRole(nueva); }
						if ((keyValue.Value.TeamRole.Role == Role.SCP_079) && (nueva == Role.SCP_173))
						{
							keyValue.Value.ChangeRole(nueva); keyValue.Value.Teleport(PluginManager.Manager.Server.Map.GetSpawnPoints(Role.SCP_049).First());
						}

					}
				}
			}
		}

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			if (!Pasivaa.ContainsKey(ev.Player.SteamId)) { Pasivaa.Add(ev.Player.SteamId, ev.Player); }
			if ((ev.Role == Role.SCP_079) && (computerchan != ev.Player.SteamId) && (Computerr.ContainsKey(ev.Player.SteamId)))
			{
				computerchan = ev.Player.SteamId;
				Computerr.Add(ev.Player.SteamId, true);
			}
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			Nuket = false;
			Boom = false;   
			overcharge = false;
			level = 0;
			computerchan = "0";
			gen = 0;
		}

		public void OnDetonate()
		{
			if (Boom == false)
			{
				Timing.Run(Secondboom());
				Boom = true;
			}
		}

		public void OnStartCountdown(WarheadStartEvent ev)
		{
			Nuket = true;
		}

		public void OnStopCountdown(WarheadStopEvent ev)
		{
			Nuket = false;
		}
	}
}
