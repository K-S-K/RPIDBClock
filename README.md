# RPIDBClock

## Raspberry PI & .NET - based Clock with train schedule

![Common View](Doc/Images/Fig_00_Common_View.jpg)

The project was developed to explore the possibilities of interacting with hardware via the I2C protocol in the .NET-based application running on Linux.

**From a utility point of view**, it is a digital table clock that displays the current date and time, as well as the next two trains to commute, to be used at work to leave the office optimally without unnecessary waiting for a train at the Bahnhof.

**Technically**, it is the Raspberry PI, connected to the DS3231 RTC module and the HD44780 LCD module with four lines by twenty symbols each.

The RPI has a headless Debian installation and the .NET SDK v8. In the production, it is possible to install Runtime instead of SDK, but as I wanted to experiment with remote debugging, SDK was installed.

The application can be built on the developer machine, deployed to the RPI via SSH, and executed as a service. For this purpose, the set of bash scripts was developed inside the project.

When the application is executed, it updates the RTC's time from an NTP server (if the network is available), then periodically retrieves the RTC's time. It has a hardcoded schedule of the trains I need to monitor for the commute, and displays the closest trains I can reach from my workplace. Yes, that's a shame to have it hardcoded, but I have limited resources for hobby projects. So I limited it by filtering one of the DB-scheduling service responses. Maybe later I'll make it updateable on its own, but not right now, because the average time for a tram commute is shorter than for a train commute, so I don't use it since I changed my commute path.

The project's architecture uses Dependency Injection. It allows me to experiment with different configurations and to use stubs for some classes to play with others in unit tests, which I added to the project to protect already implemented logic from corruption during refactoring and optimization activities.

For the project, I've used a very old Raspberry Pi 3B (just because I own it), so it is possible with a modern one, and I expect it to have a shorter startup time.

## Documentation reference

- [Some useful command line spells](Doc/Cmd/Readme.md)
Just to have a place for copy-paste from, and not to type it every time.

- [The configuration of the service on the RPI](Doc/Adm/Readme.md)
This is a process separate from software development itself, which warrants some dedicated description.

- [Raw research documentation on the train schedule provider quiring](Doc/MyVRN/Readme.md)
This is the most difficult and fragile part of the project, at least because there is no warranty of the data provider's format stability. To do this part seriously, we should be somehow connected to the data provider organization to obtain the necessary permissions and documentation for proper use of the service.
