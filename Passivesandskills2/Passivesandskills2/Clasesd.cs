﻿using System;
using System.Collections.Generic;
using Smod2.EventHandlers;

using Smod2.Events;
using Smod2.API;
using MEC;

namespace Passivesandskills2
{
	partial class classd : IEventHandlerPlayerHurt, IEventHandlerPlayerDropItem, IEventHandlerWaitingForPlayers, IEventHandlerSetRole, IEventHandlerCallCommand, IEventHandlerPlayerDie
	{
         
       
		static Dictionary<string, bool> Classdh = new Dictionary<string, bool>();
        static Dictionary<string, int> cooldownn = new Dictionary<string, int>();
        // Los clases d roban munición si su rival tiene munición, ganan salud = al daño que hacen cuando estan a poca vida el cual se duplica si estan a muy poca vida
        // tienen la habilidad de hacerse invisible por 35 de salud durante 10 segundos , disparar desactiva la habilidad
    
		
        
		
		public void OnPlayerHurt(PlayerHurtEvent ev)
		{
			//Class D - [Astucia] //
            if(ev.Attacker.TeamRole.Role == Role.CHAOS_INSURGENCY) 
            {
                if ((Classdh.ContainsKey(ev.Attacker.SteamId)))
                {
                    cooldownn[ev.Attacker.SteamId] += 12;
                }
            }
			if (ev.Attacker.TeamRole.Role == Role.CLASSD)
			{
                // detecta si la habilidad del clase d esta en cooldown y suma 3 segundos al diccionario restandole asi tiempo a la habilidad.
              
                if(Classdh[ev.Attacker.SteamId] == false) { cooldownn[ev.Attacker.SteamId] += 3; }
                
                if (ev.Attacker.GetGhostMode() == true) { ev.Attacker.SetGhostMode(false); }
                
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
				// [Dboys rules] //
				if ((ev.Attacker.GetHealth() <= 45) && (ev.DamageType != DamageType.FRAG) && (ev.DamageType != DamageType.TESLA)&&(ev.DamageType != DamageType.FALLDOWN)&&(ev.DamageType != DamageType.POCKET))
				{
					ev.Attacker.SetHealth(ev.Attacker.GetHealth() + Convert.ToInt32(ev.Damage));
					if (ev.Attacker.GetHealth() <= 25)
					{
						ev.Attacker.SetHealth(ev.Attacker.GetHealth() + Convert.ToInt32(ev.Damage));
					}
				}
			}
		}

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			if ((ev.Role == Role.CLASSD) && (!Classdh.ContainsKey(ev.Player.SteamId)))
			{
                cooldownn.Add(ev.Player.SteamId, 0);
				Classdh.Add(ev.Player.SteamId, true);
				ev.Player.PersonalBroadcast(10, "Tu pasiva es [Astucia] robas munición al disparar. [Dboy rules]: cuando estas a poca vida robas vida. Tu Habilidad es [Sigilo de doble filo]tirar tu linterna te hace invisible 10s . ", false);
				ev.Player.PersonalBroadcast(10, " [Sigilo de doble filo]:resta 35 de salud cada vez que la usas (puedes morir si tienes menos de 36 de salud), y para hacerte invisible TIRA LA LINTERNA AL SUELO. ", false);
			}
		}

		public void OnPlayerDropItem(PlayerDropItemEvent ev)
		{
			//Class D - [Sigilo de doble filo]//
			if ((ev.Player.TeamRole.Role == Role.CLASSD) && (Classdh[ev.Player.SteamId] == true) && (ev.Item.ItemType == ItemType.FLASHLIGHT))
			{
				ev.ChangeTo = ItemType.NULL;
                if (ev.Player.GetHealth() < 35)
                {
                    ev.Player.Kill(DamageType.FALLDOWN);
                }

                if (ev.Player.GetHealth() >= 35)
                {
                    Classdh[ev.Player.SteamId] = false;
                    ev.Player.AddHealth(-35);
                    int p = (int)System.Environment.OSVersion.Platform;
                    if ((p == 4) || (p == 6) || (p == 128)) 
                    {
                        MEC.Timing.RunCoroutine(Classd(ev.Player), MEC.Segment.FixedUpdate);
                
                    }
                else { MEC.Timing.RunCoroutine(Classd(ev.Player), 1); }
                 
				}
               
			}

            if ((ev.Player.TeamRole.Role == Role.CHAOS_INSURGENCY))
            {
               if ((Classdh.ContainsKey(ev.Player.SteamId))) 
               {
                    if ((Classdh[ev.Player.SteamId] == true) && (ev.Item.ItemType == ItemType.FLASHLIGHT))
                    {
                        ev.ChangeTo = ItemType.NULL;
                        if (ev.Player.GetHealth() <= 20)
                        {
                            ev.Player.Kill(DamageType.FALLDOWN);
                        }


                        if (ev.Player.GetHealth() > 20)

                        {
                            Classdh[ev.Player.SteamId] = false;
                            ev.Player.AddHealth(-20);
                            int p = (int)System.Environment.OSVersion.Platform;
                            if ((p == 4) || (p == 6) || (p == 128))
                            {
                                MEC.Timing.RunCoroutine(Classd(ev.Player), MEC.Segment.FixedUpdate);

                            }
                            else { MEC.Timing.RunCoroutine(Classd(ev.Player), 1); }


                        }
                    }
               }
            }

        }
        // registra en un diccionario los segundos de coldown hasta recuperar la habilidad y la linterna
        public IEnumerator<float> Classd(Player player)
        {
            
            player.SetGhostMode(true, false, false);
            player.PersonalBroadcast(10, "Estas en sigilo", true);
            player.PersonalBroadcast(3, "Ya no estas en sigilo", true);
            yield return MEC.Timing.WaitForSeconds(10f);
            player.SetGhostMode(false);
            player.PersonalClearBroadcasts();
            player.PersonalBroadcast(3, "Ya no estas en sigilo", true);
            while (cooldownn[player.SteamId] <= 50)
            {
               yield return MEC.Timing.WaitForSeconds(1f);
                cooldownn[player.SteamId] += 1;
            }
            if (((player.TeamRole.Role == Role.CLASSD)||(player.TeamRole.Role == Role.CHAOS_INSURGENCY)) && (cooldownn[player.SteamId] >= 50))
            {
                player.GiveItem(ItemType.FLASHLIGHT);
                Classdh[player.SteamId] = true;
                cooldownn[player.SteamId] = 0;
                yield break;
            }
         

        }

        public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{

         
            Classdh.Clear();
            cooldownn.Clear();
		}

 

        public void OnPlayerDie(PlayerDeathEvent ev)
        {
            if (ev.Player.TeamRole.Role == Role.CLASSD)
            {
                if (Classdh.ContainsKey(ev.Player.SteamId))
                {
                    Classdh.Remove(ev.Player.SteamId);
                }
            }
        
   
        }
    }
    
}
