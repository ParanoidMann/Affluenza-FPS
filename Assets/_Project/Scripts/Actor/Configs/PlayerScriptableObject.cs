using UnityEngine;

namespace ParanoidMann.Affluenza.Actor
{
	[CreateAssetMenu(fileName = "Player", menuName = "Configs/Actor/Player", order = 1)]
	public class PlayerScriptableObject : ActorScriptableObject
	{
		internal override ActorType ActorType => ActorType.Player;
	}
}