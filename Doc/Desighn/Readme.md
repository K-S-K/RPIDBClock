# The architecture

## The solution contains several assemblies

| Name  | Type     | Description                                  |
|-------|----------|----------------------------------------------|
|rdc-bas|library   |Common types                                  |
|rdc-lcd|library   |Display-related types                         |
|rdc-net|library   |Network connectivity                          |
|rdc-rtc|library   |Runtime Clock                                 |
|rdc-svc|executable|Entry point of service and some business logic|
|rdc-xut|library   |XUnit Tests for business logic                |

### Every device-related library contains following classes:

- hardware-related driver-like wrapper around I2C API which maps domain-related data and commands to the hardware - related data and commands,
- interface as an implementation-agnostic abstraction layer,
- functional hardware-dependent implementation which implements the interface, and
- hardware-independent stub to use in tests and in the working configurations without particular hardware installed.
