using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smod2;
using Smod2.EventHandlers;
using scp4aiur;
using Smod2.Events;
using Smod2.API;


namespace Passivesandskills2
{
	partial class scp939_xx : IEventHandlerPlayerHurt, IEventHandlerSetRole
	{
	
		public static IEnumerable<float> Veneno(Player player)
		{
			int cantidad = 0;
			while (cantidad <= 4)
			{
				yield return 2f;
				player.AddHealth(-4);
				cantidad += 1;
			}
			

		}

		public static IEnumerable<float> Venenomortal(Player player)
		{
			int cantidadd = 0;
			while (cantidadd <= 3)
			{
				yield return 3f;
				if (player.GetHealth() <= 8) { player.Kill(DamageType.DECONT); }
				player.AddHealth(-8);
				cantidadd += 1;
			}
			
		}


		public void OnPlayerHurt(PlayerHurtEvent ev)
		{
			//SCP 939-89 / Ramus //
			if ((ev.Player.TeamRole.Role == Role.SCP_939_89) && (ev.DamageType != DamageType.TESLA) && (ev.DamageType != DamageType.FRAG))
			{
				if (ev.Attacker.GetHealth() > 2) { ev.Attacker.AddHealth(-2); } else { ev.Attacker.Kill(DamageType.WALL); }
				if (ev.Player.GetHealth() <= 500)
				{
					ev.Damage /= 2;
					if (ev.Attacker.GetHealth() > 5) { ev.Attacker.AddHealth(-3); } else { ev.Attacker.Kill(DamageType.WALL); }

				}

			}
			//SCP 939-53 / Teemo//
			if (ev.Attacker.TeamRole.Role == Role.SCP_939_53)
			{
				Timing.Run(Veneno(ev.Player));
				if (ev.Attacker.GetHealth() <= 800)
				{
					Timing.Run(Venenomortal(ev.Player));
				}
			}


		}

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			if (ev.Player.TeamRole.Role == Role.SCP_939_53)
			{
				ev.Player.PersonalBroadcast(10, "Tu pasiva es[Fauces Venenosas]: al morder a alguien le inyectas veneno no letal. [Veneno Letal]: cuando estas a poca vida este veneno es mas letal de forma que puede incluso matar ", false);
			}
			if (ev.Player.TeamRole.Role == Role.SCP_939_89)
			{
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Espinas]: dañas a tus atacantes con 2 de daño por bala. [Mejora de Acero]:inflinges hasta 5 de daño por bala como espinas, este efecto no se aplica a daño por granada o electricidad.", true);
			}
		}
	}
}
