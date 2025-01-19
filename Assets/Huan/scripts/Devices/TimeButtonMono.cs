using Assets.scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Unity.VisualScripting.Member;

namespace STD
{
    public class TimeButtonMono : MonoBehaviour, IDeviceMono
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
            if (tb.isActive && Time.time - tb.exitTime > CastTimeS)
            {
                tb.isActive = false;
                thisAnimator.SetInteger("statu", 0);
                foreach (var td in targetDevices) td.Item1.Activate(device, 0);
            }
        }
        public ContactType ableContactType;
        public TimeButtonMono()
        {
            device = new TimeButton(this);
        }
        public IDevice device { get; }
        private TimeButton tb { get { return (TimeButton)device;} }
        public DeviceMono owner { get; set; }
    }
    public class TimeButton : IDevice
    {
        private TimeButtonMono source;
        internal float exitTime = 0f;
        internal bool isActive = false;
        private int count = 0;
        public TimeButton(TimeButtonMono source)
        {
            this.source = source;
        }
        public void Activate(IContact sender, DeviceInteraction t)
        {
            if ((sender.ContactType & source.ableContactType) > 0)
            {
                switch (t)
                {
                    case DeviceInteraction.Enter:
                        count++;
                        if(count == 1)
                        {
                            source.thisAnimator.SetInteger("statu", 1);
                            foreach (var td in source.targetDevices) td.Item1.Activate(this, td.Item2);
                        }
                        break;
                    case DeviceInteraction.Exit:
                        count--;
                        
                        if (count == 0)
                        {
                            isActive = true;
                            exitTime = Time.time;
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