using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
            
            float? image_intesity = try_parse_textbox_text(ui_lightness, 3);
            if (image_intesity == null || image_intesity < 0.0 || image_intesity > 1.0){
                Print("bad image intensity value, needs to be a decimal between 0.0 - 1.0");
                return;
            }
            float? intesity_penalty = try_parse_textbox_text(ui_penalty, 3);
            if (intesity_penalty == null || image_intesity < 0.0 || image_intesity > 1.0){
                Print("bad image penalty value, needs to be a decimal between 0.0 - 1.0");
                return;
            }

            try{Microsoft.Win32.OpenFileDialog ofd = new();
                if (ofd.ShowDialog() == true){

                    ImageArrayifier new_imagator = new ImageArrayifier();
                    bool is_observable_mode = (observable_checkbox.IsChecked == true);
                    return_object? new_image_instructions = new_imagator.pixel_queue(ofd.FileName, (float)image_intesity, is_observable_mode, is_observable_mode? ui_observable_textures.SelectedIndex : ui_textures.SelectedIndex, (float)intesity_penalty);
                    if (new_image_instructions.pixels == null){ // test to make sure we did not attempt to load an overly large image
                        Print(new_image_instructions.output_message);
                        return;
                    }
                    exit_selected_pixel_mode();
                    image_instructions = new_image_instructions;

                    Print("color converted with " + Math.Round(image_instructions.image_accuracy * 100.0, 4) + "% accuracy, " + image_instructions.visible_pixel_count + " pixels in that image to be created");

                    // display the comparison images
                    og_image.Source   = BitmapToImageSource(image_instructions.source_img);
                    demo_image.Source = BitmapToImageSource(image_instructions.visualized_img);

                    ui_pixel_count.Text = image_instructions.pixel_count.ToString();
                    ui_pixels_opaque.Text = image_instructions.visible_pixel_count.ToString();
                    ui_accuracy.Text = image_instructions.image_accuracy.ToString();
                    // functionality to output image for testing purposes
                    //image_instructions.visualized_img.Save("C:\\Users\\Joe bingle\\Downloads\\IFIMT research\\output.png", System.Drawing.Imaging.ImageFormat.Png);
            }}catch (Exception ex){
                Print(ex.ToString());
                return;
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

            int? start_index = try_parseint_textbox_text(ui_start);
            if (start_index == null || start_index >= image_instructions.pixels.Count){
                Print("bad start index");
                return;
            }

            ui_coord_x.IsEnabled = false;
            ui_coord_y.IsEnabled = false;
            ui_coord_z.IsEnabled = false;
            ui_coord_scale.IsEnabled = false;

            ui_start.IsEnabled = false;
            observable_checkbox.IsEnabled = false;
            ui_textures.IsEnabled = false;
            ui_observable_textures.IsEnabled = false;
            ui_lightness.IsEnabled = false;

            error_count = 0;

            ui_progressbar.Value = 0;
            is_running_macro = true;
            List<mapped_object> pixels_list;
            if (selected_pixels.Count == 0){ // regular mode
                pixels_list = image_instructions.pixels.Skip((int)start_index).ToList();
                ui_progressbar.Maximum = image_instructions.visible_pixel_count;
                ui_completion.Text = "0/" + image_instructions.visible_pixel_count;
            }else{ // selected indexes mode
                ui_progressbar.Maximum = selected_pixels.Count;
                ui_completion.Text = "0/" + selected_pixels.Count;
                pixels_list = new();
                for (int i = 0; i < selected_pixels.Count; i++)
                    pixels_list.Add(image_instructions.pixels[selected_pixels[i].visible_pixel_index]);
            }
            

            initiate_macro_task(pixels_list, (float)coord_scale, (float)coord_x, (float)coord_y, (float)coord_z);

        }
        private void force_abort_macro() {
            if (macro_cts != null){
                macro_cts.Cancel();
                Print("successfully called cancellation");
            }
        }
        private void ended_macro(){
            if (is_running_macro == false) return; // this was probably called by the fallback call, where we weren't reported that it ended
            ui_coord_x.IsEnabled = true;
            ui_coord_y.IsEnabled = true;
            ui_coord_z.IsEnabled = true;
            ui_coord_scale.IsEnabled = true;

            ui_start.IsEnabled = true;
            observable_checkbox.IsEnabled = true;
            ui_textures.IsEnabled = true;
            ui_observable_textures.IsEnabled = true;
            ui_lightness.IsEnabled = true;

            is_running_macro = false;
            RemoveHotkey();
            stop_timer();
        }
        float? try_parse_textbox_text(TextBox box, int decimal_points = 1){
            try{float return_ = float.Parse(box.Text);
                return_ = (float)Math.Round(return_, decimal_points);
                box.Text = return_.ToString();
                return return_;
            }catch{return null;}
        }
        int? try_parseint_textbox_text(TextBox box){
            try{int return_ = int.Parse(box.Text);
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
        CancellationTokenSource? macro_cts = null;
        async void initiate_macro_task(List<mapped_object> pixels, float coord_scale, float coord_x, float coord_y, float coord_z){
            set_hotkey();
            IProgress<macro_progress> progress = new Progress<macro_progress>(call_back_progress); // i believe we put the function in here
            Image_macro image_maker = new Image_macro();
            start_timer();
            macro_cts = new();
            await Task.Run(() => image_maker.begin_macro(pixels, coord_scale, coord_x, coord_y, coord_z, progress, macro_cts.Token));
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
        public int error_count = 0;
        public void call_back_progress(macro_progress progress_results){
            if (image_instructions == null) return; // this should never happen

            if (selected_pixels.Count == 0) // regular mode
                 ui_completion.Text = progress_results.completed + "/" + image_instructions.pixels.Count;
            else ui_completion.Text = progress_results.completed + "/" + selected_pixels.Count;

            ui_progressbar.Value = progress_results.completed;
            Print(progress_results.context);

            if (progress_results.state == macro_state.completed){
                // close the operation
                ended_macro();
            } else if (progress_results.state == macro_state.aborted){
                // close the operation
                ended_macro();
            } else if (progress_results.state == macro_state.error){
                // update error counter
                error_count++;
                ui_errors.Text = error_count.ToString();
            }
        }
        #endregion

        #region HOTKEY
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        private const int HOTKEY_ID = 9000;
        private IntPtr _windowHandle;
        private HwndSource _source;
        private void set_hotkey(){
            escape_hint.Visibility = Visibility.Visible;
            _windowHandle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);
            RegisterHotKey(_windowHandle, HOTKEY_ID, 0, 0x70); // f1 key
        }
        private void RemoveHotkey(){
            escape_hint.Visibility = Visibility.Collapsed;
            _source.RemoveHook(HwndHook);
            UnregisterHotKey(_windowHandle, HOTKEY_ID);
        }
        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled){
            const int WM_HOTKEY = 0x0312; 
            if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID){
                // escape process
                force_abort_macro();
                handled = true;
            }
            return IntPtr.Zero;
        }
        #endregion

        struct selected_pixel {
            public int x;
            public int y;
            public int visible_pixel_index;
            public Border selected_indicator;
        }

        List<selected_pixel> selected_pixels = new List<selected_pixel>();

        private void demo_image_MouseMove(object sender, MouseEventArgs e){
            if (image_instructions == null || image_instructions.visualized_img == null || is_running_macro) return;
            if (is_dragging){
                interact_with_point(e.GetPosition(demo_image));
            }else{ // attempt to select pixel here
                setup_border(e.GetPosition(demo_image), hover_border);
            }
        }
        void interact_with_point(System.Windows.Point coords){
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) // deselect
                    try_deselect_pixel_at(coords);
            else try_select_pixel_at(coords);
        }

        bool is_dragging = false;
        private void hover_border_MouseEnter(object sender, MouseEventArgs e){
            hover_border.Visibility = Visibility.Visible;
        }

        private void hover_border_MouseLeave(object sender, MouseEventArgs e){
            hover_border.Visibility = Visibility.Collapsed;
            is_dragging = false;
        }

        private void hover_border_MouseDown(object sender, MouseButtonEventArgs e){
            if (e.LeftButton != MouseButtonState.Pressed) return;
            is_dragging = true;
            e.Handled = true;
            hover_border.Visibility = Visibility.Collapsed;
            interact_with_point(e.GetPosition(demo_image));
        }

        private void hover_border_MouseUp(object sender, MouseButtonEventArgs e){
            if (e.LeftButton != MouseButtonState.Released) return;
            is_dragging = false;
            e.Handled = true;
            hover_border.Visibility = Visibility.Visible;
        }

        void try_select_pixel_at(System.Windows.Point coords){
            if (image_instructions == null || image_instructions.pixels == null || is_running_macro) return;

            int img_x = (int)Math.Truncate(coords.X / (demo_image.ActualWidth / image_instructions.visualized_img.Width));
            int img_y = (int)Math.Truncate(coords.Y / (demo_image.ActualHeight / image_instructions.visualized_img.Height));
            for (int i = 0; i < selected_pixels.Count; i++){
                if (selected_pixels[i].x == img_x && selected_pixels[i].y == img_y)
                    return;
            }
            int pixel_index = -1;
            // and then we have to make sure its a selectable pixel
            for (int i = 0; i < image_instructions.pixels.Count; i++){
                if (image_instructions.pixels[i].X == img_x && image_instructions.pixels[i].Y == img_y){
                    pixel_index = i;
                    break;
                }
            }
            if (pixel_index == -1) return; // pixel was not valid, do not select

            Border new_border = new Border();
            new_border.HorizontalAlignment = HorizontalAlignment.Left;
            new_border.VerticalAlignment = VerticalAlignment.Top;
            new_border.BorderBrush = System.Windows.Media.Brushes.White;
            new_border.BorderThickness = new Thickness(1, 1, 1, 1);
            new_border.IsHitTestVisible = false;
            setup_border(coords, new_border);

            ui_selected_pixels.Children.Add(new_border);

            selected_pixels.Add(new selected_pixel { selected_indicator = new_border, x = img_x, y = img_y, visible_pixel_index = pixel_index });
            ui_selected.Text = selected_pixels.Count.ToString();

            if (selected_pixels.Count == 1){ // then we were previously at none
                run_button.Content = "Run (selected)";
                clear_button.IsEnabled = true;
                ui_start.IsEnabled = false;
            }
        }
        void try_deselect_pixel_at(System.Windows.Point coords){
            if (image_instructions == null || image_instructions.pixels == null || is_running_macro) return;
            
            int img_x = (int)Math.Truncate(coords.X / (demo_image.ActualWidth / image_instructions.visualized_img.Width));
            int img_y = (int)Math.Truncate(coords.Y / (demo_image.ActualHeight / image_instructions.visualized_img.Height));
            for (int i = 0; i < selected_pixels.Count; i++){
                if (selected_pixels[i].x == img_x && selected_pixels[i].y == img_y){ // target found
                    ui_selected_pixels.Children.Remove(selected_pixels[i].selected_indicator);
                    selected_pixels.RemoveAt(i);
                    ui_selected.Text = selected_pixels.Count.ToString();

                    if (selected_pixels.Count == 0){ // none left, 
                        exit_selected_pixel_mode();
                    }

                    return;
                }
            }
        }
        private void clear_button_Click(object sender, RoutedEventArgs e) => exit_selected_pixel_mode();
        void exit_selected_pixel_mode(){
            selected_pixels.Clear();
            ui_selected_pixels.Children.Clear();
            run_button.Content = "Run";
            ui_selected.Text = "0";
            clear_button.IsEnabled = false;
            ui_start.IsEnabled = true;
        }
        void setup_border(System.Windows.Point coords, Border element){
            // get pixel size of image
            double real_pixels_per_img_pxiel_x = demo_image.ActualWidth / image_instructions.visualized_img.Width;
            double real_pixels_per_img_pxiel_y = demo_image.ActualHeight / image_instructions.visualized_img.Height;

            double img_x = Math.Truncate(coords.X / real_pixels_per_img_pxiel_x);
            double img_y = Math.Truncate(coords.Y / real_pixels_per_img_pxiel_y);

            double left_spacing = (250.0 - demo_image.ActualWidth) / 2;
            double top_spacing = (250.0 - demo_image.ActualHeight) / 2;

            // how do we 
            element.Width = real_pixels_per_img_pxiel_x;
            element.Height = real_pixels_per_img_pxiel_y;
            Thickness new_margin = new Thickness();
            new_margin.Left = left_spacing + img_x * real_pixels_per_img_pxiel_x;
            new_margin.Top = top_spacing + img_y * real_pixels_per_img_pxiel_y;
            element.Margin = new_margin;

        }

        
    }
}
