using UnityEngine;
using Leopotam.Ecs;
using ParanoidMann.Affluenza.Input;

namespace ParanoidMann.Affluenza.Actor
{
	internal class PlayerRotationSystem :
			IEcsRunSystem
	{
		private const float MaxAngle = 360.0f;
		private const float PitchAngle = 180.0f;

		private EcsFilter<SettingsComponent> _settingsFilter;
		private EcsFilter<ActorBaseComponent, PlayerComponent> _playerFilter;
		private EcsFilter<InputBaseComponent, InteractionInputComponent> _rotationFilter;

		public void Run()
		{
			foreach (int rotationFilterIdx in _rotationFilter)
			{
				ref EcsEntity inputEntity = ref _rotationFilter.GetEntity(rotationFilterIdx);
				ref var inputBase = ref inputEntity.Get<InputBaseComponent>();

				if (inputBase.IsInputActive)
				{
					foreach (int settingsFilterIdx in _settingsFilter)
					{
						ref EcsEntity settingsEntity = ref _settingsFilter.GetEntity(settingsFilterIdx);
						ref var settingsBase = ref settingsEntity.Get<SettingsComponent>();

						ref var interaction = ref inputEntity.Get<InteractionInputComponent>();

						foreach (int playerFilterIdx in _playerFilter)
						{
							ref EcsEntity playerEntity = ref _playerFilter.GetEntity(playerFilterIdx);
							ref var playerBase = ref playerEntity.Get<ActorBaseComponent>();

							RotatePlayer(interaction, settingsBase, playerBase);
						}
					}
				}
			}
		}

		private void RotatePlayer(
				InteractionInputComponent interaction,
				SettingsComponent settings,
				ActorBaseComponent playerBase)
		{
			PlayerView playerView = playerBase.GameObject as PlayerView;
			Transform cameraTransform = playerView.Camera.transform;

			Vector2 deltaRotation = interaction.RotationDirection * settings.Sensitivity;
			deltaRotation.y *= settings.InvertVertical ? 1.0f : -1.0f;

			float pitchAngle = GetPitchAngle(cameraTransform.localEulerAngles, deltaRotation, settings);

			playerView.transform.Rotate(Vector3.up, deltaRotation.x);
			cameraTransform.localRotation = Quaternion.Euler(pitchAngle, 0.0f, 0.0f);
		}

		private float GetPitchAngle(Vector3 cameraEulerAngles, Vector2 deltaRotation, SettingsComponent settings)
		{
			float pitchAngle = cameraEulerAngles.x;
			if (pitchAngle > PitchAngle)
			{
				pitchAngle -= MaxAngle;
			}

			pitchAngle = Mathf.Clamp(
					pitchAngle + deltaRotation.y,
					settings.MinVerticalCameraAngle,
					settings.MaxVerticalCameraAngle);

			return pitchAngle;
		}
	}
}