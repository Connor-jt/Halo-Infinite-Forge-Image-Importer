using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;
using static imaginator_halothousand.code_stuff.ImageArrayifier;
using System.Diagnostics;
using System.Windows.Input;

namespace imaginator_halothousand.code_stuff{
    internal class Image_macro{

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        // VARIABLES
        private CMem? cm;
        public static InputSimulator Simulate = new InputSimulator();

        long Halodll_address = -1L;


        // THEORETICAL VARIABLES
        bool is_menu_open = false;
        bool is_color_open = false;

        long? properties_menu_pointer = null;
        long? properties_index_pointer = null;
        long? properties_value_pointer = null;
        long? color_index_pointer = null;
        long? get_menu_pointer(){
            if (properties_menu_pointer != null) return properties_menu_pointer;
            properties_menu_pointer = return_UI_property_window_ptr();
            return properties_menu_pointer;
        }
        long? get_index_pointer(){
            if (get_menu_pointer() == null) return null;

            if (properties_index_pointer != null) return properties_index_pointer;
            properties_index_pointer = cm.read_base_pointers((long)properties_menu_pointer + UI_index_ptr[0], UI_index_ptr.Skip(1).ToArray());
            return properties_index_pointer;
        }
        long? get_value_pointer(){ // this has to be refreshed each time it is read
            if (get_menu_pointer() == null) return null;

            properties_value_pointer = cm.read_base_pointers((long)properties_menu_pointer + UI_value_ptr[0], UI_value_ptr.Skip(1).ToArray());
            return properties_value_pointer;
        }
        long? get_color_pointer(){
            if (color_index_pointer != null) return color_index_pointer;
            // we also need to calculate the position of the main UI
            long? color_ui_ptr = return_UI_color_window_ptr();
            if (color_ui_ptr == null) return null;

            color_index_pointer = cm.read_base_pointers((long)color_ui_ptr + UI_color_ptr[0], UI_color_ptr.Skip(1).ToArray());
            return color_index_pointer;
        }

        // POINTERS 
        const string module_name = "HaloInfinite.exe";
        const long   UI_array_size_offset  = 0x048089A8; // we dont actually really need this
        const long   UI_array_start_offset = 0x04808AB0;

        const long UI_element_signature_offset  = 0x10; // note this isn't actually a signature, its a floating point number and then an int, which we are using to differentiate UI windows

        int UI_property_menu_TAGID       = 0x3AEF69DB;
        int UI_color_menu_TAGID          = 0x2D9F2A06;

        long[] UI_index_ptr = new long[] { 0xC8,  0x378, 0xE44, 0x328, 0x1D0, 0xB8,  0x928 }; // property window
        long[] UI_value_ptr = new long[] { 0xB88, 0xB08, 0x8C0, 0x8,   0x0,   0xFC         }; // property window
        long[] UI_color_ptr = new long[] { 0xB88, 0x6C8, 0xA0,  0x5A8, 0x928               }; // color window
        long? return_UI_property_window_ptr() => return_UI_window_ptr(UI_property_menu_TAGID);
        long? return_UI_color_window_ptr()    => return_UI_window_ptr(UI_color_menu_TAGID);
        long? return_UI_window_ptr(int tag_id){ // doubles as a check to see if the window is open
            // attempt a few times, incase the timing was the worst ever
            for (int repeat = 0; repeat < 3; repeat++){

                int? array_length = cm.read_int32(Halodll_address + UI_array_size_offset);
                if (array_length == null) return null;
                for (int i = 0; i < array_length; i++){

                    long? UI_ptr = cm.read_module_pointer(module_name, UI_array_start_offset + (i * 8)); // offset of 7th address
                    if (UI_ptr == null) continue;

                    long? tagid_address = cm.read_int64((long)UI_ptr + 0x70);
                    if (tagid_address == null) continue;

                    int? cur_tag_id = cm.read_int32((long)tagid_address + 0x08);
                    if (cur_tag_id == null) continue;
                    if (cur_tag_id != tag_id) continue; // verify to see if it matches

                    return UI_ptr;
                }
            }
            return null; // failed all attempts
        }

        #region UI INDEXES
        const int scale_x_index = 4;
        const int scale_y_index = 5;
        const int scale_z_index = 6;
              
        const int postion_x_index  = 10;
        const int postion_y_index  = 11;
        const int postion_z_index  = 12;
              
        const int rotation_x_index = 14;
        const int rotation_y_index = 15;
        const int rotation_z_index = 16;
              
        const int color_index      = 21;
        const int intensity_index  = 22;
        #endregion

        // MAIN FUNCTIONS
        public void begin_macro(mapped_object[] pixels){
            // setup memory interface
            cm = new();
            if (!cm.hook_and_open_process("HaloInfinite")){
                return; // failed
            }
            Halodll_address = cm.return_module_address_by_name(module_name);
            if (Halodll_address == -1L){
                return; // failed
            } 

            SetForegroundWindow(cm.hooked_process.MainWindowHandle);
            Thread.Sleep(500);

            foreach (mapped_object pixel in pixels){ // we're going to need the tile index for saving purposes
                if (!create_pixel(pixel)){
                    break; // failed
                }
                Thread.Sleep(500);
            }

        }
        bool is_game_session_is_alive(){
            bool connected = (cm != null && cm.hooked_process != null && !cm.hooked_process.HasExited);
            if (!connected) game_session_lost();
            return connected;
        }
        void game_session_lost(){
            // commit save & close protocol here

        }



        bool create_pixel(mapped_object pixel){
            Debug.WriteLine("duplicating object");
            if (!DuplicateObject()) return false;
            Thread.Sleep(500); // time for the object to spawn in

            Debug.WriteLine("openning menu");
            if (!Enable_property_menu()) return false;
            Thread.Sleep(500);

            // set pos x // update value
            Debug.WriteLine("pos x");
            if (!navigate_and_assign_menu_value(postion_x_index, (float)pixel.X)) return false;
            Thread.Sleep(500);
            // set pos y // update value
            Debug.WriteLine("pos y");
            if (!navigate_and_assign_menu_value(postion_y_index, (float)pixel.Y)) return false;
            Thread.Sleep(500);
            // set pos z // update value
            Debug.WriteLine("pos z");
            if (!navigate_and_assign_menu_value(postion_z_index, (float)pixel.Z)) return false;
            Thread.Sleep(500);

            // goto color element
            Debug.WriteLine("going to color");
            if (!Set_menu_selected_index(color_index)) return false;
            Thread.Sleep(500);
            // open color element
            Debug.WriteLine("openning color");
            if (!Enable_color_menu()) return false;
            Thread.Sleep(500);
            // set color
            Debug.WriteLine("setting color");
            if (!Set_color_selected_index(55)) return false;
            Thread.Sleep(500);
            // close color element
            Debug.WriteLine("closing color");
            if (!Disable_color_menu()) return false;
            Thread.Sleep(500);

            // close panel
            Debug.WriteLine("closing menu");
            if (!Disable_property_menu()) return false;
            Thread.Sleep(500);

            return true;
        }



        private bool DuplicateObject() => do_key_press(VirtualKeyCode.VK_D, true);

        #region PROPERTY WINDOW
        private bool Enable_property_menu(){
            if (is_menu_open) return false; // failsafe for logic errors
            if (!do_key_press(VirtualKeyCode.VK_R)) return false;
            Thread.Sleep(50); // give time to open the menu
            // get the menu values, might need a longer wait
            if (get_index_pointer() == null) return false;
            is_menu_open = true;
            return true;
        }
        private bool Disable_property_menu(){
            if (!do_key_press(VirtualKeyCode.VK_R)) return false;
            if (is_menu_open == false) return false; // failsafe for logic errors
            // flush the values
            is_menu_open = false;
            is_color_open = false;
            properties_menu_pointer = null;
            properties_index_pointer = null;
            properties_value_pointer = null;
            color_index_pointer = null;
            return true;
        }
        private bool navigate_and_assign_menu_value(int index, float value){
            if (!Set_menu_selected_index(index)) return false;
            Thread.Sleep(100);
            if (!Apply_menu_selected_value(value)) return false;
            return true;
        }
        private bool Set_menu_selected_index(int index){
            long? index_ptr = get_index_pointer();
            if (index_ptr == null) return false;
            // write value
            if (!cm.write_int32((long)index_ptr, index)) return false;
            return true;
        }
        private bool Apply_menu_selected_value(float new_val){ // we're going to need to do something different for the color intensity thing
            long? value_ptr = get_value_pointer();
            if (value_ptr == null) return false;
            // write value
            if (!cm.write_float((long)value_ptr, new_val - 0.10f)) return false;
            // then we use the right arrow key to actually apply it
            Thread.Sleep(100);
            if (!do_key_press(VirtualKeyCode.RIGHT)) return false;
            return true;
        }
        #endregion

        #region COLOR WINDOW
        private bool Enable_color_menu(){
            if (is_menu_open == false) return false; // cant open color menu without property menu
            if (is_color_open) return false; // failsafe check
            if (!do_key_press(VirtualKeyCode.SPACE)) return false;
            // get the color ptr address, might need a longer wait
            Thread.Sleep(100);
            if (get_color_pointer() == null) return false;
            is_color_open = true;
            return true;
        }
        private bool Disable_color_menu(){
            if (is_menu_open == false) return false; // cant open color menu without property menu
            if (is_color_open == false) return false; // failsafe check
            if (!do_key_press(VirtualKeyCode.SPACE)) return false;
            is_color_open = false;
            color_index_pointer = null;
            Simulate.Keyboard.KeyPress(VirtualKeyCode.SPACE);
            Thread.Sleep(5);
            Simulate.Keyboard.KeyPress(VirtualKeyCode.SPACE);
            return true;
        }
        private bool Set_color_selected_index(int index){
            long? color_index_ptr = get_color_pointer();
            if (color_index_ptr == null) return false;
            Thread.Sleep(100);
            if (!cm.write_int32((long)color_index_ptr, index)) return false;
            return true;
        }
        #endregion

        private bool do_key_press(VirtualKeyCode key, bool ctrl = false){
            if (!is_game_session_is_alive()) return false;
            if (ctrl) Simulate.Keyboard.KeyPress(VirtualKeyCode.LCONTROL);
            Simulate.Keyboard.KeyPress(key);
            Thread.Sleep(20);
            return is_game_session_is_alive();
        }


        #region UNUSED STUFF
        private void TriggerSave()
        {
            if (!is_game_session_is_alive()) return;

            SetForegroundWindow(cm.hooked_process.MainWindowHandle);
            cm.hooked_process.WaitForInputIdle();
            Thread.Sleep(20);
            Simulate.Keyboard.KeyPress(VirtualKeyCode.LCONTROL);
            Simulate.Keyboard.KeyPress(VirtualKeyCode.VK_S);
            Thread.Sleep(20);
        }
        #endregion


    }
}
