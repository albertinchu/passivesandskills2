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
    partial class Events : IEventHandlerSetRole, IEventHandlerPlayerHurt, IEventHandlerPlayerDropItem, IEventHandlerWarheadStartCountdown, IEventHandlerWarheadStopCountdown, IEventHandlerWarheadDetonate, IEventHandlerMedkitUse
        , IEventHandlerThrowGrenade, IEventHandlerScp096Panic, IEventHandlerPocketDimensionDie, IEventHandler079LevelUp, IEventHandlerCallCommand,
        IEventHandlerGeneratorFinish, IEventHandlerWaitingForPlayers, IEventHandler079AddExp, IEventHandlerPlayerDie, IEventHandlerRoundStart
    {
        private Passivesandskills2 plugin;
        public Events(Passivesandskills2 plugin)
        {
            this.plugin = plugin;
        }

        //Variables importantes y diccionarios//
        static Dictionary<string, bool> Scp173 = new Dictionary<string, bool>();
        static Dictionary<string, Vector> Scp173pos = new Dictionary<string, Vector>();
        static Dictionary<string, bool> Classdh = new Dictionary<string, bool>();
        static Dictionary<string, bool> Scientisth = new Dictionary<string, bool>();
        static Dictionary<string, int> Guardias = new Dictionary<string, int>();
        static Dictionary<string, int> Scp106 = new Dictionary<string, int>();
        static Dictionary<string, int> Zombie = new Dictionary<string, int>();
        static Dictionary<string, bool> NTFli = new Dictionary<string, bool>();
        static Dictionary<string, bool> Computerr = new Dictionary<string, bool>();
        static Dictionary<string, Player> Pasiva = new Dictionary<string, Player>();
        bool Nuket = false;
        bool Boom = false;
        bool Llorona = false;
        static bool overcharge = false;
        int bajasllorona = 0;
        static int contadorNTF = 0;
        int level = 0;
        Vector posicionteni;
        string computerchan;
        int gen = 0;
        Vector posmuertee;
        int conta049 = 0;

        static Vector llorondead;
        public void OnSetRole(PlayerSetRoleEvent ev)
        {

            if (!Pasiva.ContainsKey(ev.Player.SteamId)) { Pasiva.Add(ev.Player.SteamId, ev.Player); }

            if ((ev.Role == Role.SCP_079) && (computerchan != ev.Player.SteamId) && (Computerr.ContainsKey(ev.Player.SteamId)))
            {
                computerchan = ev.Player.SteamId;
                Computerr.Add(ev.Player.SteamId, true);
            }
            if ((ev.Player.TeamRole.Role == Role.SCP_049_2))
            {
                if (!Zombie.ContainsKey(ev.Player.SteamId))
                {
                    Zombie.Add(ev.Player.SteamId, 0);
                    Timing.Run(Zombielive(ev.Player));
                }
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Cuerpo errante]: Cuanto mas tiempo permanezcas con vida mas daño haces (15% + de daño cada 1 minuto de vida).", false);
            }
            if (ev.Player.TeamRole.Role == Role.SCP_049)
            {
                ev.Player.SendConsoleMessage("[Mutar]: Cada 6 Zombies curados el zombie número 6 tiene un 35 % de mutar en otro SCP a los 3 minutos, No puede mutar en SCP-096 o en SCP-079, La mutación es totalmente aleatoria ", "red");
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Manipulador de cuerpos]: Curas de forma instantanea a clasesd/scientists [Mutar]: Cada 6 zombies uno tiene posibilidades de mutar (mas info en la consola)  .", false);
            }
            if (ev.Player.TeamRole.Role == Role.CHAOS_INSURGENCY)
            {
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Carroñero]: Recives un botiquin por cada NTF que asesines [Luchador de doble filo]: La vida que te falte la inflinges como daño adicional entre 2.", false);
            }
            if (ev.Player.TeamRole.Role == Role.NTF_LIEUTENANT)
            {
                if (!NTFli.ContainsKey(ev.Player.SteamId))
                {
                    NTFli.Add(ev.Player.SteamId, true);
                }
                contadorNTF += 1;
                ev.Player.SendConsoleMessage("[cambiar las tornas]: Cambiar las tornas es una pasiva Tactica con 20s de cooldown  la cual intercambia tu posición con la del enemigo cuando este esta a menos del 50% de vida atrapandolo durante 2 segundos en tu posición. (Esta habilidad no se aplica a SCPS pero si a Zombies y tampoco se aplica a aliados)", "blue");
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [cambiar las tornas]: Cambias la posición del enemigo con la tuya cuando esta por debajo de 50% atrapandolo (mas info en la consola)", false);
            }
            if (ev.Player.TeamRole.Role == Role.NTF_CADET)
            {
                contadorNTF += 1;
                ev.Player.SendConsoleMessage("[Flash rápido]: Tras lanzar una granada cegadora obtienes un escudo de 20 de salud, (este se anula si el comandante usa su granada para aplicarte 200 de salud pero se acumula si se aplicó los 200 de salud antes)", "blue");
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Tenacidad explosiva]: Recives daño reducido entre 2 de las granadas.[Flash Rápido]: (mas info en la consola)", false);
            }
            if ((ev.Player.TeamRole.Role == Role.NTF_COMMANDER))
            {

                contadorNTF += 1;
                ev.Player.SendConsoleMessage("[Preocupación por los tuyos]: Tus disparos hacen como cura la mitad del daño que causarían a tus aliados y las granadas Instacuran 200 de salud (¡OJO!: No se aplica a guardias ni científicos", "blue");
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Lider del Escudrón]: Inflinges daño adicional segun el número de NTF vivos [Preocupación por los tuyos]: tus ataques curan aliados (mas info en la consola)", false);
            }
            if ((ev.Player.TeamRole.Role == Role.SCP_106) && (!Scp106.ContainsKey(ev.Player.SteamId)))
            {
                Scp106.Add(ev.Player.SteamId, 0);
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Digestión]: Te curas cuando alguien muere en tu dimensión [Golpe Crítico]: Tu quinta victima muere al instante.", false);
            }
            if ((ev.Player.TeamRole.Role == Role.SCP_096))
            {
                Llorona = true;
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Gritos de guerra]: Matar a jugadores cura a todo tu equipo ,Habilidad [Recordatorio mortal]: revives perdiendo vida de forma progresiva.", false);
            }
            if ((ev.Player.TeamRole.Role == Role.FACILITY_GUARD) && (!Guardias.ContainsKey(ev.Player.SteamId)))
            {
                Guardias.Add(ev.Player.SteamId, 0);
                ev.Player.SendConsoleMessage("[Cazadores]: Ganancia de XP = 1 por atacar un SCP, 3 por atacar a un chaos, 20 por eliminar un chaos o zombie , 60 por eliminar un SCP. Nivel: 2 Ganas 500 de todas las municiones y vida, Nivel 3 Ganas Veneno el las balas que causa 3 de daño adicional, Nivel: 4 Ganas 1 granada y cada vez que la lanzas la vuelves a obtener y obtienes mas vida, Nivel 5 nueva pasiva [Mismo destino]: te llevas a tu asesino con tigo ");
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Cazadores]: subes de nivel por atacar scps y chaos, recompensas por nivel en la consola.", false);
            }
            if (ev.Player.TeamRole.Role == Role.SCP_939_53)
            {
                ev.Player.PersonalBroadcast(10, "Tu pasiva es[Fauces Venenosas]: al morder a alguien le inyectas veneno no letal. [Veneno Letal]: cuando estas a poca vida este veneno es mas letal de forma que puede incluso matar ", false);
            }
            if (ev.Player.TeamRole.Role == Role.SCP_939_89)
            {
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Espinas]: dañas a tus atacantes con 2 de daño por bala. [Mejora de Acero]:inflinges hasta 5 de daño por bala como espinas, este efecto no se aplica a daño por granada o electricidad.", true);
            }
            if ((ev.Player.TeamRole.Role == Role.NTF_SCIENTIST))
            {
                contadorNTF += 1;
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Conocimientos SCP Avanzados] robas 3 de vida y inflinges mas daño a los Scps 1% de su vida maxima. [Medicina]: los meditkits son el doble de efectivos sobre ti.", false);
            }
            if ((ev.Player.TeamRole.Role == Role.SCIENTIST && (!Scientisth.ContainsKey(ev.Player.SteamId))))
            {
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Conocimientos SCP]: robas 1 de vida y inflinges mas daño a los Scps 0.5% de su vida maxima, tu habilidad es [el cafe mañanero]: te hace invulnerable drurante 5 segundos y te cura .", false);
                Timing.Run(Coffe(ev.Player));
                Scientisth.Add(ev.Player.SteamId, true);
            }
            if ((ev.Role == Role.CLASSD) && (!Classdh.ContainsKey(ev.Player.SteamId)))
            {
                Classdh.Add(ev.Player.SteamId, true);
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Astucia] robas munición al disparar. [Dboy rules]: cuando estas a poca vida robas vida. Tu Habilidad es [Sigilo de doble filo]tirar tu linterna te hace invisible por 35 de salud (puedes morir si tienes menos de 36 de salud). ", false);
                ev.Player.PersonalBroadcast(10, " [Sigilo de doble filo]: (puedes morir si tienes menos de 36 de salud). ", false);
            }
            if ((ev.Player.TeamRole.Role == Smod2.API.Role.SCP_173) && (!Scp173.ContainsKey(ev.Player.SteamId)))
            {
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Go big or go Home]: cuando mueres te vas a lo GRANDE, tu habilidad es [Resurgir etereo]: revives al minuto con intervalos de invisibilidad. ", false);

                Scp173.Add(ev.Player.SteamId, true);
                Scp173pos.Add(ev.Player.SteamId, ev.Player.GetPosition());
            }

        }
        public static IEnumerable<float> Intimidacion(Player player, Vector pos3)
        {
            int contadorb = 0;
            while ((contadorb < 8))
            {
                contadorb += 1;
                yield return 0.25f;
                player.Teleport(pos3);
            }


        }
        public static IEnumerable<float> Coffe(Player player)
        {
            yield return 5f;
            player.GiveItem(ItemType.CUP);
        }
        public static IEnumerable<float> Cooldown(Player player)
        {
            yield return 20f;
            NTFli[player.SteamId] = true;
        }
        public static IEnumerable<float> Zombielive(Player player)
        {
            while (player.TeamRole.Role == Role.SCP_049_2)
            {
                yield return 60f;
                Zombie[player.SteamId] += 1;
            }
            if (player.TeamRole.Role != Role.SCP_049_2)
            {
                Zombie[player.SteamId] = 0;
            }
        }
        public static IEnumerable<float> Resurrec(Player player, Vector posdead)
        {
            yield return 3f;
            player.ChangeRole(Role.SCP_049_2);
            yield return 0.2f;
            player.Teleport(posdead);
        }



        public static IEnumerable<float> Mutar(Player player)
        {
            System.Random proba = new Random();
            int numero = proba.Next(0, 100);
            yield return 5f;

            if ((numero >= 5))
            { player.ChangeRole(Role.SCP_106, false); }
            if ((numero >= 10) && (numero <= 6))
            { player.ChangeRole(Role.SCP_049, false); }
            if ((numero >= 16) && (numero <= 20))
            { player.ChangeRole(Role.SCP_049, false); }
            if ((numero >= 95) && (numero <= 100))
            { player.ChangeRole(Role.SCP_173, false); }
            if (((numero >= 89) && (numero <= 94)))
            {
                if (numero < 92)
                {
                    player.ChangeRole(Role.SCP_939_53, false);
                }
                else
                {
                    player.ChangeRole(Role.SCP_939_89, false);
                }
            }

        }
        public static IEnumerable<float> LLORON(Player player, Vector pos)
        {

            yield return 1f;
            player.ChangeRole(Role.SCP_096);
            yield return 0.2f;
            player.Teleport(pos);

            while (true)
            {
                if (player.TeamRole.Role == Role.SCP_096)
                {
                    if (player.GetHealth() > 35) { player.AddHealth(-35); } else { player.Kill(DamageType.CONTAIN); }
                    yield return 3f;

                }
                else
                {
                    break;
                }

            }
        }
        public static IEnumerable<float> Scp173timer(Player player, Vector pos)
        {

            yield return 60f;


            Scp173[player.SteamId] = false;
            player.ChangeRole(Role.SCP_173);
            yield return 0.2f;
            player.Teleport(pos);

            while (true)
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
        public static IEnumerable<float> ClassdTimer(Player player)
        {
            player.SetGhostMode(true, false, false);
            yield return 10f;
            player.SetGhostMode(false);
            yield return 50f;
            player.GiveItem(ItemType.FLASHLIGHT);
            Classdh[player.SteamId] = true;
        }
        public static IEnumerable<float> ScientistTimer(Player player)
        {
            player.SetGodmode(true);
            yield return 5f;
            player.SetGodmode(false);
            yield return 55f;
            player.GiveItem(ItemType.CUP);
            Scientisth[player.SteamId] = true;

        }
        public static IEnumerable<float> Veneno(Player player)
        {
            int cantidad = 0;
            while (cantidad != 4)
            {
                yield return 2f;
                player.AddHealth(-4);
                cantidad += 1;
            }
            if (cantidad == 4) { cantidad = 0; }

        }
        public static IEnumerable<float> Venenomortal(Player player)
        {
            int cantidadd = 0;
            while (cantidadd != 3)
            {
                yield return 3f;
                if (player.GetHealth() <= 8) { player.Kill(DamageType.DECONT); }
                player.AddHealth(-8);
                cantidadd += 1;
            }
            if (cantidadd == 3) { cantidadd = 0; }
        }
        public static IEnumerable<float> Venenoguardias(Player player)
        {
            int cantidad = 0;
            while (cantidad != 4)
            {
                yield return 2f;
                player.AddHealth(-3);
                cantidad += 1;
            }
            if (cantidad == 4) { cantidad = 0; }

        }
        public static IEnumerable<float> Secondboom()
        {
            yield return 5f;
            PluginManager.Manager.Server.Map.DetonateWarhead();
            yield return 2f;
            PluginManager.Manager.Server.Map.DetonateWarhead();
            yield return 2f;
            PluginManager.Manager.Server.Map.DetonateWarhead();
            yield return 2f;
            PluginManager.Manager.Server.Map.DetonateWarhead();

        }


        public static IEnumerable<float> Computer(Player player)
        {
            yield return 0.5f;
            player.ChangeRole(Role.SCP_079);
        }


        public static IEnumerable<float> Pcoff()
        {
            yield return 60f;
            overcharge = true;
        }








        public void OnPlayerHurt(PlayerHurtEvent ev)
        {
            // COMANDANTE //
            if ((ev.Attacker.TeamRole.Role == Role.NTF_COMMANDER) && (ev.Player.TeamRole.Team == Team.NINETAILFOX) && (ev.DamageType != DamageType.FRAG) && (ev.DamageType != DamageType.TESLA) && (ev.DamageType != DamageType.FALLDOWN))
            {
                float damage = ev.Damage;
                ev.Damage = 0;
                if (ev.Player.GetHealth() < 150) { ev.Player.AddHealth((int)Math.Round(damage) / 2); }


            }
            if ((ev.Attacker.TeamRole.Role == Role.NTF_COMMANDER) && (ev.Player.TeamRole.Team == Team.NINETAILFOX) && (ev.DamageType == DamageType.FRAG))
            {
                ev.Damage = 0;
                ev.Player.SetHealth(200, DamageType.FRAG);
            }
            // TENIENTE //
            if (ev.Attacker.TeamRole.Role == Role.NTF_LIEUTENANT)
            {
                Vector posli = ev.Player.GetPosition();
                if ((ev.Player.TeamRole.Role == Role.CLASSD) || (ev.Player.TeamRole.Role == Role.SCIENTIST))
                {
                    if ((ev.Player.GetHealth() <= 50) && (NTFli[ev.Attacker.SteamId] == true))
                    {
                        NTFli[ev.Attacker.SteamId] = false;
                        Timing.Run(Intimidacion(ev.Player, ev.Attacker.GetPosition()));
                        Timing.Run(Cooldown(ev.Player));
                        ev.Attacker.Teleport(posli);
                    }

                }
                if ((ev.Player.TeamRole.Role == Role.CHAOS_INSURGENCY))
                {
                    if ((ev.Player.GetHealth() <= 60) && (NTFli[ev.Attacker.SteamId] == true))
                    {
                        NTFli[ev.Attacker.SteamId] = false;
                        Timing.Run(Intimidacion(ev.Player, ev.Attacker.GetPosition()));
                        Timing.Run(Cooldown(ev.Player));
                        ev.Attacker.Teleport(posli);
                    }

                }
            }

            //Zombie//
            if (ev.Attacker.TeamRole.Role == Role.SCP_049_2)
            {
                ev.Damage += (ev.Damage / 100) * 15 * Zombie[ev.Attacker.SteamId];
            }
            // CADETES //
            if ((ev.Player.TeamRole.Role == Role.NTF_CADET) && (ev.DamageType == DamageType.FRAG))
            {
                ev.Damage /= 2;
            }
            //chaos - chaos//
            if (ev.Attacker.TeamRole.Role == Role.CHAOS_INSURGENCY)
            {
                ev.Damage += ((120 - ev.Attacker.GetHealth()) / 2);
            }
            //comandante//
            if (ev.Attacker.TeamRole.Role == Role.NTF_COMMANDER)
            {
                ev.Damage += (contadorNTF * 4);
            }
            //SCP 106 - El negro - shadow man//
            if ((ev.Attacker.TeamRole.Role == Role.SCP_106))
            {
                Scp106[ev.Attacker.SteamId] += 1;
                if (Scp106[ev.Attacker.SteamId] == 5)
                { ev.Player.Kill(DamageType.SCP_106);
                    Scp106[ev.Attacker.SteamId] = 0;
                }
            }
            //Guardias//
            if ((ev.Attacker.TeamRole.Role == Role.FACILITY_GUARD) && ((ev.Player.TeamRole.Team == Team.SCP) || (ev.Player.TeamRole.Team == Team.CHAOS_INSURGENCY)))
            {
                Guardias[ev.Attacker.SteamId] += 1;
                if (Guardias[ev.Attacker.SteamId] == 50)
                {
                    ev.Attacker.PersonalBroadcast(3, "<color=#FF05FF> Nivel 2 </color>", false);
                    ev.Attacker.SetHealth(150, DamageType.CONTAIN);
                    ev.Attacker.SetAmmo(AmmoType.DROPPED_5, 500);
                    ev.Attacker.SetAmmo(AmmoType.DROPPED_7, 500);
                    ev.Attacker.SetAmmo(AmmoType.DROPPED_9, 500);
                }
                if (Guardias[ev.Attacker.SteamId] == 150)
                {
                    ev.Attacker.PersonalBroadcast(3, "<color=#FF0500> Nivel 3 </color>", false);
                }
                if (Guardias[ev.Attacker.SteamId] >= 150) { Timing.Run(Venenoguardias(ev.Player)); }
                if (Guardias[ev.Attacker.SteamId] == 300)
                {
                    ev.Attacker.PersonalBroadcast(3, "<color=#C50000> Nivel 4 </color>", false);
                    ev.Attacker.GiveItem(ItemType.FRAG_GRENADE);
                    ev.Attacker.SetHealth(300);
                }
                if (Guardias[ev.Attacker.SteamId] == 550)
                {
                    ev.Attacker.PersonalBroadcast(3, "<color=#C50000> Nivel 5 </color>", false);
                }
            }



            //Class D - Dboyssssss//
            if (ev.Attacker.TeamRole.Role == Role.CLASSD)
            {
                if (ev.Player.GetGhostMode() == true) { ev.Player.SetGhostMode(false); }
                if (ev.Player.GetAmmo(AmmoType.DROPPED_5) >= 3)
                {
                    ev.Attacker.SetAmmo(AmmoType.DROPPED_5, ev.Attacker.GetAmmo(AmmoType.DROPPED_5) + 3);
                    ev.Player.SetAmmo(AmmoType.DROPPED_5, ev.Player.GetAmmo(AmmoType.DROPPED_5) - 3);
                }
                if (ev.Player.GetAmmo(AmmoType.DROPPED_7) >= 3)
                {
                    ev.Attacker.SetAmmo(AmmoType.DROPPED_7, ev.Attacker.GetAmmo(AmmoType.DROPPED_7) + 3);
                    ev.Player.SetAmmo(AmmoType.DROPPED_7, ev.Player.GetAmmo(AmmoType.DROPPED_7) - 3);
                }
                if (ev.Player.GetAmmo(AmmoType.DROPPED_9) >= 3)
                {
                    ev.Attacker.SetAmmo(AmmoType.DROPPED_9, ev.Attacker.GetAmmo(AmmoType.DROPPED_9) + 3);
                    ev.Player.SetAmmo(AmmoType.DROPPED_9, ev.Player.GetAmmo(AmmoType.DROPPED_9) - 3);
                }

                if ((ev.Attacker.GetHealth() <= 40) && (ev.DamageType != DamageType.FRAG) && (ev.DamageType != DamageType.TESLA))
                {
                    ev.Attacker.SetHealth(ev.Attacker.GetHealth() + Convert.ToInt32(ev.Damage));
                    if (ev.Attacker.GetHealth() <= 20)
                    {
                        ev.Attacker.SetHealth(ev.Attacker.GetHealth() + Convert.ToInt32(ev.Damage));
                    }
                }
            }
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

        public void OnPlayerDropItem(PlayerDropItemEvent ev)
        {
            //Class D - Dboysss//
            if ((ev.Player.TeamRole.Role == Role.CLASSD) && (Classdh[ev.Player.SteamId] == true) && (ev.Item.ItemType == ItemType.FLASHLIGHT))
            {
                ev.ChangeTo = ItemType.NULL;
                if (ev.Player.GetHealth() >= 35)
                {
                    Classdh[ev.Player.SteamId] = false;
                    ev.Player.AddHealth(-35);
                    Timing.Run(ClassdTimer(ev.Player));
                }
                else
                {
                    ev.Player.Kill();
                }

            }
            //Scientistsssssss//
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

        public void OnStartCountdown(WarheadStartEvent ev)
        {
            Nuket = true;
        }

        public void OnStopCountdown(WarheadStopEvent ev)
        {
            Nuket = false;
        }

        public void OnDetonate()
        {
            if (Boom == false)
            {
                Timing.Run(Secondboom());
                Boom = true;
            }
        }

        public void OnMedkitUse(PlayerMedkitUseEvent ev)
        {
            if (ev.Player.TeamRole.Role == Role.CLASSD) { Pasiva[ev.Player.SteamId].GiveItem(ItemType.CHAOS_INSURGENCY_DEVICE); } else { Pasiva[ev.Player.SteamId].GiveItem(ItemType.FRAG_GRENADE); }
            if (ev.Player.TeamRole.Role == Role.NTF_SCIENTIST)
            {
                ev.RecoverHealth *= 2;
            }
        }

        public void OnThrowGrenade(PlayerThrowGrenadeEvent ev)
        {
            if ((ev.Player.TeamRole.Role == Role.FACILITY_GUARD) && (Guardias[ev.Player.SteamId] >= 300) && (ev.GrenadeType == GrenadeType.FRAG_GRENADE))
            {
                ev.Player.GiveItem(ItemType.FRAG_GRENADE);

            }
            if ((ev.Player.TeamRole.Role == Role.NTF_CADET) && (ev.GrenadeType == GrenadeType.FLASHBANG))
            {
                ev.Player.AddHealth(20);
            }
        }



        public void OnScp096Panic(Scp096PanicEvent ev)
        {
            ev.Player.AddHealth(25 * bajasllorona);
            bajasllorona = 0;
        }

        public void OnPocketDimensionDie(PlayerPocketDimensionDieEvent ev)
        {
            //SCP 106 - El negro - shadow man//
            foreach (Player player in PluginManager.Manager.Server.GetPlayers())
            {
                if (player.TeamRole.Role == Role.SCP_106) { player.AddHealth(20); }
            }

        }

        public void On079LevelUp(Player079LevelUpEvent ev)
        {
            //Computerchan//
            level = ev.Player.Scp079Data.Level;
            if (level == 3)
            {
                ev.Player.Scp079Data.MaxAP = 200;
            }
            if ((level > 3))
            {
                ev.Player.Scp079Data.MaxAP = 300;
                PluginManager.Manager.Server.Map.AnnounceCustomMessage("G G . SCP 079 LEVEL 5");
            }
        }

        public void OnGeneratorFinish(GeneratorFinishEvent ev)
        {
            gen += 1;
            if (gen == 5) { Timing.Run(Pcoff()); }

        }

        public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
        {
            NTFli.Clear();
            Scp173.Clear();
            Scp173pos.Clear();
            Classdh.Clear();
            Scientisth.Clear();
            Guardias.Clear();
            Scp106.Clear();
            Zombie.Clear();
            Pasiva.Clear();


            Nuket = false;
            Boom = false;
            Llorona = false;
            overcharge = false;


            bajasllorona = 0;

            contadorNTF = 0;
            level = 0;
            computerchan = "0";
            gen = 0;
            conta049 = 0;

        }

        public void On079AddExp(Player079AddExpEvent ev)
        {
            float Xp;
            Xp = ev.ExpToAdd;
            if (level > 3)
            {
                ev.Player.Scp079Data.APPerSecond = (ev.Player.Scp079Data.APPerSecond + (Xp / 25));
                ev.Player.Scp079Data.MaxAP = ev.Player.Scp079Data.MaxAP + (Xp / 10);

            }
        }

        public void OnPlayerDie(PlayerDeathEvent ev)
        {
            //173 //
            if (ev.Player.TeamRole.Role == Role.SCP_173)
            {
                Vector posd = ev.Player.GetPosition();

                if (Scp173[ev.Player.SteamId] == true)
                {
                    ev.SpawnRagdoll = false;
                    ev.Player.ThrowGrenade(GrenadeType.FRAG_GRENADE, true, posd, true, posd, true, 0, true);
                    ev.Player.GiveItem(ItemType.FRAG_GRENADE);
                    ev.Player.GiveItem(ItemType.FRAG_GRENADE);
                    ev.Player.ThrowGrenade(GrenadeType.FRAG_GRENADE, true, posd, true, posd, true, 0, true);
                    ev.Player.GiveItem(ItemType.FRAG_GRENADE);
                    ev.Player.GiveItem(ItemType.FRAG_GRENADE);
                    ev.Player.ThrowGrenade(GrenadeType.FRAG_GRENADE, true, posd, true, posd, true, 0, true);
                    ev.Player.GiveItem(ItemType.FRAG_GRENADE);
                    ev.Player.ThrowGrenade(GrenadeType.FRAG_GRENADE, true, posd, true, posd, true, 0, true);
                    Timing.Run(Scp173timer(ev.Player, posd));
                }
                else
                {
                    Scp173[ev.Player.SteamId] = true;
                }
            }
            // 096//
            if (ev.Killer.TeamRole.Role == Role.SCP_096)
            {
                bajasllorona += 1;
                foreach (KeyValuePair<string, Player> keyValuePair in Pasiva)
                {
                    if ((keyValuePair.Value.TeamRole.Team == Team.SCP) && (keyValuePair.Value.TeamRole.Role != Role.SCP_079))
                    {
                        if (keyValuePair.Value.TeamRole.Role == Role.SCP_096) { keyValuePair.Value.AddHealth(20); }
                        if (keyValuePair.Value.TeamRole.Role == Role.SCP_049) { keyValuePair.Value.AddHealth(20); }
                        if (keyValuePair.Value.TeamRole.Role == Role.SCP_049_2) { keyValuePair.Value.AddHealth(10); }
                        if (keyValuePair.Value.TeamRole.Role == Role.SCP_173) { keyValuePair.Value.AddHealth(45); }
                        if (keyValuePair.Value.TeamRole.Role == Role.SCP_106) { keyValuePair.Value.AddHealth(10); }
                        if (keyValuePair.Value.TeamRole.Role == Role.SCP_939_89) { keyValuePair.Value.AddHealth(20); }
                        if (keyValuePair.Value.TeamRole.Role == Role.SCP_939_53) { keyValuePair.Value.AddHealth(20); }
                    }

                }
            }
            if (ev.Player.TeamRole.Role == Role.SCP_096)
            {
                if (Llorona == true)
                {
                    llorondead = ev.Player.GetPosition();
                    Timing.Run(LLORON(ev.Player, llorondead));
                    Llorona = false;
                }
                else { Llorona = true; }
            }







            //Guardias//
            if((ev.Killer.TeamRole.Role == Role.FACILITY_GUARD)&&(ev.Player.TeamRole.Role == Role.CHAOS_INSURGENCY)) { Guardias[ev.Player.SteamId] += 30; }
            if ((ev.Player.TeamRole.Role == Role.FACILITY_GUARD) && (Guardias[ev.Player.SteamId] >= 550))
            {
                ev.Killer.ChangeRole(Role.SPECTATOR);
                PluginManager.Manager.Server.Map.Broadcast(4, "<color=#ABABAB" + ev.Player.Name + "</color> se llevó a <color=#C50000> " + ev.Killer.Name + " </color> con él...", false);
            }
            // Chaos //
            if ((ev.Killer.TeamRole.Role == Role.CHAOS_INSURGENCY) && (ev.Player.TeamRole.Team == Team.NINETAILFOX))
            {
                ev.Killer.GiveItem(ItemType.MEDKIT);
            }

            //doctor//
            if (ev.Killer.TeamRole.Role == Smod2.API.Role.SCP_049)
            {

                posmuertee = ev.Player.GetPosition();
                if ((ev.Player.TeamRole.Team == Team.SCIENTIST) || (ev.Player.TeamRole.Team == Team.CLASSD))
                {
                    Timing.Run(Resurrec(ev.Player, posmuertee));
                    conta049 += 1;
                    if (conta049 == 6)
                    {
                        Timing.Run(Mutar(ev.Player));
                        conta049 = 0;
                    }
                }

            }
            //////////////////////////////////////////////////////ordenador/////////////////////////////////////////////////////
            if (overcharge == false)
            {
                if ((Computerr.ContainsKey(ev.Killer.SteamId))) { PluginManager.Manager.Server.Map.Broadcast(1, "079 mató a un jugador", true); }
                posicionteni = ev.Player.GetPosition();

                if ((Boom == false) && (Computerr.ContainsKey(ev.Player.SteamId))) { Timing.Run(Computer(ev.Player)); }

                if ((ev.Player.SteamId == ev.Killer.SteamId) && (Nuket == true) && (ev.DamageTypeVar == DamageType.TESLA) && (ev.Player.TeamRole.Role != Role.SCP_096))
                {
                    var nueva = ev.Player.TeamRole.Role;
                    ev.SpawnRagdoll = false;
                    foreach (KeyValuePair<string, Player> keyValue in Pasiva)
                    {
                        if ((keyValue.Value.TeamRole.Role == Role.SCP_079) && (nueva != Role.SCP_173)) { keyValue.Value.ChangeRole(nueva); }
                        if ((keyValue.Value.TeamRole.Role == Role.SCP_079) && (nueva == Role.SCP_173))
                        {
                            keyValue.Value.ChangeRole(nueva); keyValue.Value.Teleport(PluginManager.Manager.Server.Map.GetSpawnPoints(Role.SCP_049).First());
                        }

                    }
                }
            }



        }

        public void OnRoundStart(RoundStartEvent ev)
        {

        }

        public void OnCallCommand(PlayerCallCommandEvent ev)
        {
            if (ev.Command.StartsWith("passivesandskillsinfo"))
            {
                ev.Player.SendConsoleMessage("Passives and skills es un modo de juego que aporta nuevas habilidades a todos los roles Excepto tutorial, Para información sobre cada role usa .passivesandskills [role en ingles] (classd,scientist,ntfscientist,ntfcadet,ntfliuternat,ntfcomander,guard,scp-xxx,chaos)", "blue");
                string commandal = ev.Command.ToString();
                System.Text.RegularExpressions.MatchCollection collectional = new System.Text.RegularExpressions.Regex("[^\\s\"\']+|\"([^\"]*)\"|\'([^\']*)\'").Matches(commandal);
                string[] argsal = new string[collectional.Count - 1];



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
                        case "classd":
                            ev.Player.SendConsoleMessage("La pasiva de los clases D es [Astucia], esta hace que robes munición en orden de 3 balas por disparo acertado a un enemigo, si el enemigo no tiene munición no le robas nada " +
                                "[Dboys rules], esta pasiva hace que cuando estas con de 40 o menos de salud te curas el daño causado y si el caso es menor a 20 te curas el doble del daño" +
                                "por último tu habilidad es [Sigilo de doble filo] esta te permite hacerte invisible durante 10 segundos por 35 de salud, si estas a menos de 36 de vida mueres al usar la habilidad y se puede usar cada 60s, hacer daño mientras eres invisible cancela la invisibilidad (los disarmers no la cancelan)", "orange");

                            break;
                        case "scientist":
                            ev.Player.SendConsoleMessage("La pasiva de los científicos es [Conocimientos SCP]: roban 1 de vida y inflingen mas daño a los Scps 0.5% de su vida maxima por bala, La habilidad es [el cafe mañanero]: la cual te hace invulnerable drurante 5 segundos y te cura 25 de salud, esta se puede acumular como escudo permanente y se puede usar una vez cada 60s  .", "white");
                            break;
                        case "ntfscientist":
                            ev.Player.SendConsoleMessage("La pasiva de los científicos es [Conocimientos SCP Avanzado]: roban 3 de vida y inflingen mas daño a los Scps 1% de su vida máxima por bala, [Medicína] Los botiquines curan el doble","blue");
                            break;
                        case "ntfcadet":
                            ev.Player.SendConsoleMessage("La pasiva de los cadetes es [Tenacidad Explosiva] reciven daño reducido entre 2 de granadas,[Flash rápido]: Tras lanzar una granada cegadora obtienes un escudo de 20 de salud, (este se anula si el comandante usa su granada para aplicarte 200 de salud pero se acumula si se aplicó los 200 de salud antes de forma que acabarías con 220 de salud) ", "blue");
                            break;
                        case "chaos":
                            ev.Player.SendConsoleMessage("La pasiva de los chaos es [Carroñero]: Reciven un botiquín por cada NTF que asesinan [Luchador de doble filo]: La vida que le falte la inflinge como daño adicional entre 2 si les falta 50 causan 25 de daño", "green");
                            break;
                        case "scp-173":
                            ev.Player.SendConsoleMessage("La pasiva del 173 es [Go big or go Home]: cuando mueres te vas a lo GRANDE Literalmente, tu habilidad es [Resurgir etereo]: revives al minuto con intervalos de invisibilidad, aunque hagas respawn revivirás como 173", "red");
                            break;
                        case "scp-079":
                            ev.Player.SendConsoleMessage("La pasiva del 079 es [Mejorado] ahora cuando el SCP 079 sube al maximo nivel transforma su xp en ap máximo, su habilidad es [Control remoto] cuando la nuke es activada si algo muere por el tesla perdera el control y sera controlado por SCP 079 SCPS, incluidos ","red");
                            break;
                        case "scp-106":
                            ev.Player.SendConsoleMessage("La pasiva es [Digestion] cada vez que alguien muere en la dimensión de bolsillo SCP 106 se cura 20 de salud que se puede acumular como escudo permanente y su habilidad es [Golpe Crítico] Cada 5 víctimas la 5 muere al instante al ser capturada por SCP 106 ", "red");
                            break;
                        case "scp-939":
                            ev.Player.SendConsoleMessage("Dependiendo de la variante del SCP 939 puede tener 1º variante: [Fauces venenosa] la cual hace que sus mordiscos apliquen veneno y cuando este esta a poca vida se activa su pasiva [Veneno Letal] de forma que puedes morir por el veneno y hace mas daño. 2º variante [Espinas] los ataques contra el reflejan daño, [Mejora de acero] Recive menos daño y refleja mas daño (esta pasiva es ignorada por granadas y daño eléctrico)", "red");
                            break;
                        case "scp-049":
                            ev.Player.SendConsoleMessage("La pasiva es [Manipulador de cuerpos] puede curar a clases d y científicos al instante, [Mutar] cada 6 zombies curados el 6 puede mutar en otro SCP con un 35% (no puede mutar en 096 o 079), Pasiva del 049-2 es [Cuerpo errante]: Cuanto mas tiempo permanezcas con vida mas daño haces (15% + de daño cada 1 minuto de vida)", "red");
                            break;
                        case "scp-096":
                            ev.Player.SendConsoleMessage("La pasiva es[Gritos de guerra]: Matar a jugadores cura a todo su equipo ,Habilidad [Recordatorio mortal]: revives perdiendo vida de forma progresiva pero solo 1 vez", "red");
                            break;
                        case "guards":
                            ev.Player.SendConsoleMessage("La pasiva es [Cazadores] esta pasiva es un sistema de niveles y xp, empiezan con nivel 1 y van ganando 3 de xp por bala acertada a chaos y scps y ganan 30 de xp al matar chaos, la recompensa por nivel es Nivel 2: obtienen 500 de todas las municiones + 150 de salud instantanea. Nivel 3: Balas venenosas que hacen daño adicional. Nivel 4: Granada Frag que cuando es lanzada se recupera de forma indefinida y 300 de salud. Nivel 5: Mismo destino....   ", "grey");
                            break;
                        case "ntfcomander":
                            ev.Player.SendConsoleMessage("La pasiva es [Preocupación por los tuyos]: Los disparos del comandante hacen como cura la mitad del daño que causarían a sus aliados y las granadas Instacuran 200 de salud (¡OJO!: No se aplica a guardias ni científicos) por lo que no uses esta pasiva con ellos ", "blue");
                            break;
                        case "ntfliuternant":
                            ev.Player.SendConsoleMessage("La pasiva es [cambiar las tornas]: Cambiar las tornas es una pasiva Tactica con 20s de cooldown  la cual intercambia tu posición con la del enemigo cuando este esta a menos del 50% de vida atrapandolo durante 2 segundos en tu posición. (Esta habilidad no se aplica a SCPS pero si a Zombies, tampoco se aplica a aliados por lo que no intentes estupideces...)", "blue");
                            break;

                    }
                }
            }
        } 
    }
}



