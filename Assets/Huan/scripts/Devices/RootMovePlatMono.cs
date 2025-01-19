using Assets.scripts;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;
using UnityEngine.UIElements;
using System.Collections;

namespace STD
{
    [Serializable]
    public struct RootMovePlatCom
    {
        [SerializeField]
        public Vector2 targetPosition;
        [SerializeField]
        public Vector2 targetRotation;
    }
    public class RootMovePlatMono : MonoBehaviour, IDeviceMono
    {
        internal bool isActive = false;
        public float Speed = 5f;
        public List<RootMovePlatCom> instructions;
        [NonSerialized]
        public Vector3 DefaultPosition;
        [NonSerialized]
        public Quaternion DefaultRotation;
        private void Start()
        {
            DefaultPosition = transform.position;
            DefaultRotation = transform.rotation;
        }
        public IDevice device { get; }
        public RootMovePlatMono()
        {
            device = new RootMovePlat(this);
        }
        public DeviceMono owner { get; set; }
        private int position = 0;
        internal void NextStep()
        {
            if (!isActive)
            {
                return;
            }
            StartCoroutine(e());
        }
        private IEnumerator e()
        {
            yield return null;
            var t = instructions[position++];
            if (position == instructions.Count) position = 0;
            var mm = MovementMono.StartMove(gameObject, (Vector2)DefaultPosition + t.targetPosition,
                Quaternion.LookRotation(new Vector3(0, 0, 1), t.targetRotation), Speed);
            mm.MoveEnd += NextStep;
        }
    }
    public class RootMovePlat : IDevice
    {
        private ContactType ableContactType;
        private RootMovePlatMono source;
        public RootMovePlat(RootMovePlatMono source)
        {
            this.source = source;
        }
        public void Activate(IContact sender, DeviceInteraction t)
        {

        }


        public void Activate(IDevice sender, int instruction)
        {
            if (instruction == 0)
            {
                source.isActive = false;
            }
            else
            {
                source.isActive = true;
                source.NextStep();
            }
        }
    }
}
