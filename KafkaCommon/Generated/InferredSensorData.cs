// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.2
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace KafkaCommon.Generated
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.2")]
	public partial class InferredSensorData : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""InferredSensorData"",""namespace"":""KafkaCommon.Generated"",""fields"":[{""name"":""Id"",""type"":{""type"":""string"",""logicalType"":""uuid""}},{""name"":""TimeStamp"",""type"":{""type"":""long"",""logicalType"":""local-timestamp-micros""}},{""name"":""Temperature"",""type"":{""type"":""bytes"",""logicalType"":""decimal"",""precision"":6,""scale"":2}},{""name"":""Humidity"",""type"":{""type"":""bytes"",""logicalType"":""decimal"",""precision"":6,""scale"":2}},{""name"":""BatteryLevel"",""type"":[""null"",""int""]},{""name"":""ExtraPayload"",""type"":""bytes""}]}");
		private System.Guid _Id;
		private System.DateTime _TimeStamp;
		private Avro.AvroDecimal _Temperature;
		private Avro.AvroDecimal _Humidity;
		private System.Nullable<System.Int32> _BatteryLevel;
		private byte[] _ExtraPayload;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return InferredSensorData._SCHEMA;
			}
		}
		public System.Guid Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				this._Id = value;
			}
		}
		public System.DateTime TimeStamp
		{
			get
			{
				return this._TimeStamp;
			}
			set
			{
				this._TimeStamp = value;
			}
		}
		public Avro.AvroDecimal Temperature
		{
			get
			{
				return this._Temperature;
			}
			set
			{
				this._Temperature = value;
			}
		}
		public Avro.AvroDecimal Humidity
		{
			get
			{
				return this._Humidity;
			}
			set
			{
				this._Humidity = value;
			}
		}
		public System.Nullable<System.Int32> BatteryLevel
		{
			get
			{
				return this._BatteryLevel;
			}
			set
			{
				this._BatteryLevel = value;
			}
		}
		public byte[] ExtraPayload
		{
			get
			{
				return this._ExtraPayload;
			}
			set
			{
				this._ExtraPayload = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.Id;
			case 1: return this.TimeStamp;
			case 2: return this.Temperature;
			case 3: return this.Humidity;
			case 4: return this.BatteryLevel;
			case 5: return this.ExtraPayload;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.Id = (System.Guid)fieldValue; break;
			case 1: this.TimeStamp = (System.DateTime)fieldValue; break;
			case 2: this.Temperature = (Avro.AvroDecimal)fieldValue; break;
			case 3: this.Humidity = (Avro.AvroDecimal)fieldValue; break;
			case 4: this.BatteryLevel = (System.Nullable<System.Int32>)fieldValue; break;
			case 5: this.ExtraPayload = (System.Byte[])fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
