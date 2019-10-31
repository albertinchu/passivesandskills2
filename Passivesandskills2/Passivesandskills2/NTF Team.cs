using System;
using System.Collections.Generic;
using Smod2;
using Smod2.EventHandlers;
using scp4aiur;
using Smod2.Events;
using Smod2.API;

namespace Passivesandskills2
{
	partial class Ntfteam : IEventHandlerSetRole, IEventHandlerPlayerHurt, IEventHandlerWaitingForPlayers, IEventHandlerDisconnect, IEventHandlerThrowGrenade, IEventHandlerPlayerDie
	{
		
		static Dictionary<string, bool> NTFli = new Dictionary<string, bool>();
        


        // el comandante hace mas daño segun los jugadores NTF vivos y sus granadas aplican 200 de salud ademas de curar a sus aliados cuando disparan a aliados
        // el Teniente tiene la habilidad de teletransportar a un enemigo no scp a una sala aleatoria , funciona con los zombies.
        // cadetes resisten mejor el daño de explosiones y las flash agregan 30 de salud
        public void OnPlayerHurt(PlayerHurtEvent ev)
		{
			// COMANDANTE //
		
            if ((ev.Attacker.TeamRole.Role == Role.NTF_COMMANDER)&&((ev.Player.TeamRole.Team == Team.SCP)||(ev.Player.TeamRole.Team == Team.CLASSD) || (ev.Player.TeamRole.Team == Team.CHAOS_INSURGENCY))) 
            {
                ev.Damage += 15;
                if(ev.Player.GetHealth() <= ev.Player.TeamRole.MaxHP / 2) 
                {
                    ev.Damage += 15;
                }
            }


            
		
			// CADETES //
			if ((ev.Player.TeamRole.Role == Role.NTF_CADET) && (ev.DamageType == DamageType.FRAG))
			{
				ev.Damage /= 2;
			}
			// TENIENTE //
			if (ev.Attacker.TeamRole.Role == Role.NTF_LIEUTENANT)
			{
				
                if ((ev.Player.TeamRole.Role == Role.CLASSD) || (ev.Player.TeamRole.Role == Role.SCIENTIST))
                {
                    if (ev.Player.TeamRole.Role == Role.SCIENTIST) { ev.Damage = ev.Damage / 3; }
                    if ((ev.Player.GetHealth() <= 50) && (NTFli[ev.Attacker.SteamId] == true))
                    {
                        NTFli[ev.Attacker.SteamId] = false;
                        Timing.Run(Intimidacion(ev.Player));
                        Timing.Run(Cooldown(ev.Attacker));

                    }
                }
                    if(NTFli[ev.Attacker.SteamId] == false)
                    {
                        if(ev.Player.TeamRole.Team == Team.SCP)
                        ev.Damage += ev.Player.GetHealth()/100;
                        if(ev.Player.TeamRole.Team != Team.SCP)
                        {
                            ev.Damage += 15;
                            if(ev.Player.TeamRole.Role == Role.CHAOS_INSURGENCY)
                            {
                                ev.Damage += 10;
                            }
                        }
                    }

				
				if ((ev.Player.TeamRole.Role == Role.CHAOS_INSURGENCY))
				{
					if ((ev.Player.GetHealth() <= 85) && (NTFli[ev.Attacker.SteamId] == true))
					{
						NTFli[ev.Attacker.SteamId] = false;
						Timing.Run(Intimidacion(ev.Player));
						Timing.Run(Cooldown(ev.Attacker));
						
					}
				}
				if(ev.Player.TeamRole.Role == Role.SCP_049_2)
				{
					if ((ev.Player.GetHealth() <= (ev.Player.TeamRole.MaxHP/2)) && (NTFli[ev.Attacker.SteamId] == true))
					{
						NTFli[ev.Attacker.SteamId] = false;
						Timing.Run(Intimidacion(ev.Player));
						Timing.Run(Cooldown(ev.Attacker));
						
					}
				}
			}
		}
		// Teniente //
		public static IEnumerable<float> Cooldown(Player player)
		{
			yield return 40f;
			NTFli[player.SteamId] = true;
		}
		// TENEINETE HABILIDAD //
		public static IEnumerable<float> Intimidacion(Player player)
		{
            System.Random sala = new System.Random();

			int contadorb = sala.Next(0,100);
            
            yield return 0.1f;
            if (player.TeamRole.Role != Role.SCIENTIST)
            {
                if ((contadorb >= 0) && (contadorb <= 33))
                {
                    player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_173));
                }           
                if ((contadorb >= 34) && (contadorb <= 50))
                {
                    player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_049));
                }
                if ((contadorb >= 51) && (contadorb <= 81))
                {
                    player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_939_53));
                }
                if ((contadorb >= 82) && (contadorb <= 95))
                {
                    player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.FACILITY_GUARD));
                }
                if ((contadorb >= 96) && (contadorb <= 100))
                {
                    player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCIENTIST));
                }
            }
            if (player.TeamRole.Role == Role.SCIENTIST)
            {
             
                if ((contadorb >= 0) && (contadorb <= 33))
                {
                    player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_096));
                }
                if ((contadorb >= 34) && (contadorb <= 50))
                {
                    player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_049));
                }
                if ((contadorb >= 51) && (contadorb <= 74))
                {
                    player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_939_53));
                }
                if ((contadorb >= 75) && (contadorb <= 100))
                {
                    player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.FACILITY_GUARD));
                }
                
            }
            yield return 1f;
            if(player.TeamRole.Role == Role.SCIENTIST) { player.AddHealth(25); }
        }



		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			// TENIENTE //
            
			if (ev.Player.TeamRole.Role == Role.NTF_LIEUTENANT)
			{
				if (!NTFli.ContainsKey(ev.Player.SteamId))
				{
					NTFli.Add(ev.Player.SteamId, true);
                    ev.Player.SendConsoleMessage("[cambiar las tornas]: Cambiar las tornas es una pasiva Tactica con 40s de cooldown  la cual teletransporta al enemigo cuando este esta a menos del 50% de vida . (Esta habilidad no se aplica a SCPS pero si a Zombies y tampoco se aplica a aliados)", "blue");
                    ev.Player.PersonalBroadcast(10, "Tu pasiva es [cambiar las tornas]: Cambias la posición del enemigo de forma aleatoria cuando esta por debajo de 50% atrapandolo (mas info en la consola), cuando has usado tu habilidad durante 40s tienes la pasiva", false);
                    ev.Player.PersonalBroadcast(10, "De servicio que aumenta tu daño en 15 a todos los objetivos y es el doble de daño contra chaos, con SCPS 1% de la vida actual del SCP", false);
                }
				
				
            }
            
			// CADETE //
			if (ev.Player.TeamRole.Role == Role.NTF_CADET)
			{
				
				ev.Player.SendConsoleMessage("[Flash rápido]: Tras lanzar una granada cegadora obtienes un escudo de 30 de salud, (este se anula si el comandante usa su granada para aplicarte 200 de salud pero se acumula si se aplicó los 200 de salud antes)", "blue");
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Tenacidad explosiva]: Recibes daño reducido entre 2 de las granadas.[Flash Rápido]: (mas info en la consola)", false);
			}
			// COMANDANTE //
			if ((ev.Player.TeamRole.Role == Role.NTF_COMMANDER))
			{
				
				ev.Player.SendConsoleMessage("[Preocupación por los tuyos]: Tus disparos hacen como cura la mitad del daño que causarían a tus aliados y las granadas Instacuran 200 de salud (¡OJO!: No se aplica a guardias ni científicos", "blue");
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Lider del Escudrón]: Inflinges daño adicional a secas (15)[Preocupación por los tuyos]: tus ataques curan aliados (mas info en la consola)", false);
			}
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			NTFli.Clear();
			
		}

		public void OnThrowGrenade(PlayerThrowGrenadeEvent ev)
		{
            if((ev.Player.TeamRole.Role == Role.NTF_CADET)&&(ev.GrenadeType == GrenadeType.FRAG_GRENADE))
            {
                ev.Player.AddHealth(20);
                ev.Player.GiveItem(ItemType.FLASHBANG);
            }
			if ((ev.Player.TeamRole.Role == Role.NTF_CADET) && (ev.GrenadeType == GrenadeType.FLASHBANG))
			{
				ev.Player.AddHealth(30);
			}
		}
		public void OnDisconnect(DisconnectEvent ev)
		{
			
			foreach (Player player in PluginManager.Manager.Server.GetPlayers())
			{
				if((player.TeamRole.Team == Team.NINETAILFOX)&&(player.TeamRole.Role != Role.FACILITY_GUARD))
				{
					
				}
			}
		}

        public void OnPlayerDie(PlayerDeathEvent ev)
        {
            if((ev.Player.TeamRole.Role == Role.NTF_COMMANDER)&&(ev.Killer.TeamRole.Team == Team.NINETAILFOX)&&(ev.Killer.TeamRole.Role != Role.FACILITY_GUARD))
            { ev.Killer.Kill(DamageType.LURE); }


        }
    }
}
