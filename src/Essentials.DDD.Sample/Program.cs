using test_project;
using System.Text;
using Newtonsoft.Json;
using Essentials.DDD.Sample;
using Essentials.Serialization.Deserializers;
using Essentials.Serialization.Serializers;

var settings = new JsonSerializerSettings
{
    Converters = new List<JsonConverter>
    {
        new MoneyNewtonsoftJsonConverter()
    }
};

var serializer = new NewtonsoftJsonSerializer(settings);
var deserializer = new NewtonsoftJsonDeserializer(settings);

var money = new Money(210.146M);
var serialized = serializer.Serialize(money);
Console.WriteLine($"Serialized: {Encoding.UTF8.GetString(serialized)}");

var deserialized = deserializer.Deserialize<Money>(serialized.AsSpan());
Console.WriteLine($"Deserialized: {deserialized.Value}");