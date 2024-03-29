﻿
using System.Collections.Generic;
using Smod2;
using Smod2.EventHandlers;
using Smod2.Events;
using Smod2.API;


namespace Passivesandskills2
{
	partial class scp106 : IEventHandlerSetRole, IEventHandlerPocketDimensionDie, IEventHandlerPlayerHurt, IEventHandlerWaitingForPlayers, IEventHandlerPlayerDie
	{
	
		static Dictionary<string, int> Scp106 = new Dictionary<string, int>();
        // elimina cuerpos pero no objetos
        public void OnPlayerDie(PlayerDeathEvent ev)
        {
            if(ev.DamageTypeVar == DamageType.POCKET )
            { 
                ev.SpawnRagdoll = false; 
                
            }
        }
        scp939_xx scp;
        public void OnPlayerHurt(PlayerHurtEvent ev)
		{
            //[Presencia Espectral] elimina ghost mode al atacar a un jugador en estado de invisibilidad
            if((ev.Attacker.GetGhostMode() == true)&&(ev.Attacker.TeamRole.Role == Role.SCP_106)) { ev.Attacker.SetGhostMode(false); }
			//[Golpe Crítico]// ejecuta a un jugador
			if ((ev.Attacker.TeamRole.Role == Role.SCP_106))
			{
                
                Scp106[ev.Attacker.SteamId] += 1;
				if (Scp106[ev.Attacker.SteamId] >= 5)
				{
                    ev.Attacker.SetGhostMode(true);
					ev.Player.Kill(DamageType.SCP_106);
					Scp106[ev.Attacker.SteamId] = 0;
				}
			}
		}

		public void OnPocketDimensionDie(PlayerPocketDimensionDieEvent ev)
		{
         //cura a todos los SCP 106 30 de salud cuando un jugador muere en la pocket   
            foreach (Player player in PluginManager.Manager.Server.GetPlayers())
            {
                if (player.TeamRole.Role == Role.SCP_106) { player.AddHealth(30); }
            }
        }

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
            if ((ev.Player.TeamRole.Role == Role.SCP_106) && (!Scp106.ContainsKey(ev.Player.SteamId)))
            {
                Scp106.Add(ev.Player.SteamId, 0);
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Digestión]: Te curas cuando alguien muere en tu dimensión [Golpe Crítico]: Tu quinta victima muere al instante.", false);
                ev.Player.PersonalBroadcast(10, "Tu Habilidad es [Presencia Espectral]: Cuando ejecutas a una persona por Golpe crítico te haces invisible hasta que ataques a otra persona.", false);
            }
           
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			Scp106.Clear();
		}

       
    }
}
