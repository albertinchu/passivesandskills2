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
              
				
                    ev.Player.PersonalBroadcast(10, "Tu pasiva es [Recuerdos]:Recibes efectos según tu role anterior, si eras clase D: eres invisible cada 5s.", false);
                    ev.Player.PersonalBroadcast(10, "Cientifico: inmortalidad cada 5s, cadete resistencia a disparos x/2 ", false);
                    ev.Player.PersonalBroadcast(10, "Comandante: todos los efectos menos el clase d, cientifico NTF: dañar te cura 700 de vida", false);
                    ev.Player.PersonalBroadcast(10, "Chaos: matas de un hit, Guardia: reflejas daño/10, Teniente: al morir explotas sin causar daño a aliados", false);
                int p = (int)System.Environment.OSVersion.Platform;
                if (Zombie.ContainsKey(ev.Player.SteamId)) 
                {
                   
                    if (Zombie[ev.Player.SteamId] == 0)
                    {

                        if ((p == 4) || (p == 6) || (p == 128))
                        {
                            MEC.Timing.RunCoroutine(Zombie0(ev.Player), MEC.Segment.FixedUpdate);

                        }
                        else { MEC.Timing.RunCoroutine(Zombie0(ev.Player), 1); }
                    }
                    if ((Zombie[ev.Player.SteamId] == 1)||(Zombie[ev.Player.SteamId ]== 7))
                    {

                        if ((p == 4) || (p == 6) || (p == 128))
                        {
                            MEC.Timing.RunCoroutine(Zombie1(ev.Player), MEC.Segment.FixedUpdate);

                        }
                        else { MEC.Timing.RunCoroutine(Zombie1(ev.Player), 1); }
                    }

                }
				
             
                if (conta049 >= 6)
                {
                  
                    if ((p == 4) || (p == 6) || (p == 128))
                    {
                        MEC.Timing.RunCoroutine(Mutar(ev.Player), MEC.Segment.FixedUpdate);

                    }
                    else { MEC.Timing.RunCoroutine(Mutar(ev.Player), 1); }
                    conta049 = 0;
                }
                
              

            }
			if (ev.Player.TeamRole.Role == Role.SCP_049)
			{
				ev.Player.SendConsoleMessage("[Mutar]: Cada 6 Zombies curados el zombie número 6 tiene un 35 % de mutar en otro SCP a los 3 minutos, No puede mutar en SCP-096 o en SCP-079, La mutación es totalmente aleatoria ", "red");
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Manipulador de cuerpos]: Curas de forma instantanea a clasesd/scientists [Mutar]: Cada 6 bajas una tiene posibilidades de mutar (mas info en la consola)  .", false);
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Padre vengativo]: cuando muere 1 zombie todo plaga se cura 75 de hp   .", false);
            }
		}
     //class d
		private IEnumerator<float> Zombie0(Player player)
		{
			while (player.TeamRole.Role == Role.SCP_049_2)
			{
                yield return MEC.Timing.WaitForSeconds(5f);              
                player.SetGhostMode(true);
                yield return MEC.Timing.WaitForSeconds(5f);
                player.SetGhostMode(false);
            }
			
		}
        //Scientis
        private IEnumerator<float> Zombie1(Player player)
        {
            while (player.TeamRole.Role == Role.SCP_049_2)
            {
                yield return MEC.Timing.WaitForSeconds(5f);
                player.SetGodmode(true);
                yield return MEC.Timing.WaitForSeconds(5f);
                player.SetGodmode(false);
            }
        }
    
     
        //
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
            if (Zombie.ContainsKey(ev.Player.SteamId))
            {
                if (ev.Player.TeamRole.Role == Role.SCP_049_2)
                {

                    if ((Zombie[ev.Player.SteamId] == 7) || (Zombie[ev.Player.SteamId] == 3))
                    {
                        ev.Damage /= 2;
                    }
                    if ((Zombie[ev.Player.SteamId] == 7) || (Zombie[ev.Player.SteamId] == 2))
                    {
                        ev.Attacker.SetHealth(ev.Attacker.GetHealth() - 1);
                    }
                }

                if ((ev.Attacker.TeamRole.Role == Role.SPECTATOR) && (Zombie[ev.Player.SteamId] == -1))
                {
                    if (ev.DamageType == DamageType.FRAG)
                    {
                        if (ev.Player.TeamRole.Team == Smod2.API.Team.SCP) { ev.Damage = 0; } else { ev.Damage = 90; }

                    }
                }
                if (ev.Attacker.TeamRole.Role == Role.SCP_049_2)
                {

                    if (ev.DamageType == DamageType.FRAG)
                    {
                        if (ev.Player.TeamRole.Team == Smod2.API.Team.SCP) { ev.Damage = 0; } else { ev.Damage = 90; }

                    }
                    if (Zombie.ContainsKey(ev.Attacker.SteamId))
                    {
                        if ((Zombie[ev.Attacker.SteamId] == 5) || (Zombie[ev.Attacker.SteamId] == 7))
                        {
                            ev.Damage += ev.Player.GetHealth();
                        }
                        if ((Zombie[ev.Attacker.SteamId] == 6) || (Zombie[ev.Attacker.SteamId] == 7))
                        {
                            ev.Attacker.AddHealth(700);
                        }
                    }
                   
                }
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
         if(ev.Player.TeamRole.Role == Role.SCP_049_2) 
         {
                if (!Zombie.ContainsKey(ev.Player.SteamId)) { Zombie.Add(ev.Player.SteamId, 0); }
                if ((Zombie[ev.Player.SteamId] == 4)||(Zombie[ev.Player.SteamId] == 7)) { ev.Player.ThrowGrenade(GrenadeType.FRAG_GRENADE, true, new Vector(0, 0, 0), true, ev.Player.GetPosition(), true, 0, true); }
                Zombie[ev.Player.SteamId] = -1;
                foreach(Player player in Smod2.PluginManager.Manager.Server.GetPlayers())
                {
                    if(player.TeamRole.Role == Role.SCP_049) 
                    {
                        player.AddHealth(75);
                    }
                }
            
         }       
			if ((ev.Killer.TeamRole.Role == Smod2.API.Role.SCP_049)||(ev.Killer.TeamRole.Role == Role.SCP_049_2))
			{
                if(ev.Player.TeamRole.Role == Role.CLASSD) 
                {
                    if (!Zombie.ContainsKey(ev.Player.SteamId)) 
                    {
                        Zombie.Add(ev.Player.SteamId, 0);
                    }
                    else 
                    {
                        Zombie[ev.Player.SteamId] = 0;
                    }
                }
                if (ev.Player.TeamRole.Role == Role.SCIENTIST)
                {
                    if (!Zombie.ContainsKey(ev.Player.SteamId))
                    {
                        Zombie.Add(ev.Player.SteamId, 1);
                    }
                    else
                    {
                        Zombie[ev.Player.SteamId] = 1;
                    }
                }
                if (ev.Player.TeamRole.Role == Role.FACILITY_GUARD)
                {
                    if (!Zombie.ContainsKey(ev.Player.SteamId))
                    {
                        Zombie.Add(ev.Player.SteamId, 2);
                    }
                    else
                    {
                        Zombie[ev.Player.SteamId] = 2;
                    }
                }
                if (ev.Player.TeamRole.Role == Role.NTF_CADET)
                {
                    if (!Zombie.ContainsKey(ev.Player.SteamId))
                    {
                        Zombie.Add(ev.Player.SteamId, 3);
                    }
                    else
                    {
                        Zombie[ev.Player.SteamId] = 3;
                    }
                }
                if (ev.Player.TeamRole.Role == Role.NTF_LIEUTENANT)
                {
                    if (!Zombie.ContainsKey(ev.Player.SteamId))
                    {
                        Zombie.Add(ev.Player.SteamId, 4);
                    }
                    else
                    {
                        Zombie[ev.Player.SteamId] = 4;
                    }
                }
                if (ev.Player.TeamRole.Role == Role.CHAOS_INSURGENCY)
                {
                    if (!Zombie.ContainsKey(ev.Player.SteamId))
                    {
                        Zombie.Add(ev.Player.SteamId, 5);
                    }
                    else
                    {
                        Zombie[ev.Player.SteamId] = 5;
                    }
                }
                if (ev.Player.TeamRole.Role == Role.NTF_SCIENTIST)
                {
                    if (!Zombie.ContainsKey(ev.Player.SteamId))
                    {
                        Zombie.Add(ev.Player.SteamId, 6);
                    }
                    else
                    {
                        Zombie[ev.Player.SteamId] = 6;
                    }
                }
                if (ev.Player.TeamRole.Role == Role.NTF_COMMANDER)
                {
                    if (!Zombie.ContainsKey(ev.Player.SteamId))
                    {
                        Zombie.Add(ev.Player.SteamId, 7);
                    }
                    else
                    {
                        Zombie[ev.Player.SteamId] = 7;
                    }
                }
                // revive a clases d y cientificos al instante
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
