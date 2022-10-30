# PLCSiemensSymulatorHMI

## Overview
Application serves as customizable (at certain level) desktop **HMI** panel and enables simultaneous communication between PC and **Siemens** PLC’s. 
Most of already existing dektop HMI applications for Siemens PLC's (like e.g. WinCC) are commercial and paid, therefore the main purposes 
during development process of my application was to take advantage of free tools and libraries. Application with its look and functionalities resembles 
HMI operation panels, which are widely known in industrial environment as devices that allow to monitor and control PLC controllers or executive devices. 

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
Communication between application and PLC is realized by Sharp7 library based on Siemens Ethernet S7Protocol. Files and classes architecture is 
based on MVVM architectural pattern which main advantage is providing separation between business logic, back-end logic and frond-end layers. User is
able to create his own HMI panel, enabling control and interaction with PLC program. Application can store many HMI panel's configurations connected to 
different devices in the same time (different connection details for each PLC). Once created, the configuration is stored in .xml file and loaded every
time during program start. The user has a possiblity to take advantage of generated excel file (PLC SIM) which contains PLC's database variables and use 
it for simplyfied controls creation. 

--most imporant classes description

___
## Used libraries and third part applications
>* [Caliburn.Micro](https://caliburnmicro.com/) - library based on name convenction that supports MVVM architectural pattern
>* [Sharp7](https://snap7.sourceforge.net/sharp7.html) - C# implementation of the S7Protocol for ommunication between application and PLC
>* [TIA Portal V15](https://www.siemens.com/global/en.html) - complex Siemens enviromet for creation, testing and configuratiom PLC's programms
>* [PLC SIM](https://www.siemens.com/global/en.html) - component of Siemens enviromet responsible for simulation PLC device on local PC 
>* [NetToPLCSim](https://nettoplcsim.sourceforge.net/) - allows you to access the PLC-Simulation S7 from your network via TCP/IP  communication, using the network interface of the PC on which the simulation is running.
>* [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/) - A free IDE used during application creation
___
## Additional Info
 -- aditional info - the same as in the video introduction
___
## Word of thanks
-- put info about used resources and links to them
