﻿using System.Collections.Generic;
using Smod2.EventHandlers;
using scp4aiur;
using Smod2.Events;
using Smod2.API;

namespace Passivesandskills2
{
	partial class scp096 : IEventHandlerPlayerDie, IEventHandlerSetRole , IEventHandlerWaitingForPlayers, IEventHandlerScp096Panic
	{
		static bool Llorona = true;
        static int muertes = 0;
        static Dictionary<string, Player> Pasiva = new Dictionary<string, Player>();

		int bajasllorona = 0;
		Vector llorondead;

        public static IEnumerable<float> Lloron2()
        {
            yield return 0.3f;
            Llorona = false;
        }

        public static IEnumerable<float> LLORON(Player player, Vector pos)
		{
            
			yield return 1f;
			player.ChangeRole(Role.SCP_096);
			yield return 0.2f;
			player.Teleport(pos);

			while (muertes == 0)
			{
				if (player.TeamRole.Role == Role.SCP_096)
				{
					if (player.GetHealth() > 35) { player.AddHealth(-35); } else { player.Kill(DamageType.CONTAIN); muertes = 1; }
					yield return 3f;

				}
				else
				{
                    Llorona = true;
                    break;
				}

			}

            yield return 5f;
            muertes = 0;
		}

		public void OnPlayerDie(PlayerDeathEvent ev)
		{
			if (ev.Killer.TeamRole.Role == Role.SCP_096)
			{
				bajasllorona += 1;
				foreach (KeyValuePair<string, Player> keyValuePair in Pasiva)
				{
					if ((keyValuePair.Value.TeamRole.Team == Team.SCP) && (keyValuePair.Value.TeamRole.Role != Role.SCP_079))
					{
						if (keyValuePair.Value.TeamRole.Role == Role.SCP_096) { keyValuePair.Value.AddHealth(20); }
						if (keyValuePair.Value.TeamRole.Role == Role.SCP_049) { keyValuePair.Value.AddHealth(20); }
						if (keyValuePair.Value.TeamRole.Role == Role.SCP_049_2) { keyValuePair.Value.AddHealth(10); }
						if (keyValuePair.Value.TeamRole.Role == Role.SCP_173) { keyValuePair.Value.AddHealth(45); }
						if (keyValuePair.Value.TeamRole.Role == Role.SCP_106) { keyValuePair.Value.AddHealth(10); }
						if (keyValuePair.Value.TeamRole.Role == Role.SCP_939_89) { keyValuePair.Value.AddHealth(20); }
						if (keyValuePair.Value.TeamRole.Role == Role.SCP_939_53) { keyValuePair.Value.AddHealth(20); }
					}

				}
			}
			if ((ev.Player.TeamRole.Role == Role.SCP_096)&&(Llorona == true))
			{
                    ev.SpawnRagdoll = false;
					llorondead = ev.Player.GetPosition();
					Timing.Run(LLORON(ev.Player, llorondead));
                    Timing.Run(Lloron2());


            }
            if ((ev.Player.TeamRole.Role == Role.SCP_096) && (Llorona == false))
            {
                muertes = 0;
                Llorona = true;
                
                
            }

        }

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			if (!Pasiva.ContainsKey(ev.Player.SteamId)) { Pasiva.Add(ev.Player.SteamId, ev.Player); }
			if (((ev.Player.TeamRole.Role == Role.SCP_096))&&(Llorona == true))
			{
				Llorona = true;
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Gritos de guerra]: Matar a jugadores cura a todo tu equipo y te cura ,Habilidad [Recordatorio mortal]: revives perdiendo vida de forma progresiva.", false);
			}
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			Llorona = true;
			Pasiva.Clear();
			bajasllorona = 0;
		}
		public void OnScp096Panic(Scp096PanicEvent ev)
		{
			ev.Player.AddHealth(25 * bajasllorona);
			bajasllorona = 0;
		}
	}
}
