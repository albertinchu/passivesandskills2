using System.Collections.Generic;
using System.Linq;
using Smod2;
using Smod2.EventHandlers;
using scp4aiur;
using Smod2.Events;
using Smod2.API;

namespace Passivesandskills2
{
	partial class scp079 : IEventHandler079AddExp, IEventHandler079LevelUp,IEventHandlerCallCommand,
		IEventHandlerSetRole, IEventHandlerWaitingForPlayers, IEventHandlerWarheadDetonate, IEventHandler079TeslaGate
        ,IEventHandlerElevatorUse
	{
		static bool elevatoss = false;
		
		static bool Boom = false;
		
		int level = 0;
		
		string computerchan;
		
		static Dictionary<string, bool> Pasivaa = new Dictionary<string, bool>();
        static bool habilidad079 = true;
        private List<Smod2.API.TeslaGate> teslas;

        // Este codigo ace que cuando la nuke sea activada el pc pueda robar el cuerpo de quien muera en el tesla y usarlo como quiera
        // Ademas de que ganas ap infinito al nivel 5 en funcion de la xp que ganes

        public static IEnumerable<float> Secondboom()
		{
			yield return 5f;
			PluginManager.Manager.Server.Map.DetonateWarhead();
		
			
            yield return 60f;
            if (Boom)
            {
                PluginManager.Manager.Server.Map.DetonateWarhead();
            }
               
        }
        public IEnumerable<float> Teslass()
        {
            
            int contador = 0;
            
            while (contador <= 10)
            {
                

                
                    foreach (Smod2.API.TeslaGate tesla in teslas)
                    {
                        tesla.Activate(true);
                        tesla.TriggerDistance *= 2;
                    }


                yield return 1f;
                contador += 1;
            }
        }
        public static IEnumerable<float> liberar()
        {
            int contador = 0;
            yield return 5f;
            System.Random Number = new System.Random();
            int proba = Number.Next(0, 100);
            if (proba <= 20)
            {
                contador = 1;
                foreach (Player player in PluginManager.Manager.Server.GetPlayers())
                {
                    if(player.TeamRole.Role == Role.SPECTATOR)
                    {
                        if(contador == 1)
                        {
                            player.ChangeRole(Role.SCP_939_89);
                        }
                    }
                }

            }
            if((proba >= 21)&&(proba <= 31))
            {

                foreach (Player player in PluginManager.Manager.Server.GetPlayers())
                {
                    
                    if (player.TeamRole.Role == Role.SPECTATOR)
                    {
                        if (contador <= 3)
                        {
                            contador += 1;
                            player.ChangeRole(Role.SCIENTIST);
                            yield return 0.2f;
                            if(contador == 1) { player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_049)); }
                            if (contador == 2) { player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_096)); }
                            if (contador == 3) { player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_939_53)); }
                        }
                    }
                }
            }
            if ((proba >= 32))
            {

                foreach (Player player in PluginManager.Manager.Server.GetPlayers())
                {

                    if (player.TeamRole.Role == Role.SPECTATOR)
                    {
                        if (contador <= 3)
                        {
                            contador += 1;
                            player.ChangeRole(Role.CLASSD);
                            yield return 0.2f;
                            if (contador == 1) { player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_049)); }
                            if (contador == 2) { player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_096)); }
                            if (contador == 3) { player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_939_53)); }
                        }
                    }
                }
            }


        }

        public static IEnumerable<float> Cooldown079(Player player)
        {
            
            yield return 120f;
            Pasivaa[player.SteamId] = true;

        }
        public static IEnumerable<float> Cooldown0792(Player player)
        {

            yield return 60f;
            Pasivaa[player.SteamId] = true;

        }
        public static IEnumerable<float> Cooldown0793(Player player)
        {

            yield return 60f;
            Pasivaa[player.SteamId] = true;

        }
        public static IEnumerable<float> elevators()
        {
            elevatoss = true;
            yield return 20f;
            elevatoss = false;

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

		public void On079LevelUp(Player079LevelUpEvent ev)
		{
			level = ev.Player.Scp079Data.Level;
			if (ev.Player.Scp079Data.Level == 3)
			{
				ev.Player.Scp079Data.MaxAP = 200;
			}
			if ((ev.Player.Scp079Data.Level > 3))
			{
				ev.Player.Scp079Data.MaxAP = 300;
				PluginManager.Manager.Server.Map.AnnounceCustomMessage("G G . SCP 079 LEVEL 5");
			}
		}

	

	

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			if (!Pasivaa.ContainsKey(ev.Player.SteamId)) { Pasivaa.Add(ev.Player.SteamId, true); }
			if ((ev.Role == Role.SCP_079) && (computerchan != ev.Player.SteamId) )
			{
				computerchan = ev.Player.SteamId;
				
			}
            if(ev.Player.TeamRole.Role == Role.SCP_079) { ev.Player.PersonalBroadcast(10, "Tu habilidad es [control absoluto]: puedes usar los comandos .nukeoff .cellsopen .nukenow .nanobots y .elevatorsoff", false); }
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			
			Boom = false;
            Pasivaa.Clear();
			level = 0;
			computerchan = "0";
			         
           
            elevatoss = false;
		}

		public void OnDetonate()
		{
			if (Boom == false)
			{
				Timing.Run(Secondboom());
				Boom = true;
			}
		}

	

        public void On079TeslaGate(Player079TeslaGateEvent ev)
        {
            if(level >= 3)
            {
                ev.APDrain /= 2;
                
                if(ev.Player.Scp079Data.MaxAP >= 400)
                {
                    ev.APDrain = 0;
                }

            }

        }

        public void OnCallCommand(PlayerCallCommandEvent ev)
        {
            if (ev.Command.StartsWith("nukeoff"))
            {
                if(ev.Player.TeamRole.Role != Role.SCP_079) { ev.ReturnMessage = "Tu no eres SCP-079, pero buen inteneto ;)"; }
                if (ev.Player.TeamRole.Role == Role.SCP_079)
                { 
                    if(ev.Player.Scp079Data.Level < 2) { ev.ReturnMessage = "Necesitas mas nivel"; }
                    if (ev.Player.Scp079Data.Level >= 2)
                    {
                        if (ev.Player.Scp079Data.AP < 200) { ev.ReturnMessage = "Necesitas mas Energía (200)"; }
                        if ((Pasivaa[ev.Player.SteamId] == false) && (ev.Player.Scp079Data.AP >= 200)) { ev.ReturnMessage = "Habilidad en cooldown"; }

                        if ((ev.Player.Scp079Data.AP >= 200)&&(Pasivaa[ev.Player.SteamId] == true))
                        {
                            ev.Player.Scp079Data.AP -= 200;
                            ev.Player.SendConsoleMessage("Procedimiento 70726F746F636F6C6F206465206175746F646573747275636369F36E Cancelado.", "blue");
                            ev.ReturnMessage = "Procedimiento 70726F746F636F6C6F206465206175746F646573747275636369F36E Cancelado. ";
                            Pasivaa[ev.Player.SteamId] = false;
                            Timing.Run(Cooldown079(ev.Player));
                            PluginManager.Manager.Server.Map.StopWarhead();
                            ev.Player.Scp079Data.Exp += 100;
                            if(ev.Player.Scp079Data.Level >= 4) { ev.Player.Scp079Data.MaxAP += 10; }
                        }
                        if (Pasivaa[ev.Player.SteamId] == false) { ev.ReturnMessage = "habilidad en cooldown"; }
                    }
                    
                }
            }
            if (ev.Command.StartsWith("cellsopen"))
            {
                if (ev.Player.TeamRole.Role != Role.SCP_079) { ev.ReturnMessage = "Tu no eres SCP-079, pero buen inteneto ;)"; }
                if (ev.Player.TeamRole.Role == Role.SCP_079)
                {
                    
                    
                        if (ev.Player.Scp079Data.AP < 350) { ev.ReturnMessage = "Necesitas mas Energía (350)"; }

                    if ((Pasivaa[ev.Player.SteamId] == false) && (ev.Player.Scp079Data.AP >= 350)) { ev.ReturnMessage = "Habilidad en cooldown"; }
                    if ((ev.Player.Scp079Data.AP >= 200) && (Pasivaa[ev.Player.SteamId] == true))
                        {
                            ev.Player.Scp079Data.AP -= 350;
                        ev.Player.SendConsoleMessage("Procedimiento 50726F746F636F6C6F20646520456D657267656E63696120416374697661646F2070756572746173206162696572746173 ejecutado.", "blue");
                        ev.ReturnMessage = "Procedimiento 50726F746F636F6C6F20646520456D657267656E63696120416374697661646F2070756572746173206162696572746173 ejecutado. ";
                        Pasivaa[ev.Player.SteamId] = false;
                            Timing.Run(Cooldown079(ev.Player));
                            Timing.Run(liberar());
                            ev.Player.Scp079Data.Exp += 350;
                            if (ev.Player.Scp079Data.Level >= 4) { ev.Player.Scp079Data.MaxAP += 35; }
                        }
                    if (!habilidad079) { ev.ReturnMessage = "habilidad en cooldown"; }
                    

                }
            }
            if (ev.Command.StartsWith("nukenow"))
            {
                if (ev.Player.TeamRole.Role != Role.SCP_079) { ev.ReturnMessage = "Tu no eres SCP-079, pero buen inteneto ;)"; }
                if (ev.Player.TeamRole.Role == Role.SCP_079)
                {
                    if (ev.Player.Scp079Data.AP < 400) { ev.ReturnMessage = "Necesitas mas Energía (400)"; }
                    if (ev.Player.Scp079Data.AP >= 400)
                    {
                        ev.Player.Scp079Data.AP -= 400;
                        ev.Player.SendConsoleMessage("Lo importante es ganar... no?", "red");
                        ev.ReturnMessage = "..., Lo importante es ganar ....";
                        PluginManager.Manager.Server.Map.DetonateWarhead();
                    }
                }
                
            }
            if (ev.Command.StartsWith("elevatorsoff"))
            {
                if (ev.Player.TeamRole.Role != Role.SCP_079) { ev.ReturnMessage = "Tu no eres SCP-079, pero buen inteneto ;)"; }
                if (ev.Player.TeamRole.Role == Role.SCP_079)
                {
                    if (ev.Player.Scp079Data.AP < 200) { ev.ReturnMessage = "Necesitas mas Energía (200)"; }
                    if ((Pasivaa[ev.Player.SteamId] == false) && (ev.Player.Scp079Data.AP >= 200)) { ev.ReturnMessage = "Habilidad en cooldown"; }
                    if ((ev.Player.Scp079Data.AP >= 200)&& (Pasivaa[ev.Player.SteamId] == true))
                    {
                        ev.Player.Scp079Data.AP -= 200;
                        ev.Player.Scp079Data.Exp += 50;
                        if (ev.Player.Scp079Data.Level >= 4) { ev.Player.Scp079Data.MaxAP += 7; }
                        ev.Player.SendConsoleMessage("Protocolo 496E63656E64696F2064657465637461646F2C20616E756C616E646F20617363656E736F72657320 ejecutado", "blue");
                        ev.ReturnMessage = "Protocolo 496E63656E64696F2064657465637461646F2C20616E756C616E646F20617363656E736F72657320 ejecutado";
                        Timing.Run(Cooldown0792(ev.Player));
                        Timing.Run(elevators());
                        Pasivaa[ev.Player.SteamId] = false;
                    }
                }

            }
            if (ev.Command.StartsWith("nanobots"))
            {
                if (ev.Player.TeamRole.Role != Role.SCP_079) { ev.ReturnMessage = "Tu no eres SCP-079, pero buen inteneto ;)"; }
                if (ev.Player.TeamRole.Role == Role.SCP_079)
                {


                    if (ev.Player.Scp079Data.Level < 3)
                    {
                        if (ev.Player.Scp079Data.AP < 100) { ev.ReturnMessage = "Necesitas mas Energía (100)"; }

                        if ((Pasivaa[ev.Player.SteamId] == false) && (ev.Player.Scp079Data.AP >= 100)) { ev.ReturnMessage = "Habilidad en cooldown"; }

                        if ((ev.Player.Scp079Data.AP >= 100) && (Pasivaa[ev.Player.SteamId] == true))
                        {
                            ev.Player.Scp079Data.AP -= 100;
                            ev.Player.SendConsoleMessage("Enviando nanobots al ataque.", "blue");
                            ev.ReturnMessage = "Enviando nanobots al ataque .";
                            Pasivaa[ev.Player.SteamId] = false;
                            Timing.Run(Cooldown0792(ev.Player));
                            System.Random playrs = new System.Random();
                            int posic = playrs.Next(0, PluginManager.Manager.Server.GetPlayers().Count);
                            while ((PluginManager.Manager.Server.GetPlayers()[posic].TeamRole.Team == Team.SPECTATOR) || (PluginManager.Manager.Server.GetPlayers()[posic].TeamRole.Role == Role.SCP_079)) 
                            {
                                if (posic > PluginManager.Manager.Server.NumPlayers)
                                { 
                                    posic = 0; 
                                }
                                posic = posic + 1; 
                            }
                            if (PluginManager.Manager.Server.GetPlayers()[posic].TeamRole.Team == Team.SCP) { PluginManager.Manager.Server.GetPlayers()[posic].AddHealth(50); }

                            if (PluginManager.Manager.Server.GetPlayers()[posic].TeamRole.Team != Team.SCP)
                            {
                                if (PluginManager.Manager.Server.GetPlayers()[posic].GetHealth() <= 50)
                                {
                                    PluginManager.Manager.Server.GetPlayers()[posic].Kill(DamageType.TESLA);
                                    ev.Player.Scp079Data.Exp += 30;
                                    if (ev.Player.Scp079Data.Level >= 4) { ev.Player.Scp079Data.MaxAP += 3; }
                                }
                                if (PluginManager.Manager.Server.GetPlayers()[posic].GetHealth() > 50)
                                {
                                    PluginManager.Manager.Server.GetPlayers()[posic].AddHealth(-50);
                                }


                            }
                            ev.Player.Scp079Data.Exp += 30;
                            if (ev.Player.Scp079Data.Level >= 4) { ev.Player.Scp079Data.MaxAP += 10; }
                        }
                        if (Pasivaa[ev.Player.SteamId] == false) { ev.ReturnMessage = "habilidad en cooldown"; }
                    }
                    if (ev.Player.Scp079Data.Level >= 3)
                    {
                        if (ev.Player.Scp079Data.AP < 50) { ev.ReturnMessage = "Necesitas mas Energía (50)"; }

                        if ((Pasivaa[ev.Player.SteamId] == false) && (ev.Player.Scp079Data.AP >= 50)) { ev.ReturnMessage = "Habilidad en cooldown"; }

                        if ((ev.Player.Scp079Data.AP >= 50) && (Pasivaa[ev.Player.SteamId] == true))
                        {
                            ev.Player.Scp079Data.AP -= 50;
                            ev.Player.SendConsoleMessage("Enviando nanobots mejorados al ataque.", "red");
                            ev.ReturnMessage = "Enviando nanobots mejorados al ataque .";
                            Pasivaa[ev.Player.SteamId] = false;
                            if(ev.Player.Scp079Data.Level <= 3) { Timing.Run(Cooldown0792(ev.Player)); } else { Timing.Run(Cooldown0793(ev.Player)); }
                            
                            System.Random playrs = new System.Random();
                            int posic = playrs.Next(0, PluginManager.Manager.Server.GetPlayers().Count);
                            while ((PluginManager.Manager.Server.GetPlayers()[posic].TeamRole.Team == Team.SPECTATOR)||(PluginManager.Manager.Server.GetPlayers()[posic].TeamRole.Role == Role.SCP_079)) { if (posic > PluginManager.Manager.Server.NumPlayers) { posic = 0; } posic = posic + 1; }
                            if (PluginManager.Manager.Server.GetPlayers()[posic].TeamRole.Team == Team.SCP) { PluginManager.Manager.Server.GetPlayers()[posic].AddHealth(50); }

                            if (PluginManager.Manager.Server.GetPlayers()[posic].TeamRole.Team != Team.SCP)
                            {
                                if (PluginManager.Manager.Server.GetPlayers()[posic].GetHealth() <= 75)
                                {
                                    PluginManager.Manager.Server.GetPlayers()[posic].Kill(DamageType.TESLA);
                                    ev.Player.Scp079Data.Exp += 30;
                                    if (ev.Player.Scp079Data.Level >= 4) { ev.Player.Scp079Data.MaxAP += 3; }
                                }
                                if (PluginManager.Manager.Server.GetPlayers()[posic].GetHealth() > 75)
                                {
                                    PluginManager.Manager.Server.GetPlayers()[posic].AddHealth(-75);
                                }


                            }
                            ev.Player.Scp079Data.Exp += 30;
                            if (ev.Player.Scp079Data.Level >= 4) { ev.Player.Scp079Data.MaxAP += 15; }
                        }
                        if (Pasivaa[ev.Player.SteamId] == false) { ev.ReturnMessage = "habilidad en cooldown"; }
                    }

                }
            }
            if (ev.Command.StartsWith("teslas"))
            {
                if (ev.Player.TeamRole.Role != Role.SCP_079) { ev.ReturnMessage = "Tu no eres SCP-079, pero buen inteneto ;)"; }
                if (ev.Player.TeamRole.Role == Role.SCP_079)
                {
                    if (ev.Player.Scp079Data.AP < 125) { ev.ReturnMessage = "Necesitas mas Energía (125)"; }
                    if((Pasivaa[ev.Player.SteamId] == false) && (ev.Player.Scp079Data.AP >= 125)) { ev.ReturnMessage = "Habilidad en cooldown"; }
                    if ((ev.Player.Scp079Data.AP >= 125)&& (Pasivaa[ev.Player.SteamId] == true))
                    {
                        ev.Player.Scp079Data.AP -= 125;
                        ev.Player.Scp079Data.Exp += 35;
                        if (ev.Player.Scp079Data.Level >= 4) { ev.Player.Scp079Data.MaxAP += 7; }
                        ev.Player.SendConsoleMessage("Sobrecargando Teslas", "blue");
                        ev.ReturnMessage = "Protocolo Sobrecarga ejecutado";
                        Timing.Run(Cooldown0792(ev.Player));
                        Timing.Run(Teslass());
                        Pasivaa[ev.Player.SteamId] = false;
                    }
                }

            }
        }

        public void OnElevatorUse(PlayerElevatorUseEvent ev)
        {
            if (elevatoss)
            {
                if(ev.Player.TeamRole.Team != Team.SCP) { ev.AllowUse = false; }
               
                
            }
        }
    }
}
