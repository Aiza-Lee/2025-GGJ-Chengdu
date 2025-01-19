using Assets.scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

namespace STD
{
    [Serializable]
    public struct MoveablePlatCom
    {
        [SerializeField]
        public int instruction;
        [SerializeField]
        public Vector2 targetPosition;
        [SerializeField]
        public Vector2 targetRotation;
        [SerializeField]
        public float speed;
    }
    public class MoveablePlatMono : MonoBehaviour, IDeviceMono
    {
        public List<MoveablePlatCom> instructions;
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
        public MoveablePlatMono()
        {
            device = new MoveablePlat(this);
        }
        public DeviceMono owner { get; set; }
    }
    public class MoveablePlat : IDevice
    {
        private ContactType ableContactType;
        private MoveablePlatMono source;
        public MoveablePlat(MoveablePlatMono source)
        {
            this.source = source;
        }
        public void Activate(IContact sender, DeviceInteraction t)
        {

        }
        public void Activate(IDevice sender, int instruction)
        {     
            var t = source.instructions.Find((val) => { return val.instruction == instruction; });
            MovementMono.StartMove(source.owner.gameObject, t.targetPosition + (Vector2)source.DefaultPosition,
                Quaternion.LookRotation(new Vector3(0, 0, 1), t.targetRotation), t.speed);
        }
    }
    public class MovementMono : MonoBehaviour
    {
        public Vector2 targetPosition; // 目标位置
        public Quaternion targetRotation;
        public float smoothTime = 0.3f; // 平滑时间
        public float dampConstant = 2.0f; // 阻尼常数
        private Vector3 velocity = Vector3.zero; // 当前速度
        public float enterTime;
#nullable enable
        public event Action? MoveEnd;
#nullable disable
        public static MovementMono StartMove(GameObject target, Vector2 position,  Quaternion rotation, float speed)
        {
            var mm = target.GetOrAddComponent<MovementMono>();
            mm.targetPosition = position;
            mm.targetRotation = rotation;
            mm.enterTime = Time.time;
            var u = ((Vector3)mm.targetPosition - mm.transform.position).magnitude;
            mm.smoothTime = u
                / speed;
            return mm;
        }
        void Update()
        {
            // 使用 SmoothDamp 实现平滑移动
            Vector3 newPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, dampConstant);
            transform.position = newPosition;
            // 使用Slerp实现平滑旋转
            // t参数是插值因子，范围从0到1。在这里，我们使用Time.deltaTime和rotationSpeed来计算它。
            // 但是，直接使用Time.deltaTime * rotationSpeed作为t可能会导致旋转速度不均匀，
            // 因为随着旋转接近目标，所需的“剩余”旋转量会减少。
            // 为了解决这个问题，我们可以使用一个循环或迭代方法来逐渐减小t，直到达到目标旋转。
            // 然而，对于简单的平滑旋转，下面的方法通常足够好，并且更容易实现。

            // 一个更简单的方法是使用固定的步长或基于剩余角度的步长来调整t，
            // 但这里为了演示目的，我们仍然使用基于时间的步长。

            float t = Time.deltaTime * rotationSpeed;
            // 限制t的值，以防止在非常高的rotationSpeed下跳过目标旋转
            t = Mathf.Clamp01(t);

            // 计算平滑旋转后的四元数
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
            // 应用新的旋转到物体上
            transform.rotation = newRotation;
            if (Time.time - enterTime > smoothTime*6)
            {
                transform.position = targetPosition;
                transform.rotation = targetRotation;
                MoveEnd?.Invoke();
                Destroy(this);
            }
        }
        public float rotationSpeed; // 旋转速度，可以理解为每秒旋转的“比例”或“进度”
        void Start()
        {
            enterTime = Time.time;
            rotationSpeed = 1 / smoothTime;
        }
    }
}
