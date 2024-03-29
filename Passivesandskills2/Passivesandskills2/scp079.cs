﻿using System.Collections.Generic;
using Smod2;
using Smod2.EventHandlers;
using MEC;
using Smod2.Events;
using Smod2.API;

namespace Passivesandskills2
{
	partial class scp079 : IEventHandler079AddExp, IEventHandler079LevelUp,IEventHandlerCallCommand,
		IEventHandlerSetRole, IEventHandlerWaitingForPlayers, IEventHandlerWarheadDetonate, IEventHandler079TeslaGate
        ,IEventHandlerElevatorUse, IEventHandlerCheckRoundEnd, IEventHandlerSetConfig
	{
		static bool elevatoss = false;
		
		static bool Boom = false;
		
		int level = 0;
		
		
		
		static Dictionary<string, bool> Pasivaa = new Dictionary<string, bool>();
        static bool habilidad079 = true;
        private List<Smod2.API.TeslaGate> teslas;

        
        // Ademas de que ganas ap infinito al nivel 5 en funcion de la xp que ganes
        // detona la nuke 2 veces
        private IEnumerator<float> Secondboom()
		{
            yield return MEC.Timing.WaitForSeconds(5f);
            PluginManager.Manager.Server.Map.DetonateWarhead();


            yield return MEC.Timing.WaitForSeconds(60f);
            if (Boom)
            {
                PluginManager.Manager.Server.Map.DetonateWarhead();
            }
               
        }
        // aumenta el rango de los teslas en 2 y los activa durante 10 s
        private IEnumerator<float> Teslass()
        {
            
            int contador = 0;
            
            while (contador <= 10)
            {
                

                
                    foreach (Smod2.API.TeslaGate tesla in teslas)
                    {
                        tesla.Activate(true);
                        tesla.TriggerDistance *= 2;
                    }


                yield return MEC.Timing.WaitForSeconds(1f);
                contador += 1;
            }
        }
        //libera de forma aleatoria clases d, cientificos o un perro de espinas
        private IEnumerator<float> liberar()
        {
            int contador = 0;
            yield return MEC.Timing.WaitForSeconds(5f);
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
                            yield return MEC.Timing.WaitForSeconds(0.2f);
                            if (contador == 1) { player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_049)); }
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
                            yield return MEC.Timing.WaitForSeconds(0.2f);
                            if (contador == 1) { player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_049)); }
                            if (contador == 2) { player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_096)); }
                            if (contador == 3) { player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_939_53)); }
                        }
                    }
                }
            }


        }
        // cooldown de habilidad
        private IEnumerator<float> Cooldown079(Player player)
        {

            yield return MEC.Timing.WaitForSeconds(120f);
            Pasivaa[player.SteamId] = true;

        }
        // cooldown de habilidad
        private IEnumerator<float> Cooldown0792(Player player)
        {

            yield return MEC.Timing.WaitForSeconds(60f);
            Pasivaa[player.SteamId] = true;

        }
        // cooldown de habilidad
        private IEnumerator<float> Cooldown0793(Player player)
        {

            yield return MEC.Timing.WaitForSeconds(60f);
            Pasivaa[player.SteamId] = true;

        }
        // detiene el funcionamiento de los ascensores
        private IEnumerator<float> elevators()
        {
            elevatoss = true;
            yield return MEC.Timing.WaitForSeconds(20f);
            elevatoss = false;

        }



       
        //en tier 5 la xp se acumula como xp adicional y velocidad en la regeneración de la xp
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
        // aplica una mejora en el tier IV y alerta de que el SCP 079 es tier V
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
            // asigna la pasiva al 079
			if (!Pasivaa.ContainsKey(ev.Player.SteamId)) { Pasivaa.Add(ev.Player.SteamId, true); }
			
            if(ev.Player.TeamRole.Role == Role.SCP_079) { ev.Player.PersonalBroadcast(10, "Tu habilidad es [control absoluto]: puedes usar los comandos .nukeoff .cellsopen .nukenow  y .elevatorsoff", false); }
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			
			Boom = false;
            Pasivaa.Clear();
			level = 0;
			
            
           
            elevatoss = false;
		}
        // registra si la boma explotó para activar el timer 
		public void OnDetonate()
		{
			if (Boom == false)
			{
                int p = (int)System.Environment.OSVersion.Platform;
                if ((p == 4) || (p == 6) || (p == 128))
                {
                    MEC.Timing.RunCoroutine(Secondboom(), MEC.Segment.FixedUpdate);

                }
                else { MEC.Timing.RunCoroutine(Secondboom(), 1); }
                Boom = true;
			}
		}

	
        // segun el ap del ordenador los teslas cuestan menos
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
            // cancela la nuke
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
                            int p = (int)System.Environment.OSVersion.Platform;
                            if ((p == 4) || (p == 6) || (p == 128))
                            {
                                MEC.Timing.RunCoroutine(Cooldown079(ev.Player), MEC.Segment.FixedUpdate);

                            }
                            else { MEC.Timing.RunCoroutine(Cooldown079(ev.Player), 1); }
                            PluginManager.Manager.Server.Map.StopWarhead();
                            ev.Player.Scp079Data.Exp += 100;
                            if(ev.Player.Scp079Data.Level >= 4) { ev.Player.Scp079Data.MaxAP += 10; }
                        }
                        if (Pasivaa[ev.Player.SteamId] == false) { ev.ReturnMessage = "habilidad en cooldown"; }
                    }
                    
                }
            }

            //libera clases d o cientificos o 1 scp
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
                        int p = (int)System.Environment.OSVersion.Platform;
                        if ((p == 4) || (p == 6) || (p == 128))
                        {
                            MEC.Timing.RunCoroutine(Cooldown079(ev.Player), MEC.Segment.FixedUpdate);

                        }
                        else { MEC.Timing.RunCoroutine(Cooldown079(ev.Player), 1); }
                        
                        if ((p == 4) || (p == 6) || (p == 128))
                        {
                            MEC.Timing.RunCoroutine(liberar(), MEC.Segment.FixedUpdate);

                        }
                        else { MEC.Timing.RunCoroutine(liberar(), 1); }
                        ev.Player.Scp079Data.Exp += 350;
                            if (ev.Player.Scp079Data.Level >= 4) { ev.Player.Scp079Data.MaxAP += 35; }
                        }
                    if (!habilidad079) { ev.ReturnMessage = "habilidad en cooldown"; }
                    

                }
            }
            // instadetona la nuke
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
            //cancela ascensores
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
                        int p = (int)System.Environment.OSVersion.Platform;
                        if ((p == 4) || (p == 6) || (p == 128))
                        {
                            MEC.Timing.RunCoroutine(Cooldown0792(ev.Player), MEC.Segment.FixedUpdate);

                        }
                        else { MEC.Timing.RunCoroutine(Cooldown0792(ev.Player), 1); }
                        
                        if ((p == 4) || (p == 6) || (p == 128))
                        {
                            MEC.Timing.RunCoroutine(elevators(), MEC.Segment.FixedUpdate);

                        }
                        else { MEC.Timing.RunCoroutine(elevators(), 1); }
                        Pasivaa[ev.Player.SteamId] = false;
                    }
                }

            }

          
                  
            // aumenta el rango en el que los teslas se activan
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
                        int p = (int)System.Environment.OSVersion.Platform;
                        if ((p == 4) || (p == 6) || (p == 128))
                        {
                            MEC.Timing.RunCoroutine(Cooldown0792(ev.Player), MEC.Segment.FixedUpdate);

                        }
                        else { MEC.Timing.RunCoroutine(Cooldown0792(ev.Player), 1); }
                        
                        if ((p == 4) || (p == 6) || (p == 128))
                        {
                            MEC.Timing.RunCoroutine(Teslass(), MEC.Segment.FixedUpdate);

                        }
                        else { MEC.Timing.RunCoroutine(Teslass(), 1); }
                        Pasivaa[ev.Player.SteamId] = false;
                    }
                }

            }
        }

        public void OnElevatorUse(PlayerElevatorUseEvent ev)
        {
            //cancela ascensores
            if (elevatoss)
            {
                if(ev.Player.TeamRole.Team != Smod2.API.Team.SCP) { ev.AllowUse = false; }
               
                
            }
        }

        public void OnCheckRoundEnd(CheckRoundEndEvent ev)
        {
            Pasivaa.Clear();
            level = 0;
           


            elevatoss = false;
        }

        public void OnSetConfig(SetConfigEvent ev)
        {
            // cancela la nuke , puesto que el 079 puede destruir la instalación al instante
            switch (ev.Key)
            {
                case "auto_warhead_start_lock":
                    ev.Value = false;
                    break;


            }
        }
    }
}
