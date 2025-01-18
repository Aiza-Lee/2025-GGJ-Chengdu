using Assets.scripts;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace STD
{
    public class LeverMono:MonoBehaviour,IDeviceMono
    {
        public List<mTuple<GameObject, int>> targets = new List<mTuple<GameObject, int>>(); 
        [NonSerialized]
        public List<mTuple<IDevice, int>> targetDevices = new List<mTuple<IDevice, int>>();
        public SpriteRenderer thisSpriteRenderer;
        private void Start()
        {
            S1 = Resources.Load<Sprite>("sourceImages/LeverS1");
            S2 = Resources.Load<Sprite>("sourceImages/LeverS2");
            thisSpriteRenderer = GetComponent<SpriteRenderer>();
            foreach (var target in targets)
            {
                var obj = target.Item1.GetComponent<DeviceMono>();
                if (obj == null) throw new Exception("DeviceMono Start");
                targetDevices.Add(new mTuple<IDevice, int>(obj.device, target.Item2));
            }
        }
        public ContactType ableContactType;
        public LeverMono()
        {
            device = new Lever(this);
        }
        public IDevice device { get; }
        internal Sprite S1;
        internal Sprite S2;
        public DeviceMono owner { get; set; }
    }
    public class Lever : IDevice
    {
        private LeverMono source;

        public Lever(LeverMono source)
        {
            this.source = source;
        }
        private int statu = 0;
#nullable enable

        public void Activate(IContact sender, DeviceInteraction t)
        {
            if ((sender.ContactType & source.ableContactType) > 0)
            {
                switch (t)
                {
                    case DeviceInteraction.Interact:
                        if (statu == 0)
                        {
                            foreach (var td in source.targetDevices) td.Item1.Activate(this, td.Item2);
                            statu = 1;
                            source.thisSpriteRenderer.sprite = source.S2;
                        }
                        else
                        {
                            foreach (var td in source.targetDevices) td.Item1.Activate(this, 0);
                            statu = 0;
                            source.thisSpriteRenderer.sprite = source.S1;
                        }
                        break;
                }
            }
        }
#nullable disable
        public void Activate(IDevice sender, int instruction)
        {
            throw new NotImplementedException();
        }
    }
}
