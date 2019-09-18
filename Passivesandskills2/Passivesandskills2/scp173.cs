using System.Collections.Generic;
using Smod2.EventHandlers;
using Smod2.Events;
using Smod2.API;
using scp4aiur;
namespace Passivesandskills2
{
	partial class scp173 : IEventHandlerPlayerDie, IEventHandlerSetRole, IEventHandlerWaitingForPlayers
	{
	
		static Dictionary<string, bool> Scp173 = new Dictionary<string, bool>();
		static Dictionary<string, Vector> Scp173pos = new Dictionary<string, Vector>();
        static Dictionary<string, bool> Scp173deads = new Dictionary<string, bool>();

        public static IEnumerable<float> Scp173timer(Player player, Vector pos)
		{

			yield return 60f;


			Scp173[player.SteamId] = false;
			player.ChangeRole(Role.SCP_173);
			yield return 0.2f;
			player.Teleport(pos);

			while (Scp173deads[player.SteamId])
			{
				if (player.TeamRole.Role == Role.SCP_173)
				{
					player.SetGhostMode(true, false, false);
					yield return 3f;
					player.SetGhostMode(false);
				}
				else
				{
					break;
				}

			}
		}


		public void OnPlayerDie(PlayerDeathEvent ev)
		{
			if (ev.Player.TeamRole.Role == Role.SCP_173)
			{
				Vector posd = ev.Player.GetPosition();

				if (Scp173[ev.Player.SteamId] == true)
				{
                    Scp173deads[ev.Player.SteamId] = true;
                    ev.SpawnRagdoll = false;
					ev.Player.ThrowGrenade(GrenadeType.FRAG_GRENADE, true, posd, true, posd, true, 0, true);
					ev.Player.GiveItem(ItemType.FRAG_GRENADE);
					ev.Player.GiveItem(ItemType.FRAG_GRENADE);
					ev.Player.ThrowGrenade(GrenadeType.FRAG_GRENADE, true, posd, true, posd, true, 0, true);
					ev.Player.GiveItem(ItemType.FRAG_GRENADE);
					ev.Player.GiveItem(ItemType.FRAG_GRENADE);
					ev.Player.ThrowGrenade(GrenadeType.FRAG_GRENADE, true, posd, true, posd, true, 0, true);
					ev.Player.GiveItem(ItemType.FRAG_GRENADE);
                    ev.Player.GiveItem(ItemType.FRAG_GRENADE);
                    ev.Player.ThrowGrenade(GrenadeType.FRAG_GRENADE, true, posd, true, posd, true, 0, true);
					Timing.Run(Scp173timer(ev.Player, posd));
                    
                }
				else
				{
					Scp173[ev.Player.SteamId] = true;
                    Scp173deads[ev.Player.SteamId] = false;
                }
			}

		}

		public void OnSetRole(PlayerSetRoleEvent ev)
		{

			if ((ev.Player.TeamRole.Role == Smod2.API.Role.SCP_173) && (!Scp173.ContainsKey(ev.Player.SteamId)))
			{
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Go big or go Home]: cuando mueres te vas a lo GRANDE, tu habilidad es [Resurgir etereo]: revives al minuto con intervalos de invisibilidad. ", false);
                Scp173deads.Add(ev.Player.SteamId, true);
                Scp173.Add(ev.Player.SteamId, true);
				Scp173pos.Add(ev.Player.SteamId, ev.Player.GetPosition());
			}
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			Scp173.Clear();
			Scp173pos.Clear();
            Scp173deads.Clear();
		}
	}
}
