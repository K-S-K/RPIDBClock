# The architecture

## Abstract

This structure provides a clear separation between domain logic, hardware abstraction, and infrastructure, allowing the application to be tested and executed with or without physical devices present.

## The solution contains several assemblies

| Name  | Type     | Description                                      |
|-------|----------|--------------------------------------------------|
|rdc-bas|library   |Common types                                      |
|rdc-lcd|library   |Display-related types                             |
|rdc-net|library   |Network connectivity                              |
|rdc-rtc|library   |Real-Time Clock (RTC)                             |
|rdc-svc|executable|Entry point of the service and some business logic|
|rdc-xut|library   |XUnit Tests for business logic                    |

### Every device-related library contains the following classes

- hardware-oriented driver-like wrapper around the I2C API that maps domain concepts to low-level commands,
- an interface defining an implementation-agnostic abstraction,
- a functional hardware-dependent implementation, and
- a hardware-independent stub used for tests or for configurations without physical hardware.
