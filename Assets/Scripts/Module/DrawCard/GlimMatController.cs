using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class GlimMatController : MonoBehaviour {

	public Material material;

	/// <summary>
	/// 位移
	/// </summary>
	public float Transfrom;
	private float _transfrom;

	/// <summary>
	/// 角度
	/// </summary>
	public float Angle;
	private float _angle;

	/// <summary>
	/// 宽窄
	/// </summary>
	public float Scale;
	private float _scale;

	public void Start()
	{
		if (material != null)
		{
			material.SetFloat("_Offset", Transfrom);
			_transfrom = Transfrom;

			material.SetFloat("Angle", Angle);
			_angle = Angle;
		}
	}

	public void Update()
	{
		if (material == null) return;
		if(Transfrom != _transfrom)
		{
			material.SetFloat("_Offset", Transfrom);
			_transfrom = Transfrom;
		}
		if (Angle != _angle)
		{
			material.SetFloat("Angle", Angle);
			_angle = Angle;
		}
		if (Scale != _scale)
		{
			material.SetFloat("_Scale", Scale);
			_scale = Scale;
		}
	}
}
