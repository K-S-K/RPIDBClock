# RPIDBClock

## A .NET Raspberry Pi Commuter Desk Clock

### What is this all about

**RPIDBClock** is a thoughtfully crafted Raspberry Pi project written in **.NET** that turns a simple LCD display into a smart desk clock with **real-time Deutsche Bahn departures**. It’s not just a clock — it’s a personal mobility companion designed for people who need to stay in sync with the train schedule.

This project may be interesting for engineers exploring .NET on Linux, Raspberry Pi–based automation, or simply looking for a small but complete hardware-software system.

**Technically**, it is the Raspberry PI, connected to the DS3231 RTC module and the HD44780 LCD module with four lines by twenty symbols each.

![Common View](Doc/Images/Fig_00_Common_View.jpg)

### Motivation Story

Initially, I built this clock for my desk because I was tired of missing trains after long coding hours. I wanted a reminder that speaks my language — clear, precise, and human-readable — and also have it displayed on the dedicated display I like to see.

After getting the first working version, I can say that this project shows that .NET on Raspberry Pi isn’t just possible — it’s a pleasure to work with for IoT and automation tasks.

### Project Features (Initial Requirements)

Here are the theses I've made before the implementation as an initial concept. The product should:

- Display current date and time on the symbol LCD using a DS3231 hardware RTC
- Show the next two train departures from a personal schedule
- Have a clean .NET design with dependency injection and unit tests
- Works as a Linux service on Raspberry Pi

### Technical Brief on the result

Built to explore how expressive and maintainable .NET can be in automation and embedded scenarios, RPIDBClock demonstrates:

- Reliable real-time timekeeping using a DS3231 RTC module.
- Elegant hardware interaction via I2C (LCD and RTC).
- Clean architecture with dependency injection, testability, and remote debugging.
- Headless deployment workflows with simple bash scripts. The application can be built on the developer machine, deployed to the RPI via SSH, and executed as a service. For this purpose, the set of bash scripts was developed inside the project.
- When the service is executed, it updates the RTC's time from an NTP server (if the network is available), then periodically retrieves the RTC's time to update the display content.
- It has a hardcoded schedule of the trains I need to monitor for the commute, and displays the closest trains I can reach from my workplace.
- For this project, I used an older Raspberry Pi 3B (just because I own it), so it is more than possible with a modern one, and I expect it to have a shorter startup time and faster remote debugging.

### Possible Future Development Directions


- Add PWM brightness control for the display, with a photo-resistor to comply with the environmental conditions.
- Find out how to reliably integrate with the actual live train schedule with train delays. Because it's a shame to have a hardcoded schedule. Currently, I limited it by filtering one of the DB-scheduling service responses.
- Add an infra-red human presence detector to turn off the display backlight when nobody is there to see it.
- Add a morning-alarm-clock functionality, which will postpone the wake-up signal if the train is cancelled or significantly late.
- Add a WEB UI for adjusting the brightness coefficients, alarm settings, and commute schedule parameters updating.

### Usage Experience

It is nice and convenient to have this old-school display shining on the desk.

## Documentation reference

- [Architecture description](./Doc/Desighn/Readme.md)
The brief description of architecture decisions that were maid during the project implementation.

- [Deployment procedure](./Doc/Deploymment/Readme.md)
The description of the deployment procedure in two variants - for debugging and for production. Deployment is script-driven and ready for the CI-CD automation.

- [Some useful command line spells](Doc/Cmd/Readme.md)
Just to have a place for copy-paste from, and not to type it every time.

- [The configuration of the service on the RPI](Doc/Adm/Readme.md)
This is a process separate from software development itself, which warrants some dedicated description.

- [Raw research documentation on the train schedule provider quiring](Doc/MyVRN/Readme.md)
This is the most difficult and fragile part of the project, at least because there is no warranty of the data provider's format stability. To do this part seriously, we should be somehow connected to the data provider organization to obtain the necessary permissions and documentation for proper use of the service.

- [The formal article about the project](Doc/Readme.md) with description of purposes, architecture decisions involved, ans conclusions from the experiment results.

All bash commands described in the documentation are tested on MacOS with Homebrew shell. For different operation systems they probably need to be adopted.
