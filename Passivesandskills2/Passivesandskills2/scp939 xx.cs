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
    partial class scp939_xx : IEventHandlerPlayerHurt, IEventHandlerSetRole, IEventHandlerWaitingForPlayers, IEventHandlerPlayerDie, IEventHandlerCallCommand, IEventHandlerCheckRoundEnd, IEventHandlerPlayerPickupItem
        ,IEventHandlerDoorAccess
    {
        // En este codigo se supone que lo que hace es que un perro tenga reduccion de daño cuando esta a poca vida y además quién le dispare recivirá daño
        // y el otro perro causa daño por veneno el cual es mortal y mas dañino cuando el perro esta a poca vida.

       
        static Dictionary<Player, int> mordido = new Dictionary<Player, int>();
        static Dictionary<Player, bool> Vmortal = new Dictionary<Player, bool>();
        static Dictionary<Player, Player> atacant = new Dictionary<Player, Player>();
        static Dictionary<string, bool> Habilidad = new Dictionary<string, bool>();
        static bool end = false;
        int health = 0;

        public static IEnumerable<float> Skill(Player player, Role role,int salud) 
        {
            player.ChangeRole(role, false, false, false);
            Habilidad[player.SteamId] = false;
            yield return 20f;
            player.ChangeRole(Role.SCP_939_53);
            yield return 0.2f;
            player.SetHealth(salud);
            yield return 40f;
            if (Habilidad.ContainsKey(player.SteamId)) { Habilidad[player.SteamId] = true; }
            
        }
        public static IEnumerable<float> Veneno()
        {
            
            while (true)
            {
                if(end == true) { break; }
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

     

        public void OnPlayerDie(PlayerDeathEvent ev)
        {
            int contador = 0;
            if (ev.Player.TeamRole.Role == Role.SCP_939_53)
            {
                foreach (Player player in PluginManager.Manager.Server.GetPlayers())
                {

                    if(player.TeamRole.Role == Role.SCP_939_53) 
                    {
                        contador += 1;
                    }
                }
                if(contador <= 0) { contador = 0; end = true; }
                Habilidad.Remove(ev.Player.SteamId);
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
            if((Habilidad.ContainsKey(ev.Attacker.SteamId))&&(ev.Attacker.TeamRole.Role != Role.SCP_939_53)) 
            {
                ev.Damage = 0;
            }
            if((Habilidad.ContainsKey(ev.Player.SteamId))&&(ev.Player.TeamRole.Role != Role.SCP_939_53)) 
            {
                if(ev.Attacker.TeamRole.Team == Team.SCP) { ev.Damage = 0; }
                    ev.Player.ChangeRole(Role.SCP_939_53, false, false, false);
                ev.Player.SetHealth(health);
            }

		}

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			if (ev.Player.TeamRole.Role == Role.SCP_939_53)
			{
                if(end == true) { Timing.Run(Veneno()); end = false; }
                if (!Vmortal.ContainsKey(ev.Player)) { Vmortal.Add(ev.Player, false);  }
                if (!Habilidad.ContainsKey(ev.Player.SteamId)) { Habilidad.Add(ev.Player.SteamId, true); }
				ev.Player.PersonalBroadcast(10, "Tu pasiva es[Fauces Venenosas]: al morder a alguien le inyectas veneno no letal. [Veneno Letal]: cuando estas a poca vida este veneno es mas letal de forma que puede incluso matar ", false);
			}
			if (ev.Player.TeamRole.Role == Role.SCP_939_89)
			{
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Espinas]: dañas a tus atacantes con 4 de daño por bala. [Mejora de Acero]:inflinges hasta 10 de daño por bala como espinas, este efecto no se aplica a daño por granada o electricidad.", true);
                ev.Player.PersonalBroadcast(10, " [Mejora Titanio]: dañas a tus atacantes con 12 de daño por bala. ", true);
            }
            if((ev.Player.TeamRole.Role != Role.SCP_939_53) && (Habilidad.ContainsKey(ev.Player.SteamId))){ Habilidad.Remove(ev.Player.SteamId); }
		}

        public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
        {
            mordido.Clear();
            atacant.Clear();
            Vmortal.Clear();
            Habilidad.Clear();
            health = 0;
            end = false;
        }
        public void OnCallCommand(PlayerCallCommandEvent ev)
        {
            if (ev.Command.StartsWith("skill"))
            {
                if (ev.Player.TeamRole.Role != Role.SCP_939_53) { ev.ReturnMessage = "nice try"; }
                if (ev.Player.TeamRole.Role == Role.SCP_939_53)
                {
                    if(Habilidad[ev.Player.SteamId]== false) { ev.Player.SendConsoleMessage("habilidad en cooldown", "red"); }
                    if (Habilidad[ev.Player.SteamId] == true) 
                    { 
                    ev.Player.SendConsoleMessage("usa skill [un numero] pra transformarte en algo 1 = clasd 2 = scientist 3 = Chaos 4 = guard 5 = NTF Commander" +
                        " 6 = NTF Liuternant 7 = NTF Scientist 8 = NTF Cadet 9 = SCP-173 10 = SCP 049 11 = Zombie  ", "red");
                    ev.ReturnMessage = "usa skill [un numero] pra transformarte en algo 1 = clasd 2 = scientist 3 = Chaos 4 = guard 5 = NTF Commander" +
                        " 6 = NTF Liuternant 7 = NTF Scientist 8 = NTF Cadet 9 = SCP-173 10 = SCP 049 11 = Zombie  ";
                    string commandal = ev.Command.ToString();
                    System.Text.RegularExpressions.MatchCollection collectional = new System.Text.RegularExpressions.Regex("[^\\s\"\']+|\"([^\"]*)\"|\'([^\']*)\'").Matches(commandal);
                    string[] argsal = new string[collectional.Count - 1];
                    ev.ReturnMessage = "morphing";
                    Role role;
                        health = ev.Player.GetHealth();
                    for (int i = 1; i < collectional.Count; i++)
                    {

                        if (collectional[i].Value[0] == '\"' && collectional[i].Value[collectional[i].Value.Length - 1] == '\"')
                        {
                            argsal[i - 1] = collectional[i].Value.Substring(1, collectional[i].Value.Length - 2);
                        }
                        else
                        {
                            argsal[i - 1] = collectional[i].Value;
                        }
                    }
                    if (argsal.Length == 1)
                    {
                        switch (argsal[0])
                        {
                            case "1":
                                role = Role.CLASSD;
                                Timing.Run(Skill(ev.Player, role, ev.Player.GetHealth()));
                                break;
                            case "2":
                                role = Role.SCIENTIST;
                                Timing.Run(Skill(ev.Player, role, ev.Player.GetHealth()));
                                break;
                            case "3":
                                role = Role.CHAOS_INSURGENCY;
                                Timing.Run(Skill(ev.Player, role, ev.Player.GetHealth()));
                                break;
                            case "4":
                                role = Role.FACILITY_GUARD;
                                Timing.Run(Skill(ev.Player, role, ev.Player.GetHealth()));
                                break;
                            case "5":
                                role = Role.NTF_COMMANDER;
                                Timing.Run(Skill(ev.Player, role, ev.Player.GetHealth()));
                                break;
                            case "6":
                                role = Role.NTF_LIEUTENANT;
                                Timing.Run(Skill(ev.Player, role, ev.Player.GetHealth()));
                                break;
                            case "7":
                                role = Role.NTF_SCIENTIST;
                                Timing.Run(Skill(ev.Player, role, ev.Player.GetHealth()));
                                break;
                            case "8":
                                role = Role.NTF_CADET;
                                Timing.Run(Skill(ev.Player, role, ev.Player.GetHealth()));
                                break;
                            case "9":
                                role = Role.SCP_173;
                                Timing.Run(Skill(ev.Player, role, ev.Player.GetHealth()));
                                break;
                            case "10":
                                role = Role.SCP_049;
                                Timing.Run(Skill(ev.Player, role, ev.Player.GetHealth()));
                                break;
                            case "11":
                                role = Role.ZOMBIE;
                                Timing.Run(Skill(ev.Player, role, ev.Player.GetHealth()));
                                break;
                        }
                    }
                }
            }
            }
        }

        public void OnCheckRoundEnd(CheckRoundEndEvent ev)
        {
            foreach(KeyValuePair<string,bool> pair in Habilidad) 
            { 
            if(Habilidad[pair.Key] == false) { ev.Status = ROUND_END_STATUS.ON_GOING; }
            }
        }

        public void OnPlayerPickupItem(PlayerPickupItemEvent ev)
        {
            if (Habilidad.ContainsKey(ev.Player.SteamId)) 
            {
                ev.Allow = false;
            }
        }

        public void OnDoorAccess(PlayerDoorAccessEvent ev)
        {
            if (Habilidad.ContainsKey(ev.Player.SteamId)) 
            {
                if (ev.Door.Permission.Contains("CHCKPOINT_ACC")) { ev.Allow = true; ev.Door.Open = true; }
            }
        }
    }

}
