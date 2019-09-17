﻿using System;
using System.Collections.Generic;
using Smod2.EventHandlers;
using scp4aiur;
using Smod2.Events;
using Smod2.API;

namespace Passivesandskills2
{
	partial class scp049and0492 : IEventHandlerSetRole, IEventHandlerPlayerHurt, IEventHandlerWaitingForPlayers, IEventHandlerPlayerDie
	{
		private Passivesandskills2 plugin;
		public scp049and0492(Passivesandskills2 plugin)
		{
			this.plugin = plugin;
		}
		static Dictionary<string, int> Zombie = new Dictionary<string, int>();
		Vector posmuertee;
		int conta049 = 0;

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			if ((ev.Player.TeamRole.Role == Role.SCP_049_2))
			{
				if (!Zombie.ContainsKey(ev.Player.SteamId))
				{
					Zombie.Add(ev.Player.SteamId, 0);
					Timing.Run(Zombielive(ev.Player));
				}
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Cuerpo errante]: Cuanto mas tiempo permanezcas con vida mas daño haces (15% + de daño cada 1 minuto de vida).", false);
			}
			if (ev.Player.TeamRole.Role == Role.SCP_049)
			{
				ev.Player.SendConsoleMessage("[Mutar]: Cada 6 Zombies curados el zombie número 6 tiene un 35 % de mutar en otro SCP a los 3 minutos, No puede mutar en SCP-096 o en SCP-079, La mutación es totalmente aleatoria ", "red");
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Manipulador de cuerpos]: Curas de forma instantanea a clasesd/scientists [Mutar]: Cada 6 zombies uno tiene posibilidades de mutar (mas info en la consola)  .", false);
			}
		}

		public static IEnumerable<float> Zombielive(Player player)
		{
			while (player.TeamRole.Role == Role.SCP_049_2)
			{
				yield return 60f;
				Zombie[player.SteamId] += 1;
			}
			if (player.TeamRole.Role != Role.SCP_049_2)
			{
				Zombie[player.SteamId] = 0;
			}
		}
		public static IEnumerable<float> Resurrec(Player player, Vector posdead)
		{
			yield return 3f;
			player.ChangeRole(Role.SCP_049_2);
			yield return 0.2f;
			player.Teleport(posdead);
		}
		public static IEnumerable<float> Mutar(Player player)
		{
			System.Random proba = new Random();
			int numero = proba.Next(0, 100);
			yield return 5f;

			if ((numero >= 5))
			{ player.ChangeRole(Role.SCP_106, false); }
			if ((numero >= 10) && (numero <= 6))
			{ player.ChangeRole(Role.SCP_049, false); }
			if ((numero >= 16) && (numero <= 20))
			{ player.ChangeRole(Role.SCP_049, false); }
			if ((numero >= 95) && (numero <= 100))
			{ player.ChangeRole(Role.SCP_173, false); }
			if (((numero >= 89) && (numero <= 94)))
			{
				if (numero < 92)
				{
					player.ChangeRole(Role.SCP_939_53, false);
				}
				else
				{
					player.ChangeRole(Role.SCP_939_89, false);
				}
			}

		}

		public void OnPlayerHurt(PlayerHurtEvent ev)
		{
			//[Pasiva zombie Cuerpo errante]//
			if (ev.Attacker.TeamRole.Role == Role.SCP_049_2)
			{
				ev.Damage += (ev.Damage / 100) * 15 * Zombie[ev.Attacker.SteamId];
			}
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			Zombie.Clear();
			conta049 = 0;
		}

		public void OnPlayerDie(PlayerDeathEvent ev)
		{
			if (ev.Killer.TeamRole.Role == Smod2.API.Role.SCP_049)
			{

				posmuertee = ev.Player.GetPosition();
				if ((ev.Player.TeamRole.Team == Team.SCIENTIST) || (ev.Player.TeamRole.Team == Team.CLASSD))
				{
					Timing.Run(Resurrec(ev.Player, posmuertee));
					conta049 += 1;
					if (conta049 == 6)
					{
						Timing.Run(Mutar(ev.Player));
						conta049 = 0;
					}
				}

			}
		}
	}
}
