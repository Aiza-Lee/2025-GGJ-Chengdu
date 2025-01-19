using Assets.scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.AssetImporters;
using UnityEngine;
using static Unity.VisualScripting.Member;

namespace STD
{
    public class LastButtonMono : MonoBehaviour, IDeviceMono
    {
        public float CastTimeS = 1f;
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
        private void Update()
        {
            if (tb.isActive && Time.time - tb.enterTime > CastTimeS)
            {
                tb.isActive = false;
                thisAnimator.SetInteger("statu", 1);
                foreach (var td in targetDevices) td.Item1.Activate(device, td.Item2);
            }
        }
        public ContactType ableContactType;
        public LastButtonMono()
        {
            device = new LastButton(this);
        }
        public IDevice device { get; }
        private LastButton tb { get { return (LastButton)device; } }
        public DeviceMono owner { get; set; }
    }
    public class LastButton : IDevice
    {
        private LastButtonMono source;
        public float enterTime = 0f;
        public bool isActive = false;
        public LastButton(LastButtonMono source)
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
                            isActive = true;
                            enterTime = Time.time; 
                        }
                        break;
                    case DeviceInteraction.Exit:
                        count--;
                        if(count == 0)
                        {
                            isActive = false;
                            foreach (var td in source.targetDevices) td.Item1.Activate(this, 0);
                            source.thisAnimator.SetInteger("statu", 0);
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