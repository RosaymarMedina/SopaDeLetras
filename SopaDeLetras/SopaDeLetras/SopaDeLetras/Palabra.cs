using System;
using System.Collections.Generic;
using System.Text;

namespace SopaDeLetras
{
    public class Palabra
    {
        string pal;
        List<int[]> coords;

        public string Pal
        {
            get => pal; set
            {
                pal = value;
                if (pal[pal.Length - 1] < 65 || pal[pal.Length - 1] > 90) pal = pal.Substring(0, pal.Length - 1);
            }
        }

        public Palabra(string pal, int xi, int yi, int xf, int yf)
        {
            this.Pal = pal;
            if (pal[pal.Length - 1] < 65 || pal[pal.Length - 1] > 90) pal = pal.Substring(0, pal.Length - 1);
            coords = new List<int[]>();
            int dirX, dirY;
            try { dirX = (xf - xi) / Math.Abs(xf - xi); }
            catch (System.DivideByZeroException) { dirX = 0; }
            try { dirY = (yf - yi) / Math.Abs(yf - yi); }
            catch (System.DivideByZeroException) { dirY = 0; }

            for (int i = 0; i < pal.Length; i++)
            {
                int[] xy = new int[3];
                xy[0] = xi + (dirX * i);
                xy[1] = yi + (dirY * i);
                xy[2] = pal[i];
                coords.Add(xy);
            }
        }
        public int[] getCoord(int n) { if (n < Pal.Length && n >= 0) return coords[n]; return new int[3]; }
        public override string ToString() { return Pal; }
    }
}
