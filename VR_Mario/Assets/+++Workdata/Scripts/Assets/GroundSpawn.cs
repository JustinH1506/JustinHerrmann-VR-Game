using UnityEngine;

public class GroundSpawn : MonoBehaviour
{
	[SerializeField] private float totalLevelLength = 160f;
	[SerializeField] private GameObject groundSlice = null;
	[SerializeField] private Transform groundParent = null;

	private void Awake()
	{
		float groundWide = groundSlice.transform.localScale.x;

		float amountNeeded = totalLevelLength / groundWide;

		Vector3 groundPosition = groundSlice.transform.position;
		
		for (int groundIndex = 0; groundIndex < amountNeeded; groundIndex++)
		{
			Instantiate(groundSlice, groundPosition, Quaternion.identity, groundParent);

			groundPosition.x += groundWide;
		}
	}
}
