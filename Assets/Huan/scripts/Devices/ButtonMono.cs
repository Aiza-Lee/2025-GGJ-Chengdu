﻿using Assets.scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace STD
{
    public class ButtonMono : MonoBehaviour, IDeviceMono
    {
        public List<mTuple<GameObject, int>> targets = new List<mTuple<GameObject, int>>(); 
        [NonSerialized]
        public List<mTuple<IDevice, int>> targetDevices = new List<mTuple<IDevice, int>>();
        [NonSerialized]
        public Animator thisAnimator;
        private void Start()
        {
            thisAnimator = GetComponent<Animator>();
            foreach (var target in targets)
            {
                var obj = target.Item1.GetComponent<DeviceMono>();
                if (obj == null) throw new Exception("DeviceMono Start");
                targetDevices.Add(new mTuple<IDevice, int>(obj.device, target.Item2));
            }
        }
        public ContactType ableContactType;

        public ButtonMono()
        {
            device = new Button(this);
        }

        public IDevice device { get; }

        public DeviceMono owner { get ; set ; }
    }
    public class Button : IDevice
    {
        private ButtonMono source;

        public Button(ButtonMono source)
        {
            this.source = source;
        }
        private int count = 0;
        public void Activate(IContact sender, DeviceInteraction t)
        {
            if ((sender.ContactType & source.ableContactType) > 0)
            {
                switch (t)
                {
                    case DeviceInteraction.Enter:
                        count++;
                        if (count == 1)
                        {
                            source.thisAnimator.SetInteger("statu", 1);
                            foreach (var td in source.targetDevices) td.Item1.Activate(this, td.Item2);
                        }
                        break;
                    case DeviceInteraction.Exit:
                        count--;
                        if (count == 0)
                        {
                            if (source.owner.triggerCount != 0) break;
                            source.thisAnimator.SetInteger("statu", 0);
                            foreach (var td in source.targetDevices) td.Item1.Activate(this, 0);
                        }
                        break;
                }
            }
        }
        public void Activate(IDevice sender, int instruction)
        {
            throw new NotImplementedException();
        }
    }
}
