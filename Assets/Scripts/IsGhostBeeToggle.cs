using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGhostBeeToggle : MonoBehaviour
{
	public static bool CheckGhostBeeToggle;

	private void OnMouseDown()
	{
		CheckGhostBeeToggle = !CheckGhostBeeToggle;
		if (CheckGhostBeeToggle == true)
		{
			GetComponent<Renderer>().material.color = Color.blue;
		}
		else
		{
			GetComponent<Renderer>().material.color = Color.white;
		}
	}
}
