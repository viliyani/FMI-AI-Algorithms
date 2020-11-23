using System;

namespace FirstSteps
{
    public class City
    {

        public City(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }

        public int Id { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        // Calculate the distance to given city
        public double distanceTo(City city)
        {
            int xDistance = Math.Abs(this.X - city.X);
            int yDistance = Math.Abs(this.Y - city.Y);
            double distance = Math.Sqrt((xDistance * xDistance) + (yDistance * yDistance));

            return distance;
        }
    }
}
