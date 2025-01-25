using STD;
using System;
using UnityEngine;

namespace Assets.scripts
{
    [Serializable]
    public struct mTuple<T1, T2>
    {
        [SerializeField]
        private T1 Device;
        public T1 Item1 { get { return Device; } set { Device = value; } }
        [SerializeField]
        private T2 Instruction;
        public T2 Item2 { get { return Instruction; } set { Instruction = value; } }
        public mTuple(T1 s1, T2 s2)
        {
            Device = s1;
            Instruction = s2;
        }
    }
    public class DeviceMono : MonoBehaviour
    {
        public IDevice device { get; private set; }
        public int triggerCount { get; private set; }
        public string Device;
        public void Awake() 
        { 
            var devicemono = (IDeviceMono)GetComponent(DIService.GoFind(Device));
            device = devicemono.device;
            devicemono.owner = this;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            triggerCount++;
            if (!collision.gameObject.TryGetComponent<ContactMono>(out var obj)) return;
            device.Activate(obj, DeviceInteraction.Enter);
            obj.OnInteract += device.Activate;
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            //var obj = collision.gameObject.GetComponent<ContactMono>();
            //if (obj == null) return;
            //device.Activate(obj, DeviceInteraction.Stay);
        }
    private void OnTriggerExit2D(Collider2D collision)
        {
            triggerCount--;
            var obj = collision.gameObject.GetComponent<ContactMono>();
            if (obj == null) return;
            device.Activate(obj, DeviceInteraction.Exit);
            obj.OnInteract -= device.Activate;
        }
    }
}
