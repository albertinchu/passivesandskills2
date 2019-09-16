﻿using System;
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
    partial class Clasesd : IEventHandlerPlayerHurt, IEventHandlerSetRole, IEventHandlerPlayerDropItem, IEventHandlerWaitingForPlayers
    {

        static Dictionary<string, bool> Classdh = new Dictionary<string, bool>();

        public static IEnumerable<float> ClassdTimer(Player player)
        {
            player.SetGhostMode(true, false, false);
            yield return 10f;
            player.SetGhostMode(false);
            yield return 50f;
            player.GiveItem(ItemType.FLASHLIGHT);
            Classdh[player.SteamId] = true;
        }

        private Passivesandskills2 plugin;
        public Clasesd(Passivesandskills2 plugin)
        {
            this.plugin = plugin;
        }
        public void OnPlayerHurt(PlayerHurtEvent ev)
        {
            //Class D - [Astucia] //
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
                // [Dboys rules] //
                if ((ev.Attacker.GetHealth() <= 40) && (ev.DamageType != DamageType.FRAG) && (ev.DamageType != DamageType.TESLA))
                {
                    ev.Attacker.SetHealth(ev.Attacker.GetHealth() + Convert.ToInt32(ev.Damage));
                    if (ev.Attacker.GetHealth() <= 20)
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
                Classdh.Add(ev.Player.SteamId, true);
                ev.Player.PersonalBroadcast(10, "Tu pasiva es [Astucia] robas munición al disparar. [Dboy rules]: cuando estas a poca vida robas vida. Tu Habilidad es [Sigilo de doble filo]tirar tu linterna te hace invisible por 35 de salud (puedes morir si tienes menos de 36 de salud). ", false);
                ev.Player.PersonalBroadcast(10, " [Sigilo de doble filo]: (puedes morir si tienes menos de 36 de salud). ", false);
            }
        }

        public void OnPlayerDropItem(PlayerDropItemEvent ev)
        {
            //Class D - [Sigilo de doble filo]//
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
        }

        public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
        {
            Classdh.Clear();
        }
    }
}