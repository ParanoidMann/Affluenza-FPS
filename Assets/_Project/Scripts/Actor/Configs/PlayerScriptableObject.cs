using UnityEngine;

namespace ParanoidMann.Affluenza.Actor
{
	[CreateAssetMenu(fileName = "Player", menuName = "Configs/Actor/Player", order = 1)]
	internal class PlayerScriptableObject : ActorScriptableObject
	{
		[Header("Player")]
		[SerializeField]
		private PlayerSubType _subType;

		public override ActorType ActorType => ActorType.Player;
		public override int ActorSubType => (int)_subType;
	}
}