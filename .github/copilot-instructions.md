# GitHub Copilot Instructions for Rational Romance 2 (Continued)

## Mod Overview and Purpose

Rational Romance 2 (Continued) is an updated and enhanced version of the original mod by Rainbeau Flambe/Zombie_Striker. With the cessation of development by the original author, this mod aims to maintain and expand upon the original concepts by updating the mod for RimWorld version 1.1 and adding new features and performance improvements. This includes adjustments to pawns' romantic interactions, such as adding orientation traits, modifying the influence of misogynistic and misandristic traits, and incorporating cultural nuances into romance mechanics. The ultimate aim is to offer more realism and adaptability in pawns' romantic behaviors.

## Key Features and Systems

1. **Orientation Traits**: Pawns are assigned a trait indicating sexual orientation, including options like asexual, bisexual, gay, or straight. This assignment occurs post-regular trait allocation and can be customized via the mod's options menu.

2. **New Traits**: Adds "Faithful" and "Philanderer" traits that influence pawns' propensity to cheat.

3. **New Activities**: Includes long walks as date options and casual hookups as joy activities.

4. **Romantic Behavior Enhancements**: Adjustments are made to prevent romance attempts with already engaged pawns, rebuffed attempts, and those currently experiencing a mental break. Additionally, hardcoded gender-based romance preferences and biases have been refined to improve inclusivity and realism.

5. **Cultural Influence**: Romantic assertiveness varies depending on cultural backgrounds, enhancing the diversity of romantic dynamics.

6. **Polyamory Support**: Introduces polyamorous relationships while allowing customization of occurrence likelihood via options.

7. **Mood Penalty Adjustments**: Reduced penalties for romantic failures to prevent extreme reactions like murder sprees from rejection.

8. **Bug Fixes**: Corrects biases in romance initiation, marriage proposals, and parenting likelihood across genders.

## Coding Patterns and Conventions

- **Class and Method Organization**: The mod's C# scripts are organized by functionality, often grouping related actions like romance attempts, trait configurations, and interactions within dedicated classes and methods.
  
- **Naming Conventions**: Classes and methods use CamelCase for naming, reflecting their roles and the actions they perform (e.g., `InteractionWorker_RomanceAttempt_SuccessChance`, `Pawn_RelationsTracker_SecondaryLovinChanceFactor`).

- **Enum and Def Use**: Utilizes enums and definitions for job roles, traits, relations, and thoughts for modularity and ease of extension (`RRRJobDefOf`, `RRRRelationsDefsOf`).

## XML Integration

- XML-based definitions are employed to integrate new traits, job roles, and interaction mechanics. The definitions link C# scripts with RimWorld's in-game data structures to modify behaviors, traits, and interactions.

## Harmony Patching

- Harmony patches are used to alter existing behaviors within RimWorld's codebase without direct modification. This includes enhancing romance-related methods and ensuring the integrations work seamlessly with existing systems.

## Suggestions for Copilot

1. **Autocomplete for Harmony Methods**: When developing or adjusting patches, Copilot should suggest code blocks pertinent to Harmony prefix, postfix, and transpiler methods based on the existing class structure.

2. **Trait and Thought Extensions**: Suggest additions or modifications for new traits or thoughts based on pattern recognition from similar mods or code structures.

3. **XML-Driven Method Suggestions**: Offer potential XML structures or C# methods related to def additions when defining new game mechanics or interactions (e.g., modifying `RRRMiscDefOf` or `Thought_Worker`).

4. **Debugging Aids**: Generate snippets for logging, debugging, and error handling, particularly useful when resolving compatibility issues or integrating new features.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.
