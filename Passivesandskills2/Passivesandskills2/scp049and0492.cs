using System;
using System.Collections.Generic;
using Smod2.EventHandlers;
using scp4aiur;
using Smod2.Events;
using Smod2.API;
using Smod2.Attributes;
using MEC;

namespace Passivesandskills2
{
	partial class scp049and0492 : IEventHandlerSetRole, IEventHandlerPlayerHurt, IEventHandlerWaitingForPlayers, IEventHandlerPlayerDie, IEventHandlerWarheadDetonate
	{
		
		static Dictionary<string, int> Zombie = new Dictionary<string, int>();
        static bool nuke = false;
        Vector posmuertee;
		int conta049 = 0;

        // El 049 cura instantaneo a los clases d y scientist y cada 6 clases d 1 puede mutar con un 35% en otro scp
        // Los zombies cuanto mas tiempo vivan mas daño hacen y reciben  150 de vida 5 veces

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
            
            if ((ev.Player.TeamRole.Role == Role.SCP_049_2))
			{
                conta049 += 1;
                if (!Zombie.ContainsKey(ev.Player.SteamId))
				{
                    ev.Player.PersonalBroadcast(10, "Tu pasiva es [Cuerpo errante]: Cuanto mas tiempo permanezcas con vida mas daño haces (15% + de daño cada 1 minuto de vida).", false);
                    ev.Player.PersonalBroadcast(10, "Tu pasiva es [Cuerpo Creciente]: Cada minuto ganas 150 de salud de forma permanente hasta 5 veces a no ser que te quedes quieto, (perderas la vida extra)", false);
                    Zombie.Add(ev.Player.SteamId, 0);	            
				}
                int p = (int)System.Environment.OSVersion.Platform;
                if ((p == 4) || (p == 6) || (p == 128))
                {
                    MEC.Timing.RunCoroutine(Zombielive(ev.Player), MEC.Segment.FixedUpdate);

                }
                else { MEC.Timing.RunCoroutine(Zombielive(ev.Player), 1); }
                if (conta049 >= 6)
                {
                  
                    if ((p == 4) || (p == 6) || (p == 128))
                    {
                        MEC.Timing.RunCoroutine(Mutar(ev.Player), MEC.Segment.FixedUpdate);

                    }
                    else { MEC.Timing.RunCoroutine(Mutar(ev.Player), 1); }
                    conta049 = 0;
                }
                Zombie[ev.Player.SteamId] = 0;
              

            }
			if (ev.Player.TeamRole.Role == Role.SCP_049)
			{
				ev.Player.SendConsoleMessage("[Mutar]: Cada 6 Zombies curados el zombie número 6 tiene un 35 % de mutar en otro SCP a los 3 minutos, No puede mutar en SCP-096 o en SCP-079, La mutación es totalmente aleatoria ", "red");
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Manipulador de cuerpos]: Curas de forma instantanea a clasesd/scientists [Mutar]: Cada 6 bajas una tiene posibilidades de mutar (mas info en la consola)  .", false);
			}
		}
        //registra los minutos con vida del zombie
		private IEnumerator<float> Zombielive(Player player)
		{
			while (player.TeamRole.Role == Role.SCP_049_2)
			{
                yield return MEC.Timing.WaitForSeconds(60f);
                Zombie[player.SteamId] += 1;
                player.AddHealth(150);
                if(Zombie[player.SteamId] >= 5) { break; }
                if(Zombie[player.SteamId] < 0) { break; }
                
			}
			
		}
        // revive a un zombie
		private IEnumerator<float> Resurrec(Player player, Vector posdead)
		{
            yield return MEC.Timing.WaitForSeconds(3f);
            player.ChangeRole(Role.SCP_049_2);
            yield return MEC.Timing.WaitForSeconds(0.2f);
            player.Teleport(posdead);
		}
        // muta a un zombie en un scp
		private IEnumerator<float> Mutar(Player player)
		{
            if(nuke) { yield break; }
			System.Random proba = new Random();
			int numero = proba.Next(0, 100);
            yield return MEC.Timing.WaitForSeconds(5f);

            if ((numero <= 5))
			{
                player.ChangeRole(Role.SCP_106, false);
                player.SetHealth(325);
            }
            if ((numero <= 10) && (numero >= 6))
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
            //si el zombie tiene 5 minutos de vida instakillea
			if (ev.Attacker.TeamRole.Role == Role.SCP_049_2)
			{
				ev.Damage += (ev.Damage / 100) * 20 * Zombie[ev.Attacker.SteamId];
                if(Zombie[ev.Attacker.SteamId] == 5) { ev.Damage += ev.Player.GetHealth(); }
			}
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			Zombie.Clear();
			conta049 = 0;
            nuke = false;
		}

		public void OnPlayerDie(PlayerDeathEvent ev)
		{// cancela la pasiva del zombie
            if (Zombie.ContainsKey(ev.Player.SteamId)) { Zombie[ev.Player.SteamId]= -1; }
            // revive a clases d y cientificos al instante
			if (ev.Killer.TeamRole.Role == Smod2.API.Role.SCP_049)
			{

				posmuertee = ev.Player.GetPosition();
				if ((ev.Player.TeamRole.Team == Smod2.API.Team.SCIENTIST) || (ev.Player.TeamRole.Team == Smod2.API.Team.CLASSD))
				{
                    ev.SpawnRagdoll = false;
                    int p = (int)System.Environment.OSVersion.Platform;
                    if ((p == 4) || (p == 6) || (p == 128))
                    {
                        MEC.Timing.RunCoroutine(Resurrec(ev.Player,posmuertee), MEC.Segment.FixedUpdate);

                    }
                    else { MEC.Timing.RunCoroutine(Resurrec(ev.Player,posmuertee), 1); }


                }

			}
		}

        public void OnDetonate()
        {
            nuke = true;
        }
    }
}
