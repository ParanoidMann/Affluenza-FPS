﻿using Zenject;

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
			BindSystems();

			Container.BindInterfacesAndSelfTo<ActorSystemsBinder>().AsSingle();
			Container.Resolve<ActorSystemsBinder>();

			PLog.Info($"Completed binding {GetType()}");
		}

		private void BindConfigs()
		{
			Container.Bind<PlayerScriptableObject>().FromAddressable(AddressablePaths.PlayerConfig);
		}

		private void BindSystems()
		{
			Container.Bind<PlayerBuildSystem>().AsSingle();
		}
	}
}