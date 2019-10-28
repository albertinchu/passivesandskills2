using System.Collections.Generic;
using Smod2.EventHandlers;
using scp4aiur;
using Smod2.Events;
using Smod2.API;
using Smod2;

namespace Passivesandskills2
{
	partial class scp096 : IEventHandlerPlayerDie, IEventHandlerSetRole , IEventHandlerWaitingForPlayers, IEventHandlerScp096Panic
	{
		static bool Llorona = true;
        static int muertes = 0;
        

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
					if (player.GetHealth() > 35) 
                    { 
                        player.AddHealth(-35); 
                    }
                    else 
                    {
                        player.Kill(DamageType.CONTAIN); muertes = 1;
                        Llorona = true;
                        break;
                    
                    }
					yield return 3f;

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
				foreach (Player player in PluginManager.Manager.Server.GetPlayers())
				{
					if ((player.TeamRole.Team == Team.SCP) && (player.TeamRole.Role != Role.SCP_079))
					{
						if (player.TeamRole.Role == Role.SCP_096) { player.AddHealth(20); }
						if (player.TeamRole.Role == Role.SCP_049) { player.AddHealth(20); }
						if (player.TeamRole.Role == Role.SCP_049_2) { player.AddHealth(10); }
						if (player.TeamRole.Role == Role.SCP_173) { player.AddHealth(45); }
						if (player.TeamRole.Role == Role.SCP_106) { player.AddHealth(10); }
						if (player.TeamRole.Role == Role.SCP_939_89) { player.AddHealth(40); }
						if (player.TeamRole.Role == Role.SCP_939_53) { player.AddHealth(40); }
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
			
			if (((ev.Player.TeamRole.Role == Role.SCP_096))&&(Llorona == true))
			{
				Llorona = true;
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Gritos de guerra]: Matar a jugadores cura a todo tu equipo y te cura ,Habilidad [Recordatorio mortal]: revives perdiendo vida de forma progresiva.", false);
			}
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			Llorona = true;
			
			bajasllorona = 0;
		}
		public void OnScp096Panic(Scp096PanicEvent ev)
		{
			ev.Player.AddHealth(25 * bajasllorona);
			bajasllorona = 0;
		}
	}
}
