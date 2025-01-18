using Assets.scripts;
using System;
using UnityEngine;

namespace STD
{
    public class TestDeviceMono : MonoBehaviour, IDeviceMono
    {
        public IDevice device { get; } = new TestDevice();

        public DeviceMono owner { get; set; }
    }
    public class TestDevice : IDevice
    {
        private ContactType ableContactType = ContactType.Human | ContactType.BubbleMan | ContactType.BubbleBullet;

        public void Activate(IContact sender, DeviceInteraction t)
        {
            if ((sender.ContactType & ableContactType) > 0)
            {
                switch (t)
                {
                    case DeviceInteraction.Enter:
                        Debug.Log("YES");
                        break;
                        case DeviceInteraction.Exit:
                        Debug.Log("NO");
                        break;
                    case DeviceInteraction.Interact:
                        Debug.Log("dss");
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
