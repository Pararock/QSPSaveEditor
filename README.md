# QSPSaveEditor
A save editor for QSP games

# Download
You can download the latest version in [Releases](https://github.com/Pararock/QSPSaveEditor/releases)
### Support for .NET 4.0
If you need a version that will run on .NET 4.0 you can download the [0.2.1 release.](https://github.com/Pararock/QSPSaveEditor/releases/tag/0.2.1)

# Features
Load a save and modify the values of each variables.
Enter a custom command to run.
See changes between two saves.

# Issues
In some games, your first action after loading a modified save MUST be a move action. I'm still trying to find why.

# Requirements
* [Microsoft .NET Framework 4.6.1](https://www.microsoft.com/en-ca/download/details.aspx?id=49981)
* [Visual C++ Redistributable for Visual Studio 2015 (x86 version)](https://www.microsoft.com/en-ca/download/details.aspx?id=48145)

# Notes
If you have any critics or suggestion for future version. Please let me know.

# Future versions
My current idea for future version: fix the slow scrolling speed.

# Changes notes
## Version 0.3 - 2016-09-27
* Text Editor for large string variable
* Modified QSPLib to support games that uses the ADDQST instruction
* Fixed a bug where the ' character was not escaped

## Version 0.2 - 2016-09-13
* Highlight modified variable after reloading a save.
* You can reset the baseline when you reload a save.
* Minor UI Change.
* Filter by name works with on all cases now.
* Fixed crash on canceled exec command.
