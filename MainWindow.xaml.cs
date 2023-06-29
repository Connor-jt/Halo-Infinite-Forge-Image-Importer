using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using imaginator_halothousand.code_stuff;
using static imaginator_halothousand.code_stuff.ImageArrayifier;

namespace imaginator_halothousand{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window{
        public MainWindow(){
            InitializeComponent();
        }


        mapped_object[]? pixels;

        private void Load_image(object sender, RoutedEventArgs e){

            Microsoft.Win32.OpenFileDialog ofd = new();

            if (ofd.ShowDialog() == true){

                ImageArrayifier new_imagator = new ImageArrayifier(4.0, 0.0, 50.0, 550.0, this);
                pixels = new_imagator.pixel_queue(ofd.FileName, true);




            }
        }
        private void Run_Macro(object sender, RoutedEventArgs e)
        {
            if (pixels == null){

                return;
            }
            Image_macro image_maker = new Image_macro();
            image_maker.begin_macro(pixels);
        }


        private void Print(string text){
            status.Text = "Status: " + text;
        }
    }
}
