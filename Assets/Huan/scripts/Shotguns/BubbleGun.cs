using System.Xml;
using NSFrame;
using UnityEngine;

namespace STD
{

    public class BubbleGun : BaseGun
    {
        [SerializeField]
        private float coolDownTimeS = 0.1f;
        [SerializeField]
        private float maxAmmo = 100f;
        [SerializeField]
        private float ammoGetPerS = 20f;
        [SerializeField]
        private float Ammocost = 6f;

        public float Ammo { get; private set; }
        private float lastAmmoGetTimeS = 0f;
        private float lastShotTimeS = 0f;

        public BubbleGun(float coolDownTimeS, float maxAmmo, float ammoGetPerS, float ammocost)
        {
            this.coolDownTimeS = coolDownTimeS;
            this.maxAmmo = maxAmmo;
            this.ammoGetPerS = ammoGetPerS;
            Ammocost = ammocost;
        }
        public BubbleGun(XmlDocument doc)
        {
            foreach(XmlNode node in doc.LastChild.ChildNodes)
            {
                if (node.Name == "coolDownTimeS") coolDownTimeS = float.Parse(node.InnerText);
                else if(node.Name =="maxAmmo") maxAmmo = float.Parse(node.InnerText);
                else if (node.Name == "ammoGetPerS") ammoGetPerS = float.Parse(node.InnerText);
                else if (node.Name == "Ammocost") Ammocost = float.Parse(node.InnerText);
            }
        }
        public override string Name { get{ return "BubbleGun"; } }
        public override void Shot(Transform transform)
        {
            if(Time.time - lastShotTimeS > coolDownTimeS && Ammo > Ammocost)
            {
				AudioSystem.PlaySFX("bubble");
                Ammo -= Ammocost;
                lastShotTimeS = Time.time;
                baseShot(transform, "BubbleBullet");
            }
        }
        public override void Update()
        {
            var cast = Time.time - lastAmmoGetTimeS;
            lastAmmoGetTimeS = Time.time;
            Ammo += ammoGetPerS * cast;
            if(Ammo >  maxAmmo) Ammo = maxAmmo;
        }
    }
}
