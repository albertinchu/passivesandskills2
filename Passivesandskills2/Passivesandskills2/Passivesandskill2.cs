﻿using Smod2;
using Smod2.Attributes;

namespace Passivesandskills2
{
	[PluginDetails(
		author = "Albertinchu con la ayuda de InsetJesux y Roger",
		name = "Passives and skills 2",
		description = "Que pasará si dejamos que los jugadores tengan poderes...",
		id = "albertinchu.gamemode.Passivesandskills2",
		version = "3.5.0",
		SmodMajor = 3,
		SmodMinor = 4,
		SmodRevision = 0
		)]
public class Passivesandskills : Plugin
{

	public override void OnDisable()
	{
		this.Info("Passives and Skills - Desactivado");
	}

	public override void OnEnable()
	{
		this.Info("Passives and Skills - Activado para mas información usa .passivesandskillsinfo.");
	}

	public override void Register()
	{
            
			
            this.AddEventHandlers(new chaos(this));
            this.AddEventHandlers(new classd(this));
            this.AddEventHandlers(new guards(this));
            this.AddEventHandlers(new Ntfteam(this));
            this.AddEventHandlers(new scientist(this));
            this.AddEventHandlers(new scp049and0492(this));
            this.AddEventHandlers(new scp079(this));
            this.AddEventHandlers(new scp096(this));
            this.AddEventHandlers(new scp106(this));
            this.AddEventHandlers(new scp173(this));
            this.AddEventHandlers(new scp939_xx(this));
           
            GamemodeManager.Manager.RegisterMode(this);

	}
	public void RefreshConfig()
	{


	}
}

}

