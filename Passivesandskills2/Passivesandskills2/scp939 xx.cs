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
        ,IEventHandlerDoorAccess, IEventHandlerMedkitUse
    {
        // En este codigo se supone que lo que hace es que un perro tenga reduccion de daño cuando esta a poca vida y además quién le dispare recivirá daño
        // y el otro perro causa daño por veneno el cual es mortal y mas dañino cuando el perro esta a poca vida, este veneno reduce la vida maxima del jugador de
        //forma permanente hasta que muera.

        static Dictionary<Player, int> Mordido = new Dictionary<Player, int>();
        
        static Dictionary<string, bool> Habilidad = new Dictionary<string, bool>();
        static bool end = false;
        int health = 0;
        //camuflaje
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
        

     

        public void OnPlayerDie(PlayerDeathEvent ev)
        {
            //permite al scp 939-53 recoger objetos si murió y respawnea como otro role
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
            if (Mordido.ContainsKey(ev.Player)) { Mordido.Remove(ev.Player); }
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
                if (!Mordido.ContainsKey(ev.Player)) { Mordido.Add(ev.Player, (ev.Player.TeamRole.MaxHP - 20)); }
                if (Mordido.ContainsKey(ev.Player)) 
                {
                    ev.Attacker.AddHealth(20);
                    Mordido[ev.Player] -= 20; 
                if(ev.Attacker.GetHealth() <= 1450) 
                    {
                        Mordido[ev.Player] -= 15;
                        ev.Attacker.AddHealth(30);
                    }
                
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
            // el comandante hace mas daño segun los jugadores NTF vivos y sus granadas aplican 200 de salud ademas de curar a sus aliados cuando disparan a aliados
            //El codigo del comandante se encuentra aquí para ajustarse mejor a la pasiva del 939-53
            //anticomandante
            //en este caso la cura del comandante esta limitada a el SCP-939-53
            if ((ev.Attacker.TeamRole.Role == Role.NTF_COMMANDER) && (ev.DamageType != DamageType.FRAG) && (ev.DamageType != DamageType.TESLA) && (ev.DamageType != DamageType.FALLDOWN)&&(Mordido.ContainsKey(ev.Player)))
            {
                if (ev.Player.TeamRole.Team == Team.NINETAILFOX) { ev.Damage = 0; }

                if ((ev.Player.TeamRole.Role == Role.NTF_CADET) && (ev.Player.GetHealth() < Mordido[ev.Player])) { ev.Player.AddHealth(5); }
                if ((ev.Player.TeamRole.Role == Role.NTF_LIEUTENANT) && (ev.Player.GetHealth() < Mordido[ev.Player])) { ev.Player.AddHealth(8); }
                if ((ev.Player.TeamRole.Role == Role.NTF_SCIENTIST) && (ev.Player.GetHealth() < Mordido[ev.Player])) { ev.Player.AddHealth(8); }




            }
            if ((ev.Attacker.TeamRole.Role == Role.NTF_COMMANDER) && (ev.Player.TeamRole.Team == Team.NINETAILFOX) && (ev.DamageType == DamageType.FRAG))
            {
                ev.Damage = 0;
                ev.Player.SetHealth(Mordido[ev.Player], DamageType.FRAG);
            }
            //comandante
            if ((ev.Attacker.TeamRole.Role == Role.NTF_COMMANDER) && (ev.DamageType != DamageType.FRAG) && (ev.DamageType != DamageType.TESLA) && (ev.DamageType != DamageType.FALLDOWN)&&(!Mordido.ContainsKey(ev.Player)))
            {
                if (ev.Player.TeamRole.Team == Team.NINETAILFOX) { ev.Damage = 0; }

                if ((ev.Player.TeamRole.Role == Role.NTF_CADET) && (ev.Player.GetHealth() <= 150)) { ev.Player.AddHealth(5); }
                if ((ev.Player.TeamRole.Role == Role.NTF_LIEUTENANT) && (ev.Player.GetHealth() <= 180)) { ev.Player.AddHealth(8); }
                if ((ev.Player.TeamRole.Role == Role.NTF_SCIENTIST) && (ev.Player.GetHealth() <= 180)) { ev.Player.AddHealth(8); }




            }
            if ((ev.Attacker.TeamRole.Role == Role.NTF_COMMANDER) && (ev.Player.TeamRole.Team == Team.NINETAILFOX) && (ev.DamageType == DamageType.FRAG))
            {
                ev.Damage = 0;
                ev.Player.SetHealth(200, DamageType.FRAG);
            }
        }

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			if (ev.Player.TeamRole.Role == Role.SCP_939_53)
			{
              
          
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
            Mordido.Clear();
            Habilidad.Clear();
            health = 0;
            end = false;
        }
        public void OnCallCommand(PlayerCallCommandEvent ev)
        {
            //Esta habilidad transforma al 939-53 en un role a eleccion del 939-53
            //guarda la salud del 939-53 y si recibe daño trasnforma vuelve a su forma original con la salud registrada cuando se usó el comando
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
            //por si se transforma en un role y es el ultimo scp que no termine la ronda
            foreach(KeyValuePair<string,bool> pair in Habilidad) 
            { 
            if(Habilidad[pair.Key] == false) { ev.Status = ROUND_END_STATUS.ON_GOING; }
            }
        }

        public void OnPlayerPickupItem(PlayerPickupItemEvent ev)
        {
            //no permite al SCP-939-53 tomar objetos
            if (Habilidad.ContainsKey(ev.Player.SteamId)) 
            {
                ev.Allow = false;
            }
        }

        public void OnDoorAccess(PlayerDoorAccessEvent ev)
        {//le permite abrir puertas del checkpoint en su estado de camuflaje
            if (Habilidad.ContainsKey(ev.Player.SteamId)) 
            {
                if (ev.Door.Permission.Contains("CHCKPOINT_ACC")) { ev.Allow = true; ev.Door.Open = true; }
            }
        }

        public void OnMedkitUse(PlayerMedkitUseEvent ev)
        {
            //limita la cura según el scp 939-53
            if (Mordido.ContainsKey(ev.Player)) { if((ev.Player.GetHealth() + ev.RecoverHealth) >= Mordido[ev.Player]) { ev.Player.SetHealth(Mordido[ev.Player]); } }
        }
    }

}
