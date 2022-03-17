using NUnit.Framework;
using RIAT.serializer;
using RIAT.unit;

namespace TestProject1
{
    public class Tests
    {
        Serializer serializer = new Serializer();


        [Test]
        public void SeralizerJsonInputTest()
        {
            Input input = Input.CreteRandom();
            Input newInput = serializer.deserializeJson<Input>(serializer.serializationJosn(input));

            Assert.IsTrue(input.Equals(newInput));
        }

        [Test]
        public void SeralizerXmlInputTest()
        {
            Input input = Input.CreteRandom();
            Input newInput = serializer.deserializeXml<Input>(serializer.serializationXml(input));

            Assert.IsTrue(input.Equals(newInput));
        }

        [Test]
        public void SeralizerJsonOutputTest()
        {
            Output output = Output.createFrom(Input.CreteRandom());

            Output newInput = serializer.deserializeJson<Output>(serializer.serializationJosn(output));

            Assert.IsTrue(output.Equals(newInput));
        }

        [Test]
        public void SeralizerXmlOutputTest()
        {
            Output output = Output.createFrom(Input.CreteRandom());

            Output newInput = serializer.deserializeXml<Output>(serializer.serializationXml(output));

            Assert.IsTrue(output.Equals(newInput));
        }
    }
}