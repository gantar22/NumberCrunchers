using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keys : MonoBehaviour {

	public static KeyCode a(string id){
		#if UNITY_STANDALONE_WIN
		if(id == "1"){
			return KeyCode.Joystick1Button0;
		}
		if(id == "2"){
			return KeyCode.Joystick2Button0;
		}
		if(id == "3"){
			return KeyCode.Joystick3Button0;
		}
		if(id == "4"){
			return KeyCode.Joystick4Button0;
		}
		#endif
		#if UNITY_EDITOR_WIN
		if(id == "1"){
			return KeyCode.Joystick1Button0;
		}
		if(id == "2"){
			return KeyCode.Joystick2Button0;
		}
		if(id == "3"){
			return KeyCode.Joystick3Button0;
		}
		if(id == "4"){
			return KeyCode.Joystick4Button0;
		}
		#endif
		#if UNITY_STANDALONE_OSX
		if(id == "1"){
			return KeyCode.Joystick1Button16;
		}
		if(id == "2"){
			return KeyCode.Joystick2Button16;
		}
		if(id == "3"){
			return KeyCode.Joystick3Button16;
		}
		if(id == "4"){
			return KeyCode.Joystick4Button16;
		}
		#endif
		#if UNITY_EDITOR_OSX
		if(id == "1"){
			return KeyCode.Joystick1Button16;
		}
		if(id == "2"){
			return KeyCode.Joystick2Button16;
		}
		if(id == "3"){
			return KeyCode.Joystick3Button16;
		}
		if(id == "4"){
			return KeyCode.Joystick4Button16;
		}
		#endif
		return KeyCode.JoystickButton0;
	}
	public static KeyCode b(string id){
		#if UNITY_STANDALONE_WIN
		if(id == "1"){
			return KeyCode.Joystick1Button1;
		}
		if(id == "2"){
			return KeyCode.Joystick2Button1;
		}
		if(id == "3"){
			return KeyCode.Joystick3Button1;
		}
		if(id == "4"){
			return KeyCode.Joystick4Button1;
		}
		#endif
		#if UNITY_EDITOR_WIN
		if(id == "1"){
			return KeyCode.Joystick1Button1;
		}
		if(id == "2"){
			return KeyCode.Joystick2Button1;
		}
		if(id == "3"){
			return KeyCode.Joystick3Button1;
		}
		if(id == "4"){
			return KeyCode.Joystick4Button1;
		}
		#endif
		#if UNITY_STANDALONE_OSX
		if(id == "1"){
			return KeyCode.Joystick1Button17;
		}
		if(id == "2"){
			return KeyCode.Joystick2Button17;
		}
		if(id == "3"){
			return KeyCode.Joystick3Button17;
		}
		if(id == "4"){
			return KeyCode.Joystick4Button17;
		}
		#endif
		#if UNITY_EDITOR_OSX
		if(id == "1"){
			return KeyCode.Joystick1Button17;
		}
		if(id == "2"){
			return KeyCode.Joystick2Button17;
		}
		if(id == "3"){
			return KeyCode.Joystick3Button17;
		}
		if(id == "4"){
			return KeyCode.Joystick4Button17;
		}
		#endif
		return KeyCode.JoystickButton1;
	}


	public static KeyCode x(string id){
		#if UNITY_STANDALONE_WIN
		if(id == "1"){
			return KeyCode.Joystick1Button2;
		}
		if(id == "2"){
			return KeyCode.Joystick2Button2;
		}
		if(id == "3"){
			return KeyCode.Joystick3Button2;
		}
		if(id == "4"){
			return KeyCode.Joystick4Button2;
		}
		#endif
		#if UNITY_EDITOR_WIN
		if(id == "1"){
			return KeyCode.Joystick1Button2;
		}
		if(id == "2"){
			return KeyCode.Joystick2Button2;
		}
		if(id == "3"){
			return KeyCode.Joystick3Button2;
		}
		if(id == "4"){
			return KeyCode.Joystick4Button2;
		}
		#endif
		#if UNITY_STANDALONE_OSX
		if(id == "1"){
			return KeyCode.Joystick1Button18;
		}
		if(id == "2"){
			return KeyCode.Joystick2Button18;
		}
		if(id == "3"){
			return KeyCode.Joystick3Button18;
		}
		if(id == "4"){
			return KeyCode.Joystick4Button18;
		}
		#endif
		#if UNITY_EDITOR_OSX
		if(id == "1"){
			return KeyCode.Joystick1Button18;
		}
		if(id == "2"){
			return KeyCode.Joystick2Button18;
		}
		if(id == "3"){
			return KeyCode.Joystick3Button18;
		}
		if(id == "4"){
			return KeyCode.Joystick4Button18;
		}
		#endif
		return KeyCode.JoystickButton2;
	}
	


}
