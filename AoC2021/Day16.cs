using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2021
{
    [TestClass]
    public class Day16 : AoCTestClass
    {
        [TestMethod]
        public void Part1()
        {
            var dayInput = DayInput();
            var stringReader = new StringReader(dayInput);
            string? line;
            while ((line = stringReader.ReadLine()) != null)
            {
                var packet = CreatePacket(string.Concat(line.Select(ToBinary)));
                var versionNumberSum = GetVersionNumberSum(packet);
                TestContext.WriteLine($"Sum: {versionNumberSum}");

            }
        }

        [TestMethod]
        public void Part2()
        {
            var dayInput = DayInput();
            var stringReader = new StringReader(dayInput);
            string? line;
            while ((line = stringReader.ReadLine()) != null)
            {
                var packet = CreatePacket(string.Concat(line.Select(ToBinary)));
                TestContext.WriteLine($"Value: {packet.Value}");
            }
        }

        private long GetVersionNumberSum(Packet packet)
        {
            switch (packet)
            {
                case ValuePacket valuePacket:
                    return valuePacket.Version;
                case OperationPacket operationPacket:
                    return operationPacket.Version + operationPacket.Packets.Sum(GetVersionNumberSum);
            }

            throw new ArgumentException("Unexpected packet type");
        }

        private Packet CreatePacket(string binary)
        {
            var version = Convert.ToInt16(binary[..3], 2);
            var type = Convert.ToInt16(binary[3..6], 2);
            if (type == 4)
            {
                var i = 6;
                var valueString = new StringBuilder();
                while (binary[i] == '1')
                {
                    valueString.Append(binary[(i + 1)..(i + 5)]);
                    i += 5;
                }

                valueString.Append(binary[(i + 1)..(i + 5)]);

                var value = Convert.ToInt64(valueString.ToString(), 2);
                return new ValuePacket(version, type, value, i + 5);
            }

            var packets = new List<Packet>();
            if (binary[6] == '0')
            {
                var lengthInBits = Convert.ToInt32(binary[7..22], 2);
                var i = 22;
                while (i - 22 < lengthInBits)
                {
                    var packet = CreatePacket(binary[i..]);
                    packets.Add(packet);
                    i += packet.Size;
                }

                return new OperationPacket(version, type, packets, i);
            }
            else
            {
                var lengthInPackets = Convert.ToInt32(binary[7..18], 2);
                var i = 18;
                while (packets.Count < lengthInPackets)
                {
                    var packet = CreatePacket(binary[i..]);
                    packets.Add(packet);
                    i += packet.Size;
                }
                return new OperationPacket(version, type, packets, i);
            }
        }

        private string ToBinary(char c)
        {
            var b = Convert.ToByte(c.ToString(), 16);
            return Convert.ToString(b, 2).PadLeft(4, '0');
        }

        private static string TestInput() => @"D2FE28";
        private static string TestInput2() => @"38006F45291200";
        private static string TestInput3() => @"EE00D40C823060";
        private static string TestInput4() => @"8A004A801A8002F478
620080001611562C8802118E34
C0015000016115A2E0802F182340
A0016C880162017C3686B18A3D4780";
        private static string TestInput5() => @"C200B40A82
04005AC33890
880086C3E88112
CE00C43D881120
D8005AC2A8F0
F600BC2D8F
9C005AC2F8F0
9C0141080250320F1802104A08";

    }

    internal abstract class Packet
    {
        protected Packet(short version, short type, int size)
        {
            Version = version;
            Type = type;
            Size = size;
        }

        public short Version { get; }
        public short Type { get; }
        public int Size { get; }
        public abstract long Value { get; }
    }

    class OperationPacket : Packet
    {
        public IList<Packet> Packets { get; }

        public OperationPacket(short version, short type, IList<Packet> packets, int size) : base(version, type, size)
        {
            Packets = packets;
        }

        public override long Value
        {
            get
            {
                switch (Type)
                {
                    case 0:
                        return Packets.Sum(p => p.Value);
                    case 1:
                        return Packets.Aggregate(1L, (p, c) => p * c.Value);
                    case 2:
                        return Packets.Min(p => p.Value);
                    case 3:
                        return Packets.Max(p => p.Value);
                    case 5:
                        return Packets[0].Value > Packets[1].Value ? 1 : 0;
                    case 6:
                        return Packets[0].Value < Packets[1].Value ? 1 : 0;
                    case 7:
                        return Packets[0].Value == Packets[1].Value ? 1 : 0;
                    default:
                        throw new ArgumentException("Unexpected type");
                }
            }
        }
    }

    internal class ValuePacket : Packet
    {
        public override long Value { get; }

        public ValuePacket(short version, short type, long value, int size) : base(version, type, size)
        {
            Value = value;
        }
    }
}
