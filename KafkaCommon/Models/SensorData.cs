namespace KafkaCommon.Models;

public class SensorData
{
    public Guid Id { get; set; }
    public DateTime TimeStamp { get; set; }
    public decimal Temperature { get; set; }
    public decimal Humidity { get; set; }
    public int BatteryLevel { get; set; }
    public byte[] ExtraPayload { get; set; } = ""u8.ToArray();

}
