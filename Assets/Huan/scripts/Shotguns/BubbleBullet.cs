using System.Collections;
using UnityEngine;

namespace STD
{
    public class BubbleBullet: MonoBehaviour
    {
        [SerializeField]
        private float existTimeS = 4.0f;
        [SerializeField]
        private float speed;
        [SerializeField]
        private Sprite BreakBubble;
        
        private float startTimeS;
        private Rigidbody2D rb;
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = rb.velocity * speed;
            startTimeS = Time.time;
        }
        private void Update()
        {
            if(Time.time - startTimeS > existTimeS)
            {
                DestroySelf();
            }
        }
        private bool isDestoryStart = false;
        public void DestroySelf()
        {
            if (isDestoryStart) return;
            isDestoryStart=true;
            GetComponent<SpriteRenderer>().sprite = BreakBubble;
            StartCoroutine(enumerator());
        }
        IEnumerator enumerator()
        {
            for(int i = 0;i < 20; i++)
            {
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}
