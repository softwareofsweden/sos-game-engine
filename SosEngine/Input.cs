using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using SlimDX.DirectInput;

namespace SosEngine
{

    /// <summary>
    /// Handle joystick input for generic game controllers using DirectInput.
    /// </summary>
    public class Input : IDisposable
    {

        private static int JoyMin = 16000;
        private static int JoyMax = 65535-16000;

        public enum PlayerInput
        {
            Left,
            Right,
            Up,
            Down,
            A,
            B,
            C,
            D,
            Select,
            Start
        }

        /*
        private SlimDX.DirectInput.DirectInput directInput;
        private List<SlimDX.DirectInput.Joystick> joysticks;
        private List<SlimDX.DirectInput.JoystickState> currentJoystickStates;
        private List<SlimDX.DirectInput.JoystickState> lastJoyStickStates;
        */

        /// <summary>
        /// Number of connected game controllers.
        /// </summary>
        public int NumberOfJoysticks
        {
            get { return numberOfJoysticks; }
        }
        private int numberOfJoysticks;

        public Input()
        {
            /*
            joysticks = new List<SlimDX.DirectInput.Joystick>();
            directInput = new SlimDX.DirectInput.DirectInput();
            currentJoystickStates = new List<JoystickState>();
            lastJoyStickStates = new List<JoystickState>();
            numberOfJoysticks = 0;
            foreach (SlimDX.DirectInput.DeviceInstance deviceInstance in directInput.GetDevices(SlimDX.DirectInput.DeviceClass.GameController, SlimDX.DirectInput.DeviceEnumerationFlags.AttachedOnly))
            {
                SosEngine.Core.Log("Game controller detected: " + deviceInstance.InstanceName);
                joysticks.Add(new SlimDX.DirectInput.Joystick(directInput, deviceInstance.InstanceGuid));
                joysticks[joysticks.Count - 1].Acquire();
                currentJoystickStates.Add(joysticks[joysticks.Count - 1].GetCurrentState());
                lastJoyStickStates.Add(joysticks[joysticks.Count - 1].GetCurrentState());
                numberOfJoysticks++;
            }
            */
        }

        public bool IsInput(int controllerIndex, PlayerInput playerInput)
        {
            switch (playerInput)
            {
                case PlayerInput.Left:
                    return JoystickLeft(controllerIndex);
                case PlayerInput.Right:
                    return JoystickRight(controllerIndex);
                case PlayerInput.Up:
                    return JoystickUp(controllerIndex);
                case PlayerInput.Down:
                    return JoystickDown(controllerIndex);
                case PlayerInput.A:
                    return JoystickButtonDown(controllerIndex, 0);
                case PlayerInput.B:
                    return JoystickButtonDown(controllerIndex, 1);
                case PlayerInput.C:
                    return JoystickButtonDown(controllerIndex, 2);
                case PlayerInput.D:
                    return JoystickButtonDown(controllerIndex, 3);
                case PlayerInput.Select:
                    return JoystickButtonDown(controllerIndex, 4);
                case PlayerInput.Start:
                    return JoystickButtonDown(controllerIndex, 5);
            }
            return false;
        }

        public bool IsInputPushed(int controllerIndex, PlayerInput playerInput)
        {
            switch (playerInput)
            {
                case PlayerInput.Left:
                    return JoystickLeftPushed(controllerIndex);
                case PlayerInput.Right:
                    return JoystickRightPushed(controllerIndex);
                case PlayerInput.Up:
                    return JoystickUpPushed(controllerIndex);
                case PlayerInput.Down:
                    return JoystickDownPushed(controllerIndex);
                case PlayerInput.A:
                    return JoystickButtonPushed(controllerIndex, 0);
                case PlayerInput.B:
                    return JoystickButtonPushed(controllerIndex, 1);
                case PlayerInput.C:
                    return JoystickButtonPushed(controllerIndex, 2);
                case PlayerInput.D:
                    return JoystickButtonPushed(controllerIndex, 3);
                case PlayerInput.Select:
                    return JoystickButtonPushed(controllerIndex, 4);
                case PlayerInput.Start:
                    return JoystickButtonPushed(controllerIndex, 5);
            }
            return false;
        }

        protected bool JoystickLeft(int controllerIndex)
        {
            if (controllerIndex < numberOfJoysticks)
            {
                return false;
                //return (currentJoystickStates[controllerIndex].X < JoyMin);
            }
            else
            {
                return false;
            }
        }

        protected bool JoystickRight(int controllerIndex)
        {
            if (controllerIndex < numberOfJoysticks)
            {
                return false;
                //return (currentJoystickStates[controllerIndex].X > JoyMax);
            }
            else
            {
                return false;
            }
        }

        protected bool JoystickUp(int controllerIndex)
        {
            if (controllerIndex < numberOfJoysticks)
            {
                return false;
                //return (currentJoystickStates[controllerIndex].Y < JoyMin);
            }
            else
            {
                return false;
            }
        }

        protected bool JoystickDown(int controllerIndex)
        {
            if (controllerIndex < numberOfJoysticks)
            {
                return false;
                //return (currentJoystickStates[controllerIndex].Y > JoyMax);
            }
            else
            {
                return false;
            }
        }

        protected bool JoystickLeftPushed(int controllerIndex)
        {
            if (controllerIndex < numberOfJoysticks)
            {
                return false;
                /*
                Core.Log(lastJoyStickStates[controllerIndex].X.ToString());
                return (currentJoystickStates[controllerIndex].X < JoyMin) &&
                    (lastJoyStickStates[controllerIndex].X > JoyMin) && (lastJoyStickStates[controllerIndex].X < JoyMax);
                */
            }
            else
            {
                return false;
            }
        }

        protected bool JoystickRightPushed(int controllerIndex)
        {
            if (controllerIndex < numberOfJoysticks)
            {
                return false;
                /*
                return (currentJoystickStates[controllerIndex].X > JoyMax) &&
                    (lastJoyStickStates[controllerIndex].X > JoyMin) && (lastJoyStickStates[controllerIndex].X < JoyMax);
                */
            }
            else
            {
                return false;
            }
        }

        protected bool JoystickUpPushed(int controllerIndex)
        {
            if (controllerIndex < numberOfJoysticks)
            {
                return false;
                /*
                return (currentJoystickStates[controllerIndex].Y < JoyMin) &&
                    (lastJoyStickStates[controllerIndex].Y > JoyMin) && (lastJoyStickStates[controllerIndex].Y < JoyMax);
                */
            }
            else
            {
                return false;
            }
        }

        protected bool JoystickDownPushed(int controllerIndex)
        {
            if (controllerIndex < numberOfJoysticks)
            {
                return false;
                /*
                return (currentJoystickStates[controllerIndex].Y > JoyMax) &&
                    (lastJoyStickStates[controllerIndex].Y > JoyMin) && (lastJoyStickStates[controllerIndex].Y < JoyMax);
                */
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Check which button that is pressed for specified joystick.
        /// </summary>
        /// <param name="controllerIndex"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool JoystickButtonDown(int controllerIndex, int button)
        {
            if (controllerIndex < numberOfJoysticks)
            {
                return false;
                // return (currentJoystickStates[controllerIndex].IsPressed(button));
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if button is being pressed for specified joystick.
        /// </summary>
        /// <param name="controllerIndex"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool JoystickButtonPushed(int controllerIndex, int button)
        {
            if (controllerIndex < numberOfJoysticks)
            {
                return false;
                // return (currentJoystickStates[controllerIndex].IsPressed(button) && lastJoyStickStates[controllerIndex].IsReleased(button));
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if button was pressed and then released for specified joystick.
        /// </summary>
        /// <param name="controllerIndex"></param>
        /// <returns></returns>
        public int GetPushedButton(int controllerIndex)
        {
            if (controllerIndex < numberOfJoysticks)
            {
                /*
                bool[] buttons = currentJoystickStates[controllerIndex].GetButtons();
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (buttons[i])
                    {
                        return i;
                    }
                }
                */
                return -1;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Check joystick state.
        /// </summary>
        public void Update()
        {
            /*
            for (int i = 0; i < joysticks.Count; i++)
            {
                JoystickState state = joysticks[i].GetCurrentState();
                lastJoyStickStates[i] = currentJoystickStates[i];
                currentJoystickStates[i] = state;
            }
            */
        }

        public void Dispose()
        {
            /*
            directInput.Dispose();
            foreach (SlimDX.DirectInput.Joystick joystick in joysticks)
            {
                joystick.Dispose();
            }
            */
        }

    }
}
