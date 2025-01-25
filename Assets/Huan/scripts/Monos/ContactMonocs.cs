using Assets.scripts;
using System;
using UnityEngine;

namespace STD
{
    public class ContactMono : MonoBehaviour, IContact
    {
        [SerializeField]
        ContactType _ContactType;
        public ContactType ContactType { get { return _ContactType; } }
#nullable enable
        public event Action<IContact, DeviceInteraction>? OnInteract;
#nullable disable
		public void InteractW(){
			OnInteract?.Invoke(this, DeviceInteraction.Interact);
		}
    }
}
