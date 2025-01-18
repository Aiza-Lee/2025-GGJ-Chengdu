using Assets.scripts;
using System;
using UnityEngine;

namespace STD
{
    public class FansMono : MonoBehaviour, IDeviceMono
    {
        
        public ContactType ableContactType;
        public IDevice device { get; }
        public FansMono()
        {
            device = new Fans(this);
        }
        public DeviceMono owner { get; set; }

        public float WindZoneSize;
        public float WindStrength;
        public float HighWindZoneSize;
        public float HighWindStrength;

        [NonSerialized] 
        public int statu = 0;
        private void Update()
        {
            switch (statu)
            {
                case 0:
                    thisAnimator.SetInteger("statu", 0);
                    break;
                case 1:
                    thisAnimator.SetInteger("statu", 1);
                    var ray = Physics2D.Raycast(transform.position, new Vector2(0,1), WindZoneSize);
                    if(ray.collider == null) break;
                    var target = ray.collider.attachedRigidbody;
                    if (target == null) break;
                    target.AddForce(new Vector2(0,WindStrength/ray.distance));
                    break;
                case 2:
                    thisAnimator.SetInteger("statu", 2);
                    var hray = Physics2D.Raycast(transform.position, new Vector2(0, 1), HighWindZoneSize);
                    if (hray.collider == null) break;
                    var htarget = hray.collider.attachedRigidbody;
                    if (htarget == null) break;
                    htarget.AddForce(new Vector2(0, HighWindStrength / hray.distance));
                    break;
            }
            
        }
        private Animator thisAnimator;
        private void Start()
        {
            thisAnimator = GetComponent<Animator>();
        }
    }
    public class Fans : IDevice
    {
        private FansMono source;
        public Fans(FansMono source)
        {
            this.source = source;
        }
        public void Activate(IContact sender, DeviceInteraction t)
        {
        }
        public void Activate(IDevice sender, int instruction)
        {
            switch (instruction)
            {
                case 0:
                    break;
                case 1:
                    switch (source.statu)
                    {
                        case 0: source.statu = 1; break;
                        case 1: source.statu = 2; break;
                        case 2: source.statu = 0; break;
                    }
                    break;
                case 2:
                    switch (source.statu)
                    {
                        case 0: source.statu = 2; break;
                        case 1: source.statu = 0; break;
                        case 2: source.statu = 1; break;
                    }
                    break;
            }
        }
    }
}
