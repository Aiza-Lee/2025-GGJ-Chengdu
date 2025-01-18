using Assets.scripts;
using System;

namespace STD
{
    [Flags]
    public enum ContactType
    {
        Plat = 1,
        Human = 2,
        BubbleMan = 4,
        BubbleBullet = 8,
        Stone = 16,
        All = 1|2|4|8|16
    }
    public interface IContact
    {
        public ContactType ContactType { get; }
#nullable enable
        public event Action<IContact, DeviceInteraction>? OnInteract;
#nullable disable
    }
    public class FixContact : IContact
    {
        public ContactType ContactType { get { return ContactType.All; } }

#nullable enable
        public event Action<IContact, DeviceInteraction>? OnInteract;
#nullable disable
    }
}
