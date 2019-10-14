using System.Collections.Generic;
using System.Linq;
using Smod2;
using Smod2.EventHandlers;
using scp4aiur;
using Smod2.Events;
using Smod2.API;

namespace Passivesandskills2
{
	partial class scp079 : IEventHandler079AddExp, IEventHandlerPlayerDie, IEventHandlerGeneratorFinish, IEventHandler079LevelUp,IEventHandlerCallCommand,
		IEventHandlerSetRole, IEventHandlerWaitingForPlayers, IEventHandlerWarheadDetonate, IEventHandlerWarheadStartCountdown, IEventHandlerWarheadStopCountdown, IEventHandler079TeslaGate
	{
		
		bool Nuket = false;
		static bool Boom = false;
		int gen = 0;
		int level = 0;
		static bool overcharge = false;
		string computerchan;
		static Dictionary<string, bool> Computerr = new Dictionary<string, bool>();
		Vector posicionteni;
		static Dictionary<string, Player> Pasivaa = new Dictionary<string, Player>();
        static bool habilidad079 = true;

        // Este codigo ace que cuando la nuke sea activada el pc pueda robar el cuerpo de quien muera en el tesla y usarlo como quiera
        // Ademas de que ganas ap infinito al nivel 5 en funcion de la xp que ganes

		public static IEnumerable<float> Secondboom()
		{
			yield return 5f;
			PluginManager.Manager.Server.Map.DetonateWarhead();
			yield return 2f;
			
			yield return 2f;
			
			yield return 2f;
			PluginManager.Manager.Server.Map.DetonateWarhead();
            yield return 60f;
            if (Boom)
            {
                PluginManager.Manager.Server.Map.DetonateWarhead();
            }
               
        }
        public static IEnumerable<float> liberar()
        {
            int contador = 0;
            yield return 5f;
            System.Random Number = new System.Random();
            int proba = Number.Next(0, 100);
            if (proba <= 15)
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
            if((proba >= 16)&&(proba <= 29))
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
            if ((proba >= 30))
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

        public static IEnumerable<float> Cooldown079()
        {
            
            yield return 120f;
            habilidad079 = true;

        }



        public static IEnumerable<float> Pcoff()
		{
			yield return 60f;
			overcharge = true;
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

		public void OnPlayerDie(PlayerDeathEvent ev)
		{
			if (overcharge == false)
			{
			
			}
		}

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			if (!Pasivaa.ContainsKey(ev.Player.SteamId)) { Pasivaa.Add(ev.Player.SteamId, ev.Player); }
			if ((ev.Role == Role.SCP_079) && (computerchan != ev.Player.SteamId) && (Computerr.ContainsKey(ev.Player.SteamId)))
			{
				computerchan = ev.Player.SteamId;
				Computerr.Add(ev.Player.SteamId, true);
			}
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			Nuket = false;
			Boom = false;   
			overcharge = false;
			level = 0;
			computerchan = "0";
			gen = 0;
            Timing.Remove(1);
            habilidad079 = true;
		}

		public void OnDetonate()
		{
			if (Boom == false)
			{
				Timing.Run(Secondboom());
				Boom = true;
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

        public void On079TeslaGate(Player079TeslaGateEvent ev)
        {
            if(level > 3)
            {
                ev.APDrain /= 2;
                if(ev.Player.Scp079Data.MaxAP >= 400)
                {
                    ev.APDrain = 10;
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


                        if ((ev.Player.Scp079Data.AP >= 200)&&(habilidad079))
                        {
                            ev.Player.Scp079Data.AP -= 200;
                            ev.ReturnMessage = "Procedimiento 70726F746F636F6C6F206465206175746F646573747275636369F36E Cancelado. ";
                            habilidad079 = false;
                            Timing.Run(Cooldown079());
                            PluginManager.Manager.Server.Map.StopWarhead();
                            ev.Player.Scp079Data.Exp += 100;
                            if(ev.Player.Scp079Data.Level >= 4) { ev.Player.Scp079Data.MaxAP += 10; }
                        }
                        if (!habilidad079) { ev.ReturnMessage = "habilidad en cooldown"; }
                    }
                    
                }
            }
            if (ev.Command.StartsWith("cellsopen"))
            {
                if (ev.Player.TeamRole.Role != Role.SCP_079) { ev.ReturnMessage = "Tu no eres SCP-079, pero buen inteneto ;)"; }
                if (ev.Player.TeamRole.Role == Role.SCP_079)
                {
                    
                    
                        if (ev.Player.Scp079Data.AP < 350) { ev.ReturnMessage = "Necesitas mas Energía (350)"; }


                        if ((ev.Player.Scp079Data.AP >= 200) && (habilidad079))
                        {
                            ev.Player.Scp079Data.AP -= 350;
                            ev.ReturnMessage = "Procedimiento 50726F746F636F6C6F20646520456D657267656E63696120416374697661646F2070756572746173206162696572746173 ejecutado. ";
                            habilidad079 = false;
                            Timing.Run(Cooldown079());
                            Timing.Run(liberar());
                            ev.Player.Scp079Data.Exp += 350;
                            if (ev.Player.Scp079Data.Level >= 4) { ev.Player.Scp079Data.MaxAP += 35; }
                        }
                    if (!habilidad079) { ev.ReturnMessage = "habilidad en cooldown"; }
                    

                }
            }
        }
    }
}
