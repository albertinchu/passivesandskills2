using Smod2;
using Smod2.Attributes;
using scp4aiur;
namespace Passivesandskills2
{
	[PluginDetails(
		author = "Albertinchu con la ayuda de InsetJesux y Roger",
		name = "Passivesandskills2",
		description = "Que pasará si dejamos que los jugadores tengan poderes...",
		id = "albertinchu.gamemode.Passivesandskills2",
		version = "3.5.0",
		SmodMajor = 3,
		SmodMinor = 4,
		SmodRevision = 0
		)]
public class Passivesandskills2 : Plugin
{

	public override void OnDisable()
	{
		this.Info("PassivesAndSkills - Desactivado");
	}

	public override void OnEnable()
	{
		this.Info("PassivesAndSkills - Activado para mas información usa .passivesandskills.");
	}

	public override void Register()
	{
			Timing.Init(this);
			this.AddEventHandlers(new Events(this));
			GamemodeManager.Manager.RegisterMode(this);

		}
	public void RefreshConfig()
	{


	}
}

}

