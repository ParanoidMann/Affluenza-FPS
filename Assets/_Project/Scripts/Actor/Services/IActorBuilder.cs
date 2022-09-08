using UnityEngine;

namespace ParanoidMann.Affluenza.Actor
{
	public interface IActorBuilder
	{
		void BuildPlayer(PlayerSubType subType, Vector3 spawnPoint, Transform spawnContainer);
		void BuildEnemy(EnemySubType subType, Vector3 spawnPoint, Transform spawnContainer);
		void BuildNpc(NpcSubType subType, Vector3 spawnPoint, Transform spawnContainer);
	}
}