# Use .NET for Hardware Control on Embedded Linux Projects

Keywords: .NET, Linux, Embedded, I2C, GPIO, Raspberry Pi, Dependency Injection, xUnit, Unit Test.

## Abstract

In industrial hardware-related projects, we are usually limited in the tools for programming and configuring. Nowadays, with modern hardware and software assets, we have more agility to express our engineering ideas in more manageable, maintainable, and explainable forms than we did in the past. During this experiment, I've attempted to apply the architectural approaches I use in the. NET-related part of the software development industry to a simple embedded project.

## Introduction

Tooling decisions in industrial systems are rarely about fashion. They are about predictability, clarity, and long-term behavior under constraints.

Tooling choices are usually conservative. And that's for good reasons: engineering traditions, well-experienced approaches, certification, customer trust, and easily accessible service employees matter more than novelty. Nevertheless, new technologies are advancing and reach a point where it is at least possible to try to apply them, see the results, and draw some conclusions. The experiment discussed in this article was conducted to determine whether .NET stack tools and approaches perform well in hardware-adjacent domains.

During this small experiment, I built an embedded device using .NET on Linux, interacting directly with I2C peripherals. The visible result is a desk clock, but the purpose was different: to evaluate how modern .NET fits hardware-adjacent development and how architectural approaches from .NET can be translated to embedded systems.

To evaluate how modern .NET behaves at the hardware level, a small embedded Linux device was built. This device communicates directly with I2C peripherals using System.Device.Gpio. The visible result is a simple clock, but the focus was on validation rather than functionality.

![Common View](Images/Fig_00_Common_View.jpg)

## Experiment

### Hardware access from .NET

The project uses the System.Device.Gpio library provided as part of the .NET ecosystem. It allows developers to deal with GPIO and I2C directly from managed code. In this project, only the two following possibilities provided by the library were used:

- Explicit GPIO control
- I2C communication with byte-level access via I2cDevice class provided by the library

Thanks to easy-to-use abstractions, the project's code remains readable while staying close to the hardware.

### Architecture decisions

Even to this small device, I applied the internal structure familiar industrial principles:

- Hardware access separated from business logic
- The domain part (time control and scheduling) is handled independently from the presentation at the UI
- Every hardware part has its logical abstraction and its own "service" in terms of DI
- The business logic of the project is covered by unit tests, which use stub-implemented abstractions hidden behind interfaces instead of real hardware, which can be inaccessible on the developer's machine
- The clear and deterministic build procedure, which can be automated by a script
- The clear and deterministic deployment process is compatible with the "Infrastructure as a software" concept

The last three items on the list above allow us to wrap the development process into a CI/CD approach.

### Industrial approaches prototyping

In this project, .NET on embedded Linux was used to build a small device that communicates directly with I2C peripherals. While the device itself is simple, the constraints are familiar: continuous operation, controlled updates, external data integration, and predictable behavior over time.

These constraints naturally led to an architecture with clear hardware abstraction, explicit system state, and deterministic execution flow — the same qualities expected in industrial edge devices.

This made the system easier to reason about and naturally scaled to larger devices, such as controllers or edge gateways.

## Experiment Result Analysis

The project output shows that the applied technology enables meeting the constraints common in industrial devices, such as clear responsibility ownership and controlled data flow. The system continuously maintains state, integrates domain-related data, and runs predictably over time.

From this perspective, the project is less about the clock itself and more about validating an approach to small embedded systems.

The project served as a compact environment to test how modern .NET supports these patterns when operating close to hardware.

## Conclusions

This experiment confirmed that modern .NET can be a practical option for embedded Linux devices when clarity, structure, and maintainability are priorities. For robust Linux-based platforms like Raspberry Pi, BeagleBone, or industrial PCs, as well as modular systems such as the Revolution Pi, the decisive factor in some projects can be not only the language itself but also whether it supports disciplined system design and industrial architecture practices.
