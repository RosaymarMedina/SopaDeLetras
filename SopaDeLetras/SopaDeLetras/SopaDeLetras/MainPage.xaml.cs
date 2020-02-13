using SopaDeLetras.Page;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SopaDeLetras
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]

    public partial class MainPage : ContentPage
    {
        static string NameFile = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        bool isUsuario;
        string[] arc;
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            var filePath = Path.Combine(NameFile, "Usuarios.txt");
            try
            {
                arc = System.IO.File.ReadAllLines(filePath);
                isUsuario = true;
            }
            catch (System.IO.FileNotFoundException){ isUsuario = false; }
            if (!isUsuario)
            {
                stackB.Children.Remove(Btn2);
                stackB.Children.Remove(Btn4);
            }


        }

        private void Btn3_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<ICloseApplication>().closeApplication();
        }
        private void Btn4_Clicked(object sender, EventArgs e)
        {
                File.Delete(Path.Combine(NameFile, "Usuarios.txt"));
                stackB.Children.Remove(Btn2);
                stackB.Children.Remove(Btn4);
        }

        private void Btn1_Clicked(object sender, EventArgs e)
        {
            
            ((NavigationPage)this.Parent).PushAsync(new Page1());

        }

        private async void Btn2_Clicked(object sender, EventArgs e)
        {
            if (isUsuario)
            {
                if (arc.Length == 5) {
                    double w = Width;
                    double h = Height;
                    ((NavigationPage)this.Parent).Navigation.InsertPageBefore(new Niveles(int.Parse(arc[0]),
                        int.Parse(arc[1]), int.Parse(arc[2]), int.Parse(arc[3]), arc[4], $"SopaDeLetras.Nivel {int.Parse(arc[3])}.txt",w,h),
                        ((NavigationPage)this.Parent).RootPage);
                    await ((NavigationPage)this.Parent).Navigation.PopToRootAsync();
                } else { await DisplayAlert("Error", "No se ha encontrado los datos de partida", "OK"); } }
        }
    }

    public interface ICloseApplication
    {

        void closeApplication();
    }


}
