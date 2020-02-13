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
    public partial class Page1 : ContentPage
    {
        Button bt;
        public Page1()
        {
            
            

            
            InitializeComponent();
            Bt = new Button();
            Bt.Text = "prueba";

            okBtn.Clicked += OkBtn_Clicked;
            volverBtn.Clicked += VolverBtn_Clicked;
            NavigationPage.SetHasNavigationBar(this, false);
            name.Text = "";
            
            //lay.SetBinding()
        }

        public Button Bt { get => bt; set => bt = value; }

        private async void OkBtn_Clicked(object sender, EventArgs e)
        {
            double w = Width;
            double h = Height;
            var n = name;
            if (n.Text != "")
            {
                await DisplayAlert("Mensaje", $"Your name is: {n.Text}.", "Ok");
                ((NavigationPage)this.Parent).Navigation.InsertPageBefore(new Niveles(7, 7, 5, 1, n.Text, "SopaDeLetras.Nivel 1.txt", w, h),
                    ((NavigationPage)this.Parent).RootPage);
                await ((NavigationPage)this.Parent).Navigation.PopToRootAsync();
                //await ((NavigationPage)this.Parent).Navigation.PushAsync(new Niveles(12, 12, 5, 1, n.Text, "SopaDeLetras.Nivel 1.txt"));

            }
        }
        private async void VolverBtn_Clicked(object sender, EventArgs e)
        {       
                ((NavigationPage)this.Parent).Navigation.InsertPageBefore(new MainPage(),
                    ((NavigationPage)this.Parent).RootPage);
                await ((NavigationPage)this.Parent).Navigation.PopToRootAsync();
        }

    }
}