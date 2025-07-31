# GitHub Copilot Instructions for RimWorld Modding Project in C#

This document provides instructions and guidelines for using GitHub Copilot effectively in the development of a RimWorld mod project using C#. This project involves modifying and enhancing relationship dynamics in the game.

## Mod Overview and Purpose
The mod, tentatively named "Rainbeau's Rational Romance," aims to enrich the romantic interactions and relationship complexities among pawns in RimWorld. By integrating nuanced social behaviors and reactions, the mod introduces more realistic and varied romantic scenarios.

## Key Features and Systems

- **Enhanced Romance System**: New classes and methods are implemented to dictate romantic attempts, proposals, and breakups with probabilities influenced by custom factors.
- **Detailed Traits and Thoughts**: Additional traits and thought classes align with the enhanced relationship dynamics, allowing for expanded narrative possibilities and gameplay impacts.
- **New Job Drivers**: Introduces new job drivers, such as "JobDriver_ProposeDate" and "JobDriver_CasualHookup," expanding the types of social interactions and activities pawns can engage in.
- **Human Contact Needs**: Pawns have enhanced needs related to human contact, affecting their moods and interactions.

## Coding Patterns and Conventions

- **Naming Conventions**: Classes and methods use PascalCase, following C# conventions. Private method names start with lowercase letters and follow camelCase.
- **Static Classes for Helpers**: Static classes like `AndroidsCompatibility` are used to encapsulate helper methods, ensuring ease of integration without instantiation.
- **Modular Design**: Methods are structured to be small and focused, aiding in single responsibility and reusability.
- **Use of Linq**: Methods often leverage Linq queries for efficient collection processing, reflecting common practice in C#.

## XML Integration

While most of the logic is coded in C#, the mod leverages RimWorld's XML definitions for jobs, traits, and thoughts. It is essential to maintain consistent naming between C# classes and relevant XML to avoid runtime errors.

- **Defining Elements**: Use XML files for defining `JobDefs`, `TraitDefs`, and `ThoughtDefs`, ensuring they correspond to the C# names defined, e.g., `RRRJobDefOf`, `RRRTraitDefOf`.
- **Localization Support**: Ensure all XML elements support localization for internationalization and accessibility.

## Harmony Patching

The mod employs Harmony to patch existing game methods, extending or altering behaviors without directly modifying game code.

- **Patch Structure**: Using Harmony, prefix and postfix methods are applied to extend or modify existing game functionalities.
- **Non-Invasive Modifications**: Patches should be performed in a way that allows the game to remain functional even if the mod is removed, adhering to best practices for mod compatibility. 

### Suggestions for Copilot

- **Pattern Recognition**: Utilize Copilot to suggest code completion for repetitive tasks like creating new JobDrivers or ThoughtWorkers by learning from existing patterns in the project.
- **Linq Queries**: Leverage Copilot for optimized Linq expressions, especially when processing collections within the game's environment.
- **Method Stubbing**: Utilize Copilot to generate method stubs when adding new features, ensuring conformity with existing design patterns.
- **XML Template Generation**: Ask Copilot to assist in creating new XML templates for job or thought definitions, using existing examples as reference.

By following these guidelines, developers can efficiently use Copilot to enhance their workflow while developing and maintaining the mod. The goal is to use automation to supplement creativity, not replace the unique game design necessary for a rich, immersive player experience.
