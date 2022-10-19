using Zenject;

using ParanoidMann.Core;
using ParanoidMann.Core.PLog;

namespace ParanoidMann.Affluenza.Actor
{
	public class ActorInstaller : Installer<ActorInstaller>
	{
		public override void InstallBindings()
		{
			PLog.Info($"Started binding {GetType()}");

			BindConfigs();
			BindServices();

			Container.BindInterfacesAndSelfTo<ActorSystemsBinder>().AsSingle();
			Container.Resolve<ActorSystemsBinder>();

			PLog.Info($"Completed binding {GetType()}");
		}

		private void BindConfigs()
		{
			Container.Bind<PlayerScriptableObject>().WithId(PlayerSubType.Player)
					.FromAddressable(AddressablePaths.PlayerConfig).AsCached();
			Container.Bind<PlayerScriptableObject>().WithId(PlayerSubType.PlayerWithWeapon)
					.FromAddressable(AddressablePaths.PlayerWithWeaponConfig).AsCached();
		}

		private void BindServices()
		{
			Container.Bind<IActorBuilder>().To<ActorBuilder>().AsSingle();
		}
	}
}