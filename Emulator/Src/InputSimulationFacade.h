#pragma once

// Alex Soca 24/04/2025
// Class to act as an interface for simulating input to the emulator

#include <windows.h>

class InputSimulator {
public:
    // Normal Keys
    static void PressKey(int keyCode); // Momentarily presses a key
    static void HoldKey(int keyCode); // Holds down a key until the ReleaseKey function is called
    static void ReleaseKey(int keyCode); // Releases a key being held down

    // Arrow Keys
    static void PressArrowKey(char direction); // Momentarily presses an arrow key
    static void HoldArrowKey(char direction);
    static void ReleaseArrowKey(char direction);

    // Function Keys
    static void PressFunctionKey(int functionNumber);

    InputSimulator() = delete; // Prevent the class from being instantiated

private:
    static char GetArrowKeyCode(char direction); // Converts a direction into an arrow keycode
    static INPUT GenerateKeyDownInput(int keyCode); // Converts a keycode into an Input
    static INPUT GenerateKeyUpInput(INPUT input); // Converts a keydown input into a key up input
};