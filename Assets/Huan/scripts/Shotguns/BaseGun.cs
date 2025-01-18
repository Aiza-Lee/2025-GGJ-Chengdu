using UnityEngine;

namespace STD
{
    public interface IShotGun
    {
        public void Shot(Transform transform);
        public void Update();
        public string Name { get; }
    }
    public interface IBullet
    {
        public string Name{ get; }
    }
    public abstract class BaseGun : IShotGun
    {
        public abstract string Name { get; }
        protected void baseShot(Transform transform, string bulletName)
        {
            GameObject obj = GameObject.Instantiate((GameObject)Resources.Load(bulletName));
            obj.transform.position = transform.position + new Vector3(0, 1f);
            var targetRigidbody = obj.GetComponent<Rigidbody2D>();
            Vector3 wp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                Camera.main.orthographic ? -Camera.main.orthographicSize : Camera.main.nearClipPlane));
            targetRigidbody.velocity = Vector3.Normalize((Vector2)(-transform.position + wp))
                + Vector3.Normalize(transform.GetComponent<Rigidbody2D>().velocity)/2;
        }
        public abstract void Shot(Transform transform);
        public abstract void Update();
    }
}
