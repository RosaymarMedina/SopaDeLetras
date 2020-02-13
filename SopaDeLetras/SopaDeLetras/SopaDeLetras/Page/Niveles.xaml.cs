using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SopaDeLetras.Page
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Niveles : ContentPage
    {
        Casillas[,] Tablero;
        int nivel;
        string user;
        int ancho, largo, n;
        List<Palabra> lisPal, pisLisPal;
        int xact, yact, xant, yant, dirX, dirY, pos;
        string pal;
        Label[] labelPal;
        bool pist2;
        double tamY;
        double tamX;

        public Niveles(int ancho, int largo, int N, int nivel, string user, string fileName, double w, double h)
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            Title = $"Nivel {nivel}";
            this.xact = this.yact = this.xant = this.yant = -1;
            this.dirX = this.dirY = 0;
            this.pal = "";
            StackLayout[] largoTablero = new StackLayout[largo];
            var rand = new Random();
            this.ancho = ancho;
            this.largo = largo;
            pist2 = false;
            this.n = N;
            this.nivel = nivel;
            this.user = user;
            tamX = w - ((8*w)/100);
            tamY = tamX;

            string[] usuaText = new string[5];
            usuaText[0] = ancho.ToString();
            usuaText[1] = largo.ToString();
            usuaText[2] = n.ToString();
            usuaText[3] = nivel.ToString();
            usuaText[4] = user;


            EscribirArchivo("Usuarios.txt", usuaText);

            lisPal = new List<Palabra>();
            pisLisPal = new List<Palabra>();
            int[,] auxDir = { { 1, 1 }, { 1, -1 }, { -1, -1 }, { -1, 1 }, { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
            usuarioL.Text = user;
            NivelL.Text = $"Nivel {nivel}";
            try
            {
                string[] s = LeerArchivo(fileName);
                Tablero = new Casillas[ancho, largo];
                for (int i = 0; i < ancho; i++)
                {
                    for (int j = 0; j < largo; j++)
                    {
                        Tablero[i, j] = new Casillas(i, j, ' ', Color.FromHex("#381515"),tamX/ ancho);
                        Tablero[i, j].Clicked += Niveles_Clicked;


                    }
                }
                for (int i = 0; i < n; i++)
                {
                    bool ban = true;
                    do
                    {
                        int x, y;
                        x = rand.Next(0, ancho);
                        y = rand.Next(0, largo);

                        for (int j = 0; j < s.Length - i; j++)
                        {
                            int auxn;
                            int m = rand.Next(0, s.Length - i);
                            auxn = m;
                            string pal = s[auxn];

                            for (int k = 0; k < 8; k++)
                            {
                                try
                                {
                                    m = rand.Next(0, 8 - k);
                                   if (Comprobar(pal, x, y, auxDir[m, 0], auxDir[m, 1], Tablero))
                                    {
                                        if (pal[pal.Length - 1] < 65 || pal[pal.Length - 1] > 90) pal = pal.Substring(0, pal.Length - 1);
                                        Llenar(pal, x, y, auxDir[m, 0], auxDir[m, 1], Tablero);
                                        lisPal.Add(new Palabra(pal, x, y, x + (auxDir[m, 0] * pal.Length), y + (auxDir[m, 1] * pal.Length)));
                                        pisLisPal.Add(new Palabra(pal, x, y, x + (auxDir[m, 0] * pal.Length), y + (auxDir[m, 1] * pal.Length)));
                                        ban = false;
                                    }
                                    for (int l = 0; l < 2; l++)
                                    {
                                        int tempo = auxDir[m, l];
                                        auxDir[m, l] = auxDir[8 - k - 1, l];
                                        auxDir[8 - k - 1, l] = tempo;
                                    }
                                    if (!ban)
                                    {
                                        String temp = s[auxn];
                                        s[auxn] = s[s.Length - i - 1];
                                        s[s.Length - i - 1] = temp;

                                        j = s.Length;
                                        k = 8;
                                    }

                                
                                }
                                catch (System.IndexOutOfRangeException) { }
                            }
                        }
                    } while (ban);
                }
                
                for (int j = 0; j < largo; j++)
                {
                    largoTablero[j] = new StackLayout() {Spacing = 2 };
                    flay.Children.Add(largoTablero[j]);
                    for (int k = 0; k < ancho; k++)
                    {
                        if (Tablero[j, k].Letra < 65 || Tablero[j, k].Letra > 90) Tablero[j, k].Letra = ((char)(rand.Next(65, 91)));
                        string te = Tablero[j, k].Letra.ToString();
                        Tablero[j, k].HeightRequest = tamY / largo;
                        Tablero[j, k].WidthRequest = tamX / ancho;
                        Tablero[j, k].Padding = -2;

                        largoTablero[j].Children.Add(Tablero[j, k]);

                        Console.Write(Tablero[k, j].Letra + " ");
                    }
                    Console.WriteLine();
                }
                labelPal = new Label[lisPal.Count];
                for (int i = 0; i < lisPal.Count; i++)
                {
                    if (lisPal[i].Pal[lisPal[i].Pal.Length - 1] < 65 || lisPal[i].Pal[lisPal[i].Pal.Length - 1] > 90) lisPal[i].Pal.Substring(0, lisPal[i].Pal.Length - 1);
                    labelPal[i] = new Label{Text = lisPal[i].ToString(),
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.White, WidthRequest = 350/5};
                    ListaPalabrasLayout.Children.Add(labelPal[i]);
                    Console.WriteLine(lisPal[i].ToString());
                }

                volBtn.Clicked += VolBtn_ClickedAsync;
                Pista1.Clicked += Pista1_Clicked;
                Pista2.Clicked += Pista2_Clicked;

            }
            catch (System.IO.FileNotFoundException) { Console.WriteLine("Archivo no encontrado."); }
            //GridItemsLayout

            
        }

        private void Pista2_Clicked(object sender, EventArgs e)
        {
            if (!pist2 && pisLisPal.Count > 0)
            {
                var r = new Random();
                int posi = r.Next(pisLisPal.Count);
                CambioColor(pisLisPal, posi, Color.FromHex("#D9D0C618"));
                pisLisPal.Remove(pisLisPal[posi]);
                pist2 = true;
            }
        }

        private void Pista1_Clicked(object sender, EventArgs e)
        {
            
            if (pisLisPal.Count > 0)
            {
                var r = new Random();
                
                int posi = r.Next(pisLisPal.Count);
                int[] xY = pisLisPal[posi].getCoord(0);
                Tablero[xY[0], xY[1]].BackgroundColor = Color.FromHex("#D9D0C618");
                pisLisPal.Remove(pisLisPal[posi]);
            }
        }

        private void Niveles_Clicked(object sender, EventArgs e)
        {
            int x = ((Casillas)sender).X1;
            int y = ((Casillas)sender).Y1;
            Desplazamiento(Tablero, largo, ancho, lisPal, x, y);
        }

        private async void VolBtn_ClickedAsync(object sender, EventArgs e)
        {
            string r = await DisplayActionSheet("Salir", "No", "Si");
            if (r == "Si")
            {
                ((NavigationPage)this.Parent).Navigation.InsertPageBefore(new MainPage(),
                    ((NavigationPage)this.Parent).RootPage);
                await ((NavigationPage)this.Parent).Navigation.PopToRootAsync();
            }

        }

        public static bool Comprobar(string palabra, int x, int y, int xve, int yve, Casillas[,] sopa)
        {
            int[] coord = new int[2];
            try
            {
                for (int i = 0; i < palabra.Length; i++)
                {
                    coord[0] = x + (xve * i);
                    coord[1] = y + (yve * i);
                    if (sopa[coord[0], coord[1]].Letra != ' ' && sopa[coord[0], coord[1]].Letra != palabra[i]) return false;
                }
            }
            catch (System.IndexOutOfRangeException) { return false; }
            return true;
        }
        public static void Llenar(string palabra, int x, int y, int xve, int yve, Casillas[,] sopa)
        {
            int[] coord = new int[2];
            try
            {
                for (int i = 0; i < palabra.Length; i++)
                {
                    coord[0] = x + (xve * i);
                    coord[1] = y + (yve * i);
                    sopa[coord[0], coord[1]].Letra = palabra[i];
                }
            }
            catch (System.IndexOutOfRangeException) { }
        }

        string[] LeerArchivo(string nameFile)
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Casillas)).Assembly;
            Stream stream = assembly.GetManifestResourceStream(nameFile);
            string text = "";


            using (var reader = new System.IO.StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            return text.Split('\n');
        }
        static string NameFile = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public void EscribirArchivo(string fileName, string[] text)
        {
            var filePath = Path.Combine(NameFile, fileName);
            if (!File.Exists(filePath))
                File.Delete(filePath);
            File.WriteAllLines(filePath, text);
            
        }

        public static bool Distancia(int xant, int yant, int xact, int yact)
        {
            int disX = (xact - xant); int disY = (yact - yant);
            if ((disX == -1 || disX == 1) && (disY == -1 || disY == 1)) return true;
            if (disX == 0 && (disY == -1 || disY == 1)) return true;
            if (disY == 0 && (disX == -1 || disX == 1)) return true;
            return false;
        }
        public static bool Direccion(int xant, int yant, int xact, int yact, int dx, int dy)
        {
            int dirX, dirY;
            try { dirX = (xact - xant) / Math.Abs(xact - xant); }
            catch (System.DivideByZeroException) { dirX = 0; }
            try { dirY = (yact - yant) / Math.Abs(yact - yant); }
            catch (System.DivideByZeroException) { dirY = 0; }
            if (dirX == dx && dirY == dy) return true;
            return false;
        }
        public static int PalabraCompleta(List<Palabra> lis, string pal)
        {
            if (lis.Count == 0) return -1;
            for (int i = 0; i < lis.Count; i++)
                if (lis[i].Pal == pal) return i;
            return -1;
        }
        public static bool Comparar(int ran, List<int> lis, int tam)
        {
            for (int i = 0; i < tam; i++)
            {
                if (ran == lis[i]) return true;
            }
            return false;
        }
        public void CambioColor(List<Palabra> palis, int n, Color c)
        {
            int x, y;
            for (int i = 0; i < palis[n].Pal.Length; i++)
            {
                x = palis[n].getCoord(i)[0];
                y = palis[n].getCoord(i)[1];
                Tablero[x, y].C = c;
            }
        }
        public void Desplazamiento(Casillas[,] sopa, int largo, int ancho, List<Palabra> lis, int x, int y)
        {

            var rand = new Random();

            if (pal.Length == 0)
            {
                xact = xant = x; yact = yant = y;
                pal = sopa[x, y].Letra.ToString();
                sopa[x, y].BackgroundColor = Color.FromRgb(109, 7, 7);
            }
            else
            {
                if (pal.Length == 1)
                {

                    if (Distancia(xant, yant, x, y))
                    {
                        xact = x; yact = y;
                        try { dirX = (xact - xant) / Math.Abs(xact - xant); }
                        catch (System.DivideByZeroException) { dirX = 0; }
                        try { dirY = (yact - yant) / Math.Abs(yact - yant); }
                        catch (System.DivideByZeroException) { dirY = 0; }
                        pal += sopa[x, y].Letra.ToString();
                        sopa[x, y].BackgroundColor = Color.FromRgb(109, 7, 7);


                    }
                    else if (x == xact && y == yact)
                    {
                        pal = pal.Substring(0, pal.Length - 1);
                        sopa[x, y].BackgroundColor = sopa[x, y].C;
                        xact = xant = -1; yact = yant = -1;
                    }
                }
                else if (pal.Length >= 2)
                {
                    if (Distancia(xact, yact, x, y) && Direccion(xact, yact, x, y, dirX, dirY))
                    {
                        xant = xact; yant = yact;
                        xact = x; yact = y;
                        pal += sopa[x, y].Letra.ToString();
                        sopa[x, y].BackgroundColor = Color.FromRgb(109, 7, 7);
                    }
                    else if (x == xact && y == yact)
                    {
                        sopa[x, y].BackgroundColor = sopa[x, y].C;
                        xact = xant; yact = yant;
                        if (pal.Length > 2)
                        {
                            xant += (dirX * (-1));
                            yant += (dirY * (-1));
                        }
                        pal = pal.Substring(0, pal.Length - 1);
                    }
                }
            }
            pos = PalabraCompleta(lis, pal);
            if (pos != -1)
            {
                CambioColor(lisPal, pos, Color.FromHex("D99F3F13"));
                lis.Remove(lis[pos]);
                pos = PalabraCompleta(pisLisPal, pal);
                if (pos != -1)
                {
                    pisLisPal.Remove(pisLisPal[pos]);
                }
                

                for (int i = 0; i < labelPal.Length; i++)
                {
                    if (labelPal[i].Text == pal) { pos = i; break; }
                }

                labelPal[pos].TextDecorations = TextDecorations.Strikethrough;
                xact = yact = xant = yant = -1;
                dirX = dirY = 0;
                pal = "";
                if (lisPal.Count == 0) NextLevel();
            }
        }
        private async void NextLevel()
        {

            if (nivel < 6)
            {
                await DisplayAlert("", "¡Ganaste!", "Siguiente nivel.");
                ((NavigationPage)this.Parent).Navigation.InsertPageBefore(new Niveles(ancho + 1, largo + 1, n + 1, nivel + 1, user, $"SopaDeLetras.Nivel {nivel + 1}.txt",tamX, tamY),
                    ((NavigationPage)this.Parent).RootPage);
                await ((NavigationPage)this.Parent).Navigation.PopToRootAsync();
            }
            else if (nivel == 6)
            {
                await DisplayAlert("¡Ganaste!", "¡Has completado el juego!", "Menú");
                ((NavigationPage)this.Parent).Navigation.InsertPageBefore(new MainPage(),
                    ((NavigationPage)this.Parent).RootPage);
                await ((NavigationPage)this.Parent).Navigation.PopToRootAsync();
            }
        }
    }






}