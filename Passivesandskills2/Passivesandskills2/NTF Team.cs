using System;
using System.Collections.Generic;
using Smod2;
using Smod2.EventHandlers;
using scp4aiur;
using Smod2.Events;
using Smod2.API;
namespace Passivesandskills2
{
	partial class Ntfteam : IEventHandlerSetRole, IEventHandlerPlayerHurt, IEventHandlerWaitingForPlayers, IEventHandlerDisconnect, IEventHandlerThrowGrenade
	{
		
		static Dictionary<string, bool> NTFli = new Dictionary<string, bool>();
		int contadorNTF = 0;

		public void OnPlayerHurt(PlayerHurtEvent ev)
		{
			// COMANDANTE //
			if ((ev.Attacker.TeamRole.Role == Role.NTF_COMMANDER) && (ev.Player.TeamRole.Team == Team.NINETAILFOX) && (ev.DamageType != DamageType.FRAG) && (ev.DamageType != DamageType.TESLA) && (ev.DamageType != DamageType.FALLDOWN))
			{
				float damage = ev.Damage;
				ev.Damage = 0;
				if (ev.Player.GetHealth() < 150) { ev.Player.AddHealth((int)Math.Round(damage) / 2); }


			}

			if ((ev.Attacker.TeamRole.Role == Role.NTF_COMMANDER) && (ev.Player.TeamRole.Team == Team.NINETAILFOX) && (ev.DamageType == DamageType.FRAG))
			{
				ev.Damage = 0;
				ev.Player.SetHealth(200, DamageType.FRAG);
			}
			if (ev.Attacker.TeamRole.Role == Role.NTF_COMMANDER)
			{
				ev.Damage += (contadorNTF * 4);
			}
			// CADETES //
			if ((ev.Player.TeamRole.Role == Role.NTF_CADET) && (ev.DamageType == DamageType.FRAG))
			{
				ev.Damage /= 2;
			}
			// TENIENTE //
			if (ev.Attacker.TeamRole.Role == Role.NTF_LIEUTENANT)
			{
				Vector posli = ev.Player.GetPosition();
				if ((ev.Player.TeamRole.Role == Role.CLASSD) || (ev.Player.TeamRole.Role == Role.SCIENTIST))
				{
					if ((ev.Player.GetHealth() <= 50) && (NTFli[ev.Attacker.SteamId] == true))
					{
						NTFli[ev.Attacker.SteamId] = false;
						Timing.Run(Intimidacion(ev.Player, ev.Attacker.GetPosition()));
						Timing.Run(Cooldown(ev.Player));
						ev.Attacker.Teleport(posli);
					}

				}
				if ((ev.Player.TeamRole.Role == Role.CHAOS_INSURGENCY))
				{
					if ((ev.Player.GetHealth() <= 60) && (NTFli[ev.Attacker.SteamId] == true))
					{
						NTFli[ev.Attacker.SteamId] = false;
						Timing.Run(Intimidacion(ev.Player, ev.Attacker.GetPosition()));
						Timing.Run(Cooldown(ev.Player));
						ev.Attacker.Teleport(posli);
					}

				}
				if(ev.Player.TeamRole.Role == Role.SCP_049_2)
				{
					if ((ev.Player.GetHealth() <= (ev.Player.TeamRole.MaxHP/2)) && (NTFli[ev.Attacker.SteamId] == true))
					{
						NTFli[ev.Attacker.SteamId] = false;
						Timing.Run(Intimidacion(ev.Player, ev.Attacker.GetPosition()));
						Timing.Run(Cooldown(ev.Player));
						ev.Attacker.Teleport(posli);
					}
				}
			}
		}
		// Teniente //
		public static IEnumerable<float> Cooldown(Player player)
		{
			yield return 20f;
			NTFli[player.SteamId] = true;
		}
		// TENEINETE HABILIDAD //
		public static IEnumerable<float> Intimidacion(Player player, Vector pos3)
		{
			int contadorb = 0;
			while ((contadorb < 8))
			{
				contadorb += 1;
				yield return 0.25f;
				player.Teleport(pos3);
			}


		}



		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			// TENIENTE //
			if (ev.Player.TeamRole.Role == Role.NTF_LIEUTENANT)
			{
				if (!NTFli.ContainsKey(ev.Player.SteamId))
				{
					NTFli.Add(ev.Player.SteamId, true);
				}
				contadorNTF += 1;
				ev.Player.SendConsoleMessage("[cambiar las tornas]: Cambiar las tornas es una pasiva Tactica con 20s de cooldown  la cual intercambia tu posición con la del enemigo cuando este esta a menos del 50% de vida atrapandolo durante 2 segundos en tu posición. (Esta habilidad no se aplica a SCPS pero si a Zombies y tampoco se aplica a aliados)", "blue");
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [cambiar las tornas]: Cambias la posición del enemigo con la tuya cuando esta por debajo de 50% atrapandolo (mas info en la consola)", false);
			}
			// CADETE //
			if (ev.Player.TeamRole.Role == Role.NTF_CADET)
			{
				contadorNTF += 1;
				ev.Player.SendConsoleMessage("[Flash rápido]: Tras lanzar una granada cegadora obtienes un escudo de 20 de salud, (este se anula si el comandante usa su granada para aplicarte 200 de salud pero se acumula si se aplicó los 200 de salud antes)", "blue");
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Tenacidad explosiva]: Recives daño reducido entre 2 de las granadas.[Flash Rápido]: (mas info en la consola)", false);
			}
			// COMANDANTE //
			if ((ev.Player.TeamRole.Role == Role.NTF_COMMANDER))
			{

				contadorNTF += 1;
				ev.Player.SendConsoleMessage("[Preocupación por los tuyos]: Tus disparos hacen como cura la mitad del daño que causarían a tus aliados y las granadas Instacuran 200 de salud (¡OJO!: No se aplica a guardias ni científicos", "blue");
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Lider del Escudrón]: Inflinges daño adicional segun el número de NTF vivos [Preocupación por los tuyos]: tus ataques curan aliados (mas info en la consola)", false);
			}
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			NTFli.Clear();
			contadorNTF = 0;
		}

		public void OnThrowGrenade(PlayerThrowGrenadeEvent ev)
		{

			if ((ev.Player.TeamRole.Role == Role.NTF_CADET) && (ev.GrenadeType == GrenadeType.FLASHBANG))
			{
				ev.Player.AddHealth(20);
			}
		}
		public void OnDisconnect(DisconnectEvent ev)
		{
			contadorNTF = 0;
			foreach (Player player in PluginManager.Manager.Server.GetPlayers())
			{
				if((player.TeamRole.Team == Team.NINETAILFOX)&&(player.TeamRole.Role != Role.FACILITY_GUARD))
				{
					contadorNTF += 1;
				}
			}
		}
	}
}
