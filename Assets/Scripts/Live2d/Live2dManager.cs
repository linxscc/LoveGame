using System.Collections;
using System.Collections.Generic;
using game.main.Live2d;
using live2d;
using live2d.framework;
using UnityEngine;

public class Live2dManager : MonoBehaviour
{
	private void Awake()
	{
		Live2DFramework.SetPlatformManager(new Live2dPlatformManager());
	}

	public static L2DView CreateL2DView(string modelId, Live2dCanvas live2DCanvas, List<string> _donotUnloadIds)
	{
		L2DView l2dView = live2DCanvas.gameObject.AddComponent<L2DView>();
		l2dView.LoadModel(modelId, _donotUnloadIds);
		
		return l2dView;
	}
}
