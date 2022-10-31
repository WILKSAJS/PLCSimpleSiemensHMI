# PLCSiemensSymulatorHMI

## Overview
Application serves as customizable (at certain level) desktop **HMI** panel and enables simultaneous communication between PC and **Siemens** S7-family PLC’s. Most of already existing dektop HMI applications for Siemens PLC's (like e.g. WinCC) are commercial and paid, therefore the main purposes during development process of my application was to take advantage of free tools and libraries. Application with its look and functionalities resembles HMI operation panels, which are widely known in industrial environment as devices that allow to monitor and control either PLC controllers or executive devices. 

___
## Application presentaion video
-- put YT link here

___
## Technology Stack
>* C# 7.0
>* .NET Framework 4.7.2
>* WPF (Windows Presentation Foundation)
>* Siemens PLC's programming & simulation environment

___
## Application description
Communication between application and PLC is realized by Sharp7 library based on Siemens Ethernet S7Protocol. Files and classes architecture is based on MVVM architectural pattern which main advantage is to providing separation between business logic, back-end logic and frond-end layers. User is able to create his own HMI panel, enabling control and interaction with PLC program. Application can store many HMI panel's configurations attached to different devices (different connection details for each PLC). Once created, the configuration is stored in .xml file and loaded every time during program start. The user has a possiblity to take advantage of generated excel file (PLC SIM) which contains PLC's database variables and use it for simplyfied HMI's controls creation. 

___
## Program architecture description
The application architecture is based on **MVVM pattern**. I've used **Caliburn.Micro** library to simplify pattern implementation and take advantage of built **dependency injection** fuctionality. Caliburn.micro is based on name convention therefore **before getting started working with the source code, I strongly recomend to get more familar with the framework itself.** 

___
## Used libraries and third part applications
>* [Caliburn.Micro](https://caliburnmicro.com/) - library based on name convenction that supports MVVM architectural pattern
>* [Sharp7](https://snap7.sourceforge.net/sharp7.html) - C# implementation of the S7Protocol for ommunication between application and PLC
>* [TIA Portal V15](https://www.siemens.com/global/en.html) - complex Siemens environmet for creation, testing and configuratiom PLC's programms
>* [PLC SIM](https://www.siemens.com/global/en.html) - component of Siemens environmet responsible for simulation PLC device on local PC 
>* [NetToPLCSim](https://nettoplcsim.sourceforge.net/) - allows you to access the PLC-Simulation S7 from your network via TCP/IP  communication, using the network interface of the PC on which the simulation is running.
>* [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/) - A free IDE used during application creation
___
## Additional Info
Above video presents functionalities of the program and environment configuration process step by step. It was part of my final semester project's presentation.

It's my **VERY FIRST program written around 1-2 years ago** when attending university. At that time I attended classes themed for objected-oriented programing and PLC programming with use of Siemens environment and ladder programing language, so I decided to combine gained knowledge into one.

During writting I took advantage of TIA Portal V15 and PLCSIM (provided by university) to simulate S7-familly PLC environment. Program was built with use of C# programming language, WPF(.NET Framework), Sharp7 library and Microsoft IDE - Visual Studio 2019. **The PLC's program and NetToPlcSIM configuration examples are included within project solution** in '..\PLCSiemensSymulatorHMI\ExternalComponents\ThirdPartFiles' directory. Feel free to use them or create new ones on your own.

During development I took advantage of maerials already presented on YT or other sources, the authors whom I'm able to remember will be mentioned below, so visit their content as well.

Program probably contains some undiscovered bugs and is written in 'not the best' manner as it was my first project but I've decided to share it with society anyway. For me is not a meaningful effort and I don't plan to return to desktop applications. Hope it'll be helpful for some of you. It was tested only in simulation environment. I haven't got possibility to test it with use of physical PLC device.

**Feel free to take a source code from repository and extend/improve it according to your will.**
___
## Word of thanks
I'm really grateful below mentioned authors for sharing their knowledge, so I could take advantage of it and use in my project.
>* [Mesta Automation](https://www.mesta-automation.com/) - Blog generally focused on combining PLC's and C# technologies
>* [Industrial Networks YT](https://www.youtube.com/c/IndustrialNetworks/featured) - YT channel about HMI's controls creation for desktop WPF applications
>* [Payload YT](https://www.youtube.com/channel/UCOoKt2u-bE1NuELXSFaEdUw/featured) - YT channel cocentrated on building modern looking WPF applications interfaces
