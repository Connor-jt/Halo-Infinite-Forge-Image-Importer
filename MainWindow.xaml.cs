using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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


        return_object? image_instructions;

        private void Load_image(object sender, RoutedEventArgs e){

            Microsoft.Win32.OpenFileDialog ofd = new();

            if (ofd.ShowDialog() == true){

                ImageArrayifier new_imagator = new ImageArrayifier(4.0, 0.0, 50.0, 550.0);
                image_instructions = new_imagator.pixel_queue(ofd.FileName, true);

                Print("color converted with " + Math.Round(image_instructions.image_accuracy * 100.0, 4) + "% accuracy");

                // display the comparison images
                og_image.Source   = BitmapToImageSource(image_instructions.source_img);
                demo_image.Source = BitmapToImageSource(image_instructions.visualized_img);

                // functionality to output image for testing purposes
                //image_instructions.visualized_img.Save("C:\\Users\\Joe bingle\\Downloads\\IFIMT research\\output.png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }
        BitmapImage BitmapToImageSource(Bitmap bitmap){
            using (MemoryStream memory = new MemoryStream()){
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        private void Run_Macro(object sender, RoutedEventArgs e){
            if (image_instructions == null || image_instructions.pixels == null) return;

            Image_macro image_maker = new Image_macro();
            image_maker.begin_macro(image_instructions.pixels);
            Print(image_maker.last_step);
        }


        private void Print(string text){
            status.Text = "Status: " + text;
        }
    }
}
