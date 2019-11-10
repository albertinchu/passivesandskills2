using System.Collections.Generic;
using Smod2.EventHandlers;
using Smod2.Events;
using Smod2.API;
using Smod2;
using MEC;

namespace Passivesandskills2
{
	partial class scp096 : IEventHandlerPlayerDie, IEventHandlerSetRole , IEventHandlerWaitingForPlayers, IEventHandlerScp096Panic
	{
		static bool Llorona = true;
        static int muertes = 0;
        

		int bajasllorona = 0;
		Vector llorondead;
        // evita errores de respawn doble
        private IEnumerator<float> Lloron2()
        {
            yield return MEC.Timing.WaitForSeconds(0.3f);
            Llorona = false;
        }
        // revive al 096 cuando muere y lo va matando poco a poco
        private IEnumerator<float> LLORON(Player player, Vector pos)
		{

            yield return MEC.Timing.WaitForSeconds(1f);
            player.ChangeRole(Role.SCP_096);
            yield return MEC.Timing.WaitForSeconds(0.2f);
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
                    yield return MEC.Timing.WaitForSeconds(3f);

                }
                else 
                {
                    break;
                }
				

			}

            yield return 5f;
            muertes = 0;
		}

		public void OnPlayerDie(PlayerDeathEvent ev)
		{
            // cura a todos los scps cuando asesina a alguien 
			if (ev.Killer.TeamRole.Role == Role.SCP_096)
			{
				bajasllorona += 1;
				foreach (Player player in PluginManager.Manager.Server.GetPlayers())
				{
					if ((player.TeamRole.Team == Smod2.API.Team.SCP) && (player.TeamRole.Role != Role.SCP_079))
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
            //revive al 096
			if ((ev.Player.TeamRole.Role == Role.SCP_096)&&(Llorona == true))
			{
                    ev.SpawnRagdoll = false;
					llorondead = ev.Player.GetPosition();
                int p = (int)System.Environment.OSVersion.Platform;
                if ((p == 4) || (p == 6) || (p == 128))
                {
                    MEC.Timing.RunCoroutine(LLORON(ev.Player,llorondead), MEC.Segment.FixedUpdate);

                }
                else { MEC.Timing.RunCoroutine(LLORON(ev.Player,llorondead), 1); }
                
                if ((p == 4) || (p == 6) || (p == 128))
                {
                    MEC.Timing.RunCoroutine(Lloron2(), MEC.Segment.FixedUpdate);

                }
                else { MEC.Timing.RunCoroutine(Lloron2(), 1); }


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
            // cuando scp 096 se enfada se cura 25 por el numero de bajas en el rage anterior
			ev.Player.AddHealth(30 * bajasllorona);
			bajasllorona = 0;
		}
	}
}
