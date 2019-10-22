
using Smod2;
using Smod2.EventHandlers;
using scp4aiur;
using Smod2.Events;
using Smod2.API;
using System.Collections.Generic;

namespace Passivesandskills2
{
	partial class guards : IEventHandlerPlayerHurt, IEventHandlerThrowGrenade, IEventHandlerPlayerDie, IEventHandlerWaitingForPlayers, IEventHandlerCallCommand, IEventHandlerSetRole
	{
		
		static Dictionary<string, int> Guardias = new Dictionary<string, int>();
        // Los guardias tienen un sistema de xp con el que suben de nivel y desbloquean distintas habilidades
        // esta explicado en .info todo lo que hace este codigo.
		public static IEnumerable<float> Venenoguardias(Player player)
		{
			int cantidad = 0;
            
            while (cantidad != 4)
			{
				yield return 2f;
				player.AddHealth(-3);
				cantidad += 1;
			}
			if (cantidad == 4) { cantidad = 0; }

		}


		public void OnPlayerHurt(PlayerHurtEvent ev)
		{
            
            if ((ev.Attacker.TeamRole.Role == Role.FACILITY_GUARD))
            {
                if (!Guardias.ContainsKey(ev.Attacker.SteamId)) { Guardias.Add(ev.Attacker.SteamId, 0); }
                if ((ev.Attacker.TeamRole.Role == Role.CHAOS_INSURGENCY) || (ev.Player.TeamRole.Team == Team.SCP))
                {
                    Guardias[ev.Attacker.SteamId] += 1;
                    if (Guardias[ev.Attacker.SteamId] == 50)
                    {
                        ev.Attacker.PersonalBroadcast(3, "<color=#FF05FF> Nivel 2 </color>", false);
                        ev.Attacker.SetHealth(150, DamageType.CONTAIN);
                        ev.Attacker.SetAmmo(AmmoType.DROPPED_5, 500);
                        ev.Attacker.SetAmmo(AmmoType.DROPPED_7, 500);
                        ev.Attacker.SetAmmo(AmmoType.DROPPED_9, 500);
                    }
                    if (Guardias[ev.Attacker.SteamId] == 150)
                    {
                        ev.Attacker.PersonalBroadcast(3, "<color=#FF0500> Nivel 3 </color>", false);
                    }
                    if (Guardias[ev.Attacker.SteamId] >= 150) { Timing.Run(Venenoguardias(ev.Player)); }
                    if (Guardias[ev.Attacker.SteamId] == 300)
                    {
                        ev.Attacker.PersonalBroadcast(3, "<color=#C50000> Nivel 4 </color>", false);
                        ev.Attacker.GiveItem(ItemType.FRAG_GRENADE);
                        ev.Attacker.SetHealth(300);
                    }
                    if (Guardias[ev.Attacker.SteamId] == 550)
                    {
                        ev.Attacker.PersonalBroadcast(3, "<color=#C50000> Nivel 5 </color>", false);
                    }
                }
            }
		}

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			if ((ev.Player.TeamRole.Role == Role.FACILITY_GUARD) && (!Guardias.ContainsKey(ev.Player.SteamId)))
			{
				Guardias.Add(ev.Player.SteamId, 0);
				ev.Player.SendConsoleMessage("[Cazadores]: Ganancia de XP = 1 por atacar un SCP, 1 por atacar a un chaos, 30 por eliminar un chaos o zombie , (60-100) por eliminar un SCP. Nivel: 2 Ganas 500 de todas las municiones y vida, Nivel 3 Ganas Veneno el las balas que causa 3 de daño adicional, Nivel: 4 Ganas 1 granada y cada vez que la lanzas la vuelves a obtener y obtienes mas vida, Nivel 5 nueva pasiva [Mismo destino]: te llevas a tu asesino con tigo ");
				
			}
            if(ev.Player.TeamRole.Role == Role.FACILITY_GUARD)
            {
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Cazadores]: subes de nivel por atacar y matar scps y chaos, recompensas por nivel en la consola.", false);
            }
            
        }

		public void OnThrowGrenade(PlayerThrowGrenadeEvent ev)
		{
			if ((ev.Player.TeamRole.Role == Role.FACILITY_GUARD) && (Guardias[ev.Player.SteamId] >= 300) && (ev.GrenadeType == GrenadeType.FRAG_GRENADE))
			{
				ev.Player.GiveItem(ItemType.FRAG_GRENADE);

			}
		}

		public void OnPlayerDie(PlayerDeathEvent ev)
		{
            if ((ev.Killer.TeamRole.Role == Role.FACILITY_GUARD) && ((ev.Player.TeamRole.Role == Role.SCP_939_53)||(ev.Player.TeamRole.Role == Role.SCP_939_53))) { Guardias[ev.Player.SteamId] += 80; }
            if ((ev.Killer.TeamRole.Role == Role.FACILITY_GUARD) && (ev.Player.TeamRole.Role == Role.SCP_096)) { Guardias[ev.Player.SteamId] += 75; }
            if ((ev.Killer.TeamRole.Role == Role.FACILITY_GUARD) && (ev.Player.TeamRole.Role == Role.SCP_173)) { Guardias[ev.Player.SteamId] += 75; }
            if ((ev.Killer.TeamRole.Role == Role.FACILITY_GUARD) && (ev.Player.TeamRole.Role == Role.SCP_106)) { Guardias[ev.Player.SteamId] += 70; }
            if ((ev.Killer.TeamRole.Role == Role.FACILITY_GUARD) && (ev.Player.TeamRole.Role == Role.SCP_049)) { Guardias[ev.Player.SteamId] += 70; }
            if ((ev.Killer.TeamRole.Role == Role.FACILITY_GUARD) && (ev.Player.TeamRole.Role == Role.SCP_049_2)) { Guardias[ev.Player.SteamId] += 30; }
            if ((ev.Killer.TeamRole.Role == Role.FACILITY_GUARD) && (ev.Player.TeamRole.Role == Role.CHAOS_INSURGENCY)) { Guardias[ev.Player.SteamId] += 30; }
			if ((ev.Player.TeamRole.Role == Role.FACILITY_GUARD) && (Guardias[ev.Player.SteamId] >= 550))
			{
				ev.Killer.ChangeRole(Role.SPECTATOR);
				PluginManager.Manager.Server.Map.Broadcast(4, "<color=#ABABAB" + ev.Player.Name + "</color> se llevó a <color=#C50000> " + ev.Killer.Name + " </color> con él...", false);
			}
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			Guardias.Clear();
		}

        public void OnCallCommand(PlayerCallCommandEvent ev)
        {
            if (ev.Command.StartsWith("passivesandskillslevel"))
            {
                if ((Guardias.ContainsKey(ev.Player.SteamId))&& (ev.Player.TeamRole.Role == Role.FACILITY_GUARD))
                {
                    ev.Player.SendConsoleMessage("Tu Xp es " + Guardias[ev.Player.SteamId].ToString(), "blue"); 
                    if(Guardias[ev.Player.SteamId] <= 49)
                    {
                        ev.Player.SendConsoleMessage("Tu Nivel es 1", "blue");
                    }
                    if ((Guardias[ev.Player.SteamId] >= 50)&&(Guardias[ev.Player.SteamId] <= 149))
                    {
                        ev.Player.SendConsoleMessage("Tu Nivel es 2", "blue");

                    }
                    if ((Guardias[ev.Player.SteamId] >= 150) && (Guardias[ev.Player.SteamId] <= 299))
                    {
                        ev.Player.SendConsoleMessage("Tu Nivel es 3", "blue");
                    }
                    if ((Guardias[ev.Player.SteamId] >= 300) && (Guardias[ev.Player.SteamId] <= 449))
                    {
                        ev.Player.SendConsoleMessage("Tu Nivel es 4", "blue");
                    }
                    if ((Guardias[ev.Player.SteamId] >= 550) )
                    {
                        ev.Player.SendConsoleMessage("Nivel Máximo, Nivel 5", "red");
                    }


                }
                if ((ev.Player.TeamRole.Role != Role.FACILITY_GUARD))
                {
                    ev.Player.SendConsoleMessage("Tu no eres un guradia", "blue");

                }
                if (!Guardias.ContainsKey(ev.Player.SteamId))
                {
                    ev.Player.SendConsoleMessage("O tu no eres un guardia o estas mas bugeado que este juego", "blue");
                }


            }
        }
    }
}
