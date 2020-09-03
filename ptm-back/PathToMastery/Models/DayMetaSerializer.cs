using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using PathToMastery.Models.State;

namespace PathToMastery.Models
{
    public class DayMetaSerializer : SerializerBase<DayMeta>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DayMeta value)
        {
            if (value == null)
            {
                context.Writer.WriteNull();
            }
            else
            {
                context.Writer.WriteString(value.ToString());
            }           
        }

        public override DayMeta Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonReader = context.Reader;
            var str = bsonReader.ReadString();
            return DayMeta.FromString(str);
        }
    }
}