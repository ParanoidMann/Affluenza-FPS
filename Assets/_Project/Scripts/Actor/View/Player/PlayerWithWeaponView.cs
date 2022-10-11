using UnityEngine;

namespace ParanoidMann.Affluenza.Actor
{
	internal class PlayerWithWeaponView : PlayerView
	{
		[Header("Player With Weapon")]
		[SerializeField]
		private Light _flashlight;

		public Light Flashlight => _flashlight;
	}
}