# SoundBlasterCinemaProfilesSwitch
Sound Blaster Cinema 1 solutions for Windows 10 problems.

		██╗    ██╗ █████╗ ██████╗ ███╗   ██╗██╗███╗   ██╗ ██████╗                           
            ██║    ██║██╔══██╗██╔══██╗████╗  ██║██║████╗  ██║██╔════╝                           
            ██║ █╗ ██║███████║██████╔╝██╔██╗ ██║██║██╔██╗ ██║██║  ███╗                          
            ██║███╗██║██╔══██║██╔══██╗██║╚██╗██║██║██║╚██╗██║██║   ██║                          
            ╚███╔███╔╝██║  ██║██║  ██║██║ ╚████║██║██║ ╚████║╚██████╔╝                          
             ╚══╝╚══╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝╚═╝  ╚═══╝ ╚═════╝                           
                                                                                    
            These programs will modify a value in the Windows Registry. 
            Although this will be faily safe, please do backup your registry base first !  
			
# Legal : 
This software is provided "as is" and I am not responsible for any damage possibly caused by it.

In this repository you'll find three things :

- A list of many method to try running Sound Blaster Cinema.
- A "manual" way to switch the profile loaded with Sound Blaster Cinema at launch. It will not correct the problems but with allow you to enjoy your profiles for now. Two software are provided :
	- ~~A script **SoundBlasterCinemaProfilesSwitch.ps1** that will do similar things as the executable program but with which you can actually directly see was it is doing. You'll probably need permission to run it. Prefer the executable if you can't set the permissions.~~
	- A source code **Program.cs** which correspond of the source for the SoundBlasterCinemaProfilesSwitch.exe of the release. Feel free to compile it yourself and check if you prefer for your safety ;) This program is the easiest way to change the profile without having you manually edit the registry base.


#Commons solutions

##The first solution to try
##Sound Blaster Cinema freeze at launch or after launching

You'll need to delete or move the file ```C:\Program Files (x86)\Creative\Sound Blaster Cinema\Sound Blaster Cinema\SBCinema.exe.config```


#The program

If like me after installing the Windows 10 Creator Update you have can't make Sound Blaster Cinema work, the only way to change the profile is to do it manually editing the registry base. To make this process faster and easier, I've wrote a program for that.

This program does the following in order :

- Put a warning about the registry modification,
- Get the profiles from the ```C:\\Program Files (x86)\\Creative\\Sound Blaster Cinema\\Sound Blaster Cinema\\Profile``` folder. *Please edit the program.cs file and compile it if you use another path.*
- Terminate the current Sound Blaster Cinema process,
- Get your profile choice,
- Set the ```HKEY_CURRENT_USER\Software\Creative Tech\Sound Blaster Cinema\ProfileIndex``` registry key with the choosen value.
- Launch the execution file ```C:\Program Files (x86)\Creative\Sound Blaster Cinema\Sound Blaster Cinema\SBCinema.exe```. *Please edit the program.cs file and compile it if you use another path.*

Please report any problems you might have with the executable.
