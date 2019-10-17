using System;
using System.Collections.Generic;
using Smod2.EventHandlers;
using scp4aiur;
using Smod2.Events;
using Smod2.API;
using Smod2.Attributes;

namespace Passivesandskills2
{
	partial class scp049and0492 : IEventHandlerSetRole, IEventHandlerPlayerHurt, IEventHandlerWaitingForPlayers, IEventHandlerPlayerDie
	{
		
		static Dictionary<string, int> Zombie = new Dictionary<string, int>();
        static Dictionary<string, Role> Roles = new Dictionary<string, Role>();
        Vector posmuertee;
		int conta049 = 0;

        // El 049 cura instantaneo a los clases d y scientist y cada 6 clases d 1 puede mutar con un 35% en otro scp
        // Los zombies cuanto mas tiempo vivan mas daño hacen

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
            if (!Roles.ContainsKey(ev.Player.SteamId)) { Roles.Add(ev.Player.SteamId, ev.Player.TeamRole.Role); }
            Roles[ev.Player.SteamId] = ev.Player.TeamRole.Role;
            if ((ev.Player.TeamRole.Role == Role.SCP_049_2))
			{
                conta049 += 1;
                if (!Zombie.ContainsKey(ev.Player.SteamId))
				{                
					Zombie.Add(ev.Player.SteamId, 0);	            
				}
                Timing.Run(Zombielive(ev.Player));
                if (conta049 >= 6)
                {
                    Timing.Run(Mutar(ev.Player));
                    conta049 = 0;
                }
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Cuerpo errante]: Cuanto mas tiempo permanezcas con vida mas daño haces (15% + de daño cada 1 minuto de vida).", false);
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Cuerpo Creciente]: Cada minuto ganas 75 de salud de forma permanente a no ser que te quedes quieto, (perderas la vida extra)", false);

            }
			if (ev.Player.TeamRole.Role == Role.SCP_049)
			{
				ev.Player.SendConsoleMessage("[Mutar]: Cada 6 Zombies curados el zombie número 6 tiene un 35 % de mutar en otro SCP a los 3 minutos, No puede mutar en SCP-096 o en SCP-079, La mutación es totalmente aleatoria ", "red");
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Manipulador de cuerpos]: Curas de forma instantanea a clasesd/scientists [Mutar]: Cada 6 bajas una tiene posibilidades de mutar (mas info en la consola)  .", false);
			}
		}

		public static IEnumerable<float> Zombielive(Player player)
		{
			while (Roles[player.SteamId] == Role.SCP_049_2)
			{
				yield return 60f;
				Zombie[player.SteamId] += 1;
                player.AddHealth(75);
			}
			if (Roles[player.SteamId] != Role.SCP_049_2)
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
			{
                player.ChangeRole(Role.SCP_106, false);
                player.SetHealth(325);
            }
            if ((numero >= 10) && (numero <= 6))
            {
                player.ChangeRole(Role.SCP_049, false);
                player.SetHealth(1000);
            }
			if ((numero >= 16) && (numero <= 20))
			{
                player.ChangeRole(Role.SCP_049, false);
                player.SetHealth(1000);
            }
			if ((numero >= 95) && (numero <= 100))
			{
                player.ChangeRole(Role.SCP_173, false);
                player.SetHealth(1600);
                yield return 0.2f;
                player.Teleport(Smod2.PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_049));
            }
			if (((numero >= 89) && (numero <= 94)))
			{
				if (numero < 92)
				{
					player.ChangeRole(Role.SCP_939_53, false);
                    player.SetHealth(1600);
                }
				else
				{
					player.ChangeRole(Role.SCP_939_89, false);
                    player.SetHealth(1600);
                }
			}

		}

		public void OnPlayerHurt(PlayerHurtEvent ev)
		{
			//[Pasiva zombie Cuerpo errante]//
			if (ev.Attacker.TeamRole.Role == Role.SCP_049_2)
			{
				ev.Damage += (ev.Damage / 100) * 20 * Zombie[ev.Attacker.SteamId];
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
					
					
				}

			}
		}
	}
}
