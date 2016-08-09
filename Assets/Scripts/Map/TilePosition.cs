namespace Rpg.Map
{
    public class TilePosition
    {
        public int x;
        public int y;

        public TilePosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return "x: " + x + " y: " + y;
        }
    }
}