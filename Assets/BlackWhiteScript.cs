﻿using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KMNeedyModule), typeof(KMAudio))]
public abstract class BlackWhiteScript : MonoBehaviour
{
	protected virtual string Name
	{
		get
		{
			return "Default";
		}
	}

	public object NeedyComponent { get; private set; }

	private void Awake()
	{
		if (!_idc.ContainsKey(Name))
		{
			_idc[Name] = 0;
		}
		Dictionary<string, int> idc = _idc;
		string name = Name;
		_id = idc[name] + 1;
	}

	protected void Start()
	{
		Audio = GetComponent<KMAudio>();
		Module = GetComponent<KMNeedyModule>();
		KMNeedyModule module = Module;
		module.OnActivate = (KMNeedyModule.KMModuleActivateEvent)Delegate.Combine(module.OnActivate, new KMNeedyModule.KMModuleActivateEvent(Activate));
		KMNeedyModule module2 = Module;
		module2.OnNeedyActivation = (KMNeedyModule.KMNeedyActivationEvent)Delegate.Combine(module2.OnNeedyActivation, new KMNeedyModule.KMNeedyActivationEvent(NeedyStart));
		KMNeedyModule module3 = Module;
		module3.OnTimerExpired = (KMNeedyModule.KMTimerExpiredEvent)Delegate.Combine(module3.OnTimerExpired, new KMNeedyModule.KMTimerExpiredEvent(NeedyEnd));
		KMNeedyModule module4 = Module;
		module4.OnNeedyDeactivation = (KMNeedyModule.KMNeedyDeactivationEvent)Delegate.Combine(module4.OnNeedyDeactivation, new KMNeedyModule.KMNeedyDeactivationEvent(BombOver));
		NeedyComponent = gameObject.GetComponent(ReflectionHelper.FindTypeInGame("ModNeedyComponent"));
	}

	protected abstract void NeedyStart();

	protected abstract void NeedyEnd();

	protected abstract void BombOver();

	protected abstract void Activate();

	protected void Log(string message, params object[] args)
	{
		Debug.LogFormat(string.Concat(new object[]
		{
			"[",
			Name,
			" #",
			_id,
			"] ",
			message
		}), args);
	}

	public void SetIdentifier(int id)
	{
		_identifier.text = id.ToString();
	}

	protected void Error()
	{
		_identifier.text = "!!!";
		_identifier.color = Color.red;
		Errored = true;
	}

	[SerializeField]
	private TextMesh _identifier;

	private int _id;

	private static readonly Dictionary<string, int> _idc = new Dictionary<string, int>();

	public BlackWhiteScript Partner;

	public KMNeedyModule Module;

	protected KMAudio Audio;

	protected bool Errored;

	protected static int Identifier;
}