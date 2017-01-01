using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighPerformanceTests.Message
{
    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        public void EnclodeTest()
        {
            var message = new Message
            {
                Type = 12,
                Tag = 3,
                ["s1"] = "George"
            };

            var data = message.Encode();
            Assert.AreEqual("SMA\t0\t12\t3\t1\ts1\tGeorge\t", data);
        }

        [TestMethod]
        public void DecodeTest()
        {
            var message = new Message();
            message.Decode("SMA\t0\t12\t3\t1\ts1\tGeorge\t");

            Assert.AreEqual(12, message.Type);
            Assert.AreEqual(3, message.Tag);
            Assert.AreEqual("George", message["s1"]);
        }
    }
}