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
     
        
       
        
        
        
       
      
     
        
        
      
        
        Vector posicionteni;
       
        
        Vector posmuertee;
        int conta049 = 0;

     
        public void OnSetRole(PlayerSetRoleEvent ev)
        {

            

           
            
            
            
            
           
           
            
           
           
           
          
        }
 
      
   
       
        
       
        
       
        








        public void OnPlayerHurt(PlayerHurtEvent ev)
        {
            // COMANDANTE //
           
            // TENIENTE //
           

            //Zombie//
          
            // CADETES //
        
            
            //comandante//
       
            //SCP 106 - El negro - shadow man//
           
            //Guardias//
            





        }

        public void OnPlayerDropItem(PlayerDropItemEvent ev)
        {
            
            

        }

        public void OnStartCountdown(WarheadStartEvent ev)
        {
           
        }

        public void OnStopCountdown(WarheadStopEvent ev)
        {
          
        }

        public void OnDetonate()
        {
            
        }

        public void OnMedkitUse(PlayerMedkitUseEvent ev)
        {
            
        }

        public void OnThrowGrenade(PlayerThrowGrenadeEvent ev)
        {
            
            
        }



        public void OnScp096Panic(Scp096PanicEvent ev)
        {

        }

        public void OnPocketDimensionDie(PlayerPocketDimensionDieEvent ev)
        {
            //SCP 106 - El negro - shadow man//
       

        }

        public void On079LevelUp(Player079LevelUpEvent ev)
        {
            //Computerchan//
     
        }

        public void OnGeneratorFinish(GeneratorFinishEvent ev)
        {
           

        }

        public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
        {
            
           
            
            
            
            
           



        }

        public void On079AddExp(Player079AddExpEvent ev)
        {
         
        }
       
        public void OnPlayerDie(PlayerDeathEvent ev)
        {
            //173 //
           







            //Guardias//
          


            //doctor//
            
            //////////////////////////////////////////////////////ordenador/////////////////////////////////////////////////////
            



        }

        public void OnRoundStart(RoundStartEvent ev)
        {

        }

        public void OnCallCommand(PlayerCallCommandEvent ev)
        {
            if (ev.Command.StartsWith("passivesandskillsinfo"))
            {
                ev.Player.SendConsoleMessage("Passives and skills es un modo de juego que aporta nuevas habilidades a todos los roles Excepto tutorial, Para información sobre cada role usa .passivesandskills [role en ingles] (classd,scientist,ntfscientist,ntfcadet,ntfliuternant,ntfcomander,guard,scp-xxx,chaos)", "blue");
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



