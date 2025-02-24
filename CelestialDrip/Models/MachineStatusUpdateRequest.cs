namespace CelestialDrip.Models
{
    public class MachineStatusUpdateRequest
    {
        public string Status { get; set; }
        public byte[] RowVersion { get; set; }
    }
}