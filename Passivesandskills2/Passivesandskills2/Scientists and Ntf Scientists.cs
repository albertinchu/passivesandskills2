using System.Collections.Generic;
using Smod2.EventHandlers;
using scp4aiur;
using Smod2.Events;
using Smod2.API;

namespace Passivesandskills2
{
	partial class Scientists_and_Ntf_Scientists : IEventHandlerPlayerDropItem, IEventHandlerPlayerHurt, IEventHandlerMedkitUse, IEventHandlerSetRole, IEventHandlerWaitingForPlayers
	{
		private Passivesandskills2 plugin;
		public Scientists_and_Ntf_Scientists(Passivesandskills2 plugin)
		{
			this.plugin = plugin;
		}
		static Dictionary<string, bool> Scientisth = new Dictionary<string, bool>();

		public static IEnumerable<float> ScientistTimer(Player player)
		{
			player.SetGodmode(true);
			yield return 5f;
			player.SetGodmode(false);
			yield return 55f;
			player.GiveItem(ItemType.CUP);
			Scientisth[player.SteamId] = true;

		}

		public static IEnumerable<float> Coffe(Player player)
		{
			yield return 5f;
			player.GiveItem(ItemType.CUP);
		}

		public void OnMedkitUse(PlayerMedkitUseEvent ev)
		{
			if (ev.Player.TeamRole.Role == Role.NTF_SCIENTIST)
			{
				ev.RecoverHealth *= 2;
			}
		}

		public void OnPlayerDropItem(PlayerDropItemEvent ev)
		{
			//Scientistsssssss [Café mañanero]//
			if ((ev.Player.TeamRole.Role == Role.SCIENTIST) && (Scientisth[ev.Player.SteamId]) && (ev.Item.ItemType == ItemType.CUP))
			{
				Scientisth[ev.Player.SteamId] = false;
				if (ev.Player.GetHealth() <= 100)
				{
					ev.Player.AddHealth(25);
				}
				Timing.Run(ScientistTimer(ev.Player));
			}
		}

		public void OnPlayerHurt(PlayerHurtEvent ev)
		{
			// [Conocimiento SCP] + Conocimiento SCP Avanzado] //
			//Scientists - Vayne early game//
			if ((ev.Attacker.TeamRole.Role == Role.SCIENTIST) && (ev.Player.TeamRole.Team == Team.SCP))
			{
				if (ev.Attacker.GetHealth() <= 125)
				{
					ev.Attacker.AddHealth(1);
				}
				if (ev.Player.TeamRole.Role == Role.SCP_173) { ev.Damage = ev.Damage + 18; }
				if (ev.Player.TeamRole.Role == Role.SCP_049) { ev.Damage = ev.Damage + 9; }
				if (ev.Player.TeamRole.Role == Role.SCP_049_2) { ev.Damage = ev.Damage + 25; }
				if (ev.Player.TeamRole.Role == Role.SCP_096) { ev.Damage = ev.Damage + 10; }
				if (ev.Player.TeamRole.Role == Role.SCP_106) { ev.Damage = ev.Damage + 3; }
				if (ev.Player.TeamRole.Role == Role.SCP_939_53) { ev.Damage = ev.Damage + 13; }
				if (ev.Player.TeamRole.Role == Role.SCP_939_89) { ev.Damage = ev.Damage + 13; }
			}
			//NTF Scientist - Vayne Late game//
			if ((ev.Attacker.TeamRole.Role == Role.SCIENTIST) && (ev.Player.TeamRole.Team == Team.SCP))
			{
				if (ev.Attacker.GetHealth() <= 150)
				{
					ev.Attacker.AddHealth(3);
				}
				if (ev.Player.TeamRole.Role == Role.SCP_173) { ev.Damage = ev.Damage + 36; }
				if (ev.Player.TeamRole.Role == Role.SCP_049) { ev.Damage = ev.Damage + 18; }
				if (ev.Player.TeamRole.Role == Role.SCP_049_2) { ev.Damage = ev.Damage + 50; }
				if (ev.Player.TeamRole.Role == Role.SCP_096) { ev.Damage = ev.Damage + 20; }
				if (ev.Player.TeamRole.Role == Role.SCP_106) { ev.Damage = ev.Damage + 6; }
				if (ev.Player.TeamRole.Role == Role.SCP_939_53) { ev.Damage = ev.Damage + 26; }
				if (ev.Player.TeamRole.Role == Role.SCP_939_89) { ev.Damage = ev.Damage + 26; }
			}
		}

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			//SCIENTIST//
			if ((ev.Player.TeamRole.Role == Role.SCIENTIST && (!Scientisth.ContainsKey(ev.Player.SteamId))))
			{
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Conocimientos SCP]: robas 1 de vida y inflinges mas daño a los Scps 0.5% de su vida maxima, tu habilidad es [el cafe mañanero]: te hace invulnerable drurante 5 segundos y te cura .", false);
				Timing.Run(Coffe(ev.Player));
				Scientisth.Add(ev.Player.SteamId, true);
			}
			// NTF SCIENTIST //
			if ((ev.Player.TeamRole.Role == Role.NTF_SCIENTIST))
			{
			   
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Conocimientos SCP Avanzados] robas 3 de vida y inflinges mas daño a los Scps 1% de su vida maxima. [Medicina]: los meditkits son el doble de efectivos sobre ti.", false);
			}
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			Scientisth.Clear();
		}
	}
}
