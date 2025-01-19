using Assets.scripts;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace STD
{
    public class SpikeMono : MonoBehaviour, IDeviceMono
    {
        public ContactType ableContactType;
        public IDevice device { get; }
        public SpikeMono()
        {
            device = new Spike(this);
        }
        public DeviceMono owner { get ; set ; }
    }
    public class Spike : IDevice
    {
        private SpikeMono source;
        public Spike(SpikeMono source)
        {
            this.source = source;
        }
        public void Activate(IContact sender, DeviceInteraction t)
        {
            if (t != DeviceInteraction.Enter) return;
            if((sender.ContactType & ContactType.BubbleBullet) > 0)
            {
                var s = ((MonoBehaviour)sender).gameObject.GetComponent<BubbleBullet>();
                s.DestroySelf();
                return;
            }
            if((sender.ContactType & source.ableContactType) > 0)
            {
                var playerController = ((MonoBehaviour)sender).gameObject.GetComponent<Player>();
				if (playerController != null) {
					playerController.Die();
					return;
				}
				var bubbleController = ((MonoBehaviour)sender).gameObject.transform.parent.GetComponent<BubbleController>();
				if (bubbleController != null) {
					bubbleController.Die();
					return;
				}
            }
        }

        public void Activate(IDevice sender, int instruction)
        {
            throw new NotImplementedException();
        }
    }
}
