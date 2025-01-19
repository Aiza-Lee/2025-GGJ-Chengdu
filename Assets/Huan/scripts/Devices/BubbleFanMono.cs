using Assets.scripts;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

namespace STD
{
    public class BubbleFanMono : MonoBehaviour, IDeviceMono
    {
        
        public IDevice device { get; }
        public BubbleFanMono()
        {
            device = new BubbleFan(this);
        }
        public DeviceMono owner { get; set; }

        public float BubbleSpeed = 2f;
        public float DispersionModif = 2f;
        public int FramesPerBubble = 30;
        private int count = 0;
        public int statu = 0;
        private GameObject bb;
        private System.Random rand;
        private void Update()
        {
            if (statu == 0) return;
            count++;
            if(count >= FramesPerBubble)
            {
                count = 0;
                var b = Instantiate(bb, transform.position+transform.up/4, transform.rotation);
                float rfx = (rand.Next(200) - 100) * DispersionModif / 1000f;
                float rfy = (rand.Next(200) - 100) * DispersionModif / 1000f;
                Vector2 velocity = ((Vector2)transform.up + new Vector2(rfx,rfy))*BubbleSpeed;
                b.GetComponent<Rigidbody2D>().velocity = velocity;
            }
        }
        private Animator thisAnimator;
        private void Start()
        {
            rand = new();
            bb = Resources.Load<GameObject>("BubbleBullet");
            thisAnimator = GetComponent<Animator>();
        }
    }
    public class BubbleFan : IDevice
    {
        private BubbleFanMono source;
        public BubbleFan(BubbleFanMono source)
        {
            this.source = source;
        }
        public void Activate(IContact sender, DeviceInteraction t)
        {
        }
        public void Activate(IDevice sender, int instruction)
        {
            if(instruction == 0)
            {
                source.statu = 0;
            }
            else
            {
                source.statu = 1;
            }
        }
    }
}
