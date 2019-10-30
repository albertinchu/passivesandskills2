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
    partial class scp939_xx : IEventHandlerPlayerHurt, IEventHandlerSetRole, IEventHandlerWaitingForPlayers
    {
        // En este codigo se supone que lo que hace es que un perro tenga reduccion de daño cuando esta a poca vida y además quién le dispare recivirá daño
        // y el otro perro causa daño por veneno el cual es mortal y mas dañino cuando el perro esta a poca vida.

       
        static Dictionary<Player, int> mordido = new Dictionary<Player, int>();
        static Dictionary<Player, bool> Vmortal = new Dictionary<Player, bool>();
        static Dictionary<Player, Player> atacant = new Dictionary<Player, Player>();
        public static IEnumerable<float> Veneno()
        {
            
            while (true)
            {
               foreach(KeyValuePair<Player,int> pair in mordido) 
               {
                    if (!PluginManager.Manager.Server.GetPlayers().Contains(pair.Key)) { mordido.Remove(pair.Key); atacant.Remove(pair.Key); }
                    if(pair.Value >= 17) 
                    {
                        pair.Key.AddHealth(-4); mordido[pair.Key] -= 1; 
                        atacant[pair.Key].AddHealth(4);
                        if (Vmortal[atacant[pair.Key]]) 
                        {
                                pair.Key.AddHealth(-8);
                                atacant[pair.Key].AddHealth(5);

                        }                          
                    }
                    if (pair.Value >= 13)
                    {
                        pair.Key.AddHealth(-4); mordido[pair.Key] -= 1;
                        atacant[pair.Key].AddHealth(4);
                        if (Vmortal[atacant[pair.Key]])
                        {
                            pair.Key.AddHealth(-8);
                            atacant[pair.Key].AddHealth(5);

                        }
                    }
                    if (pair.Value >= 9)
                    {
                        pair.Key.AddHealth(-4); mordido[pair.Key] -= 1;
                        atacant[pair.Key].AddHealth(4);
                        if (Vmortal[atacant[pair.Key]])
                        {
                            pair.Key.AddHealth(-8);
                            atacant[pair.Key].AddHealth(5);

                        }
                    }
                    if (pair.Value >= 5)
                    {
                        pair.Key.AddHealth(-4); mordido[pair.Key] -= 1;
                        atacant[pair.Key].AddHealth(4);
                        if (Vmortal[atacant[pair.Key]])
                        {
                            pair.Key.AddHealth(-8);
                            atacant[pair.Key].AddHealth(5);

                        }
                    }
                    if (pair.Value >= 1)
                    {
                        pair.Key.AddHealth(-4); mordido[pair.Key] -= 1;
                        atacant[pair.Key].AddHealth(4);
                        if (Vmortal[atacant[pair.Key]])
                        {
                            pair.Key.AddHealth(-8);
                            atacant[pair.Key].AddHealth(5);

                        }
                    }
                }
                
                
                yield return 3f;
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
                if((ev.Attacker.GetHealth() <= 1550)&&(Vmortal[ev.Attacker] ==false)) {  Vmortal[ev.Attacker] = true;}
                if (ev.Player.TeamRole.Role != Role.TUTORIAL)
                {
                    if (mordido.ContainsKey(ev.Player)) { mordido[ev.Player] += 4; atacant[ev.Player] = ev.Attacker; }
                    if (!mordido.ContainsKey(ev.Player)) { mordido.Add(ev.Player, 4); ; atacant.Add(ev.Player, ev.Attacker); }
                }
			}


		}

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			if (ev.Player.TeamRole.Role == Role.SCP_939_53)
			{
                if (!Vmortal.ContainsKey(ev.Player)) { Vmortal.Add(ev.Player, false); }
				ev.Player.PersonalBroadcast(10, "Tu pasiva es[Fauces Venenosas]: al morder a alguien le inyectas veneno no letal. [Veneno Letal]: cuando estas a poca vida este veneno es mas letal de forma que puede incluso matar ", false);
			}
			if (ev.Player.TeamRole.Role == Role.SCP_939_89)
			{
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Espinas]: dañas a tus atacantes con 4 de daño por bala. [Mejora de Acero]:inflinges hasta 10 de daño por bala como espinas, este efecto no se aplica a daño por granada o electricidad.", true);
                ev.Player.PersonalBroadcast(10, " [Mejora Titanio]: dañas a tus atacantes con 12 de daño por bala. ", true);
            }
		}

        public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
        {
            mordido.Clear();
            atacant.Clear();
            Vmortal.Clear();
        }
    }
}
