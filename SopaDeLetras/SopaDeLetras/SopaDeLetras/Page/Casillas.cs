using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SopaDeLetras.Page
{
    public class Casillas: Button
    {
        private int x1, y1;
        private char letra;
        private Color c;

        public char Letra { get{ return letra; } set{ letra = value; Text = letra.ToString(); } }

        public int X1 { get => x1; set => x1 = value; }
        public int Y1 { get => y1; set => y1 = value; }
        public Color C { get => c; set { c = value; BackgroundColor = C; } }

        public Casillas(int x, int y, char letra, Color c, double cR)
        {
            this.X1 = x;
            this.Y1 = y;
            this.Letra = letra;
            Text = letra.ToString();
            MinimumHeightRequest = MinimumWidthRequest = 20;
            FontAttributes = FontAttributes.Bold;
            CornerRadius = (int)cR;
            FontSize = (((int)cR) * 3) / 5;
            BackgroundColor = Color.FromHex("#381515");
            BorderColor = Color.FromHex("#740808");
            TextColor = Color.White;
            BorderWidth = 4;
            FontFamily = "Century Gothic";
            this.c = c;


            //BackgroundColor = C;
        }
        
    }
}
