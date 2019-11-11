﻿
using Smod2.EventHandlers;
using Smod2.Events;


namespace Passivesandskills2
{
	partial class classd : IEventHandlerCallCommand
	{
       
    

    public void OnCallCommand(PlayerCallCommandEvent ev)
		{
            // solo proporciona información
			if (ev.Command.StartsWith("passivesandskillsinfo"))
			{
				ev.Player.SendConsoleMessage("Passives and skills es un modo de juego que aporta nuevas habilidades a todos los roles Excepto tutorial, Para información sobre cada role usa .passivesandskills [role en ingles] (classd,scientist,ntfscientist,ntfcadet,ntfliuternant,ntfcomander,guard,scp-xxx,chaos)", "blue");
				string commandal = ev.Command.ToString();
				System.Text.RegularExpressions.MatchCollection collectional = new System.Text.RegularExpressions.Regex("[^\\s\"\']+|\"([^\"]*)\"|\'([^\']*)\'").Matches(commandal);
				string[] argsal = new string[collectional.Count - 1];
                ev.ReturnMessage = "";


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
								"[Dboys rules], esta pasiva hace que cuando estas con de 45 o menos de salud te curas el daño causado y si el caso es menor a 25 te curas el doble del daño" +
								"por último tu habilidad es [Sigilo de doble filo] esta te permite hacerte invisible durante 10 segundos por 35 de salud, si estas a menos de 36 de vida mueres al usar la habilidad y se puede usar cada 60s, hacer daño mientras eres invisible cancela la invisibilidad (los disarmers no la cancelan)", "orange");

							break;
						case "scientist":
							ev.Player.SendConsoleMessage("La pasiva de los científicos es [Conocimientos SCP]: Roban 1 de vida y inflingen mas daño a los Scps 0.5% de su vida maxima por bala, La habilidad es [el cafe mañanero]: la cual te hace invulnerable drurante 5 segundos y te cura 25 de salud, esta se puede acumular como escudo permanente y se puede usar una vez cada 60s  .", "white");
							break;
						case "ntfscientist":
							ev.Player.SendConsoleMessage("La pasiva de los científicos es [Conocimientos SCP Avanzado]: Roban 3 de vida y causan mas daño a los Scps 1% de su vida máxima por bala, [Medicina] Los botiquines curan el doble","blue");
							break;
						case "ntfcadet":
							ev.Player.SendConsoleMessage("La pasiva de los cadetes es [Tenacidad Explosiva] Reciben daño reducido entre 2 de granadas,[Flash rápido]: Tras lanzar una granada cegadora obtienes un escudo de 30 de salud, (este se anula si el comandante usa su granada para aplicarte 200 de salud pero se acumula si se aplicó los 200 de salud antes de forma que acabarías con 230 de salud) ", "blue");
							break;
						case "chaos":
							ev.Player.SendConsoleMessage("La pasiva de los chaos es [Carroñero]: Reciben un botiquín por cada NTF que asesinan [Luchador de doble filo]: La vida que le falte la inflinge como daño adicional entre 2 si les falta 50 causan 25 de daño con los SCPS cunado estas a 80 o menos de salud les haces 20 de daño adcional y 3 de daño adicional al 106, si el chaos originalmente era clase d conservará la habilidad sigilo de doble filo y le costará solo 20 de hp ", "green");
							break;
						case "scp-173":
							ev.Player.SendConsoleMessage("La pasiva del 173 es [Go big or go Home]: Cuando mueres te vas a lo GRANDE Literalmente, tu habilidad es [Resurgir etereo]: revives al minuto con intervalos de invisibilidad, aunque hagas respawn revivirás como 173", "red");
							break;
						case "scp-079":
							ev.Player.SendConsoleMessage("La pasiva del 079 es [Mejorado] ahora cuando el SCP 079 sube al maximo nivel transforma su xp en ap máximo , su habilidad es [Mejora tesla] = ahora cuando SCP 079 es de un nivel mayor al 3 gasta la mitad de ap en los teslas y si su ap máximo es mayor a 400 gasta solo 10 ap Habilidad [Control absoluto]: habilita 3 comandos nukeoff que desactiva la nuke cooldown 120 segundos, cellsopen que libera con un 15% a un SCP 939-84 , 15% 3 cientificos y un 70% 3 clases D,elevatorsoff desactiva los ascensores 20 segundos 120 segundos de cooldown  ,nukenow (no pulses el botón rojo) y nanobots (desactivado temporalmente) que curan o causan 50 segun al jugador aleatorio que elijan (si es un scp cura si es humano daña) 120s de cooldown y cuesta 100 de energía, ","red");
							break;
						case "scp-106":
							ev.Player.SendConsoleMessage("La pasiva es [Digestion] cada vez que alguien muere en la dimensión de bolsillo SCP 106 se cura 30 de salud que se puede acumular como escudo permanente y su habilidad es [Golpe Crítico] Cada 5 víctimas la 5 muere al instante al ser capturada por SCP 106 Tu Habilidad es [Presencia Espectral]: Cuando ejecuta a una persona por Golpe crítico se hace invisible hasta que ataque a otra persona ", "red");
							break;
						case "scp-939":
							ev.Player.SendConsoleMessage("Dependiendo de la variante del SCP 939 puede tener 1º variante: [Fauces venenosa] la cual hace que sus mordiscos apliquen veneno que resta vida máxima de forma permanente y cuando este esta a poca vida se activa su pasiva [Veneno Letal] que el veneno hace mas daño y ejecuta a jugadores con vida máxima menor o igual a 60. [Chupavidas] parte del daño por veneno la recibes como curación Habilidad [Cambiaformas] se puede transformar en diferentes roles con el comando .skill. 2º variante [Espinas] los ataques contra él reflejan daño, [Mejora de acero] Recive menos daño y refleja mas daño [mejora de titanio]: refleja 12 de daño de espinas y el perro solo recibe 3 de daño por bala, las granada pueden hacer como máximo 100 de daño cuando la pasiva esta activa (esta pasiva es ignorada por granadas y daño eléctrico)", "red");
							break;
						case "scp-049":
							ev.Player.SendConsoleMessage("La pasiva es [Manipulador de cuerpos] puede curar a clases d y científicos al instante, [Mutar] cada 6 bajas la 6 puede mutar en un SCP con un 40% (no puede mutar en 096 o 079) [Padre vengativo]: cuando un zombie muere el doctor se cura 75 de hp, Pasiva del 049-2 es [Recuerdos]: Según el role con el que murió el zombie adquiere nuevas pasivas, si era Clase d se hace invisble y visible intervalos de 5 s, si era cientifico se hace inmortal en intervalos de 5 segundos , Guardia: refleja Daño/20, Cadete: recibe un 50% menos de daño, Teniente: al morir lanza una granada que causa 90 de daño, Cientifico NTF: se cura 700 de vida por cada golpe, Chaos: mata de un golpe, Comandante: todas las anteriores menos la de los clases d", "red");
							break;
						case "scp-096":
							ev.Player.SendConsoleMessage("La pasiva es[Gritos de guerra]: Matar a jugadores cura a todo su equipo, a los 939 40 de salud y a los otros aproximadamente el 1% de su salud maxima ,Habilidad [Recordatorio mortal]: revives perdiendo vida de forma progresiva pero solo 1 vez (-65 de salud cada 3 segundos)", "red");
							break;
						case "guards":
							ev.Player.SendConsoleMessage("La pasiva es [Cazadores] esta pasiva es un sistema de niveles y xp, empiezan con nivel 1 y van ganando 3 de xp por bala acertada a chaos y scps y ganan 30 de xp al matar chaos y zombies, la recompensa por nivel es Nivel 2: obtienen 500 de todas las municiones + 150 de salud instantanea. Nivel 3: Balas venenosas que hacen daño adicional. Nivel 4: Granada Frag que cuando es lanzada se recupera de forma indefinida y 300 de salud. Nivel 5: Mismo destino....   ", "grey");
							break;
						case "ntfcomander":
							ev.Player.SendConsoleMessage("La pasiva es [Preocupación por los tuyos]: Los disparos del comandante hacen como cura el daño que causarían a sus aliados y las granadas Instacuran 200 de salud (¡OJO!: No se aplica a guardias ni científicos) por lo que no uses esta pasiva con ellos ", "blue");
							break;
						case "ntfliuternant":
							ev.Player.SendConsoleMessage("La pasiva es [cambiar las tornas]: Cambiar las tornas es una pasiva Tactica con 40s de cooldown  la cual Teletransporta a un jugador cuando tiene menos del 50% de la vida a un lugar aleatorio. (Esta habilidad no se aplica a SCPS pero si a Zombies, Clases D y Scientists, tampoco se aplica a aliados por lo que no intentes estupideces...), cuando la habilidad esta en cooldown se activa la pasiva [De servicio]: incrementando el daño en 15 a todos los objetivos y es el doble de daño contra chaos, contra SCPS el daño es = al 1% de la vida actual del SCP por lo que si el SCP tiene 1000 de vida causa 10 de daño adicional y si tiene 900 causa 9,Habilidad [Expertos en explosivos]: Pueden usar el comando .c4modeon para que sus granadas actuen como c4 que explotan a los 2 minutos o al tirar una tablet, esta habilidad se aplica a granadas cegadora y se puede desactivar con .c4modeoff.", "blue");
							break;

					}
				}
			}
		}

    

   
}
}



