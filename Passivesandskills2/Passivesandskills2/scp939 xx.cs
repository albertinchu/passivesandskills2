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
        // En este codigo se supone que lo que hace es que un perro tenga reduccion de daño cuando esta a poca vida y además quién le dispare recivirá daño
        // y el otro perro causa daño por veneno el cual es mortal y mas dañino cuando el perro esta a poca vida.
        public static IEnumerable<float> Veneno(Player player, Player perro2)
        {
            int cantidad = 0;
            while (cantidad <= 4)
            {
                yield return 2f;
                perro2.AddHealth(4);
                if(player.TeamRole.Role != Role.SPECTATOR) { player.AddHealth(-4); }
                
                cantidad += 1;
            }


        }

        public static IEnumerable<float> Venenomortal(Player player, Player perro2)
        {
            int cantidadd = 0;
            while (cantidadd <= 3)
            {
                yield return 3f;
                perro2.AddHealth(5);
                if (player.GetHealth() <= 8) { player.Kill(DamageType.DECONT); }
                if(player.TeamRole.Role != Role.SPECTATOR) { player.AddHealth(-8); }
                
                cantidadd += 1;
            }

        }


        public void OnPlayerHurt(PlayerHurtEvent ev)
        {
            //SCP 939-89 / Ramus //
            if ((ev.Player.TeamRole.Role == Role.SCP_939_89) && (ev.DamageType != DamageType.TESLA) && (ev.DamageType != DamageType.FRAG))
            {
                if (ev.Attacker.GetHealth() > 4) { ev.Attacker.AddHealth(-4); } else { ev.Attacker.Kill(DamageType.WALL); }
                if (ev.Player.GetHealth() <= 600)
                {
                    ev.Damage /= 2;
                    if (ev.Attacker.GetHealth() > 10) { ev.Attacker.AddHealth(-6); } else { ev.Attacker.Kill(DamageType.WALL); }
                    if (ev.Player.GetHealth() <= 200)
                    {
                        ev.Damage = 3;
                        if (ev.Attacker.GetHealth() > 12) { ev.Attacker.AddHealth(-2); } else { ev.Attacker.Kill(DamageType.WALL); }
                    }

                }


            }
            ///[Titanium upgrade]//
            if ((ev.Player.TeamRole.Role == Role.SCP_939_89)&& (ev.Player.GetHealth() <= 200) && (ev.DamageType == DamageType.FRAG))
            {
                if(ev.Damage >= 100) { ev.Damage = 100; }
            } 
            
			//SCP 939-53 / Teemo//
			if (ev.Attacker.TeamRole.Role == Role.SCP_939_53)
			{
                if (ev.Player.TeamRole.Role != Role.TUTORIAL)
                {
                    Timing.Run(Veneno(ev.Player, ev.Attacker));
                    if (ev.Attacker.GetHealth() <= 1600)
                    {
                        Timing.Run(Venenomortal(ev.Player, ev.Attacker));
                    }
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
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Espinas]: dañas a tus atacantes con 4 de daño por bala. [Mejora de Acero]:inflinges hasta 10 de daño por bala como espinas, este efecto no se aplica a daño por granada o electricidad.", true);
                ev.Player.PersonalBroadcast(10, " [Mejora Titanio]: dañas a tus atacantes con 12 de daño por bala. ", true);
            }
		}
	}
}
