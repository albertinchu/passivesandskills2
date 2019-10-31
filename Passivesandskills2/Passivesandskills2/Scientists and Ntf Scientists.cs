using System.Collections.Generic;
using Smod2.EventHandlers;
using MEC;
using Smod2.Events;
using Smod2.API;
using System;

namespace Passivesandskills2
{
	partial class scientist : IEventHandlerPlayerDropItem, IEventHandlerPlayerHurt, IEventHandlerMedkitUse, IEventHandlerSetRole, IEventHandlerWaitingForPlayers, IEventHandlerCallCommand
	{
		
		static Dictionary<string, bool> Scientisth = new Dictionary<string, bool>();
        static Dictionary<string, bool> Items = new Dictionary<string, bool>();

        // daño % a SCPS y se curan vida por dañar scps
        // el cientifico tiene la habilidad de hacerse inmortal 5 s con su cafe mañanero 60s de cooldown
        // los NTF scientist se curan el doble con los botiquines


        
		
      

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
				MEC.Timing.RunCoroutine(ScientistTimer(ev.Player));
			}
		}

        private IEnumerator<float> ScientistTimer(Player player)
        {

            player.SetGodmode(true);
            yield return MEC.Timing.WaitForSeconds(5f);
            player.SetGodmode(false);
            yield return MEC.Timing.WaitForSeconds(55f);
            if (player.TeamRole.Role == Role.SCIENTIST)
            {
                player.GiveItem(ItemType.CUP);
            }
            Scientisth[player.SteamId] = true;
        }

        public void OnPlayerHurt(PlayerHurtEvent ev)
		{
			// [Conocimiento SCP] + Conocimiento SCP Avanzado] //
			//Scientists - Vayne early game//
			if ((ev.Attacker.TeamRole.Role == Role.SCIENTIST) && (ev.Player.TeamRole.Team == Team.SCP))
			{
				if (ev.Attacker.GetHealth() <= 125)
				{
					ev.Attacker.AddHealth(2);
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
					ev.Attacker.AddHealth(4);
				}
				if (ev.Player.TeamRole.Role == Role.SCP_173) { ev.Damage = ev.Damage + 36; }
				if (ev.Player.TeamRole.Role == Role.SCP_049) { ev.Damage = ev.Damage + 18; }
				if (ev.Player.TeamRole.Role == Role.SCP_049_2) { ev.Damage = ev.Damage + 50; }
				if (ev.Player.TeamRole.Role == Role.SCP_096) { ev.Damage = ev.Damage + 20; }
				if (ev.Player.TeamRole.Role == Role.SCP_106) { ev.Damage = ev.Damage + 6; }
				if (ev.Player.TeamRole.Role == Role.SCP_939_53) { ev.Player.AddHealth(-26); }
				if (ev.Player.TeamRole.Role == Role.SCP_939_89) { ev.Player.AddHealth(-26) ; }
			}
		}

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			//SCIENTIST//
			if ((ev.Player.TeamRole.Role == Role.SCIENTIST && (!Scientisth.ContainsKey(ev.Player.SteamId))))
			{
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Conocimientos SCP]: robas 1 de vida y inflinges mas daño a los Scps 0.5% de su vida maxima, tu habilidad es [el cafe mañanero]: te hace invulnerable drurante 5 segundos y te cura .", false);
				MEC.Timing.RunCoroutine(coffe(ev.Player));
				Scientisth.Add(ev.Player.SteamId, true);
			}
			// NTF SCIENTIST //
			if ((ev.Player.TeamRole.Role == Role.NTF_SCIENTIST))
			{
			   
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Conocimientos SCP Avanzados] robas 3 de vida y inflinges mas daño a los Scps 1% de su vida maxima. [Medicina]: los meditkits son el doble de efectivos sobre ti.", false);
			}
		}

        private IEnumerator<float> coffe(Player player)
        {
            yield return MEC.Timing.WaitForSeconds(5f);
            player.GiveItem(ItemType.CUP);
        }

        public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			Scientisth.Clear();
            Items.Clear();
		}

        public void OnCallCommand(PlayerCallCommandEvent ev)
        {
            if (ev.Command.StartsWith("habilidad"))
            {
                ev.ReturnMessage = "comando ejecutado";
                if (!Items.ContainsKey(ev.Player.SteamId))
                {
                    Items.Add(ev.Player.SteamId, true);

                }
                if((Items[ev.Player.SteamId])&&(ev.Player.TeamRole.Role == Role.CLASSD))
                {
                    ev.Player.GiveItem(ItemType.FLASHLIGHT);
                    MEC.Timing.RunCoroutine(Itemtimer(ev.Player));
                }
                if ((Items[ev.Player.SteamId]) && (ev.Player.TeamRole.Role == Role.SCIENTIST))
                {
                    MEC.Timing.RunCoroutine(Itemtimer(ev.Player));
                    ev.Player.GiveItem(ItemType.CUP);
                }
            }
        }

        private IEnumerator<float> Itemtimer(Player player)
        {
            Items[player.SteamId] = false;
            yield return 180f;
            Items[player.SteamId] = true;
        }
    }
}
