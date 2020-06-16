using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class TestCardList : MonoBehaviour 
{
	private LoopVerticalScrollRect list;
	
	private List<string> data;

	// Use this for initialization
	void Start ()
	{

		data = new List<string>();
		for (int i = 0; i < 100; i++)
		{
			data.Add(i+"");
		}
		
//		list = GetComponent<LoopVerticalScrollRect>();
//		list.UpdateCallback = ListUpdateCallback;
//		list.totalCount = data.Count;
//		list.prefabName = "Card/Prefabs/CardItem";
//		list.poolSize = 6;
	}

	public void ListUpdateCallback(GameObject go, int index)
	{
//		go.GetComponent<CollectedCardItem>().SetData(data[index]);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
