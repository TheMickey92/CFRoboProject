using System.Drawing;

namespace ConnectFour.Vision
{
    class FieldMap
    {
        private Point[,] positions;
        public FieldMap()
        {
            initialize();
        }

        private void initialize()
        {
            positions = new Point[7,6];

            positions[0, 0] = new Point(136, 92);
            positions[1, 0] = new Point(294, 88);
            positions[2, 0] = new Point(483, 86);
            positions[3, 0] = new Point(633, 77);
            positions[4, 0] = new Point(813, 66);
            positions[5, 0] = new Point(992, 77);
            positions[6, 0] = new Point(1166, 66);

            positions[0, 1] = new Point(140, 249);
            positions[1, 1] = new Point(307, 248);
            positions[2, 1] = new Point(484, 252);
            positions[3, 1] = new Point(648, 244);
            positions[4, 1] = new Point(825, 231);
            positions[5, 1] = new Point(989, 229);
            positions[6, 1] = new Point(1157, 227);

            positions[0, 2] = new Point(163, 401);
            positions[1, 2] = new Point(345, 399);
            positions[2, 2] = new Point(486, 395);
            positions[3, 2] = new Point(653, 389);
            positions[4, 2] = new Point(821, 395);
            positions[5, 2] = new Point(983, 391);
            positions[6, 2] = new Point(1157, 383);

            positions[0, 3] = new Point(165, 555);
            positions[1, 3] = new Point(331, 545);
            positions[2, 3] = new Point(501, 555);
            positions[3, 3] = new Point(663, 543);
            positions[4, 3] = new Point(817, 539);
            positions[5, 3] = new Point(985, 533);
            positions[6, 3] = new Point(1149, 533);

            positions[0, 4] = new Point(179, 696);
            positions[1, 4] = new Point(337, 701);
            positions[2, 4] = new Point(505, 699);
            positions[3, 4] = new Point(669, 693);
            positions[4, 4] = new Point(823, 689);
            positions[5, 4] = new Point(991, 689);
            positions[6, 4] = new Point(1143, 675);

            positions[0, 5] = new Point(191, 831);
            positions[1, 5] = new Point(351, 837);
            positions[2, 5] = new Point(503, 835);
            positions[3, 5] = new Point(659, 835);
            positions[4, 5] = new Point(819, 825);
            positions[5, 5] = new Point(981, 831);
            positions[6, 5] = new Point(1139, 813);
        }

        public Point GetPosition(int x, int y)
        {
            return positions[x, y];
        }
    }
}
