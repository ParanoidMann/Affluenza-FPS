using UnityEngine;

namespace ParanoidMann.Affluenza.Actor
{
	[CreateAssetMenu(fileName = "PlayerWithWeapon", menuName = "Configs/Actor/Player With Weapon", order = 2)]
	internal class PlayerWithWeaponScriptableObject : PlayerScriptableObject
	{
		[Header("Player with Weapon")]
		[SerializeField]
		private float _runSpeedMultiplier = 1.5f;

		public float RunSpeedMultiplier => _runSpeedMultiplier;
	}
}