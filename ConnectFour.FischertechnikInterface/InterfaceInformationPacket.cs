namespace ConnectFour.FischertechnikInterface
{
    public class InterfaceInformationPacket
    {
        public InterfaceInformationPacket(byte[] packet)
        {
            Packet = packet;
        }

        public byte[] Packet { get; private set; }


        public bool I(int i)
        {
            return (Packet[4 + i * 2] == 0x01) || (Packet[5 + i * 2] == 0x01);
        }

        public int C(int i)
        {
            //int value = (int)new System.ComponentModel.Int32Converter().ConvertFromString("0x" + Packet[29 + i*2] + Packet[28 + i*2]);
            int value = Packet[29 + i*2] * 256 + Packet[28 + i*2];
            return value;
        }
    }
}
