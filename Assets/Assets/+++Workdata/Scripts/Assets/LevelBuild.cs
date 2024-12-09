using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelBuild : MonoBehaviour
{
	[SerializeField] private float radius = 0f;
	[SerializeField] private float angle = 0f;

	[SerializeField] private float totalLevelLength = 160f;

	[SerializeField] private List<SpriteRenderer> levelObjects = new List<SpriteRenderer>();

	private void Start()
	{
		levelObjects = FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None).ToList();
		
		for (int spriteIndex = 0, spritemount = levelObjects.Count; spriteIndex < spritemount; spriteIndex++)
		{
			GameObject currentObject = levelObjects[spriteIndex].gameObject;

			if (currentObject.GetComponent<Enemy>())
			{
				continue;
			}

			float currentPosition = currentObject.transform.position.x / totalLevelLength;
			
			angle = currentPosition * 360;
			
			float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
			float z = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
			
			Vector3 position = new Vector3(x, currentObject.transform.position.y, z);

			currentObject.transform.position = position;
			currentObject.transform.rotation = Quaternion.identity;

			currentObject.transform.LookAt(Vector3.zero);
		}
		
		
	}
}
