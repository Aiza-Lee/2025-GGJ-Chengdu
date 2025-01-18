using STD;

namespace Assets.scripts
{
    public enum DeviceInteraction
    {
        Enter, Exit, Interact, Stay
    }
    public interface IDeviceMono
    {
        public IDevice device { get; }
        public DeviceMono owner { get; set; }
    }
    public interface IDevice
    {
        public void Activate(IContact sender, DeviceInteraction t);
        public void Activate(IDevice sender, int instruction);
    }
    
}
