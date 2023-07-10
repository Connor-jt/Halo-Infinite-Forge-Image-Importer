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
using System.Windows.Controls;

namespace imaginator_halothousand.code_stuff{
    internal class Image_macro{

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        // VARIABLES
        private CMem? cm;

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
        public string? last_obj_name = null;
        string? get_obj_name(){
            // we also need to calculate the position of the main UI
            long? name_ui_ptr = return_UI_name_window_ptr();
            if (name_ui_ptr == null) 
                return null;

            long? string_pointer = cm.read_base_pointers((long)name_ui_ptr + UI_obj_name_ptr[0], UI_obj_name_ptr.Skip(1).ToArray());
            if (string_pointer == null) 
                return null;

            string? name_string = cm.read_wide_string((long)string_pointer);
            if (name_string == null) 
                return null;
            // i forget what else we needed here

            return name_string;
        }

        // POINTERS 
        const string module_name = "HaloInfinite.exe";
        const long   UI_array_size_offset  = 0x048089A8;
        const long   UI_array_start_offset = 0x04808AB0;

        int UI_property_menu_TAGID       = 0x3AEF69DB;
        int UI_color_menu_TAGID          = 0x2D9F2A06;
        int UI_value_menu_TAGID          = 0x642CFE7F; // used for determining when color window has opened
        int UI_name_menu_TAGID           = -704575514; // 0xD60107E6; // screw you joe sharp

        long[] UI_index_ptr    = new long[] { 0xB88, 0x928                           }; // property window
        long[] UI_value_ptr    = new long[] { 0xB88, 0xB08, 0x8C0, 0x8,   0x0,  0xFC }; // property window
        long[] UI_color_ptr    = new long[] { 0xB88, 0x6C8, 0xA0,  0x5A8, 0x928      }; // color window
        long[] UI_obj_name_ptr = new long[] { 0xC8,  0x398, 0x734                    }; // helper window
        long? return_UI_property_window_ptr() => return_UI_window_ptr(UI_property_menu_TAGID);
        long? return_UI_color_window_ptr()    => return_UI_window_ptr(UI_color_menu_TAGID);
        long? return_UI_value_window_ptr()    => return_UI_window_ptr(UI_value_menu_TAGID);
        long? return_UI_name_window_ptr()     => return_UI_window_ptr(UI_name_menu_TAGID);
        long? return_UI_window_ptr(int tag_id){ // doubles as a check to see if the window is open
            // attempt a few times, incase the timing was the worst ever
            for (int repeat = 0; repeat < 3; repeat++){
                if (repeat > 0) wait(5);

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

        float global_scale = 1;
        float global_X = 0;
        float global_Y = 0;
        float global_Z = 0;

        public int pixel_index = 0;
        IProgress<MainWindow.macro_progress> macro_progress;
        CancellationToken macro_cts;
        public void begin_macro(List<mapped_object> pixels, float _scale, float x, float y, float z, IProgress<MainWindow.macro_progress> progress, CancellationToken token){
            macro_cts = token;
            // configure intial values
            global_scale = _scale;
            global_X = x;
            global_Y = y;
            global_Z = z;
            macro_progress = progress;

            // setup memory interface
            cm = new();
            if (!cm.hook_and_open_process("HaloInfinite")){
                update_status("halo process not found", MainWindow.macro_state.aborted);
                return; // failed
            }
            Halodll_address = cm.return_module_address_by_name(module_name);
            if (Halodll_address == -1L){
                update_status("halo dll address not found", MainWindow.macro_state.aborted);
                return; // failed
            }

            SetForegroundWindow(cm.hooked_process.MainWindowHandle);
            wait(500);

            try{for (pixel_index = 0; pixel_index < pixels.Count; pixel_index++){
                    restore_state = state.not_created;
                    mapped_object? current = pixels[pixel_index];
                    wait(50);
                    for (int i = 0; i < 3; i++){ // 3 attempts to create the pixel

                        if (!create_pixel((mapped_object)current)){
                            if (i == 2){ // failed too many times, ending process
                                update_status("failed pixel " + i + " too many times, aborting", MainWindow.macro_state.aborted);
                                return; // failed
                            }
                            update_status("pixel " + pixel_index + " failed, waiting for menus to close to retry", MainWindow.macro_state.error);
                            if (!await_close_all_windows()){ // then close out the windows
                                update_status("failed to close menus to restart pixel " + i, MainWindow.macro_state.aborted);
                                return; // failed
                            }
                            wait(250);
                        } else break; // if didn't fail, then nextu
            }}} catch (Exception ex){
                last_step = "failure";
                update_status("manual cancellation or error arose: " + ex.ToString(), MainWindow.macro_state.aborted); ;
                return;
            }
            last_step = "success";
            update_status("process completed", MainWindow.macro_state.completed);
        }
        void wait(int miliseconds){
            Thread.Sleep(miliseconds);
            macro_cts.ThrowIfCancellationRequested();
        }
        bool is_game_session_is_alive(){
            return (cm != null && cm.hooked_process != null && !cm.hooked_process.HasExited);
        }

        public state restore_state = state.not_created; // for reseting pixel creation attempt
        public enum state{
            not_created = 0,
            obj_created = 1,
            posx = 2, 
            posy = 3, 
            posz = 4, 
            color = 5, 
            completed = 6,
        }
        public string last_step = "None";
        private void update_status(string new_status, MainWindow.macro_state state){
            last_step = new_status;
            macro_progress.Report(new MainWindow.macro_progress { context = new_status, completed = pixel_index, state = state });
        }

        bool create_pixel(mapped_object pixel){
            if (restore_state < state.obj_created){
                update_status("duplicating object", MainWindow.macro_state.working);
                if (!DuplicateObject())
                    return false;
                wait(300); // time for the object to spawn in
            }
            // we ALWAYS need to open the menu, regardless of what step we're up to
            update_status("openning menu", MainWindow.macro_state.working);
            if (!Enable_property_menu()) 
                return false;

            if (restore_state < state.obj_created){
                bool object_was_created = false;
                for (int i = 0; i < 10; i++){
                    if (i > 0) wait(20);
                    string? curr_obj_name = get_obj_name();
                    if (curr_obj_name == null)
                        continue;
                    if (curr_obj_name == last_obj_name)
                        continue;
                    object_was_created = true;
                    break;
                }
                if (!object_was_created) return false; // rerun the whole thing
                restore_state = state.obj_created; 
            }



            if (restore_state < state.posx){
                // set pos y // update value
                for (int i = 0; i < 3; i++){
                    if (i == 0) update_status("pos y, attempt#" + i, MainWindow.macro_state.working);
                    else        update_status("pos y, attempt#" + i, MainWindow.macro_state.error);
                    if (navigate_and_assign_menu_value(postion_x_index, global_X)) break;
                    else if (i == 2) return false;
                }
                restore_state = state.posx;
            }
            
            if (restore_state < state.posy){
                // set pos x // update value
                for (int i = 0; i < 3; i++){
                    if (i == 0) update_status("pos x, attempt#" + i, MainWindow.macro_state.working);
                    else        update_status("pos x, attempt#" + i, MainWindow.macro_state.error);
                    if (navigate_and_open_close_assign_value(postion_y_index, global_Y + ((float)pixel.X * global_scale))) break;
                    else if (i == 2) return false;
                }
                restore_state = state.posy;
            }
           
            if (restore_state < state.posz){
                // set pos z // update value
                for (int i = 0; i < 3; i++){
                    if (i == 0) update_status("pos z, attempt#" + i, MainWindow.macro_state.working);
                    else        update_status("pos z, attempt#" + i, MainWindow.macro_state.error);
                    if (navigate_and_assign_menu_value(postion_z_index, global_Z - ((float)pixel.Y * global_scale))) break;
                    else if(i == 2) return false;
                }
                restore_state = state.posz;
                wait(50);
            }


            if (restore_state < state.color){
                // goto color element
                update_status("going to color", MainWindow.macro_state.working);
                if (!Set_menu_selected_index(color_index)) 
                    return false;

                // open color element
                update_status("openning color", MainWindow.macro_state.working);
                if (!Enable_color_menu()) 
                    return false;

                // set color
                update_status("setting color index " + pixel.color_index, MainWindow.macro_state.working);
                if (!Set_color_selected_index(pixel.color_index)) 
                    return false;

                // close color element
                update_status("closing color", MainWindow.macro_state.working);
                if (!Disable_color_menu()) 
                    return false;
                //wait(20);
                restore_state = state.color;
            }

            // set color intensity
            update_status("color intensity " + (float)pixel.intensity_index / 100, MainWindow.macro_state.working);
            if (!navigate_and_open_close_assign_value(intensity_index, (float)pixel.intensity_index / 100)) 
                return false;
            wait(20);
            restore_state = state.completed;

            // close panel
            update_status("closing menu", MainWindow.macro_state.working);
            if (!Disable_property_menu()) 
                return false;

            return true;
        }


        #region AWAIT WINDOWS OPEN
        private bool Await_property_menu_open(){ // loop until menu values are readable
            for (int i = 0; i < 50; i++){
                if (i > 0) wait(15);
                if (get_index_pointer() == null)
                    continue;
                if (!Set_menu_selected_index(postion_x_index))
                    continue;
                if (Get_menu_selected_value() == null)
                    continue;
                return true; // all menu systems are working
            }
            return false;
        }
        private bool Await_color_menu_open(){
            for (int i = 0; i < 50; i++){
                if (i > 0) wait(10);
                if (get_color_pointer() == null)
                    continue;
                return true; // all menu systems are working
            }
            return false;
        }
        private bool Await_value_menu_open(){
            for (int i = 0; i < 50; i++){
                if (i > 0) wait(10);
                if (return_UI_value_window_ptr() == null)
                    continue;
                return true; // all menu systems are working
            }
            return false;
        }
        #endregion

        #region AWAIT WINDOWS CLOSING

        bool await_close_all_windows(){
            is_menu_open = false;
            is_color_open = false;
            for (int i = 0; i < 5; i++){
                Thread.Sleep(200); // fair wait time for any ingame process to complete
                if (is_a_window_open()){
                    if (!do_key_press(VKey.ESCAPE)) 
                        return false;
                } else 
                    return true;
            }
            return false; // failed to close windows
        }
        bool is_a_window_open(){
            if (return_UI_property_window_ptr() != null)
                return true;
            if (return_UI_color_window_ptr() != null)
                return true;
            if (return_UI_value_window_ptr() != null)
                return true;
            return false;
        }
        private bool Await_property_menu_close(){ // loop until the pointer is invalid
            for (int i = 0; i < 50; i++){
                if (i > 0) wait(10);
                if (return_UI_property_window_ptr() != null)
                    continue;
                return true; // menu is no longer open
            }
            return false;
        }
        private bool Await_color_menu_close(){ // loop until the pointer is invalid
            for (int i = 0; i < 50; i++){
                if (i > 0) wait(10);
                if (return_UI_color_window_ptr() != null)
                    continue;
                return true; // menu is no longer open
            }
            return false;
        }
        private bool Await_value_menu_close(){ // loop until the pointer is invalid
            for (int i = 0; i< 50; i++){
                if (i > 0) wait(10);
                if (return_UI_value_window_ptr() != null)
                    continue;
                return true; // menu is no longer open
            }
            return false;
        }
        #endregion

        #region AWAIT VALUE CHANGES
        public float? selected_value = null;
        private bool record_selected_value(){
            float? value = Get_menu_selected_value();
            if (value == null)
                return false;
            selected_value = value;
            return true;
        }
        private bool Await_selected_value_change(){
            if (selected_value == null)
                return false; // logic error
            //wait(20);
            for (int i = 0; i < 40; i++){
                if (i > 0) wait(5);
                float? value = Get_menu_selected_value();
                if (value == null) continue;
                if (value == selected_value) continue;
                selected_value = null; // value has changed, make sure recorded value is invalidated
                return true;
            } // if breaks, then failed
            selected_value = null;
            return false;
        }
        private int? selected_color = null;
        private bool record_selected_color(){
            int? index = Get_color_selected_index();
            if (index == null)
                return false;
            selected_color = index;
            return true;
        }
        private bool Await_selected_color_change(){
            if (selected_color == null)
                return false; // logic error
            for (int i = 0; i < 15; i++){
                if (i > 0) wait(15);
                int? index = Get_color_selected_index();
                if (index == null) continue;
                if (index == selected_color) continue;
                selected_color = null; // value has changed, make sure recorded value is invalidated
                return true;
            } // if breaks, then failed
            selected_color = null; 
            return false;
        }
        private int? Get_color_selected_index(){
            long? color_index_ptr = get_color_pointer();
            if (color_index_ptr == null)
                return null;
            return cm.read_int32((long)color_index_ptr);
        }
        #endregion


        private bool DuplicateObject() {
            last_obj_name = get_obj_name();
            if (last_obj_name == null)
                return false;

            if (!do_key_press(VKey.VK_D, true))
                return false;

            return true;
        }
        private bool AwaitDuplicateObject(){
            last_obj_name = get_obj_name();
            if (last_obj_name == null)
                return false;

            for (int attempti = 0; attempti < 3; attempti++){ // 3 attemtps to spawn in the object
                if (attempti > 0) update_status("duplicate attempt#" + attempti, MainWindow.macro_state.error);
                if (!do_key_press(VKey.VK_D, true))
                    return false;

                for (int i = 0; i < 40; i++){ // 520ms time for waiting for object to spawn
                    if (i > 0) wait(10);
                    string? curr_obj_name = get_obj_name();
                    if (curr_obj_name == null) 
                        continue;
                    if (curr_obj_name == last_obj_name) 
                        continue;

                    return true; // all menu systems are working
                }
            }
            return false;
        }

        #region PROPERTY WINDOW
        private bool Enable_property_menu(){
            if (is_menu_open) 
                return false; // failsafe for logic errors
            if (!do_key_press(VKey.VK_R)) 
                return false;
            // get the menu values, might need a longer wait
            if (!Await_property_menu_open()) 
                return false;
            is_menu_open = true;
            wait(100);
            return true;
        }
        private bool Disable_property_menu(){
            if (is_menu_open == false) 
                return false; // failsafe for logic errors
            if (!do_key_press(VKey.VK_R))
                return false;
            if (!Await_property_menu_close())
                return false;
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
            if (!Set_menu_selected_index(index)) 
                return false;
            if (!Apply_menu_selected_value(value - 0.10f)) 
                return false;
            if (!Apply_menu_value()) 
                return false; // expected to fail sometimes
            return true;
        } // pos x,y,z
        private bool navigate_and_open_close_assign_value(int index, float value){
            if (!Set_menu_selected_index(index)) 
                return false;
            if (!Apply_menu_selected_value(value)) 
                return false;
            if (!Enter_apply_menu_value())
                return false;
            return true;
        } // color intensity
        private bool Set_menu_selected_index(int index){
            long? index_ptr = get_index_pointer();
            if (index_ptr == null) 
                return false;
            // write value
            if (!cm.write_int32((long)index_ptr, index)) 
                return false;
            wait(20); // time for the new selection to occur
            return true;
        }
        private bool Apply_menu_selected_value(float new_val){
            long? value_ptr = get_value_pointer();
            if (value_ptr == null) 
                return false;
            // write value
            if (!cm.write_float((long)value_ptr, new_val)) 
                return false;
            wait(20); // time for the value to update
            return true;
        }
        private float? Get_menu_selected_value(){
            long? value_ptr = get_value_pointer();
            if (value_ptr == null) 
                return null;
            // write value
            float? result = cm.read_float((long)value_ptr);
            if (result == null) 
                return null;
            return result;
        }

        private bool Apply_menu_value(){
            if (!record_selected_value())
                return false;
            //wait(20); // time for recorded value to register?
            if (!do_key_press(VKey.RIGHT)) 
                return false;
            wait(20); // time for the key input to register
            //if (!Await_selected_value_change())
            //    return false;
            return true;
        }
        private bool Enter_apply_menu_value(){
            if (!do_key_press(VKey.RETURN))
                return false;
            if (!Await_value_menu_open())
                return false;
            wait(50); // extra time for the menu to open
            for (int i = 0; i < 5; i++){
                if (!do_key_press(VKey.RETURN))
                    return false;
                wait(20); // time for input to register
                if (Await_value_menu_close()) break;
                else if (i == 4)
                    return false;
            }
            return true;
        }
        #endregion

        #region COLOR WINDOW
        private bool Enable_color_menu(){
            if (is_menu_open == false) 
                return false; // cant open color menu without property menu
            if (is_color_open) 
                return false; // failsafe check
            if (!do_key_press(VKey.SPACE)) 
                return false;
            if (!Await_color_menu_open())
                return false;
            wait(50);
            is_color_open = true;
            return true;
        }
        private bool Disable_color_menu(){
            if (is_menu_open == false) 
                return false; // cant open color menu without property menu
            if (is_color_open == false) 
                return false; // failsafe check
            if (!do_key_press(VKey.SPACE)) 
                return false;
            if (!Await_color_menu_close())
                return false;
            is_color_open = false;
            color_index_pointer = null;
            return true;
        }
        private bool Set_color_selected_index(int index){
            long? color_index_ptr = get_color_pointer();
            if (color_index_ptr == null) 
                return false;
            if (!cm.write_int32((long)color_index_ptr, index)) 
                return false;
            wait(20); // time for value to set
            if (!Apply_color_value(index))
                return false;


            return true;
        }
        private bool Apply_color_value(int index){
            if (index < 8){ // first row
                if (!Color_down())
                    return false;
                if (!Color_up())
                    return false;
            } else{
                if (!Color_up())
                    return false;
                if (!Color_down())
                    return false;
            }
            wait(20); // time the selection to settle
            return true;
        }
        private bool Color_up(){
            if (!record_selected_color())
                return false;
            wait(20);
            if (!do_key_press(VKey.UP))
                return false;
            //wait(20);
            if (!Await_selected_color_change())
                return false;
            return true;
        }
        private bool Color_down(){
            if (!record_selected_color())
                return false;
            wait(20);
            if (!do_key_press(VKey.DOWN))
                return false;
            //wait(20);
            if (!Await_selected_color_change())
                return false;
            return true;
        }
        #endregion

        private bool do_key_press(VKey key, bool ctrl = false){
            if (!is_game_session_is_alive()) 
                return false;

            if (ctrl){
                KSim.KeyDown(VKey.LCONTROL);
                //wait(20);
                KSim.KeyPress(key);
                //wait(20);
                KSim.KeyUp(VKey.LCONTROL);
            }
            else KSim.KeyPress(key);                

            return is_game_session_is_alive();
        }



    }
}
