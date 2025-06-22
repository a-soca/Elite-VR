// Alex Soca 24/04/2025
// Class used to configure and control the emulator

#include "EmulatorController.h"

void EmulatorController::SetEmulatorSpeed(int speed) { // Alex Soca 06/04/2025
	BeebWin *bw = BeebWin::GetBeebWin(); // Get the emulator pointer
	(*bw).m_TimingType = TimingType::FixedSpeed; // Set timing to fixed speed
	(*bw).m_TimingSpeed = speed; // Set speed as desired
	(*bw).TranslateTiming();
}

// "elite-compendium-bbc-micro.dsd" is the file name for Elite
void EmulatorController::LoadDisk(const char* fileName) {
	// Alex Soca 25/02/2025 Start of custom autoload section
	BeebWin* bw = BeebWin::GetBeebWin(); // Get the emulator pointer

	(*bw).m_NoAutoBoot = false; // Unset the auto boot lock flag
	(*bw).Load1770DiscImage(fileName, 0, DiscType::DSD); // Immediately attempt to run the disk

	if (!(*bw).m_NoAutoBoot) // If the auto boot lock is inactive (Should be at this point)
	{
		(*bw).m_AutoBootDisc = true; // Set AutoBoot to true

		if (!(*bw).m_StartPaused) // If disk start is not paused (Should not be at this point)
		{
			(*bw).SetBootDiscTimer(); // Start the boot disk timer
		}
	}
}