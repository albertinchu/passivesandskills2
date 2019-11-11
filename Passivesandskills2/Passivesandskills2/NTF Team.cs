using System;
using System.Linq;
using System.Collections.Generic;
using Smod2;
using Smod2.EventHandlers;
using Unity;
using Smod2.Events;
using Smod2.API;
using MEC;
using UnityEngine;
using UnityEditor;
using UnityStandardAssets;
using UnityEngineInternal;
using UnityEngine.Networking;



namespace Passivesandskills2
{
	partial class Ntfteam : IEventHandlerSetRole, IEventHandlerPlayerHurt, IEventHandlerWaitingForPlayers, IEventHandlerThrowGrenade, IEventHandlerPlayerDie, IEventHandlerCallCommand, IEventHandlerPlayerDropItem
	{
		
		static Dictionary<string, bool> NTFli = new Dictionary<string, bool>();

        static Dictionary<string, bool> NTFlic4 = new Dictionary<string, bool>();




        // el Teniente tiene la habilidad de teletransportar a un enemigo no scp a una sala aleatoria , funciona con los zombies.
        // cadetes resisten mejor el daño de explosiones y las flash agregan 30 de salud
        public void OnPlayerHurt(PlayerHurtEvent ev)
		{
			// COMANDANTE //
		// daño adicional al comandante
            if ((ev.Attacker.TeamRole.Role == Role.NTF_COMMANDER)&&((ev.Player.TeamRole.Team == Smod2.API.Team.SCP)||(ev.Player.TeamRole.Team == Smod2.API.Team.CLASSD) || (ev.Player.TeamRole.Team == Smod2.API.Team.CHAOS_INSURGENCY))) 
            {
                ev.Damage += 15;
                if(ev.Player.GetHealth() <= ev.Player.TeamRole.MaxHP / 2) 
                {
                    ev.Damage += 15;
                }
            }


            
		
			// CADETES //
			if ((ev.Player.TeamRole.Role == Role.NTF_CADET) && (ev.DamageType == DamageType.FRAG))
			{
				ev.Damage /= 2;
			}
			// TENIENTE //
			if (ev.Attacker.TeamRole.Role == Role.NTF_LIEUTENANT)
			{
				
                if ((ev.Player.TeamRole.Role == Role.CLASSD) || (ev.Player.TeamRole.Role == Role.SCIENTIST))
                {
                    if (ev.Player.TeamRole.Role == Role.SCIENTIST) { ev.Damage = ev.Damage / 3; }
                    if ((ev.Player.GetHealth() <= 50) && (NTFli[ev.Attacker.SteamId] == true))
                    {
                        NTFli[ev.Attacker.SteamId] = false;
                      
                        int p = (int)System.Environment.OSVersion.Platform;
                        if ((p == 4) || (p == 6) || (p == 128))
                        {
                            MEC.Timing.RunCoroutine(Cooldown(ev.Attacker), MEC.Segment.FixedUpdate);

                        }
                        else { MEC.Timing.RunCoroutine(Cooldown(ev.Attacker), 1); }
                     
                        if ((p == 4) || (p == 6) || (p == 128))
                        {
                            MEC.Timing.RunCoroutine(Intimidacion(ev.Player), MEC.Segment.FixedUpdate);

                        }
                        else { MEC.Timing.RunCoroutine(Intimidacion(ev.Player), 1); }
                    }
                }
                    if(NTFli[ev.Attacker.SteamId] == false)
                    {
                        if(ev.Player.TeamRole.Team == Smod2.API.Team.SCP)
                        ev.Damage += ev.Player.GetHealth()/100;
                        if(ev.Player.TeamRole.Team != Smod2.API.Team.SCP)
                        {
                            ev.Damage += 15;
                            if(ev.Player.TeamRole.Role == Role.CHAOS_INSURGENCY)
                            {
                                ev.Damage += 10;
                            }
                        }
                    }

				
				if ((ev.Player.TeamRole.Role == Role.CHAOS_INSURGENCY))
				{
					if ((ev.Player.GetHealth() <= 85) && (NTFli[ev.Attacker.SteamId] == true))
					{
						NTFli[ev.Attacker.SteamId] = false;
                        int p = (int)System.Environment.OSVersion.Platform;
                        if ((p == 4) || (p == 6) || (p == 128))
                        {
                            MEC.Timing.RunCoroutine(Intimidacion(ev.Player), MEC.Segment.FixedUpdate);

                        }
                        else { MEC.Timing.RunCoroutine(Intimidacion(ev.Player), 1); }
                        
                        if ((p == 4) || (p == 6) || (p == 128))
                        {
                            MEC.Timing.RunCoroutine(Cooldown(ev.Attacker), MEC.Segment.FixedUpdate);

                        }
                        else { MEC.Timing.RunCoroutine(Cooldown(ev.Attacker), 1); }

                    }
				}
				if(ev.Player.TeamRole.Role == Role.SCP_049_2)
				{
					if ((ev.Player.GetHealth() <= (ev.Player.TeamRole.MaxHP/2)) && (NTFli[ev.Attacker.SteamId] == true))
					{
						NTFli[ev.Attacker.SteamId] = false;
                        int p = (int)System.Environment.OSVersion.Platform;
                        if ((p == 4) || (p == 6) || (p == 128))
                        {
                            MEC.Timing.RunCoroutine(Intimidacion(ev.Player), MEC.Segment.FixedUpdate);

                        }
                        else { MEC.Timing.RunCoroutine(Intimidacion(ev.Player), 1); }
                        
                        if ((p == 4) || (p == 6) || (p == 128))
                        {
                            MEC.Timing.RunCoroutine(Cooldown(ev.Attacker), MEC.Segment.FixedUpdate);

                        }
                        else { MEC.Timing.RunCoroutine(Cooldown(ev.Attacker), 1); }

                    }
				}
			}
		}

    

        private IEnumerator<float> Cooldown(Player attacker)
        {
            yield return MEC.Timing.WaitForSeconds(40f);
            NTFli[attacker.SteamId] = true;
        }

        // Teniente //
      
		// TENEINETE HABILIDAD //
        //Con este fragmento el teniente teleporta a cualquier sujeto que no se Scientist al light con un 90%
        // a los Scientists los teleporta de otra forma
		private IEnumerator<float> Intimidacion(Player player)
		{
            System.Random sala = new System.Random();

			int contadorb = sala.Next(0,100);

            yield return MEC.Timing.WaitForSeconds(0.25f);
            if (player.TeamRole.Role != Role.SCIENTIST)
            {
                if ((contadorb >= 0) && (contadorb <= 50))
                {
                    player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_173));
                }
                if ((contadorb >= 51) && (contadorb <= 59))
                {
                    player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_939_89));
                }
                if ((contadorb >= 60) && (contadorb <= 89))
                {
                    player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.CLASSD));
                }
                if ((contadorb >= 90) && (contadorb <= 100))
                {
                    player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCIENTIST));
                }
            }
            if (player.TeamRole.Role == Role.SCIENTIST)
            {
             
                if ((contadorb >= 0) && (contadorb <= 33))
                {
                    player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_096));
                }
                if ((contadorb >= 34) && (contadorb <= 50))
                {
                    player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_049));
                }
                if ((contadorb >= 51) && (contadorb <= 74))
                {
                    player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.SCP_939_53));
                }
                if ((contadorb >= 75) && (contadorb <= 100))
                {
                    player.Teleport(PluginManager.Manager.Server.Map.GetRandomSpawnPoint(Role.FACILITY_GUARD));
                }
                
            }
            yield return MEC.Timing.WaitForSeconds(1f);
            if (player.TeamRole.Role == Role.SCIENTIST) { player.AddHealth(25); }
        }



		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			// TENIENTE //
      
            
			if (ev.Player.TeamRole.Role == Role.NTF_LIEUTENANT)
			{
				if (!NTFli.ContainsKey(ev.Player.SteamId))
				{
                    NTFlic4.Add(ev.Player.SteamId, false);
					NTFli.Add(ev.Player.SteamId, true);
                    ev.Player.SendConsoleMessage("[cambiar las tornas]: Cambiar las tornas es una pasiva Tactica con 40s de cooldown  la cual teletransporta al enemigo cuando este esta a menos del 50% de vida . (Esta habilidad no se aplica a SCPS pero si a Zombies y tampoco se aplica a aliados)", "blue");
                    ev.Player.PersonalBroadcast(10, "Tu pasiva es [cambiar las tornas]: Cambias la posición del enemigo de forma aleatoria cuando esta por debajo de 50%  (mas info en la consola), cuando has usado tu habilidad durante 40s tienes la pasiva", false);
                    ev.Player.PersonalBroadcast(10, "[De servicio] que aumenta tu daño en 15 a todos los objetivos y es el doble de daño contra chaos, con SCPS 1% de la vida actual del SCP", false);
                }
				
				
            }
            
			// CADETE //
			if (ev.Player.TeamRole.Role == Role.NTF_CADET)
			{
				
				ev.Player.SendConsoleMessage("[Flash rápido]: Tras lanzar una granada cegadora obtienes un escudo de 30 de salud, (este se anula si el comandante usa su granada para aplicarte 200 de salud pero se acumula si se aplicó los 200 de salud antes)", "blue");
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Tenacidad explosiva]: Recibes daño reducido entre 2 de las granadas.[Flash Rápido]: (mas info en la consola)", false);
			}
			// COMANDANTE //
			if ((ev.Player.TeamRole.Role == Role.NTF_COMMANDER))
			{
				
				ev.Player.SendConsoleMessage("[Preocupación por los tuyos]: Tus disparos hacen como cura parte del daño que causarían a tus aliados y las granadas Instacuran 200 de salud (¡OJO!: No se aplica a guardias ni científicos", "blue");
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Lider del Escudrón]: Causas daño adicional a secas (15) el doble contra sujetos a mitad de vida.[Preocupación por los tuyos]: tus ataques curan aliados (mas info en la consola)", false);
			}
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			NTFli.Clear();
            NTFlic4.Clear();
		}

		public void OnThrowGrenade(PlayerThrowGrenadeEvent ev)
		{
            //La pasiva de los cadetes de que cuando lanzan granadas flash o granadas ganan salud y con las frag ganan una flash
            if((ev.Player.TeamRole.Role == Role.NTF_CADET)&&(ev.GrenadeType == GrenadeType.FRAG_GRENADE))
            {
                ev.Player.AddHealth(20);
                ev.Player.GiveItem(ItemType.FLASHBANG);
            }
			if ((ev.Player.TeamRole.Role == Role.NTF_CADET) && (ev.GrenadeType == GrenadeType.FLASHBANG))
			{
				ev.Player.AddHealth(30);
			}
            if(ev.Player.TeamRole.Role == Role.NTF_LIEUTENANT) 
            {
                if (NTFlic4.ContainsKey(ev.Player.SteamId)) 
                { 
                
                    if(NTFlic4[ev.Player.SteamId] == true)                   
                    {
                        

                        
                        GrenadeManager grenadeManager = ((GameObject)ev.Player.GetGameObject()).GetComponent<GrenadeManager>();
                        foreach (GrenadeSettings grenade in grenadeManager.availableGrenades)
                        {
                            grenade.timeUnitilDetonation = 120f;
                            
                        }
                    }
                    else
                    {

                        GrenadeManager grenadeManager = ((GameObject)ev.Player.GetGameObject()).GetComponent<GrenadeManager>();
                        foreach (GrenadeSettings grenade in grenadeManager.availableGrenades)
                        {
                            grenade.timeUnitilDetonation = 6f;
                        }

                    }


                }

            }
               

           
		}
	

        public void OnPlayerDie(PlayerDeathEvent ev)
        {
            //Pasiva del comandante [Justicia] mata al asesino del comandante si es un ntf.
            if((ev.Player.TeamRole.Role == Role.NTF_COMMANDER)&&(ev.Killer.TeamRole.Team == Smod2.API.Team.NINETAILFOX)&&(ev.Killer.TeamRole.Role != Role.FACILITY_GUARD))
            { ev.Killer.Kill(DamageType.LURE); }


        }

       

        public void OnCallCommand(PlayerCallCommandEvent ev)
        {
            if (ev.Command.StartsWith("c4modeon")) 
            {
                if ((NTFlic4.ContainsKey(ev.Player.SteamId)) && (ev.Player.TeamRole.Role == Role.NTF_LIEUTENANT) && (NTFlic4[ev.Player.SteamId] == true)) { ev.Player.SendConsoleMessage("Ya estan activados", "blue"); }
                    if ((NTFlic4.ContainsKey(ev.Player.SteamId)) && (ev.Player.TeamRole.Role == Role.NTF_LIEUTENANT) && (NTFlic4[ev.Player.SteamId] ==false)) { NTFlic4[ev.Player.SteamId] = true; ev.ReturnMessage = "C4 activados"; }
                
                if(ev.Player.TeamRole.Role != Role.NTF_LIEUTENANT) { ev.ReturnMessage = "Tu no eres Teniente"; }

           
            }
            if (ev.Command.StartsWith("c4modeoff")) 
            {
                if ((NTFlic4.ContainsKey(ev.Player.SteamId)) && (ev.Player.TeamRole.Role == Role.NTF_LIEUTENANT) && (NTFlic4[ev.Player.SteamId] == false)) { ev.Player.SendConsoleMessage("Ya estan desactivados", "blue"); }
                if ((NTFlic4.ContainsKey(ev.Player.SteamId)) && (ev.Player.TeamRole.Role == Role.NTF_LIEUTENANT) && (NTFlic4[ev.Player.SteamId] == true)) { NTFlic4[ev.Player.SteamId] = false; ev.ReturnMessage = "C4 desactivados"; }
                if (ev.Player.TeamRole.Role != Role.NTF_LIEUTENANT) { ev.ReturnMessage = "Tu no eres Teniente"; }
            }
        }

        public void OnPlayerDropItem(PlayerDropItemEvent ev)
        {
           if((ev.Player.TeamRole.Role == Role.NTF_LIEUTENANT) &&(ev.Item.ItemType == ItemType.WEAPON_MANAGER_TABLET))
            {
                if (NTFlic4.ContainsKey(ev.Player.SteamId)) 
                {
                    if (NTFlic4[ev.Player.SteamId]) 
                    {
                        GrenadeManager grenadeManager = ((GameObject)ev.Player.GetGameObject()).GetComponent<GrenadeManager>();
                        foreach (GrenadeSettings grenade in grenadeManager.availableGrenades)
                        {
                            grenade.timeUnitilDetonation = 0.2f;
                        }
                        int p = (int)System.Environment.OSVersion.Platform;
                        if ((p == 4) || (p == 6) || (p == 128))
                        {
                            MEC.Timing.RunCoroutine(C4s(ev.Player), MEC.Segment.FixedUpdate);

                        }
                        else { MEC.Timing.RunCoroutine(C4s(ev.Player), 1); }
                    }
                }
            }
        }

        private IEnumerator<float> C4s(Player attacker)
        {
            yield return MEC.Timing.WaitForSeconds(1f);
            GrenadeManager grenadeManager = ((GameObject)attacker.GetGameObject()).GetComponent<GrenadeManager>();
            foreach (GrenadeSettings grenade in grenadeManager.availableGrenades)
            {
                grenade.timeUnitilDetonation = 120f;
            }
        }
    }
}
