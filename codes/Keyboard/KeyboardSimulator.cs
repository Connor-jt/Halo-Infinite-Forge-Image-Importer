using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using WindowsInput.Native;

namespace WindowsInput{
    static class KSim{

        #region PUBLIC FUNCTIONS
        public static void KeyDown(VKey keyCode)  => DispatchInput(new INPUT[1]{BuildKeyDown(keyCode)});
        public static void KeyUp(VKey keyCode)    => DispatchInput(new INPUT[1]{BuildKeyUp(keyCode)});
        public static void KeyPress(VKey keyCode) => DispatchInput(BuildKeyPress(keyCode));
        #endregion

        #region KEY BUILDER
        private static bool IsExtendedKey(VKey keyCode){
            if (keyCode == VKey.MENU     || keyCode == VKey.RMENU || keyCode == VKey.CONTROL || keyCode == VKey.RCONTROL || keyCode == VKey.INSERT || keyCode == VKey.SNAPSHOT ||
                keyCode == VKey.DELETE   || keyCode == VKey.HOME  || keyCode == VKey.END     || keyCode == VKey.PRIOR    || keyCode == VKey.NEXT   || keyCode == VKey.RIGHT    ||
                keyCode == VKey.UP       || keyCode == VKey.LEFT  || keyCode == VKey.DOWN    || keyCode == VKey.NUMLOCK  || keyCode == VKey.CANCEL || keyCode == VKey.DIVIDE   )
                return true;
            return false;
        }
        private static INPUT BuildKeyDown(VKey keyCode){
            return new INPUT{
                Type = (UInt32) InputType.Keyboard,
                Data = {
                    Keyboard = new KEYBDINPUT{
                            KeyCode = (UInt16) keyCode,
                            Scan = (UInt16)(NativeMethods.MapVirtualKey((UInt32)keyCode, 0) & 0xFFU),
                            Flags = IsExtendedKey(keyCode) ? (UInt32) KeyboardFlag.ExtendedKey : 0,
                            Time = 0,
                            ExtraInfo = IntPtr.Zero
        }}};}
        private static INPUT BuildKeyUp(VKey keyCode){
            return new INPUT{
                Type = (UInt32) InputType.Keyboard,
                Data = {
                    Keyboard = new KEYBDINPUT{
                        KeyCode = (UInt16) keyCode,
                        Scan = (UInt16)(NativeMethods.MapVirtualKey((UInt32)keyCode, 0) & 0xFFU),
                        Flags = (UInt32) (IsExtendedKey(keyCode)? KeyboardFlag.KeyUp | KeyboardFlag.ExtendedKey : KeyboardFlag.KeyUp),
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
        }}};}
        private static INPUT[] BuildKeyPress(VKey keyCode){
            return new INPUT[2] { BuildKeyDown(keyCode), BuildKeyUp(keyCode) };
        }
        #endregion

        private static void DispatchInput(INPUT[] inputs){
            if (inputs == null) throw new ArgumentNullException("inputs");
            if (inputs.Length == 0) throw new ArgumentException("The input array was empty", "inputs");
            var successful = NativeMethods.SendInput((UInt32)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
            if (successful != inputs.Length)
                throw new Exception("Some simulated input commands were not sent successfully. The most common reason for this happening are the security features of Windows including User Interface Privacy Isolation (UIPI). Your application can only send commands to applications of the same or lower elevation. Similarly certain commands are restricted to Accessibility/UIAutomation applications. Refer to the project home page and the code samples for more information.");
        }
    }
}