using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
	[SerializeField]
	List<GameObject> toActivate;

	private void Start()
	{
		SetAllChildrenActive(transform);
	}

	private void SetAllChildrenActive(Transform pTransform)
	{
		if(pTransform.childCount == 0)
		{
			pTransform.gameObject.SetActive(true);
			return;
		}

		foreach(Transform child in pTransform)
		{
			SetAllChildrenActive(child);
		}
	}
}
