// Alex Soca 24/04/2025
// Class to act as an interface for simulating input to the emulator
#pragma once

#include "InputSimulationFacade.h"

#include "Main.h" // For mainWin
#include <thread> // For thread sleep

using namespace std;

#define KEY_PRESS_DURATION 50 // Duration of Keypresses in milliseconds

// -------------------------- INPUT SIMULATION --------------------------

// Momentarily presses a key
void InputSimulator::PressKey(int keyCode) { // Alex Soca
    PostMessage(mainWin->m_hWnd, WM_KEYDOWN, keyCode, NULL); // Press key
    this_thread::sleep_for(std::chrono::milliseconds(KEY_PRESS_DURATION));
    PostMessage(mainWin->m_hWnd, WM_KEYUP, keyCode, NULL); // Release key
}

// Holds down a key until the ReleaseKey function is called
void InputSimulator::HoldKey(int keyCode) { // Alex Soca
    PostMessage(mainWin->m_hWnd, WM_KEYDOWN, keyCode, NULL); // Press key
}

// Releases a key being held down
void InputSimulator::ReleaseKey(int keyCode) { // Alex Soca
    PostMessage(mainWin->m_hWnd, WM_KEYUP, keyCode, NULL); // Release key
}

// Momentarily presses an arrow key
void InputSimulator::PressArrowKey(char direction) { // Alex Soca 24/04/2025
    InputSimulator::PressKey(GetArrowKeyCode(direction));
}

// Holds down an arrow key until the ReleaseArrowKey function is called
void InputSimulator::HoldArrowKey(char direction) { // Alex Soca 24/04/2025
    InputSimulator::HoldKey(GetArrowKeyCode(direction));
}

// Releases a key being held down
void InputSimulator::ReleaseArrowKey(char direction) { // Alex Soca 24/04/2025
    InputSimulator::ReleaseKey(GetArrowKeyCode(direction));
}

// Momentarily presses a function key
void InputSimulator::PressFunctionKey(int functionNumber) { // Alex Soca 24/04/2025
    InputSimulator::PressKey(0x70 + functionNumber - 1); // Add one less than the integer to the F1 keycode constant (So that F1 adds 0)
}

// -------------------------- Utility Functions --------------------------

// Converts a direction into an arrow keycode
char InputSimulator::GetArrowKeyCode(char direction) { // Alex Soca
    // Convert the direction to the virtual key constant
    switch (direction) {
    case 'L': // Left
        return VK_LEFT;
    case 'U': // Up
        return VK_UP;
    case 'R': // Right
        return VK_RIGHT;
    case 'D': // Down
        return VK_DOWN;
    }

    return NULL; // Invalid
}

// ------------------------ RAW INPUT TRANSLATION ------------------------
// These functions should be used internally if required to abstract input
// translation away from the class user.
// NOTE: The PostMessage functions should be used instead of SendInput
// as this allows the window to be targeted, allowing input simulation 
// even when the window is out of focus.

// Converts a keycode into an Input
INPUT InputSimulator::GenerateKeyDownInput(int keyCode) { // Alex Soca
    INPUT key;
    key.type = INPUT_KEYBOARD;
    key.ki.wScan = 0; // Hardware scan code for key
    key.ki.time = 0;
    key.ki.dwExtraInfo = 0;

    key.ki.wVk = keyCode; // Virtual-key code for the key
    key.ki.dwFlags = 0;

    return key;
}

// Converts a keydown input into a key up input
INPUT InputSimulator::GenerateKeyUpInput(INPUT input) { // Alex Soca
    input.ki.dwFlags = KEYEVENTF_KEYUP;
    return input;
}