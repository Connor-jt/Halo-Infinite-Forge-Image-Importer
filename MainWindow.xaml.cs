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
using System.Windows.Threading;
using imaginator_halothousand.code_stuff;
using static imaginator_halothousand.code_stuff.ImageArrayifier;

namespace imaginator_halothousand{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window{
        public MainWindow(){
            InitializeComponent();
            populate_texturesbox();
            populate_observable_texturesbox();
        }


        return_object? image_instructions;

        private void Load_image(object sender, RoutedEventArgs e){

            Microsoft.Win32.OpenFileDialog ofd = new();

            if (ofd.ShowDialog() == true){

                ImageArrayifier new_imagator = new ImageArrayifier();
                bool is_observable_mode = (observable_checkbox.IsChecked == true);
                image_instructions = new_imagator.pixel_queue(ofd.FileName, is_observable_mode, is_observable_mode? ui_observable_textures.SelectedIndex : ui_textures.SelectedIndex);

                Print("color converted with " + Math.Round(image_instructions.image_accuracy * 100.0, 4) + "% accuracy, " + image_instructions.visible_pixel_count + " pixels in that image to be created");

                // display the comparison images
                og_image.Source   = BitmapToImageSource(image_instructions.source_img);
                demo_image.Source = BitmapToImageSource(image_instructions.visualized_img);

                ui_pixel_count.Text = image_instructions.pixel_count.ToString();
                ui_pixels_opaque.Text = image_instructions.visible_pixel_count.ToString();
                ui_accuracy.Text = image_instructions.image_accuracy.ToString();
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
        #region UI STUFF

        private void CheckBox_Click(object sender, RoutedEventArgs e){
            if (observable_checkbox.IsChecked == true){
                ui_textures.Visibility = Visibility.Collapsed;
                ui_observable_textures.Visibility = Visibility.Visible;
            }else{
                ui_textures.Visibility = Visibility.Visible;
                ui_observable_textures.Visibility = Visibility.Collapsed;
        }}
        private void populate_texturesbox(){
            ui_textures.Items.Clear();
            foreach (var item in ImageArrayifier.textures){
                ComboBoxItem cb_i = new ComboBoxItem();
                cb_i.Content = item.name + " (" + item.color + ")";
                ui_textures.Items.Add(cb_i);
            }
            ui_textures.SelectedIndex = 0;
        }
        private void populate_observable_texturesbox(){
            ui_observable_textures.Items.Clear();
            foreach (var item in ImageArrayifier.observed_textures){
                ComboBoxItem cb_i = new ComboBoxItem();
                cb_i.Content = item.name + " (" + item.color + ")";
                ui_observable_textures.Items.Add(cb_i);
            }
            ui_observable_textures.SelectedIndex = 0;
        }


        #endregion

        bool is_running_macro = false;
        private void Run_Macro(object sender, RoutedEventArgs e){
            if (image_instructions == null || image_instructions.pixels == null || is_running_macro) return;


            float? coord_x = try_parse_textbox_text(ui_coord_x);
            float? coord_y = try_parse_textbox_text(ui_coord_y);
            float? coord_z = try_parse_textbox_text(ui_coord_z);
            float? coord_scale = try_parse_textbox_text(ui_coord_scale);
            if (coord_x == null || coord_y == null || coord_z == null || coord_scale == null){
                Print("coords failed to configure, please enter the ingame coords into the left side panel");
                return;
            }
            ui_coord_x.IsEnabled = false;
            ui_coord_y.IsEnabled = false;
            ui_coord_z.IsEnabled = false;
            ui_coord_scale.IsEnabled = false;



            ui_progressbar.Value = 0;
            ui_progressbar.Maximum = image_instructions.visible_pixel_count;
            ui_completion.Text = "0/" + image_instructions.visible_pixel_count;
            is_running_macro = true;
            initiate_macro_task(image_instructions.pixels, (float)coord_scale, (float)coord_x, (float)coord_y, (float)coord_z);

        }
        private void ended_macro(){
            if (is_running_macro == false) return; // this was probably called by the fallback call, where we weren't reported that it ended
            ui_coord_x.IsEnabled = true;
            ui_coord_y.IsEnabled = true;
            ui_coord_z.IsEnabled = true;
            ui_coord_scale.IsEnabled = true;
            is_running_macro = false;
            stop_timer();
        }
        float? try_parse_textbox_text(TextBox box){
            try{float return_ = float.Parse(box.Text);
                return_ = (float)Math.Round(return_, 1);
                box.Text = return_.ToString();
                return return_;
            }catch{return null;}
        }


        public void Print(string text){
            status.Text = "Status: " + text;
        }


        #region MACRO TASK
        public enum macro_state{
            working,
            error,
            aborted,
            completed
        }
        public struct macro_progress{
            public string context;
            public int completed;
            public macro_state state;
        }
        async void initiate_macro_task(List<mapped_object> pixels, float coord_scale, float coord_x, float coord_y, float coord_z){
            IProgress<macro_progress> progress = new Progress<macro_progress>(call_back_progress); // i believe we put the function in here
            Image_macro image_maker = new Image_macro();
            start_timer();
            await Task.Run(() => image_maker.begin_macro(pixels, coord_scale, coord_x, coord_y, coord_z, progress));
            ended_macro();
        }
        #region TIMER
        DispatcherTimer? macro_timer;
        DateTime? macro_start;
        private void start_timer(){
            macro_timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 50), DispatcherPriority.Background, macro_timer_tick, Dispatcher.CurrentDispatcher); 
            macro_timer.Start();
            macro_start = DateTime.Now;
        }
        private void stop_timer() {
            if (macro_timer == null) return;
            macro_timer.Stop();
            macro_timer.Tick -= macro_timer_tick; // unsubscribe from timer's ticks
            macro_timer = null;
            macro_start = null;
        }
        private void macro_timer_tick(object? sender, EventArgs e){
            ui_time.Text = Convert.ToString(DateTime.Now - macro_start);
        }
        #endregion
        public void call_back_progress(macro_progress progress_results){
            if (image_instructions == null) return; // this should never happen
            ui_completion.Text = progress_results.completed + "/" + image_instructions.pixels.Count;
            ui_progressbar.Value = progress_results.completed;
            Print(progress_results.context);

            if (progress_results.state == macro_state.completed){
                // close the operation
                ended_macro();
            } else if (progress_results.state == macro_state.aborted){
                // close the operation
                ended_macro();
            }
        }
        #endregion

    }
}
