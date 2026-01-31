# Using .NET for Hardware Interaction on Embedded Linux

This work explores how modern .NET fits hardware-adjacent development on embedded Linux systems and whether architectural approaches commonly used in .NET applications transfer to embedded environments.

A small embedded device was implemented using .NET on Linux, interacting directly with I2C peripherals via standard device libraries. The visible result is a simple desk clock, while the focus lies on system structure and behavior rather than the functionality itself.

The experiment shows that this approach aligns well with constraints typical for industrial devices: clear responsibility boundaries, controlled data flow, and predictable execution over time. The system continuously maintains state, integrates domain-related data, and operates deterministically without hidden abstractions.

The results indicate that modern .NET can be a practical option for embedded Linux devices when clarity, structure, and maintainability are priorities. For platforms such as Raspberry Pi, BeagleBone, industrial PCs, or modular systems like Revolution Pi, the decisive factor in many projects is not the language itself, but whether it supports disciplined system design and industrial architecture practices.

Source code and project details are available on GitHub: https://github.com/K-S-K/RPIDBClock